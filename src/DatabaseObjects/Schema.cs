public class Schema : ISchemaObject
{
    public int Id { get; set; }
    public string Name { get; set; }
    private readonly Dictionary<string, ISchemaObject> _objects;

    public SchemaObjectType ObjectType => SchemaObjectType.Schema;

    public IReadOnlyCollection<ISchemaObject> Objects =>
        _objects.Values;

    public Schema(string name)
    {
        Name = name;
        _objects = new Dictionary<string, ISchemaObject>();
    }

    public void RegisterObject(ISchemaObject obj) => throw new NotImplementedException();

    public ISchemaObjectIterator CreateIterator() => throw new NotImplementedException();
    public ISchemaObject UnregisterObject(string name) => throw new NotImplementedException();
    public void Drop() => throw new NotImplementedException();

    public void AddTable(Table table) => throw new NotImplementedException();
    public void DropTable(string tableName) => throw new NotImplementedException();
    public void AlterTable(string tableName, Table newTable) => throw new NotImplementedException();
    public Table GetTable(string tableName) => throw new NotImplementedException();
    public bool ContainsTable(string tableName) => throw new NotImplementedException();
    public bool ContainsObject(string objectName) => throw new NotImplementedException();
    public object ResolveObject(string objectName) => throw new NotImplementedException();

    internal void RegisterView(View view) => throw new NotImplementedException();
    internal void UnregisterView(string viewName) => throw new NotImplementedException();
    internal bool IsObjectReferenced(string objectName) => throw new NotImplementedException();
    private bool IsTableReferencedByForeignKey(string tableName) => throw new NotImplementedException();

    public void Accept(ISchemaVisitor visitor)
    {
        throw new NotImplementedException();
    }

}

