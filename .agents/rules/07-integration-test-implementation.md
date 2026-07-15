---
trigger: manual
---

# Integration Test Implementation

## 1. Objective

Implement executable integration tests from approved integration-test
specifications.

Integration tests verify that concrete components cooperate correctly
with real infrastructure or real system boundaries.

Do not implement or modify production behavior during this phase.

---

## 2. Technology

Use the technologies already approved by the repository:

- .NET
- xUnit
- FluentAssertions

Use NSubstitute only for dependencies that are outside the integration
boundary being tested.

Do not mock the real infrastructure or concrete boundary that the
integration test is intended to verify.

---

## 3. Source of Truth

Use artifacts in this priority order:

1. Approved integration-test specification.
2. Approved architecture diagrams.
3. Approved sequence and state diagrams.
4. Public production contracts.
5. Existing integration-test conventions.

Do not implement tests from specifications marked:

- Draft
- Blocked
- Specification Gap
- Design Clarification Required
- Obsolete

Do not invent missing behavior.

Report unresolved behavior as:

`Specification Gap — Design clarification required.`

---

## 4. Test Traceability

The required mapping is:

```text
One approved Test Case ID
↓
One executable integration test