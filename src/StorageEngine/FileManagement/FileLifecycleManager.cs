using System;
using System.Collections.Generic;

public class FileLifecycleManager : IFileLifecycleManager
{
    private Dictionary<FileId, string> _filePaths = new Dictionary<FileId, string>();

    public void InitializeStorage()
    {
    }

    public FileId CreateFile(string path)
    {
        return default;
    }

    public void DeleteFile(FileId fileId)
    {
    }

    public FileHandle OpenFile(FileId fileId)
    {
        return default;
    }

    public void CloseFile(FileHandle handle)
    {
    }
}
