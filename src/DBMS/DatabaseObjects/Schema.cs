public class Schema(string name)
    : ISchemaObject
{
    private readonly Dictionary<string, ISchemaObject> objects = new Dictionary<string, ISchemaObject>();

    public int Id { get; set; }

    public string Name { get; set; } = name;

    public SchemaObjectType ObjectType => SchemaObjectType.Schema;

    public IReadOnlyCollection<ISchemaObject> Objects =>
        this.objects.Values;

    public void RegisterObject(ISchemaObject obj) => throw new NotImplementedException();

    public ISchemaObjectIterator CreateIterator() => throw new NotImplementedException();

    public ISchemaObject UnregisterObject(string name) => throw new NotImplementedException();

    public void Drop() => throw new NotImplementedException();

    public void AddTable(Table table)
    {
        if (objects.ContainsKey(table.Name))
            throw new Exception($"Table {table.Name} already exists.");
        objects[table.Name] = table;
    }

    public void DropTable(string tableName)
    {
        objects.Remove(tableName);
    }

    public void AlterTable(string tableName, Table newTable) => throw new NotImplementedException();

    public Table? GetTable(string tableName)
    {
        if (objects.TryGetValue(tableName, out var obj) && obj is Table table)
            return table;
        return null;
    }

    public bool ContainsTable(string tableName)
    {
        return objects.TryGetValue(tableName, out var obj) && obj is Table;
    }

    public bool ContainsObject(string objectName) => throw new NotImplementedException();

    public object ResolveObject(string objectName) => throw new NotImplementedException();

    public void Accept(ISchemaVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public ISchemaObject Clone() => throw new NotImplementedException();

    internal void RegisterView(View view) => throw new NotImplementedException();

    internal void UnregisterView(string viewName) => throw new NotImplementedException();

    internal bool IsObjectReferenced(string objectName) => throw new NotImplementedException();

    private bool IsTableReferencedByForeignKey(string tableName) => throw new NotImplementedException();
}
