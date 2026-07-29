using System;

public abstract class Database
{
    private readonly IStorageEngine _storage = null!;
    private IDatabaseState _state = null!;

    public Database()
    {
    }

    public Database(string name, IDatabaseState state)
    {
        this.Name = name;
        this._state = state;
    }

    public Database(IStorageEngine storage)
    {
        this._storage = storage;
    }

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    protected IStorageEngine Storage => this._storage;

    public abstract void Initialize();

    public virtual Page ReadPage(int pageId)
    {
        return this._storage.FetchPage(pageId);
    }

    public virtual void WritePage(Page page)
    {
        this._storage.FlushPage(page);
    }
}
