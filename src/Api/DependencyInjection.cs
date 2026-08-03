public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IRowService, RowService>();
        services.AddScoped<ITableService, TableService>();
        services.AddScoped<IColumnService, ColumnService>();
        services.AddScoped<ISchemaService, SchemaService>();

        // Repositories
        services.AddSingleton<ITableRepository, TableRepository>();
        services.AddSingleton<IDatabaseRepository, MockDatabaseRepository>();
        services.AddSingleton<ISchemaRepository, MockSchemaRepository>();
        return services;
    }
}
