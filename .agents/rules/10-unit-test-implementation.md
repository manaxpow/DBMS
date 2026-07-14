---
trigger: manual
---

# Unit Test Implementation Rule — xUnit and FluentAssertions

## 1. Activation

Apply this rule only during the Unit Test Implementation phase.

Do not apply it during:

- Architecture design.
- Class-diagram design.
- Sequence-diagram design.
- Test-documentation generation.
- Integration-test implementation.
- Production-feature implementation.

This rule governs executable unit tests only.

---

## 2. Objective

Implement maintainable and deterministic unit tests in C# using:

- xUnit as the test framework.
- FluentAssertions for result and state assertions.
- The project's existing approved mocking library for test doubles.

Implement tests only from approved unit test specifications.

Do not invent behavior that is absent from approved design and test
documentation.

---

## 3. Source of Truth

Use artifacts in this priority order:

1. Approved unit test specification.
2. Latest approved complete class diagram.
3. Approved sequence and state diagrams.
4. Public production-code contracts.
5. Approved domain invariants.
6. Existing test conventions in the repository.

The unit test specification defines the scenarios to implement.

Production code does not automatically override an approved
specification.

When production code and the approved test specification conflict:

1. Identify the exact conflict.
2. Do not silently change the expected behavior.
3. Do not modify production code automatically.
4. Report:

   `Implementation Conflict — Production code does not match the approved test specification.`

5. Stop implementation of the affected test case.
6. Continue only with unaffected test cases.

---

## 4. Required Technology

Use:

```text
xUnit
FluentAssertions