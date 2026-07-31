using DBMS.Exceptions;

public sealed class CatalogRepository(
    IDatabaseRepository databaseRepository,
    ISchemaRepository schemaRepository,
    ITableRepository tableRepository)
    : ICatalogRepository
{
    private readonly IDatabaseRepository _databaseRepository = databaseRepository;

    private readonly ISchemaRepository _schemaRepository = schemaRepository;

    private readonly ITableRepository _tableRepository = tableRepository;

    public async Task<int?> FindTableIdAsync(
        string databaseName,
        string schemaName,
        string tableName,
        CancellationToken cancellationToken)
    {
        var database = await _databaseRepository.GetByNameAsync(
            databaseName,
            cancellationToken);

        if (database is null)
        {
            throw new DatabaseNotFoundException();
        }

        var schema = await _schemaRepository.GetByNameAsync(
            database.Id,
            schemaName,
            cancellationToken);

        if (schema is null)
        {
            throw new SchemaNotFoundException();
        }

        var table = await _tableRepository.GetByNameAsync(
            schema.Id,
            tableName,
            cancellationToken);

        return table?.Id;
    }
}
