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
