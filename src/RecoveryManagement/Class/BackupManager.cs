using System;

public class BackupManager : IBackupManager
{
    private object _planner;

    public BackupId CreateFullBackup(string destination)
    {
        return default;
    }

    public BackupId CreateIncrementalBackup(string destination)
    {
        return default;
    }

    public void WriteManifest()
    {
    }
}
