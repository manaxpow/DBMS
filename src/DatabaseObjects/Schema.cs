using System;

public class Schema
{
    public string Name { get; set; }

    public void AddTable(Table table)
    {
        throw new NotImplementedException();
    }

    public void RemoveTable(string tableName)
    {
        throw new NotImplementedException();
    }

    public void DropTable(string tableName)
    {
        throw new NotImplementedException();
    }

    public void AlterTable(string tableName, Table newTable)
    {
        throw new NotImplementedException();
    }

    public Table GetTable(string tableName)
    {
        throw new NotImplementedException();
    }

    public bool ContainsTable(string tableName)
    {
        throw new NotImplementedException();
    }

    public bool ContainsObject(string objectName)
    {
        throw new NotImplementedException();
    }

    public object ResolveObject(string objectName)
    {
        throw new NotImplementedException();
    }
}
