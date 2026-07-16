using System;

public interface IBufferPoolManager
{
    Page FetchPage(PageId pageId);
    void UnpinPage(PageId pageId, bool isDirty);
    void FlushPage(PageId pageId);
    Page NewPage(FileId fileId);
    void DeletePage(PageId pageId);
}
