using System;

public class Database
{
    public string Name { get; set; }
    private object _storage;
    private object _schemaManager;
    private bool _isOpen;

    public void Open()
    {
        throw new NotImplementedException();
    }

    public void Close()
    {
        throw new NotImplementedException();
    }

    public void AddSchema(object schema)
    {
        throw new NotImplementedException();
    }

    public void DropSchema(string name)
    {
        throw new NotImplementedException();
    }
}
