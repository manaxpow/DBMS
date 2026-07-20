using System;
using System.Collections.Generic;

public class View
{
    public string Name { get; set; }
    public string Query { get; set; }
    public bool IsDropped { get; set; }
    public IReadOnlyList<string> Dependencies { get; }
    
    private Schema _schema;

    public View(string name, string query)
    {
        Name = name;
        Query = query;
        Dependencies = new List<string>().AsReadOnly();
    }

    public View Create(string name, string query, Schema schema) => throw new NotImplementedException();
    public void AlterView(string newQuery) => throw new NotImplementedException();
    public void DropView() => throw new NotImplementedException();
    public object Resolve(Schema schema) => throw new NotImplementedException();
    
    private void ValidateQuery(string query) => throw new NotImplementedException();
    private IReadOnlyList<string> GetDependencies(string query) => throw new NotImplementedException();
    private void EnsureDependenciesExist(Schema schema, IReadOnlyList<string> dependencies) => throw new NotImplementedException();
}
