namespace DBMS.StorageEngine.FileManagement.Domain;

public class DataFile
{
    public virtual FilePath FileName { get; private set; }
    public virtual FileHeader Header { get; private set; }
    public virtual AllocationMetadata AllocationMetadata { get; private set; }
    public virtual long CurrentSize { get; private set; }
    public virtual long? MaximumSize { get; private set; }
    public virtual bool AutoExtendEnabled { get; private set; }

    protected DataFile() { }

    public static DataFile Create(FilePath fileName, FileHeader header, AllocationMetadata metadata, long currentSize, long? maximumSize, bool autoExtendEnabled)
    {
        return new DataFile
        {
            FileName = fileName,
            Header = header,
            AllocationMetadata = metadata,
            CurrentSize = currentSize,
            MaximumSize = maximumSize,
            AutoExtendEnabled = autoExtendEnabled
        };
    }

    public static DataFile Reconstruct(FilePath fileName, FileHeader header, AllocationMetadata metadata, long currentSize, long? maximumSize, bool autoExtendEnabled)
    {
        return new DataFile
        {
            FileName = fileName,
            Header = header,
            AllocationMetadata = metadata,
            CurrentSize = currentSize,
            MaximumSize = maximumSize,
            AutoExtendEnabled = autoExtendEnabled
        };
    }

    public virtual long CalculatePageOffset(PageId pageId)
    {
        return Header.CalculatePageOffset(pageId);
    }
}
