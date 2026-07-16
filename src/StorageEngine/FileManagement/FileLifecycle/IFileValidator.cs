using DBMS.StorageEngine.FileManagement.Domain;

namespace DBMS.StorageEngine.FileManagement.FileLifecycle;

public interface IFileValidator
{
    void Validate(FileHeader header, AllocationMetadata metadata, ExtentBitmap bitmap, long physicalFileSize);
}
