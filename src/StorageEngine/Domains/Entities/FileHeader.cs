namespace DBMS.StorageEngine.FileManagement.Domain;

public class FileHeader
{
    public uint MagicNumber { get; private set; }
    public int FormatVersion { get; private set; }
    public int HeaderSize { get; private set; }
    public FileId FileId { get; private set; }
    public FileType FileType { get; private set; }
    public int PageSize { get; private set; }
    public int ExtentSize { get; private set; }
    public long AllocationMetadataOffset { get; private set; }
    public long ExtentBitmapOffset { get; private set; }
    public long DataRegionOffset { get; private set; }

    internal FileHeader() { }

    public static FileHeader Create(FileHeaderConfiguration config)
    {
        return new FileHeader
        {
            FileId = config.FileId,
            FileType = config.FileType,
            PageSize = config.PageSize,
            ExtentSize = config.ExtentSize,
            FormatVersion = config.FormatVersion,
            HeaderSize = config.HeaderSize,
            DataRegionOffset = config.DataRegionOffset
        };
    }

    public long CalculatePageOffset(PageId pageId)
    {
        return HeaderSize + (pageId.Value * PageSize);
    }
}
