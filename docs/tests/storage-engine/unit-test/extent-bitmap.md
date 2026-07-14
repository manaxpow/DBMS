# Unit Test Specification — `ExtentBitmap`

## 1. Scope

This document covers unit test specifications for `ExtentBitmap`.
Tests are isolated from all external dependencies.
Only public method behavior is verified.

## 2. Component

* **Class:** `ExtentBitmap`
* **Methods:**

```csharp
static ExtentBitmap Create(int totalExtents);
int? FindFirstFree();
bool IsAllocated(int index);
void MarkUsed(int index);
void MarkFree(int index);
void AppendFreeExtents(int count);
void Truncate(int newTotalExtents);
```

## 3. Unit Under Test

`ExtentBitmap` is responsible for:

* Allocating and parsing in-memory byte segments tracking database space allocations.
* Scanning bit values to return the first available zero index.
* Flipping bits from 0 to 1 (`MarkUsed`) and from 1 to 0 (`MarkFree`).
* Validating bitwise operations against double-marks (e.g. double-free, double-allocations).
* Rejecting negative or out-of-bounds indices.
* Extending bitmap capacities (`AppendFreeExtents`).
* Truncating capacities (`Truncate`) and blocking contractions if the tail region holds active allocations.
* Managing multi-byte bit shifting arithmetic.

## 4. Dependencies (None)

None. Tested as a pure in-memory utility representation.

---

# Test Cases

## Case 1: Create Bitmap with All Extents Free

### Input

```text
totalExtents = 16
```

### Execution

```csharp
Create(16);
```

### Expected Output

* Returns a valid `ExtentBitmap` reference.

### Expected State

* All 16 bits are set to free (0).

### Suggested Test Name

```csharp
Create_ValidCapacity_InitializesAllBitsAsFree()
```

---

## Case 2: FindFirstFree Scan

### Preconditions

* Bitmap bits are `1101` (binary indices: 0, 1, 3 are used; index 2 is free).

### Execution

```csharp
FindFirstFree();
```

### Expected Output

* Returns `2`.

### Suggested Test Name

```csharp
FindFirstFree_FreeBitExists_ReturnsFirstZeroBitIndex()
```

---

## Case 3: MarkUsed

### Input

```text
index = 2
```

### Preconditions

* Bit at index 2 is free (0).

### Execution

```csharp
MarkUsed(2);
```

### Expected State

* Bit at index 2 transitions to used (1).

### Suggested Test Name

```csharp
MarkUsed_FreeBit_SetsBitToOne()
```

---

## Case 4: MarkFree

### Input

```text
index = 2
```

### Preconditions

* Bit at index 2 is used (1).

### Execution

```csharp
MarkFree(2);
```

### Expected State

* Bit at index 2 transitions to free (0).

### Suggested Test Name

```csharp
MarkFree_UsedBit_SetsBitToZero()
```

---

## Case 5: MarkUsed Twice Safety

### Input

```text
index = 2
```

### Preconditions

* Bit at index 2 is already used (1).

### Execution

```csharp
MarkUsed(2);
```

### Expected Output

```text
Throws InvalidOperationException
```

### Suggested Test Name

```csharp
MarkUsed_AlreadyUsed_ThrowsInvalidOperationException()
```

---

## Case 6: MarkFree Twice Safety

### Input

```text
index = 2
```

### Preconditions

* Bit at index 2 is already free (0).

### Execution

```csharp
MarkFree(2);
```

### Expected Output

```text
Throws InvalidOperationException
```

### Suggested Test Name

```csharp
MarkFree_AlreadyFree_ThrowsInvalidOperationException()
```

---

## Case 7: Negative Index Rejection

### Input

```text
index = -1
```

### Execution

```csharp
MarkUsed(-1);
```

### Expected Output

```text
Throws ArgumentOutOfRangeException
```

### Suggested Test Name

```csharp
MarkUsed_NegativeIndex_ThrowsArgumentOutOfRangeException()
```

---

## Case 8: Index Out of Range Rejection

### Input

```text
index = 16
```

### Preconditions

* Bitmap capacity is 16.

### Execution

```csharp
MarkUsed(16);
```

### Expected Output

```text
Throws ArgumentOutOfRangeException
```

### Suggested Test Name

```csharp
MarkUsed_IndexExceedsCapacity_ThrowsArgumentOutOfRangeException()
```

---

## Case 9: AppendFreeExtents Growth

### Input

```text
count = 8
```

### Preconditions

* Bitmap size is 16.

### Execution

```csharp
AppendFreeExtents(8);
```

### Expected State

* Capacity grows to 24 bits.
* New bits are initialized as free (0).

### Suggested Test Name

```csharp
AppendFreeExtents_ValidCount_GrowsBitmapSize()
```

---

## Case 10: Truncate Bitmap Size

### Input

```text
newTotalExtents = 8
```

### Preconditions

* Bitmap size is 16.
* Lower 8 indices are unmodified.

### Execution

```csharp
Truncate(8);
```

### Expected State

* Capacity is truncated to 8 bits.

### Suggested Test Name

```csharp
Truncate_ValidShrink_ShrinksCapacity()
```

---

## Case 11: Do Not Truncate Allocated Extents

### Input

```text
newTotalExtents = 8
```

### Preconditions

* Bitmap size is 16.
* Bit at index 12 is used (1).

### Execution

```csharp
Truncate(8);
```

### Expected Output

```text
Throws InvalidOperationException
```

### Suggested Test Name

```csharp
Truncate_ShrinkAreaContainsUsedBits_ThrowsInvalidOperationException()
```

---

## Case 12: Bitmap Spans Multiple Bytes

### Description

Asserts that bit shifting logic calculates correctly when indexes span across byte boundaries (e.g. index 13 sits in the second byte).

### Execution

```csharp
MarkUsed(13);
```

### Expected State

* Second byte of internal bitmap array has matching bit flag set.

### Suggested Test Name

```csharp
MarkUsed_MultiByteIndex_SetsCorrectOffsetFlag()
```

---

# Summary

| ID | Scenario                        | Expected Result                      |
| -: | ------------------------------- | ------------------------------------ |
|  1 | Create bitmap                   | Initializes all bits as free         |
|  2 | Scan first free                 | Returns correct zero index           |
|  3 | Mark used                       | Flips bit to 1                       |
|  4 | Mark free                       | Flips bit to 0                       |
|  5 | Duplicate mark used             | Throws `InvalidOperationException`   |
|  6 | Duplicate mark free             | Throws `InvalidOperationException`   |
|  7 | Negative index                  | Throws `ArgumentOutOfRangeException` |
|  8 | Index exceeds capacity          | Throws `ArgumentOutOfRangeException` |
|  9 | Append capacity                 | Grows capacity, sets new bits free   |
| 10 | Truncate size                   | Contracts capacity                   |
| 11 | Truncate allocated segments     | Throws `InvalidOperationException`   |
| 12 | Check byte boundaries math      | Flips correct offsets in array bytes |

## Total

```text
12 independent unit test behaviors
```
