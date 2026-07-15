# File Management

This directory contains the split Simple Class Diagrams for the File Management component under the Storage Engine, organized by architectural concerns:

1. [Service Architecture Diagram](./service-architecture.md)
   - Defines the service interfaces, their concrete implementations, and their structural associations.
2. [File Domain Model Diagram](./domain-model.md)
   - Defines the static layout and structural compositions of the database files, headers, and allocations.
3. [Runtime File Management Diagram](./runtime-management.md)
   - Explains the tracking of open files and how file entries own OS handles.
4. [Extent Management Diagram](./extent-management.md)
   - Defines the interface and implementation class for extent-based space allocation.
5. [Service Architecture Overview Diagram](./service-architecture-overview.md)
   - High-level view of service interfaces, concrete implementations, and relationships, omitting methods and properties.
