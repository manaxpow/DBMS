---
trigger: always_on
---

# Core Workflow

## Purpose

This project follows a strict iterative design workflow.

The objective is to design one feature completely before moving to implementation.

Every phase must be reviewed and approved before continuing.

---

# Workflow

```
Existing Feature Mindmap
        ↓
1. Detailed Feature Breakdown
        ↓
Review
        ↓
2. Class Mindmap
        ↓
Review
        ↓
3. Simple Class Diagram
        ↓
Review
        ↓
4. Sequence Diagrams
        ↓
Review
        ↓
5. Complete Class Diagram
        ↓
Review
        ↓
6. Test Documentation
        ↓
Review
        ↓
7. Test Implementation
        ↓
Review
        ↓
8. Feature Implementation
```

---

# General Rules

- Work on exactly **one phase** at a time.
- Never skip a phase.
- Never combine multiple phases into one response.
- Never continue automatically.
- Stop after finishing the current phase.
- Wait for explicit user approval before continuing.

---

# Review Gate

After every phase, the response **must end** with:

```
Review Summary

Completed:
Assumptions:
Potential Issues:
Items To Review:
Recommended Changes:
Next Phase:

Status:
Waiting for review.
```

Do not continue unless the user explicitly requests the next phase.

---

# Change Management

If a previous phase changes, update every affected downstream phase.

For example:

```
Feature Breakdown
    ↓
Class Mindmap
    ↓
Simple Class Diagram
    ↓
Sequence Diagram
    ↓
Complete Class Diagram
    ↓
Test Documentation
    ↓
Implementation
```

Never modify implementation without updating the corresponding design artifacts.

---

# Phase Responsibility

Each phase has its own rule file.

This file only defines the workflow.

Do not apply rules from another phase unless that phase is currently active.

---

# Current Phase

Always determine the current phase before answering.

The response must begin with:

```
Current Phase:
Current Feature:
Current Scope:
```

---

# Scope Control

Only generate artifacts that belong to the current phase.

Examples:

Detailed Feature Breakdown

✔ responsibilities

✔ use cases

✘ classes

✘ methods

✘ diagrams

---

Class Mindmap

✔ classes

✔ interfaces

✔ responsibilities

✘ methods

✘ properties

---

Simple Class Diagram

✔ class names

✔ relationships

✘ methods

✘ properties

---

Sequence Diagram

✔ runtime interactions

✔ message flow

✔ method candidates

✘ implementation

---

Complete Class Diagram

✔ methods

✔ properties

✔ access modifiers

---

Test Documentation

✔ test specification

✘ test code

---

Test Implementation

✔ unit tests

✔ integration tests

✘ production code

---

Feature Implementation

✔ production code

✔ follow approved design

---

# Design Principles

Always follow:

- SOLID
- KISS
- YAGNI
- Composition over Inheritance

Avoid over-engineering.

Do not introduce Design Patterns unless there is a real problem that requires them.

---

# End Condition

Every response must finish with:

```
Status:
Waiting for review.
```

Never continue to the next phase automatically.