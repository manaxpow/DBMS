public class View : ISchemaObject
{
    public int Id { get; set; }

    public string Name { get; set; }

    public SchemaObjectType ObjectType => SchemaObjectType.View;
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
    public void Drop() => throw new NotImplementedException();
    public object Resolve(Schema schema) => throw new NotImplementedException();

    private void ValidateQuery(string query) => throw new NotImplementedException();
    private IReadOnlyList<string> GetDependencies(string query) => throw new NotImplementedException();
    private void EnsureDependenciesExist(Schema schema, IReadOnlyList<string> dependencies) => throw new NotImplementedException();

    public void Accept(ISchemaVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public ISchemaObject Clone() => throw new NotImplementedException();
}
