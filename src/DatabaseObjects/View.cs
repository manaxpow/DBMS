using System;

public class View
{
    public string Name { get; set; }
    public string Query { get; set; }

    public View Create(string name, string query, Schema schema)
    {
        throw new NotImplementedException();
    }

    public object Resolve(Schema schema)
    {
        throw new NotImplementedException();
    }

    public void AlterView(string newQuery)
    {
        throw new NotImplementedException();
    }

    public void DropView()
    {
        throw new NotImplementedException();
    }
}
