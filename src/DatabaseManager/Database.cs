using System;
using DBMS.Exceptions;

public class Database
{
    public int Id { get; set; }
    public string Name { get; set; }
    private object _storage;
    private object _schemaManager;
    private bool _isOpen;

    public Database()
    {
        _isOpen = false;
    }

    public Database(string name)
    {
        Name = name;
        _isOpen = false;
    }

    public void Open()
    {
        throw new StorageInitializationException();
    }

    public void Close()
    {
        throw new FlushFailureException();
    }

    public void AddSchema(object schema)
    {
        throw new SchemaAlreadyExistsException();
    }

    public void DropSchema(string name)
    {
        throw new SchemaNotFoundException();
    }
}
