public interface IFileManager
{
    public void Initialize(object fileSettings);

    public object CreateFile(string path);

    public object OpenFile(string path);

    public void DeleteFile(string path);

    public Page ReadPage(PageId pageId);

    public void CloseAllFiles();
}
