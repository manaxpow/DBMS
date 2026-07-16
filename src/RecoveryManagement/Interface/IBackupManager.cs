using System;

public interface IBackupManager
{
    BackupId CreateFullBackup(string destination);
    BackupId CreateIncrementalBackup(string destination);
}
