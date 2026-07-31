public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IRowService, RowService>();
        services.AddScoped<ITableService, TableService>();
        services.AddScoped<IColumnService, ColumnService>();

        // Repositories
        services.AddScoped<ITableRepository>(provider =>
        {
            var environment =
                provider.GetRequiredService<IWebHostEnvironment>();

            var filePath = Path.Combine(
                environment.ContentRootPath,
                "Datas",
                "tables.json");

            return new TableRepository(filePath);
        });
        return services;
    }
}
