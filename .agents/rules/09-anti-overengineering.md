---
trigger: manual
---

# Anti Over Engineering

Always keep the design simple.

Follow

- SOLID
- KISS
- YAGNI

Never introduce

- Interface for every class
- Factory without creation complexity
- Strategy with one algorithm
- Adapter without incompatible interface
- Facade for simple subsystems
- Repository without persistence
- Generic abstraction without multiple implementations

Always ask

Is this abstraction solving a real problem?

If not

Do not create it.

Prefer composition over inheritance.

Design Patterns are optional.

Business requirements are mandatory.