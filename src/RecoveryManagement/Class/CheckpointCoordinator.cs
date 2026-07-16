using System;

public class CheckpointCoordinator : ICheckpointCoordinator
{
    private object _writer;

    public CheckpointId CreateCheckpoint()
    {
        return default;
    }

    public CheckpointMetadata GetLatestCheckpoint()
    {
        return default;
    }

    public void FlushDirtyPages()
    {
    }
}
