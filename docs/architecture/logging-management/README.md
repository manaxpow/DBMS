# Logging Management

```text
Logging Management
├── Log Manager
│   ├── Log Coordination
│   ├── Log Lifecycle
│   ├── Log Flush Control
│   └── Log Metadata
│
├── WAL Protocol
│   ├── Log Before Data Rule
│   ├── Commit Flush Rule
│   ├── Page Flush Validation
│   ├── LSN Consistency
│   └── WAL Enforcement
│
├── Log Record Manager
│   ├── Record Generation
│   ├── Record Serialization
│   ├── Record Types
│   └── Record Parsing
│
├── Log Sequence Number Manager
│   ├── LSN Allocation
│   ├── LSN Tracking
│   ├── Page LSN
│   └── Flush LSN
│
├── Log Buffer Manager
│   ├── Buffer Allocation
│   ├── Buffer Append
│   ├── Buffer Flush
│   └── Buffer Reuse
│
├── Log Writer
│   ├── Disk Writer
│   ├── Group Commit
│   ├── Asynchronous Flush
│   └── Flush Scheduling
│
├── Log Block Manager
│   ├── Block Layout
│   ├── Block Allocation
│   ├── Checksum
│   └── Block Integrity
│
├── Log File Manager
│   ├── Log File Creation
│   ├── Log File Growth
│   ├── Log Truncation
│   ├── Log Archiving
│   └── Log Reuse
│
└── Log Checkpoint Coordinator
    ├── Checkpoint Trigger
    ├── Checkpoint Record
    ├── Dirty Page Tracking
    └── Checkpoint Coordination
```
