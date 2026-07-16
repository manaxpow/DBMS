const fs = require('fs');
const path = require('path');

const srcDir = path.join('c:', 'Users', 'ADMIN', 'Desktop', 'DBMS', 'src');

function cleanDir(dir) {
    if (fs.existsSync(dir)) {
        fs.rmSync(dir, { recursive: true, force: true });
    }
}

cleanDir(path.join(srcDir, 'StorageEngine'));
cleanDir(path.join(srcDir, 'QueryProcessor'));
cleanDir(path.join(srcDir, 'TransactionManagement'));

function writeFiles(componentName, files) {
    const baseDir = path.join(srcDir, componentName);
    fs.mkdirSync(path.join(baseDir, 'Interface'), { recursive: true });
    fs.mkdirSync(path.join(baseDir, 'Enum'), { recursive: true });
    fs.mkdirSync(path.join(baseDir, 'Class'), { recursive: true });

    for (const [relativePath, content] of Object.entries(files)) {
        const fullPath = path.join(baseDir, relativePath);
        fs.writeFileSync(fullPath, content.trim() + '\n', 'utf8');
        console.log('Created: ' + path.join(componentName, relativePath));
    }
}

// =======================
// 1. Storage Engine
// =======================
const seFiles = {};
seFiles['Interface/IFileLifecycleManager.cs'] = `using System;\n\npublic interface IFileLifecycleManager\n{\n    FileId CreateFile(string path);\n    void DeleteFile(FileId fileId);\n    FileHandle OpenFile(FileId fileId);\n    void CloseFile(FileHandle handle);\n}`;
seFiles['Interface/IPhysicalFileSystem.cs'] = `using System;\n\npublic interface IPhysicalFileSystem\n{\n    void ReadBlock(DiskAddress address, byte[] buffer);\n    void WriteBlock(DiskAddress address, byte[] buffer);\n}`;
seFiles['Interface/IBufferPoolManager.cs'] = `using System;\n\npublic interface IBufferPoolManager\n{\n    Page FetchPage(PageId pageId);\n    void UnpinPage(PageId pageId, bool isDirty);\n    void FlushPage(PageId pageId);\n    Page NewPage(FileId fileId);\n    void DeletePage(PageId pageId);\n}`;
seFiles['Interface/IPageReplacementPolicy.cs'] = `using System;\n\npublic interface IPageReplacementPolicy\n{\n    void Pin(FrameId frameId);\n    void Unpin(FrameId frameId);\n    FrameId Victim();\n}`;
seFiles['Interface/IRecordManager.cs'] = `using System;\n\npublic interface IRecordManager\n{\n    RecordId InsertRecord(Record record);\n    Record GetRecord(RecordId recordId);\n    void UpdateRecord(RecordId recordId, Record record);\n    void DeleteRecord(RecordId recordId);\n}`;
seFiles['Interface/IIndex.cs'] = `using System;\n\npublic interface IIndex\n{\n    void Insert(IndexKey key, RecordPointer ptr);\n    void Delete(IndexKey key);\n    RecordPointer Search(IndexKey key);\n}`;

seFiles['Class/StorageEngine.cs'] = `using System;\n\npublic class StorageEngine\n{\n    private IFileLifecycleManager _fileLifecycleManager;\n    private IBufferPoolManager _bufferPoolManager;\n    private IRecordManager _recordManager;\n    private IIndex _index;\n\n    public StorageEngine(\n        IFileLifecycleManager fileLifecycleManager,\n        IBufferPoolManager bufferPoolManager,\n        IRecordManager recordManager,\n        IIndex index)\n    {\n        _fileLifecycleManager = fileLifecycleManager;\n        _bufferPoolManager = bufferPoolManager;\n        _recordManager = recordManager;\n        _index = index;\n    }\n\n    public void Initialize()\n    {\n    }\n\n    public void Shutdown()\n    {\n    }\n}`;
seFiles['Class/FileLifecycleManager.cs'] = `using System;\nusing System.Collections.Generic;\n\npublic class FileLifecycleManager : IFileLifecycleManager\n{\n    private Dictionary<FileId, string> _filePaths = new Dictionary<FileId, string>();\n\n    public void InitializeStorage()\n    {\n    }\n\n    public FileId CreateFile(string path)\n    {\n        return default;\n    }\n\n    public void DeleteFile(FileId fileId)\n    {\n    }\n\n    public FileHandle OpenFile(FileId fileId)\n    {\n        return default;\n    }\n\n    public void CloseFile(FileHandle handle)\n    {\n    }\n}`;
seFiles['Class/PhysicalFileSystem.cs'] = `using System;\nusing System.IO;\n\npublic class PhysicalFileSystem : IPhysicalFileSystem\n{\n    private FileStream _diskStream;\n\n    public void SeekToAddress(DiskAddress addr)\n    {\n    }\n\n    public void ReadBlock(DiskAddress address, byte[] buffer)\n    {\n    }\n\n    public void WriteBlock(DiskAddress address, byte[] buffer)\n    {\n    }\n}`;
seFiles['Class/BufferPoolManager.cs'] = `using System;\n\npublic class BufferPoolManager : IBufferPoolManager\n{\n    private BufferPool _pool;\n    private IPhysicalFileSystem _fileSystem;\n    private IPageReplacementPolicy _replacer;\n\n    public BufferPoolManager(IPhysicalFileSystem fileSystem, IPageReplacementPolicy replacer)\n    {\n        _fileSystem = fileSystem;\n        _replacer = replacer;\n    }\n\n    public FrameId FindFreeFrame()\n    {\n        return default;\n    }\n\n    public Page FetchPage(PageId pageId)\n    {\n        return default;\n    }\n\n    public void UnpinPage(PageId pageId, bool isDirty)\n    {\n    }\n\n    public void FlushPage(PageId pageId)\n    {\n    }\n\n    public Page NewPage(FileId fileId)\n    {\n        return default;\n    }\n\n    public void DeletePage(PageId pageId)\n    {\n    }\n}`;
seFiles['Class/ClockReplacementPolicy.cs'] = `using System;\nusing System.Collections.Generic;\n\npublic class ClockReplacementPolicy : IPageReplacementPolicy\n{\n    private List<FrameId> _clockHand = new List<FrameId>();\n\n    public void AdvanceClock()\n    {\n    }\n\n    public void Pin(FrameId frameId)\n    {\n    }\n\n    public void Unpin(FrameId frameId)\n    {\n    }\n\n    public FrameId Victim()\n    {\n        return default;\n    }\n}`;
seFiles['Class/RecordManager.cs'] = `using System;\n\npublic class RecordManager : IRecordManager\n{\n    private RecordLayoutCalculator _layout;\n    private IBufferPoolManager _bufferPool;\n\n    public RecordManager(IBufferPoolManager bufferPool)\n    {\n        _bufferPool = bufferPool;\n    }\n\n    public void CompactPage(Page page)\n    {\n    }\n\n    public RecordId InsertRecord(Record record)\n    {\n        return default;\n    }\n\n    public Record GetRecord(RecordId recordId)\n    {\n        return default;\n    }\n\n    public void UpdateRecord(RecordId recordId, Record record)\n    {\n    }\n\n    public void DeleteRecord(RecordId recordId)\n    {\n    }\n}`;
seFiles['Class/BPlusTreeIndex.cs'] = `using System;\n\npublic class BPlusTreeIndex : IIndex\n{\n    private BPlusTreeNode _root;\n    private IBufferPoolManager _bufferPool;\n\n    public BPlusTreeIndex(IBufferPoolManager bufferPool)\n    {\n        _bufferPool = bufferPool;\n    }\n\n    public void SplitNode(BPlusTreeNode node)\n    {\n    }\n\n    public void MergeNode(BPlusTreeNode node)\n    {\n    }\n\n    public void Insert(IndexKey key, RecordPointer ptr)\n    {\n    }\n\n    public void Delete(IndexKey key)\n    {\n    }\n\n    public RecordPointer Search(IndexKey key)\n    {\n        return default;\n    }\n}`;

seFiles['Class/FileId.cs'] = `using System;\n\npublic record FileId(int Id);`;
seFiles['Class/FileHandle.cs'] = `using System;\n\npublic record FileHandle(int Descriptor);`;
seFiles['Class/DiskAddress.cs'] = `using System;\n\npublic record DiskAddress(int BlockNumber, int Offset);`;
seFiles['Class/PageId.cs'] = `using System;\n\npublic record PageId(FileId File, int PageNumber);`;
seFiles['Class/FrameId.cs'] = `using System;\n\npublic record FrameId(int Index);`;
seFiles['Class/RecordId.cs'] = `using System;\n\npublic record RecordId(PageId Page, int SlotNumber);`;
seFiles['Class/IndexKey.cs'] = `using System;\n\npublic record IndexKey(byte[] Bytes);`;
seFiles['Class/RecordPointer.cs'] = `using System;\n\npublic record RecordPointer(RecordId RecordId);`;
seFiles['Class/Page.cs'] = `using System;\n\npublic class Page\n{\n    public PageId Id { get; set; }\n    public byte[] Data { get; set; }\n    public bool IsDirty { get; set; }\n    public int PinCount { get; set; }\n}`;
seFiles['Class/Record.cs'] = `using System;\n\npublic class Record\n{\n    public RecordId Id { get; set; }\n    public byte[] Data { get; set; }\n}`;
seFiles['Class/BPlusTreeNode.cs'] = `using System;\n\npublic class BPlusTreeNode\n{\n}`;
seFiles['Class/BufferPool.cs'] = `using System;\n\npublic class BufferPool\n{\n    public Page[] Pages { get; set; }\n}`;
seFiles['Class/RecordLayoutCalculator.cs'] = `using System;\n\npublic class RecordLayoutCalculator\n{\n}`;
seFiles['Class/DataFile.cs'] = `using System;\n\npublic class DataFile\n{\n    public FileId Id { get; set; }\n    public string FilePath { get; set; }\n    public long FileSize { get; set; }\n}`;
writeFiles('StorageEngine', seFiles);

// =======================
// 2. Query Processor
// =======================
const qpFiles = {};
qpFiles['Interface/ISqlParser.cs'] = `using System;\n\npublic interface ISqlParser\n{\n    SqlStatement Parse(string sql);\n}`;
qpFiles['Interface/ISemanticAnalyzer.cs'] = `using System;\n\npublic interface ISemanticAnalyzer\n{\n    BoundStatement Analyze(SqlStatement statement, SemanticContext ctx);\n}`;
qpFiles['Interface/ILogicalPlanBuilder.cs'] = `using System;\n\npublic interface ILogicalPlanBuilder\n{\n    LogicalPlan Build(BoundStatement statement);\n}`;
qpFiles['Interface/IQueryOptimizer.cs'] = `using System;\n\npublic interface IQueryOptimizer\n{\n    LogicalPlan Optimize(LogicalPlan plan);\n}`;
qpFiles['Interface/IPhysicalPlanBuilder.cs'] = `using System;\n\npublic interface IPhysicalPlanBuilder\n{\n    PhysicalPlan Build(LogicalPlan logicalPlan);\n}`;
qpFiles['Interface/IQueryExecutor.cs'] = `using System;\n\npublic interface IQueryExecutor\n{\n    QueryResult Execute(PhysicalPlan plan, ExecutionContext ctx);\n}`;

qpFiles['Class/QueryProcessor.cs'] = `using System;\n\npublic class QueryProcessor\n{\n    private ISqlParser _sqlParser;\n    private ISemanticAnalyzer _semanticAnalyzer;\n    private ILogicalPlanBuilder _logicalPlanBuilder;\n    private IQueryOptimizer _queryOptimizer;\n    private IPhysicalPlanBuilder _physicalPlanBuilder;\n    private IQueryExecutor _queryExecutor;\n\n    public QueryProcessor(\n        ISqlParser sqlParser,\n        ISemanticAnalyzer semanticAnalyzer,\n        ILogicalPlanBuilder logicalPlanBuilder,\n        IQueryOptimizer queryOptimizer,\n        IPhysicalPlanBuilder physicalPlanBuilder,\n        IQueryExecutor queryExecutor)\n    {\n        _sqlParser = sqlParser;\n        _semanticAnalyzer = semanticAnalyzer;\n        _logicalPlanBuilder = logicalPlanBuilder;\n        _queryOptimizer = queryOptimizer;\n        _physicalPlanBuilder = physicalPlanBuilder;\n        _queryExecutor = queryExecutor;\n    }\n\n    public void Initialize()\n    {\n    }\n\n    public QueryResult ProcessQuery(string sql)\n    {\n        return default;\n    }\n}`;
qpFiles['Class/SqlParser.cs'] = `using System;\n\npublic class SqlParser : ISqlParser\n{\n    private object _lexer;\n\n    public SqlStatement Parse(string sql)\n    {\n        return default;\n    }\n\n    public ASTNode BuildAST()\n    {\n        return default;\n    }\n}`;
qpFiles['Class/SemanticAnalyzer.cs'] = `using System;\n\npublic class SemanticAnalyzer : ISemanticAnalyzer\n{\n    private object _catalog;\n\n    public BoundStatement Analyze(SqlStatement statement, SemanticContext ctx)\n    {\n        return default;\n    }\n\n    public void ValidateTypes()\n    {\n    }\n}`;
qpFiles['Class/LogicalPlanBuilder.cs'] = `using System;\n\npublic class LogicalPlanBuilder : ILogicalPlanBuilder\n{\n    public LogicalPlan Build(BoundStatement statement)\n    {\n        return default;\n    }\n\n    public void CreateOperators()\n    {\n    }\n}`;
qpFiles['Class/QueryOptimizer.cs'] = `using System;\n\npublic class QueryOptimizer : IQueryOptimizer\n{\n    private object _rules;\n\n    public LogicalPlan Optimize(LogicalPlan plan)\n    {\n        return default;\n    }\n\n    public void ApplyRules()\n    {\n    }\n}`;
qpFiles['Class/PhysicalPlanBuilder.cs'] = `using System;\n\npublic class PhysicalPlanBuilder : IPhysicalPlanBuilder\n{\n    public PhysicalPlan Build(LogicalPlan logicalPlan)\n    {\n        return default;\n    }\n\n    public void SelectAccessPath()\n    {\n    }\n}`;
qpFiles['Class/QueryExecutor.cs'] = `using System;\n\npublic class QueryExecutor : IQueryExecutor\n{\n    private object _factory;\n    public QueryResult Execute(PhysicalPlan plan, ExecutionContext ctx)\n    {\n        return default;\n    }\n\n    public void RunPipeline()\n    {\n    }\n}`;

qpFiles['Class/SqlStatement.cs'] = `using System;\n\npublic class SqlStatement\n{\n}`;
qpFiles['Class/ASTNode.cs'] = `using System;\n\npublic class ASTNode\n{\n}`;
qpFiles['Class/BoundStatement.cs'] = `using System;\n\npublic class BoundStatement\n{\n}`;
qpFiles['Class/SemanticContext.cs'] = `using System;\n\npublic class SemanticContext\n{\n}`;
qpFiles['Class/LogicalPlan.cs'] = `using System;\n\npublic class LogicalPlan\n{\n}`;
qpFiles['Class/PhysicalPlan.cs'] = `using System;\n\npublic class PhysicalPlan\n{\n}`;
qpFiles['Class/ExecutionContext.cs'] = `using System;\n\npublic class ExecutionContext\n{\n}`;
qpFiles['Class/QueryResult.cs'] = `using System;\n\npublic class QueryResult\n{\n}`;
writeFiles('QueryProcessor', qpFiles);

// =======================
// 3. Transaction Management
// =======================
const tmFiles = {};
tmFiles['Interface/ITransactionManager.cs'] = `using System;\n\npublic interface ITransactionManager\n{\n    TransactionContext BeginTransaction(IsolationLevel isolationLevel);\n    void Commit(TransactionId transactionId);\n    void Abort(TransactionId transactionId);\n    TransactionState GetState(TransactionId transactionId);\n}`;
tmFiles['Interface/IIsolationPolicy.cs'] = `using System;\n\npublic interface IIsolationPolicy\n{\n    void Enforce(TransactionContext context, LockResource resource, OperationAccess access);\n}`;
tmFiles['Interface/ILockManager.cs'] = `using System;\n\npublic interface ILockManager\n{\n    bool AcquireLock(TransactionId transactionId, LockResource resource, LockMode mode);\n    void ReleaseLock(TransactionId transactionId, LockResource resource);\n    void ReleaseAllLocks(TransactionId transactionId);\n}`;
tmFiles['Interface/IDeadlockDetector.cs'] = `using System;\n\npublic interface IDeadlockDetector\n{\n    DeadlockCycle Detect();\n    void Resolve(DeadlockCycle cycle);\n}`;
tmFiles['Interface/IConcurrencyController.cs'] = `using System;\n\npublic interface IConcurrencyController\n{\n    bool CanAccess(TransactionContext context, LockResource resource, OperationAccess access);\n}`;

tmFiles['Enum/IsolationLevel.cs'] = `using System;\n\npublic enum IsolationLevel { \n    ReadUncommitted, \n    ReadCommitted, \n    RepeatableRead, \n    Serializable \n}`;
tmFiles['Enum/TransactionState.cs'] = `using System;\n\npublic enum TransactionState { \n    Active, \n    PartiallyCommitted, \n    Committed, \n    Failed, \n    Aborted \n}`;
tmFiles['Enum/LockMode.cs'] = `using System;\n\npublic enum LockMode { \n    Shared, \n    Exclusive, \n    Update, \n    IntentShared, \n    IntentExclusive \n}`;
tmFiles['Enum/OperationAccess.cs'] = `using System;\n\npublic enum OperationAccess { \n    Read, \n    Write \n}`;

tmFiles['Class/TransactionManagement.cs'] = `using System;\n\npublic class TransactionManagement\n{\n    private ITransactionManager _transactionManager;\n    private ILockManager _lockManager;\n    private IDeadlockDetector _deadlockDetector;\n    private IConcurrencyController _concurrencyController;\n\n    public TransactionManagement(\n        ITransactionManager transactionManager,\n        ILockManager lockManager,\n        IDeadlockDetector deadlockDetector,\n        IConcurrencyController concurrencyController)\n    {\n        _transactionManager = transactionManager;\n        _lockManager = lockManager;\n        _deadlockDetector = deadlockDetector;\n        _concurrencyController = concurrencyController;\n    }\n\n    public void Initialize()\n    {\n    }\n\n    public void Shutdown()\n    {\n    }\n}`;
tmFiles['Class/TransactionManager.cs'] = `using System;\n\npublic class TransactionManager : ITransactionManager\n{\n    public TransactionContext BeginTransaction(IsolationLevel isolationLevel)\n    {\n        return default;\n    }\n\n    public void Commit(TransactionId transactionId)\n    {\n    }\n\n    public void Abort(TransactionId transactionId)\n    {\n    }\n\n    public TransactionState GetState(TransactionId transactionId)\n    {\n        return default;\n    }\n}`;
tmFiles['Class/RepeatableReadPolicy.cs'] = `using System;\n\npublic class RepeatableReadPolicy : IIsolationPolicy\n{\n    private ILockManager _lockManager;\n\n    public RepeatableReadPolicy(ILockManager lockManager)\n    {\n        _lockManager = lockManager;\n    }\n\n    public void Enforce(TransactionContext context, LockResource resource, OperationAccess access)\n    {\n    }\n}`;
tmFiles['Class/LockManager.cs'] = `using System;\n\npublic class LockManager : ILockManager\n{\n    public bool AcquireLock(TransactionId transactionId, LockResource resource, LockMode mode)\n    {\n        return default;\n    }\n\n    public void ReleaseLock(TransactionId transactionId, LockResource resource)\n    {\n    }\n\n    public void ReleaseAllLocks(TransactionId transactionId)\n    {\n    }\n}`;
tmFiles['Class/DeadlockDetector.cs'] = `using System;\n\npublic class DeadlockDetector : IDeadlockDetector\n{\n    public DeadlockCycle Detect()\n    {\n        return default;\n    }\n\n    public void Resolve(DeadlockCycle cycle)\n    {\n    }\n}`;
tmFiles['Class/ConcurrencyController.cs'] = `using System;\n\npublic class ConcurrencyController : IConcurrencyController\n{\n    private IIsolationPolicy _isolationPolicy;\n\n    public ConcurrencyController(IIsolationPolicy isolationPolicy)\n    {\n        _isolationPolicy = isolationPolicy;\n    }\n\n    public bool CanAccess(TransactionContext context, LockResource resource, OperationAccess access)\n    {\n        return default;\n    }\n}`;
tmFiles['Class/TransactionId.cs'] = `using System;\n\npublic record TransactionId(long Id);`;
tmFiles['Class/TransactionContext.cs'] = `using System;\n\npublic class TransactionContext\n{\n    public TransactionId Id { get; set; }\n    public TransactionState State { get; set; }\n    public IsolationLevel Isolation { get; set; }\n}`;
tmFiles['Class/LockResource.cs'] = `using System;\n\npublic record LockResource(string ResourceId);`;
tmFiles['Class/DeadlockCycle.cs'] = `using System;\nusing System.Collections.Generic;\n\npublic class DeadlockCycle\n{\n    public List<TransactionId> Transactions { get; set; } = new List<TransactionId>();\n}`;
writeFiles('TransactionManagement', tmFiles);
