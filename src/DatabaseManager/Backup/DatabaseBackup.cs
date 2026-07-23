public abstract class DatabaseBackup
{
    public void ExecuteBackup()
    {
        InitializeBackup();
        ExtractData();
        CompressData();
        FinalizeBackup();
    }
    public abstract void InitializeBackup();
    public abstract void ExtractData();
    public abstract void CompressData();
    public abstract void FinalizeBackup();
}
