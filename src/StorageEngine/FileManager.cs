using System;
using System.Collections.Generic;

public class FileManager : IFileManager
{
    public string RootDirectory { get; set; } = null!;

    public Dictionary<string, FileHandle> OpenFiles { get; set; } = null!;

    public void Initialize(object fileSettings)
    {
        throw new NotImplementedException();
    }

    public object CreateFile(string path)
    {
        throw new NotImplementedException();
    }

    public object OpenFile(string path)
    {
        throw new NotImplementedException();
    }

    public void CloseFile(string path)
    {
        throw new NotImplementedException();
    }

    public void DeleteFile(string path)
    {
        throw new NotImplementedException();
    }

    public Page ReadPage(PageId pageId)
    {
        throw new NotImplementedException();
    }

    public void CloseAllFiles()
    {
        throw new NotImplementedException();
    }

    private bool IsFileOpen(string path)
    {
        throw new NotImplementedException();
    }

    private void RegisterOpenFile(string path, object fileHandle)
    {
        throw new NotImplementedException();
    }
}
