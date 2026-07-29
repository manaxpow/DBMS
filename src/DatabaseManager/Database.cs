using System;

public abstract class Database
{
    private readonly IStorageEngine storage = null!;
    // private object schemaManager;
    // private bool isOpen;
    private IDatabaseState state = null!;

    public Database()
    {
    }

    public Database(string name, IDatabaseState state)
    {
        this.Name = name;
        this.state = state;
    }

    public Database(IStorageEngine storage)
    {
        this.storage = storage;
    }

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    protected IStorageEngine Storage => this.storage;

    public abstract void Initialize();

    public virtual Page ReadPage(int pageId)
    {
        return this.storage.FetchPage(pageId);
    }

    public virtual void WritePage(Page page)
    {
        this.storage.FlushPage(page);
    }
}
