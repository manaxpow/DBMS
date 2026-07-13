# Sequence Diagrams - File Management

This folder contains the Sequence Diagrams for the File Management component under the Storage Engine, organized by use cases:

1. [Create File](file:///c:/Users/ADMIN/Desktop/DBMS/docs/diagrams/sequence-diagrams/storage-engine/file-management/file-lifecycle/create-file.md)
   - Happy and failure paths for file creation on disk.
2. [Delete File](file:///c:/Users/ADMIN/Desktop/DBMS/docs/diagrams/sequence-diagrams/storage-engine/file-management/file-lifecycle/delete-file.md)
   - Checks lock states and performs safe deletion.
3. [Open File](file:///c:/Users/ADMIN/Desktop/DBMS/docs/diagrams/sequence-diagrams/storage-engine/file-management/file-lifecycle/open-file.md)
   - Opens handles, reads/formats headers, and manages open files.
4. [Close File](file:///c:/Users/ADMIN/Desktop/DBMS/docs/diagrams/sequence-diagrams/storage-engine/file-management/file-lifecycle/close-file.md)
   - Flushes modifications and safely releases raw handle.
5. [Read File](file:///c:/Users/ADMIN/Desktop/DBMS/docs/diagrams/sequence-diagrams/storage-engine/file-management/file-io/read-file.md)
   - Reads block-level data offset calculations.
6. [Write File](file:///c:/Users/ADMIN/Desktop/DBMS/docs/diagrams/sequence-diagrams/storage-engine/file-management/file-io/write-file.md)
   - Writes block-level data offsets.
7. [Sync File](file:///c:/Users/ADMIN/Desktop/DBMS/docs/diagrams/sequence-diagrams/storage-engine/file-management/file-io/sync-file.md)
   - Flushes and forces write buffers to disk to guarantee ACID durability.
8. [Resize File](file:///c:/Users/ADMIN/Desktop/DBMS/docs/diagrams/sequence-diagrams/storage-engine/file-management/file-lifecycle/resize-file.md)
   - Extends or truncates the physical database files safely.
9. [Validate File](file:///c:/Users/ADMIN/Desktop/DBMS/docs/diagrams/sequence-diagrams/storage-engine/file-management/file-lifecycle/validate-file.md)
   - Verifies format, structure, and integrity before registration.
10. [Allocate Extent](file:///c:/Users/ADMIN/Desktop/DBMS/docs/diagrams/sequence-diagrams/storage-engine/file-management/extent-management/allocate-extent.md)
    - Allocates free space inside the database data files.
11. [Free Extent](file:///c:/Users/ADMIN/Desktop/DBMS/docs/diagrams/sequence-diagrams/storage-engine/file-management/extent-management/free-extent.md)
    - Releases allocated extents back to free space.
