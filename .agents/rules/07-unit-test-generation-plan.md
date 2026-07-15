---
trigger: manual
---

# Unit Test Documentation Rule

## 1. Activation

Apply this rule only during the Test Documentation phase.

Do not apply it automatically during:

- Feature breakdown.
- Class design.
- Sequence-diagram design.
- Production-code implementation.
- Test-code implementation.

This rule governs test documentation only.

---

## 2. Objective

Create or refactor professional unit test specifications from approved
design artifacts.

The output must describe test behavior.

Do not generate test code unless the user explicitly starts the Test
Implementation phase.

---

## 3. Source of Truth

Use approved artifacts in this priority order:

1. Latest approved complete class diagram.
2. Approved sequence and state diagrams that are consistent with the
   class diagram.
3. Public interface contracts.
4. Approved domain invariants and file-format rules.
5. Existing test documentation.

Existing test documentation is not a design source of truth.

When two approved design artifacts conflict:

1. Identify the exact conflict.
2. Identify the affected methods and test documents.
3. Do not choose one behavior silently.
4. Do not update test specifications based on an assumption.
5. Report:

   `Specification Gap — Design clarification required.`

6. Stop before generating or modifying affected test specifications.

---

## 4. Unit Test Scope

Evaluate every public method for test coverage.

This does not mean:

`One public method = one test case.`

A public method may require separate cases for:

- Successful behavior.
- Alternative valid behavior.
- Invalid input.
- Invalid state.
- Boundary conditions.
- Dependency failures.
- Cleanup or compensation.
- State consistency.
- Prohibited dependency interactions.

Generate only categories that apply to the approved contract.

---

## 5. File Management Units

Generate direct unit test specifications for behavior owned by:

### FileLifecycleManager

- CreateFile
- OpenFile
- CloseFile
- DeleteFile
- ResizeFile

### FileValidator

- Validate

### FileReader

- ReadAtOffset
- ReadHeader
- ReadAllocationMetadata
- ReadExtentBitmap

### FileWriter

- WriteAtOffset
- WriteHeader
- WriteAllocationMetadata
- WriteExtentBitmap

### FileSynchronizer

- Sync

### OpenFileManager

- GetOpenFile
- RegisterOpenFile
- UnregisterOpenFile
- TryBeginDelete
- CompleteDelete
- CancelDelete

### ExtentManager

- AllocateExtent
- FreeExtent

Create tests for domain entities and value objects only when they own
validation, invariants, calculations, equality, or state transitions.

---

## 6. Excluded Direct Unit Tests

Do not create direct unit test specifications for:

- Interfaces.
- Private methods.
- Private fields.
- Auto-properties.
- Data-only DTOs.
- Enums.
- Third-party library behavior.
- .NET framework behavior.

Do not directly test:

- IFileLifecycleManager
- IFileValidator
- IFileReader
- IFileWriter
- IFileSynchronizer
- IOpenFileManager
- IPhysicalFileSystem
- IExtentManager

Private validation behavior must be tested through the public method that
owns it.

---

## 7. Physical Storage Boundary

The concrete PhysicalFileSystem belongs to integration testing.

Unit tests must not access:

- Real files.
- Real directories.
- Real operating-system handles.
- Network resources.
- Shared external state.

Use IPhysicalFileSystem as a test-double boundary when testing
FileLifecycleManager.

Do not test .NET FileStream behavior.

Test whether the application invokes the physical-storage boundary
according to the approved contract.

---

## 8. Behavior-Oriented Testing

Each test case must verify one observable behavior.

Observable results include:

- Returned value.
- Returned object properties.
- Publicly observable state.
- Defined exception or error result.
- Required external side effect.
- Required cleanup.
- Prevention of an invalid external action.

Do not test:

- Internal variable values.
- Internal helper calls.
- Private algorithms.
- Unapproved call order.
- Object construction details that are not part of the contract.

Verify dependency call order only when order is required by:

- Durability.
- Resource ownership.
- Atomicity.
- Cleanup.
- An explicitly approved sequence.

---

## 9. Dependency Interactions

Document a dependency call only when it represents:

- A required external side effect.
- Persistence.
- Resource acquisition or release.
- Cleanup or compensation.
- A contract-relevant decision.
- Prevention of an invalid operation.

For relevant dependency calls, document:

- Dependency.
- Operation.
- Expected arguments.
- Expected call count.
- Required order, when contract-relevant.

Also document prohibited calls when an operation must not occur.

Do not verify every getter, lookup, or internal collaboration
automatically.

---

## 10. Failure Behavior

For every meaningful dependency failure, define:

- Expected output or exception.
- Expected final state.
- Calls that must stop.
- Resources that must be released.
- Registrations or reservations that must be removed.
- Persisted state that must remain unchanged.
- Required compensation defined by the approved design.

Do not invent rollback or compensation behavior.

When cleanup behavior is missing from the approved design, report a
specification gap.

---

## 11. Test Specification Content

Each public behavior specification must contain:

1. Document information.
2. Purpose.
3. Unit under test.
4. Dependencies.
5. Behavioral rules.
6. Test case summary.
7. Detailed test cases.
8. Coverage review.
9. Specification gaps.
10. Review checklist.

Each detailed test case must define:

- Test case ID.
- Objective.
- Requirement or behavior references.
- Priority.
- Category.
- Input.
- Preconditions.
- Dependency setup.
- Execution.
- Expected output.
- Expected state.
- Expected dependency calls.
- Prohibited dependency calls.
- Expected failure handling.
- Cleanup.

Use conceptual terms such as Stub, Mock, Fake, and Spy.

Do not include syntax from Moq, NSubstitute, FakeItEasy, or another test
framework in documentation.

---

## 12. Refactoring Existing Documentation

When refactoring existing test documentation:

1. Discover the current test-documentation root.
2. Do not create a parallel folder tree without approval.
3. Inventory all existing File Management test documents.
4. Map each document and test case to the current public behavior.
5. Preserve valid test cases.
6. Update stale method signatures and dependencies.
7. Merge duplicate specifications.
8. Split documents that contain unrelated public behaviors.
9. Remove obsolete implementation-detail tests.
10. Move real file-system scenarios to integration specifications.
11. Update README and traceability links.
12. Use only relative repository links.

Do not silently delete a document.

Report every document that was:

- Kept.
- Updated.
- Merged.
- Split.
- Moved.
- Replaced.
- Marked obsolete.

---

## 13. Recommended Grouping

Use the existing approved documentation root and organize specifications
conceptually as:

```text
file-management/
├── README.md
├── file-lifecycle/
│   ├── create-file.md
│   ├── open-file.md
│   ├── close-file.md
│   ├── delete-file.md
│   └── resize-file.md
├── file-validation/
│   └── validate-file.md
├── file-io/
│   ├── read-at-offset.md
│   ├── read-file-structures.md
│   ├── write-at-offset.md
│   ├── write-file-structures.md
│   └── sync-file.md
├── runtime-file-management/
│   ├── open-file-registry.md
│   └── delete-coordination.md
└── extent-management/
    ├── allocate-extent.md
    └── free-extent.md