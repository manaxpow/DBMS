# Storage Engine

```text
Storage Engine
├── File Management
│   ├── File Allocator
│   │   ├── Create File
│   │   ├── Delete File
│   │   ├── Open File
│   │   ├── Close File
│   │   ├── Allocate Space
│   │   └── Resize File
│   │
│   ├── Extent Management
│   │
│   └── Data File
│       ├── File Read
│       ├── File Write
│       ├── File Flush
│       ├── File Sync
│       └── File Metadata
│
├── Page Management
│   ├── Page Allocator
│   │   ├── Allocate Page
│   │   ├── Deallocate Page
│   │   ├── Reuse Free Page
│   │   ├── Allocate Extent
│   │   └── Page Mapping
│   │
│   ├── Page Formatter
│   │
│   └── Free Space Manager
│       ├── Free Page Tracking
│       ├── Free Space Tracking
│       ├── Space Allocation
│       ├── Space Reclamation
│       └── Space Statistics
│
├── Buffer Management
│   ├── Buffer Pool
│   ├── Buffer Frame
│   ├── Replacement Policy (LRU / Clock)
│   ├── Dirty Page Manager
│   └── Page Flush Scheduler
│
├── Record Management
│   ├── Record Manager
│   ├── Slot Directory
│   ├── Heap Storage
│   └── Record Serializer
│
└── Index Management
    ├── B+Tree Manager
    ├── Index Page
    ├── Index Iterator
    ├── Split / Merge
    └── Statistics
```
