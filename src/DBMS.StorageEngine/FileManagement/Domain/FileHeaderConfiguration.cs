namespace DBMS.StorageEngine.FileManagement.Domain;

public readonly record struct FileHeaderConfiguration(
    FileId FileId,
    FileType FileType,
    int PageSize,
    int ExtentSize,
    int FormatVersion,
    int HeaderSize,
    long DataRegionOffset
);
