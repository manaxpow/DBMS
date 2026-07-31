public abstract class DatabaseBackup
{
    public void ExecuteBackup()
    {
        throw new NotImplementedException();
    }

    public abstract void InitializeBackup();

    public abstract void ExtractData();

    public abstract void CompressData();

    public abstract void FinalizeBackup();
}
