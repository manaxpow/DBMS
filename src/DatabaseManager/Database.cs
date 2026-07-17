using System;

public class Database
{
    public string Name { get; set; }

    public void Open()
    {
        throw new NotImplementedException();
    }

    public void Close()
    {
        throw new NotImplementedException();
    }

    public void AddSchema(string schemaName)
    {
        throw new NotImplementedException();
    }

    public void DropSchema(string schemaName)
    {
        throw new NotImplementedException();
    }

    public void AlterSchema(string schemaName, object newSchema)
    {
        throw new NotImplementedException();
    }
}
