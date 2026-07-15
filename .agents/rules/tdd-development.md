---
trigger: always_on
---

# TDD Development Rule

## Purpose

Implement production behavior using strict **Test-Driven Development**.

The AI must always follow this cycle:

```text
Red → Green → Refactor
```

Production behavior must not be implemented before a failing test demonstrates the required behavior.

---

## 1. Required Inputs

Before starting implementation, the AI must have:

* An approved class or interface contract.
* An approved test specification or clearly defined behavior.
* Expected inputs, outputs, state changes, and dependency interactions.
* The correct project and namespace structure.

If a required contract or behavior is missing, stop and report the missing information.

Do not invent public methods, properties, return values, exceptions, or business rules.

---

## 2. TDD Workflow

### Phase 1 — Select One Behavior

Choose exactly one observable behavior from the approved test specification.

A behavior may describe:

* A successful operation.
* A validation failure.
* A dependency failure.
* A state transition.
* A cleanup or rollback path.

Do not implement several unrelated behaviors in one TDD cycle.

---

### Phase 2 — Write the Test

Write the smallest test that describes the selected behavior.

Every test must clearly define:

* Arrange: inputs, initial state, and dependency behavior.
* Act: the single operation being tested.
* Assert: output, state changes, exceptions, and dependency calls.

Test names must describe observable behavior.

Recommended naming format:

```text
MethodName_WhenCondition_ShouldExpectedBehavior
```

Example:

```csharp
CreateFile_WhenFileDoesNotExist_ShouldCreateAndRegisterFile()
```

---

### Phase 3 — Confirm the Red State

Run the relevant test.

The test must fail for the expected reason.

Valid Red State examples:

* The required behavior is not implemented.
* The returned result is incorrect.
* The expected state change does not occur.
* The expected dependency call is missing.
* The expected exception is not thrown.

Invalid Red State examples:

* Test project does not compile because of incorrect test code.
* Namespace or project references are broken.
* The test fails because of unrelated infrastructure.
* Mock configuration is invalid.
* The test asserts an incorrect requirement.

Fix invalid test infrastructure failures before continuing.

Do not modify production behavior until a valid Red State is confirmed.

---

### Phase 4 — Implement the Minimum Code

Write only the minimum production code required to make the selected test pass.

Do not:

* Implement future test cases.
* Add speculative abstractions.
* Add design patterns without a current requirement.
* Refactor unrelated code.
* Change approved public contracts.
* Add behavior that is not covered by a test.
* Handle hypothetical edge cases that are not specified.

Existing production stubs may throw:

```csharp
throw new NotImplementedException();
```

Replace only the code required by the current TDD cycle.

---

### Phase 5 — Confirm the Green State

Run:

1. The newly added test.
2. All tests for the affected component.
3. The full relevant test project when practical.

All tests must pass.

Do not weaken, remove, skip, or rewrite an approved assertion merely to make the test pass.

If an existing test fails, determine whether the implementation introduced a regression before continuing.

---

### Phase 6 — Refactor

After all relevant tests are green, improve the implementation without changing observable behavior.

Permitted refactoring includes:

* Removing duplication.
* Improving names.
* Extracting small private methods.
* Simplifying control flow.
* Improving cohesion.
* Reducing coupling.
* Removing dead code.

Refactoring must not:

* Change public behavior.
* Introduce new features.
* Modify approved contracts.
* Add unnecessary patterns or abstractions.
* Make tests depend on implementation details.

Run the relevant tests after every meaningful refactoring step.

---

### Phase 7 — Repeat

Select the next approved behavior and repeat:

```text
Write Test
    ↓
Confirm Red
    ↓
Implement Minimum Code
    ↓
Confirm Green
    ↓
Refactor
```

---

## 3. Test Design Rules

### Test Observable Behavior

Tests may verify:

* Returned values.
* Thrown exceptions.
* Publicly observable state changes.
* Calls to external dependencies.
* Persisted or externally visible results.

Tests must not verify:

* Private methods.
* Local variables.
* Internal control flow.
* Exact implementation structure.
* Unnecessary call ordering.
* Concrete classes when an approved abstraction exists.

---

### One Primary Behavior per Test

Each test should have one primary reason to fail.

A single test may contain multiple assertions when they describe the same behavior.

Example:

```text
Creating a file successfully:
- Returns the created DataFile.
- Registers the open file entry.
- Sets the reference count to one.
```

These assertions may belong to one test because they describe one successful creation behavior.

---

### Dependency Verification

Verify dependency calls only when the interaction is part of the component contract.

For dependency calls, specify:

* Method called.
* Important arguments.
* Expected number of calls.
* Whether a call must not occur.

Avoid verifying irrelevant internal interactions.

---

### Mocking Rules

Mock only dependencies outside the unit under test.

Do not mock:

* The class being tested.
* Plain value objects.
* Simple domain entities without external behavior.
* Internal implementation details.

Use real objects when they are deterministic, lightweight, and do not cross an external boundary.

Common mockable boundaries include:

* File system access.
* Operating system handles.
* Network access.
* System clock.
* Random generators.
* External services.
* Databases in unit tests.

---

## 4. Unit Test Rules

A unit test must:

* Test one public behavior.
* Run deterministically.
* Run independently.
* Avoid real disk, network, database, and operating-system dependencies.
* Use test doubles for external boundaries.
* Verify outputs, state, and required interactions.

A unit test must not depend on execution order or data created by another test.

---

## 5. Integration Test Rules

Integration tests must verify real collaboration across approved boundaries.

Every integration test specification must define:

* Environment.
* Initial state.
* Scenario.
* Expected result.
* Expected persisted state.
* Cleanup procedure.

Integration tests may use:

* Temporary directories.
* Real file streams.
* Test databases.
* Containers.
* Real serialization.
* Real operating-system file operations.

Integration tests must clean up resources even when the test fails.

Use unique test resources to prevent conflicts between parallel test executions.

---

## 6. Contract Bootstrap Rule

TDD requires tests to compile against an approved contract.

When interfaces or concrete production classes do not yet exist, the AI may create compile-safe contract scaffolding before the first behavioral test.

Allowed bootstrap code includes:

* Approved interfaces.
* Approved method signatures.
* Approved constructors.
* Approved properties.
* Empty concrete classes.
* Methods throwing `NotImplementedException`.

Example:

```csharp
public sealed class PhysicalFileSystem : IPhysicalFileSystem
{
    public bool Exists(string fileName)
    {
        throw new NotImplementedException();
    }
}
```

Contract bootstrap code must not contain production behavior.

Creating contract scaffolding is not considered implementing the feature.

After the test project compiles, the AI must continue with the normal Red → Green → Refactor cycle.

---

## 7. Production Code Rules

Production code must:

* Follow the approved class diagram and contracts.
* Preserve dependency direction.
* Use constructor injection for required dependencies.
* Keep external operations behind approved abstractions.
* Handle only specified success and failure paths.
* Avoid unnecessary public members.
* Avoid speculative extensibility.

Do not introduce a design pattern merely because it may be useful later.

A pattern may be introduced only when the current design has a concrete problem that the pattern solves.

---

## 8. Failure Path Rules

Each specified failure path must have its own TDD cycle.

Examples:

* Invalid input.
* File does not exist.
* File already exists.
* Access mode conflict.
* Lock conflict.
* Incomplete read or write.
* Dependency exception.
* Cleanup failure.
* Rollback behavior.

For failure tests, verify:

* The correct exception or failure result.
* Required state remains unchanged.
* Partial work is cleaned up when specified.
* Forbidden dependency calls do not occur.
* Required compensation calls occur.

Do not assume rollback or cleanup behavior unless it is defined in the specification.

---

## 9. Test Modification Rules

An existing approved test may be changed only when:

* The requirement changed.
* The test contradicts the approved contract.
* The test contains a technical error.
* The test verifies an implementation detail.
* The test cannot reliably represent the intended behavior.

When modifying an approved test, explain:

* What was incorrect.
* Which requirement supports the change.
* How the revised test represents the intended behavior.

Never change a test only because implementing the behavior is difficult.

---

## 10. Scope Control

During a TDD task, modify only files required for the current behavior.

Do not:

* Reformat unrelated files.
* Rename unrelated classes.
* Move unrelated folders.
* Upgrade packages without necessity.
* Rewrite entire components.
* Fix unrelated warnings or failures.
* Add undocumented features.

Report unrelated problems separately.

---

## 11. Required AI Report

After each completed TDD cycle, report:

```markdown
## TDD Cycle

### Behavior
The behavior implemented in this cycle.

### Red
- Test added:
- Expected failure:
- Actual failure:

### Green
- Minimum implementation:
- Relevant tests passed:

### Refactor
- Refactoring performed:
- Behavior preserved:

### Files Changed
- `path/to/test-file.cs`
- `path/to/production-file.cs`

### Remaining Behaviors
- Next unimplemented test case.
```

Do not claim a Red or Green state unless the tests were actually executed.

If tests cannot be executed, state that clearly.

---

## 12. Stop Conditions

Stop and request review when:

* The approved contract is ambiguous.
* The test specification contradicts the class diagram.
* Expected behavior is missing.
* A public API change appears necessary.
* A new dependency or architectural boundary is required.
* An existing approved test conflicts with another approved test.
* The implementation would require behavior outside the current scope.

Do not silently make architectural decisions.

---

## 13. Completion Criteria

A behavior is complete only when:

* Its test exists.
* The test was observed in a valid Red State.
* Minimum production code was implemented.
* The test passes.
* Relevant regression tests pass.
* Refactoring preserves the Green State.
* No unrelated behavior was added.
* Documentation and contracts remain synchronized.

A feature is complete only when all approved behaviors have completed the full TDD cycle.

---

## Core Enforcement Rule

```text
No production behavior without a failing test.

No refactoring while tests are red.

No new behavior during refactoring.

No test weakening to force a green result.

No invented requirements.

One observable behavior per TDD cycle.
```
