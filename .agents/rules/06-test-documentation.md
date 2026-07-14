---
trigger: manual
---

# Test Documentation Rule

## 1. Objective

Create professional test specifications for the approved system design.

The output must describe **what must be tested**, not provide executable test code.

---

## 2. Input

Use only the following approved artifacts:

* Complete Class Diagram
* Public method signatures
* Defined responsibilities
* Relationships between classes
* Declared domain rules and exceptions
* Approved sequence diagrams, when available

Do not invent responsibilities, methods, states, dependencies, or exception types that are not present in the approved design.

If required information is missing or contradictory, stop and report it under **Design Issues Requiring Review**.

---

## 3. Testing Scope

Create test specifications for:

1. Unit Tests
2. Integration Tests

Only document behaviors that belong to the selected component or feature.

---

# Part A — Unit Test Specification

## 4. Unit Under Test

For each tested class or public method, identify:

* Class or interface
* Public method signature
* Primary responsibility
* Direct dependencies
* Observable state changes
* Possible outputs and exceptions

---

## 5. Behavior Coverage

Define test cases for every meaningful public behavior, including:

* Successful execution
* Alternative valid paths
* Boundary conditions
* Invalid input
* Domain rule violations
* Dependency failures
* State transition failures
* Resource cleanup
* Error recovery

Do not create separate test cases for statements that belong to the same behavioral path.

Each test case must represent one distinct behavior.

Avoid duplicated scenarios that use the same input, preconditions, execution path, and expected result.

---

## 6. Unit Test Case Format

Each unit test case must contain the following sections.

### Case ID and Title

Use a clear title describing:

```text
Method_WhenCondition_ExpectedBehavior
```

Example:

```text
OpenFile_WhenFileDoesNotExist_ThrowsFileNotFoundException
```

### Purpose

Briefly explain the behavior being verified.

### Input

List all input values passed to the unit under test.

### Preconditions

Describe the state that must exist before execution, including:

* Existing object state
* Dependency responses
* Registered resources
* Lock state
* File state
* Required domain conditions

### Execution

Describe the single public operation being invoked.

Do not include internal method calls as execution steps.

### Expected Output

Specify the observable result:

* Returned value
* Returned object
* Exception type
* Void success

When an exception wraps another exception, document both:

* Outer exception
* Root cause

### Expected State

Describe observable state after execution:

* Updated reference count
* Registered or removed object
* Changed status
* Persisted data
* Released resource
* Unchanged state after failure

Use `Not applicable` when the behavior does not change state.

### Expected Dependency Calls

Specify only interactions that are part of the component contract or are necessary to verify orchestration.

For each dependency call, define:

* Dependency
* Operation
* Important arguments
* Expected return value or exception

### Forbidden Dependency Calls

List dependency operations that must not occur after an early return, validation failure, or exception.

Include this section only when it provides meaningful behavioral verification.

### Expected Call Order

Document call order only when order affects correctness, consistency, durability, locking, or resource safety.

Do not verify order merely because the current implementation happens to use that order.

### Cleanup and Resource Safety

Specify required cleanup when the operation fails after acquiring a resource.

Examples:

* Close an opened file handle
* Release a deletion marker
* Roll back temporary registration
* Dispose a transaction
* Leave persisted state unchanged

---

## 7. Unit Test Rules

Unit tests must:

* Test public behavior
* Treat the class as a black box
* Isolate external dependencies
* Use mocks, stubs, or fakes only for direct dependencies
* Verify observable outputs and state
* Verify important dependency interactions
* Remain independent from other test cases
* Use deterministic input and expected results
* Cover success, failure, and boundary paths
* Use one primary reason for failure per test case

Unit tests must not:

* Test private methods directly
* Test local variables
* Test internal branching structure
* Assert implementation-specific object creation
* Assert unnecessary call counts
* Assert incidental call order
* Duplicate another test behavior
* Test behavior owned by a dependency
* Assume undeclared exception types
* Include executable test code

---

## 8. Unit Test Summary

After all unit test cases, provide a summary table:

| ID | Behavior    | Expected Result             |
| -: | ----------- | --------------------------- |
|  1 | Description | Returned value or exception |

Then provide:

```text
Total: X independent unit test behaviors
```

The total must count only independent behavioral paths.

---

# Part B — Integration Test Specification

## 9. Integration Test Scope

Integration tests must verify collaboration between real components.

Use real implementations for the components being integrated.

Mock only external systems that are outside the selected integration boundary.

Examples of valid integration boundaries:

* Lifecycle manager and physical file system
* File reader and real file handle
* File writer and synchronization mechanism
* Storage engine and buffer manager
* Repository and database
* API endpoint and application service

---

## 10. Integration Test Case Format

Each integration test case must contain the following sections.

### Scenario ID and Title

Describe the integrated workflow and expected result.

### Purpose

Explain which component collaboration is being verified.

### Environment

Define the required environment:

* Operating system assumptions
* Temporary directory
* Real file system
* Test database
* Configuration
* Required services
* Initial data
* File format version
* Page size or storage settings

### Initial State

Describe all files, records, registrations, locks, or database state that must exist before execution.

### Scenario

Describe the user-visible or system-visible workflow.

Use public APIs only.

### Expected Result

Specify observable results across component boundaries:

* Persisted data
* Created or deleted file
* Reconstructed domain object
* Correct file content
* Correct registry state
* Correct exception propagation
* Resource release
* Data durability

### Verification

Describe how the expected result is confirmed.

Examples:

* Reopen the file and read persisted data
* Inspect the physical file
* Query the database
* Verify that a handle can be reopened
* Confirm that no temporary files remain

### Cleanup

Define how the test environment is restored:

* Close handles
* Delete temporary files
* Remove test directories
* Roll back database data
* Stop temporary services
* Clear registries

Cleanup must run even when the test fails.

---

## 11. Integration Test Rules

Integration tests must:

* Test real collaboration between components
* Use public entry points
* Verify persisted or externally observable results
* Use an isolated test environment
* Avoid shared mutable state between scenarios
* Define deterministic setup and cleanup
* Cover important end-to-end failure paths
* Verify resource release after failure

Integration tests must not:

* Repeat unit test mocking strategies
* Verify private implementation details
* Depend on execution order
* Depend on existing developer-machine files
* Use production data
* Leave files, handles, records, or locks after completion
* Include executable test code

---

# Part C — Quality Review

## 12. Test Documentation Validation

Before completing the document, verify that:

* Every public behavior has been considered
* Every test case represents a distinct behavior
* No two test cases duplicate the same path
* Expected outputs match declared method contracts
* Expected exceptions exist in the approved design
* Dependency calls use declared dependencies only
* State changes are observable and testable
* Cleanup is defined for acquired resources
* Unit tests do not test dependency-owned behavior
* Integration tests use an explicit integration boundary
* Success, failure, and boundary paths are covered
* Test totals match the actual number of independent behaviors

---

## 13. Design Issues Requiring Review

When the approved design is insufficient, list issues such as:

* Missing exception type
* Undefined lock compatibility rule
* Undefined cleanup responsibility
* Missing dependency operation
* Unknown source of required data
* Contradictory class responsibilities
* Ambiguous state transition
* Operation that can fail without a defined recovery path

Do not silently resolve architectural ambiguity.

Do not invent missing behavior.

---

## 14. Required Output Structure

Use the following document structure:

```text
Test Specification — Component or Feature Name

1. Scope
2. Component
3. Unit Under Test
4. Dependencies
5. Unit Test Cases
6. Unit Test Summary
7. Integration Test Environment
8. Integration Test Scenarios
9. Integration Test Summary
10. Design Issues Requiring Review
```

Use consistent terminology and professional technical language throughout the document.

---

## 15. Final Rule

Produce test documentation only.

Do not write:

* Production code
* Test implementation code
* Mock setup code
* Framework-specific syntax

After completing the test specification, stop and wait for review.

Do not proceed to implementation until the document is explicitly approved.
