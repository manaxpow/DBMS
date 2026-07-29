public class LazyTableScanOperatorProxy : PhysicalOperator
{
    private readonly string tableName;
    private readonly CatalogManager catalog;

    public LazyTableScanOperatorProxy(string tableName, CatalogManager catalog)
    {
        this.tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        this.catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
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

    private void InitializeRealOperator()
    {
        throw new NotImplementedException();
    }
}
