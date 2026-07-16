using System;

public interface IFileLifecycleManager
{
    FileId CreateFile(string path);
    void DeleteFile(FileId fileId);
    FileHandle OpenFile(FileId fileId);
    void CloseFile(FileHandle handle);
}
