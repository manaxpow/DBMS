const fs = require('fs');
const path = require('path');

const baseDir = path.join('c:', 'Users', 'ADMIN', 'Desktop', 'DBMS', 'src', 'StorageEngine');

// Ensure directory exists
function ensureDir(dir) {
    if (!fs.existsSync(dir)) {
        fs.mkdirSync(dir, { recursive: true });
    }
}

ensureDir(baseDir);
ensureDir(path.join(baseDir, 'Common'));
ensureDir(path.join(baseDir, 'FileManagement'));
ensureDir(path.join(baseDir, 'BufferManagement'));
ensureDir(path.join(baseDir, 'RecordManagement'));
ensureDir(path.join(baseDir, 'IndexManagement'));

// Clean up old Domain.cs
try {
    fs.unlinkSync(path.join(baseDir, 'Common', 'Domain.cs'));
} catch (e) {
    // Ignore if not exists
}

const files = {};

// 1. StorageEngine Root
files['StorageEngine.cs'] = `
using System;

public class StorageEngine
{
    private IFileLifecycleManager _fileLifecycleManager;
    private IBufferPoolManager _bufferPoolManager;
    private IRecordManager _recordManager;
    private IIndex _index;

    public StorageEngine(
        IFileLifecycleManager fileLifecycleManager,
        IBufferPoolManager bufferPoolManager,
        IRecordManager recordManager,
        IIndex index)
    {
        _fileLifecycleManager = fileLifecycleManager;
        _bufferPoolManager = bufferPoolManager;
        _recordManager = recordManager;
        _index = index;
    }

    public void Initialize()
    {
    }

    public void Shutdown()
    {
    }
}
`;

// 2. Common Domain (Split)
files['Common/FileId.cs'] = `using System;\n\npublic record FileId(int Id);`;
files['Common/FileHandle.cs'] = `using System;\n\npublic record FileHandle(int Descriptor);`;
files['Common/DiskAddress.cs'] = `using System;\n\npublic record DiskAddress(int BlockNumber, int Offset);`;
files['Common/PageId.cs'] = `using System;\n\npublic record PageId(FileId File, int PageNumber);`;
files['Common/FrameId.cs'] = `using System;\n\npublic record FrameId(int Index);`;
files['Common/RecordId.cs'] = `using System;\n\npublic record RecordId(PageId Page, int SlotNumber);`;
files['Common/IndexKey.cs'] = `using System;\n\npublic record IndexKey(byte[] Bytes);`;
files['Common/RecordPointer.cs'] = `using System;\n\npublic record RecordPointer(RecordId RecordId);`;

files['Common/Page.cs'] = `
using System;

public class Page
{
    public PageId Id { get; set; }
    public byte[] Data { get; set; }
    public bool IsDirty { get; set; }
    public int PinCount { get; set; }
}
`;

files['Common/Record.cs'] = `
using System;

public class Record
{
    public RecordId Id { get; set; }
    public byte[] Data { get; set; }
}
`;

files['Common/BPlusTreeNode.cs'] = `
using System;

public class BPlusTreeNode
{
    // Node details
}
`;

files['Common/BufferPool.cs'] = `
using System;

public class BufferPool
{
    public Page[] Pages { get; set; }
}
`;

files['Common/RecordLayoutCalculator.cs'] = `
using System;

public class RecordLayoutCalculator
{
    // Layout calculations
}
`;

// 3. FileManagement
files['FileManagement/IFileLifecycleManager.cs'] = `
using System;

public interface IFileLifecycleManager
{
    FileId CreateFile(string path);
    void DeleteFile(FileId fileId);
    FileHandle OpenFile(FileId fileId);
    void CloseFile(FileHandle handle);
}
`;

files['FileManagement/FileLifecycleManager.cs'] = `
using System;
using System.Collections.Generic;

public class FileLifecycleManager : IFileLifecycleManager
{
    private Dictionary<FileId, string> _filePaths = new Dictionary<FileId, string>();

    public void InitializeStorage()
    {
    }

    public FileId CreateFile(string path)
    {
        return default;
    }

    public void DeleteFile(FileId fileId)
    {
    }

    public FileHandle OpenFile(FileId fileId)
    {
        return default;
    }

    public void CloseFile(FileHandle handle)
    {
    }
}
`;

files['FileManagement/IPhysicalFileSystem.cs'] = `
using System;

public interface IPhysicalFileSystem
{
    void ReadBlock(DiskAddress address, byte[] buffer);
    void WriteBlock(DiskAddress address, byte[] buffer);
}
`;

files['FileManagement/PhysicalFileSystem.cs'] = `
using System;
using System.IO;

public class PhysicalFileSystem : IPhysicalFileSystem
{
    private FileStream _diskStream;

    public void SeekToAddress(DiskAddress addr)
    {
    }

    public void ReadBlock(DiskAddress address, byte[] buffer)
    {
    }

    public void WriteBlock(DiskAddress address, byte[] buffer)
    {
    }
}
`;

// 4. BufferManagement
files['BufferManagement/IBufferPoolManager.cs'] = `
using System;

public interface IBufferPoolManager
{
    Page FetchPage(PageId pageId);
    void UnpinPage(PageId pageId, bool isDirty);
    void FlushPage(PageId pageId);
    Page NewPage(FileId fileId);
    void DeletePage(PageId pageId);
}
`;

files['BufferManagement/BufferPoolManager.cs'] = `
using System;

public class BufferPoolManager : IBufferPoolManager
{
    private BufferPool _pool;
    private IPhysicalFileSystem _fileSystem;
    private IPageReplacementPolicy _replacer;

    public BufferPoolManager(IPhysicalFileSystem fileSystem, IPageReplacementPolicy replacer)
    {
        _fileSystem = fileSystem;
        _replacer = replacer;
    }

    public FrameId FindFreeFrame()
    {
        return default;
    }

    public Page FetchPage(PageId pageId)
    {
        return default;
    }

    public void UnpinPage(PageId pageId, bool isDirty)
    {
    }

    public void FlushPage(PageId pageId)
    {
    }

    public Page NewPage(FileId fileId)
    {
        return default;
    }

    public void DeletePage(PageId pageId)
    {
    }
}
`;

files['BufferManagement/IPageReplacementPolicy.cs'] = `
using System;

public interface IPageReplacementPolicy
{
    void Pin(FrameId frameId);
    void Unpin(FrameId frameId);
    FrameId Victim();
}
`;

files['BufferManagement/ClockReplacementPolicy.cs'] = `
using System;
using System.Collections.Generic;

public class ClockReplacementPolicy : IPageReplacementPolicy
{
    private List<FrameId> _clockHand = new List<FrameId>();

    public void AdvanceClock()
    {
    }

    public void Pin(FrameId frameId)
    {
    }

    public void Unpin(FrameId frameId)
    {
    }

    public FrameId Victim()
    {
        return default;
    }
}
`;

// 5. RecordManagement
files['RecordManagement/IRecordManager.cs'] = `
using System;

public interface IRecordManager
{
    RecordId InsertRecord(Record record);
    Record GetRecord(RecordId recordId);
    void UpdateRecord(RecordId recordId, Record record);
    void DeleteRecord(RecordId recordId);
}
`;

files['RecordManagement/RecordManager.cs'] = `
using System;

public class RecordManager : IRecordManager
{
    private RecordLayoutCalculator _layout;
    private IBufferPoolManager _bufferPool;

    public RecordManager(IBufferPoolManager bufferPool)
    {
        _bufferPool = bufferPool;
    }

    public void CompactPage(Page page)
    {
    }

    public RecordId InsertRecord(Record record)
    {
        return default;
    }

    public Record GetRecord(RecordId recordId)
    {
        return default;
    }

    public void UpdateRecord(RecordId recordId, Record record)
    {
    }

    public void DeleteRecord(RecordId recordId)
    {
    }
}
`;

// 6. IndexManagement
files['IndexManagement/IIndex.cs'] = `
using System;

public interface IIndex
{
    void Insert(IndexKey key, RecordPointer ptr);
    void Delete(IndexKey key);
    RecordPointer Search(IndexKey key);
}
`;

files['IndexManagement/BPlusTreeIndex.cs'] = `
using System;

public class BPlusTreeIndex : IIndex
{
    private BPlusTreeNode _root;
    private IBufferPoolManager _bufferPool;

    public BPlusTreeIndex(IBufferPoolManager bufferPool)
    {
        _bufferPool = bufferPool;
    }

    public void SplitNode(BPlusTreeNode node)
    {
    }

    public void MergeNode(BPlusTreeNode node)
    {
    }

    public void Insert(IndexKey key, RecordPointer ptr)
    {
    }

    public void Delete(IndexKey key)
    {
    }

    public RecordPointer Search(IndexKey key)
    {
        return default;
    }
}
`;

// Write all files
for (const [relativePath, content] of Object.entries(files)) {
    const fullPath = path.join(baseDir, relativePath);
    fs.writeFileSync(fullPath, content.trim() + '\n', 'utf8');
    console.log('Created: ' + relativePath);
}
