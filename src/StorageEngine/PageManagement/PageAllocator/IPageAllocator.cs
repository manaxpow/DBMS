public interface IPageAllocator
{
    public Page Allocate();
    public bool DeAllocate();
    public bool ReuseFreePage();
    public Page PageMapping();

}