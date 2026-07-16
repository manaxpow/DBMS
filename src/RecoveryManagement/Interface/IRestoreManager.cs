using System;

public interface IRestoreManager
{
    void RestoreFromBackup(BackupId backupId);
}
