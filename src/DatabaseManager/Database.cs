using System;
using DBMS.Exceptions;

public class Database
{
    public int Id { get; set; }
    public string Name { get; set; }
    private object _storage;
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

    public void ChangeState(IDatabaseState state)
    {
        _state = state;
    }
    public void Open()
    {
        throw new NotImplementedException();
    }
    public void SetReadOnly()
    {
        throw new NotImplementedException();
    }
    public void Recovery()
    {
        throw new NotImplementedException();
    }
    public void Drop()
    {
        throw new NotImplementedException();
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
