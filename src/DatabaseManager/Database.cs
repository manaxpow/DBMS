using System;

public abstract class Database
{
    public int Id { get; set; }
    public string Name { get; set; }
    protected readonly IStorageEngine _storage;
    private object _schemaManager;
    private bool _isOpen;
    private IDatabaseState _state;

    public Database()
    {
        _isOpen = false;
    }

    public Database(string name, IDatabaseState state)
    {
        Name = name;
        _isOpen = false;
        _state = state;
    }

    public Database(IStorageEngine storage)
    {
        _storage = storage;
    }

    public abstract void Initialize();

    public virtual Page ReadPage(int pageId)
    {
        return _storage.FetchPage(pageId);
    }

    public virtual void WritePage(Page page)
    {
        _storage.FlushPage(page);
    }
}
