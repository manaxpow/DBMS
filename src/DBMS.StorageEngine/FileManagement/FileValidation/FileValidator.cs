using System;
using DBMS.StorageEngine.FileManagement.Domain;
using DBMS.StorageEngine.FileManagement.FileLifecycle;

namespace DBMS.StorageEngine.FileManagement.FileValidation;

public class FileValidator : IFileValidator
{
    public void Validate(FileHeader header, AllocationMetadata metadata, ExtentBitmap bitmap, long physicalFileSize)
    {
        throw new NotImplementedException();
    }
}
