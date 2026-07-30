public interface IStorageEngine
{
    void Mount();

    Page FetchPage(int pageId);

    void FlushPage(Page page);
}
