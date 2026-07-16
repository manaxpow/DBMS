using System;

public interface ICheckpointCoordinator
{
    CheckpointId CreateCheckpoint();
    CheckpointMetadata GetLatestCheckpoint();
}
