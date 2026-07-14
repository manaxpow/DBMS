# Unit Test Specification — `ExtentManager`

## 1. Scope

This document covers unit test specifications for `ExtentManager`.
Tests are isolated from all external dependencies using mocks.
Only public method behavior is verified.

## 2. Component

* **Class:** `ExtentManager`
* **Methods:**

```csharp
AllocatedExtent AllocateExtent(
    OpenFileEntry entry);

void FreeExtent(
    OpenFileEntry entry,
    ExtentId extentId);
```

## 3. Unit Under Test

`ExtentManager` is responsible for:

* Searching the `AllocationMetadata` bitmap for free space.
* Allocating available extents, marking them in the bitmap, and updating counters.
* Resolving space exhaustion by calling file auto-extensions (via `ResizeFile`) under Cap capacities.
* Persisting updated allocation metadata to disk.
* Safeguarding state consistencies by rolling back memory bitmap states when metadata writes throw exceptions.
* Deallocating extents, validating index bounds, and double-free protections.
* Blocking releases of extents actively in use by higher-level transaction trackers.

## 4. Mocked Dependencies

* `IFileWriter`
* `IFileLifecycleManager`
* `IExtentUsageTracker`

---

# Test Cases

## Case 1: Allocate Empty Extent Success

### Input

```text
entry = openFileEntry
```

### Preconditions

* Free extents are available.
* Metadata persistence succeeds.

### Execution

```csharp
AllocateExtent(openFileEntry);
```

### Expected Output

* Returns a valid `AllocatedExtent` mapping.

### Expected State

* Selected extent is set to allocated in-memory.
* Metadata counters are updated and persisted to disk.

### Expected Dependency Calls

```text
allocationMetadata.FindFreeExtent()
    -> returns ExtentId(5)

allocationMetadata.MarkExtentAllocated(ExtentId(5))
    -> succeeds

FileWriter.WriteAllocationMetadata(handle, header, metadata)
    -> succeeds
```

### Suggested Test Name

```csharp
AllocateExtent_FreeExtentAvailable_AllocatesAndPersists()
```

---

## Case 2: Allocate Triggering Auto-Extend

### Preconditions

* `FreeExtentCount = 0` and `AutoExtendEnabled = true`.
* Physical resizing and metadata write calls succeed.

### Execution

```csharp
AllocateExtent(openFileEntry);
```

### Expected Output

* Returns a valid `AllocatedExtent` mapping representing the newly added space.

### Expected Dependency Calls

```text
allocationMetadata.FindFreeExtent()
    -> returns null

FileLifecycleManager.ResizeFile(openFileEntry, newSize)
    -> succeeds (internally updates AllocationMetadata with new extents)

allocationMetadata.FindFreeExtent()
    -> returns the newly added ExtentId

allocationMetadata.MarkExtentAllocated(newExtentId)
    -> succeeds

FileWriter.WriteAllocationMetadata(handle, header, metadata)
    -> succeeds
```

> [!NOTE]
> `ResizeFile` is responsible for calling `AllocationMetadata.AddExtents` internally.
> `ExtentManager` does not call `AddExtents` directly; it re-scans for a free extent after `ResizeFile` returns.

### Suggested Test Name

```csharp
AllocateExtent_ExhaustedWithAutoExtend_ExtendsFileAndAllocates()
```

---

## Case 3: Allocate Triggering Auto-Extend but Disabled

### Preconditions

* `FreeExtentCount = 0` and `AutoExtendEnabled = false`.

### Execution

```csharp
AllocateExtent(openFileEntry);
```

### Expected Output

```text
Throws NoFreeExtentException
```

### Suggested Test Name

```csharp
AllocateExtent_ExhaustedWithAutoExtendDisabled_ThrowsNoFreeExtentException()
```

---

## Case 4: Maximum File Size Reached Limit

### Preconditions

* `FreeExtentCount = 0` and `AutoExtendEnabled = true`.
* Physical file size has already reached the maximum limit.

### Execution

```csharp
AllocateExtent(openFileEntry);
```

### Expected Output

```text
Throws MaximumFileSizeExceededException
```

### Expected Dependency Calls

```text
allocationMetadata.FindFreeExtent()
    -> returns null

FileLifecycleManager.ResizeFile(openFileEntry, newSize)
    -> throws MaximumFileSizeExceededException
```

### Forbidden Dependency Calls

```text
allocationMetadata.MarkExtentAllocated(...)
FileWriter.WriteAllocationMetadata(...)
```

### Suggested Test Name

```csharp
AllocateExtent_MaxSizeReached_ThrowsMaximumFileSizeExceededException()
```

---

## Case 5: Resize File Failure during Extension

### Preconditions

* `ResizeFile` throws `FileResizeException`.

### Execution

```csharp
AllocateExtent(openFileEntry);
```

### Expected Output

```text
Throws ExtentAllocationException
```

### Expected Exception

* Wraps the `FileResizeException`.

### Expected Dependency Calls

```text
FileLifecycleManager.ResizeFile(...)
    -> throws FileResizeException
```

### Suggested Test Name

```csharp
AllocateExtent_ResizeFails_ThrowsExtentAllocationException()
```

---

## Case 6: Call MarkExtentAllocated Correctly

### Input

```text
entry = openFileEntry
```

### Preconditions

* Free extents are available.
* `allocationMetadata.FindFreeExtent()` returns `ExtentId(5)`.
* Metadata persistence succeeds.

### Execution

```csharp
AllocateExtent(openFileEntry);
```

### Expected Output

* Returns a valid `AllocatedExtent` with `ExtentId = 5`.

### Expected State

* Extent `5` is marked as allocated in-memory.
* Metadata counters are updated and persisted.

### Expected Dependency Calls

```text
allocationMetadata.FindFreeExtent()
    -> returns ExtentId(5)

allocationMetadata.MarkExtentAllocated(ExtentId(5))
    -> succeeds (verifies the exact ID found is passed to the allocator)

FileWriter.WriteAllocationMetadata(handle, header, metadata)
    -> succeeds
```

### Suggested Test Name

```csharp
AllocateExtent_ValidExecution_PassesFoundIdToMarkAllocated()
```

---

## Case 7: Persist Metadata Failed during Allocation

### Preconditions

* Metadata write throws `IOException`.

### Execution

```csharp
AllocateExtent(openFileEntry);
```

### Expected Output

```text
Throws ExtentAllocationException
```

### Suggested Test Name

```csharp
AllocateExtent_PersistenceFails_ThrowsExtentAllocationException()
```

---

## Case 8: Rollback Allocation State when Persist Fails

### Description

Verifies that if writing metadata fails, the in-memory bitmap state changes are rolled back to free.

### Expected Dependency Calls

```text
FileWriter.WriteAllocationMetadata(...)
    -> throws IOException

allocationMetadata.MarkExtentFree(allocatedExtentId)
    -> succeeds
```

### Suggested Test Name

```csharp
AllocateExtent_PersistenceFails_RollsBackBitmapState()
```

---

## Case 9: Free Extent Successfully

### Input

```text
entry    = openFileEntry
extentId = ExtentId(3)
```

### Preconditions

* Extent `3` is currently marked as allocated.
* Metadata persistence succeeds.

### Execution

```csharp
FreeExtent(openFileEntry, ExtentId(3));
```

### Expected Output

* Void return (Success).

### Expected State

* Extent `3` is set to free in-memory.
* Counters incremented and metadata persisted to disk.

### Expected Dependency Calls

```text
allocationMetadata.MarkExtentFree(ExtentId(3))
    -> succeeds

FileWriter.WriteAllocationMetadata(handle, header, metadata)
    -> succeeds
```

### Suggested Test Name

```csharp
FreeExtent_AllocatedExtent_FreesAndPersists()
```

---

## Case 10: Free Non-Existent Extent

### Input

```text
extentId = ExtentId(99)
```

### Preconditions

* Total extent count in file is 16.

### Execution

```csharp
FreeExtent(openFileEntry, ExtentId(99));
```

### Expected Output

```text
Throws InvalidExtentException
```

### Suggested Test Name

```csharp
FreeExtent_NonExistentExtent_ThrowsInvalidExtentException()
```

---

## Case 11: Free Already Free Extent Rejection

### Preconditions

* Extent `3` is already marked as free.

### Execution

```csharp
FreeExtent(openFileEntry, ExtentId(3));
```

### Expected Output

```text
Throws ExtentAlreadyFreeException
```

### Suggested Test Name

```csharp
FreeExtent_AlreadyFreeExtent_ThrowsExtentAlreadyFreeException()
```

---

## Case 12: Blocked by Active Extent Usage

### Preconditions

* `ExtentUsageTracker` reports the extent is locked.

### Execution

```csharp
FreeExtent(openFileEntry, ExtentId(3));
```

### Expected Output

```text
Throws ExtentInUseException
```

### Suggested Test Name

```csharp
FreeExtent_ExtentInUse_ThrowsExtentInUseException()
```

---

## Case 13: Rollback Free State when Persist Fails

### Description

Verifies that if writing metadata fails during deallocation, the in-memory bitmap state changes are rolled back to allocated.

### Expected Dependency Calls

```text
FileWriter.WriteAllocationMetadata(...)
    -> throws IOException

allocationMetadata.MarkExtentAllocated(extentId)
    -> succeeds
```

### Suggested Test Name

```csharp
FreeExtent_PersistenceFails_RollsBackBitmapState()
```

---

# Summary

| ID | Scenario                        | Expected Result                      |
| -: | ------------------------------- | ------------------------------------ |
|  1 | Allocate empty extent success   | Allocates extent and persists        |
|  2 | Auto-extend allocation          | Extends file size and allocates      |
|  3 | Exhausted extend disabled       | Throws `NoFreeExtentException`       |
|  4 | Exceeds max file cap            | Throws `MaximumFileSizeExceededException`|
|  5 | Resize file fails               | Throws `ExtentAllocationException`   |
|  6 | Direct allocation call check    | Calls `MarkExtentAllocated` properly |
|  7 | Persist allocation fails        | Throws `ExtentAllocationException`   |
|  8 | Persist allocation rollback     | Rolls back in-memory bitmap          |
|  9 | Free extent success             | Frees extent and persists            |
| 10 | Free non-existent index         | Throws `InvalidExtentException`      |
| 11 | Double free rejection           | Throws `ExtentAlreadyFreeException`  |
| 12 | Free blocked by locks           | Throws `ExtentInUseException`        |
| 13 | Persist deallocation rollback   | Rolls back bitmap to allocated       |

## Total

```text
13 independent unit test behaviors
```
