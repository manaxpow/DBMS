public class LazyTableScanOperatorProxy : PhysicalOperator
{
    private readonly string _tableName;
    private readonly CatalogManager _catalog;
    private TableScanOperator? _realOperator;

    public LazyTableScanOperatorProxy(string tableName, CatalogManager catalog)
    {
        _tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
    }

    private void InitializeRealOperator()
    {
        throw new NotImplementedException();
    }

    public override void Open()
    {
        throw new NotImplementedException();

    }

    public override bool Next()
    {
        throw new NotImplementedException();

    }

    public override Row GetCurrent()
    {
        throw new NotImplementedException();

    }

    public override void Close()
    {
        throw new NotImplementedException();
    }
}
