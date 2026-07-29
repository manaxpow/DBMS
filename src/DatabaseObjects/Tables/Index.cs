public partial class Index
{
    private Dictionary<object, List<object>> entries;

    public Index(bool isUnique)
    {
        this.IsUnique = isUnique;
        this.entries = new Dictionary<object, List<object>>();
        this.Entries = this.entries;
    }

    public Index(string name, IEnumerable<string> columnNames, bool isUnique = false, bool allowsNull = true, IndexType type = IndexType.NonClustered)
        : this(isUnique)
    {
        this.Name = name;
        this.ColumnNames = columnNames.ToList().AsReadOnly();
        this.AllowsNull = allowsNull;
        this.Type = type;
    }

    public bool IsUnique { get; set; }

    public string Name { get; } = string.Empty;

    public bool AllowsNull { get; set; }

    public IReadOnlyList<string> ColumnNames { get; } = new List<string>().AsReadOnly();

    public IndexType Type { get; }

    public IReadOnlyDictionary<object, List<object>> Entries { get; }

    public void Insert(object key, object recordPointer) => throw new NotImplementedException();

    public object Search(object key) => throw new NotImplementedException();

    public object[] RangeSearch(object startKey, object endKey) => throw new NotImplementedException();

    public void Update(object key, object newRecordPointer) => throw new NotImplementedException();

    public bool Delete(object key) => throw new NotImplementedException();

    private bool ContainsKey(object key) => throw new NotImplementedException();

    private void AddEntry(object key, object recordPointer) => throw new NotImplementedException();

    private void ReplaceEntry(object key, object newRecordPointer) => throw new NotImplementedException();

    private IEnumerable<IndexEntry> FindEntriesInRange(object startKey, object endKey) => throw new NotImplementedException();

    private object[] OrderByKey(IEnumerable<IndexEntry> entries) => throw new NotImplementedException();
}
