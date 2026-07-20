public class Schema
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IReadOnlyCollection<Table> Tables { get; }
    public IReadOnlyCollection<View> Views { get; }
    public IReadOnlyCollection<StoredProcedure> StoredProcedures { get; }

    private Dictionary<string, Table> _tables;
    private Dictionary<string, View> _views;
    private Dictionary<string, StoredProcedure> _storedProcedures;

    public Schema(string name)
    {
        Name = name;
        _tables = new Dictionary<string, Table>();
        _views = new Dictionary<string, View>();
        _storedProcedures = new Dictionary<string, StoredProcedure>();
        Tables = _tables.Values;
        Views = _views.Values;
        StoredProcedures = _storedProcedures.Values;
    }

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
}

