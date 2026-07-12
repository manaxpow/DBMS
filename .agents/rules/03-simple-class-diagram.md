---
trigger: manual
---

# Simple Class Diagram

## Objective

Convert the approved Class Mindmap into one or more **Simple Class Diagrams**.

The purpose of this phase is to validate the architecture and responsibilities, **not** the implementation.

---

## Include

- Class names
- Interface names
- Enum names
- Structural relationships only
- Multiplicity (only when it adds value)

---

## Exclude

- Methods
- Properties
- Constructors
- Value Objects
- Access modifiers
- Design Patterns
- Behavior details
- Runtime interactions

---

## Allowed Relationships

Only use:

- Association
- Dependency
- Aggregation
- Composition
- Generalization (Inheritance)
- Interface Realization

---

## Relationship Rules

Only include **structural relationships**.

Do **NOT** include relationships that only exist because of runtime behavior.

### Keep

- owns
- contains
- references
- implements
- inherits
- aggregates
- composes

### Do NOT keep

- uses
- reads
- writes
- flushes
- syncs
- allocates
- validates
- updates
- tracks
- coordinates

Those interactions belong to **Sequence Diagrams**, not Simple Class Diagrams.

---

## Diagram Scope

One diagram should answer **one architectural question**.

If the feature is large, split it into multiple diagrams.

Example:

- Overview Diagram
- Service Diagram
- Domain Diagram
- Runtime Management Diagram

Do NOT force every class into a single diagram.

---

## Missing Responsibility

Never invent a class.

If the mindmap cannot satisfy a relationship or responsibility, stop immediately and return:

```text
Missing Responsibility

Reason:
...

Suggested Missing Class:
...
```

Do not continue generating the diagram.

---

## Relationship Explanation

After every diagram explain only:

- Why Composition is used.
- Why Aggregation is used.
- Why Inheritance is used.
- Why Interface Realization is used.

Do **NOT** explain every dependency.

---

## Output

For every generated diagram:

### Diagram Name

### Purpose

### Mermaid classDiagram

### Relationship Explanation

---

## End

Wait for review before generating another diagram.