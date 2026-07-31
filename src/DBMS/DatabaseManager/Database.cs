using System;
using System.Collections.Generic;

public abstract class Database
{
    private readonly IStorageEngine _storage = null!;
    private IDatabaseState _state = null!;
    private readonly Dictionary<string, Schema> _schemas = new();

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
    
    public IReadOnlyCollection<Schema> Schemas => _schemas.Values;

    public virtual void AddSchema(Schema schema)
    {
        if (_schemas.ContainsKey(schema.Name))
            throw new Exception($"Schema {schema.Name} already exists.");
        
        _schemas[schema.Name] = schema;
    }

    public virtual void DropSchema(string name)
    {
        _schemas.Remove(name);
    }

    public virtual Schema? GetSchema(string name)
    {
        _schemas.TryGetValue(name, out var schema);
        return schema;
    }

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
