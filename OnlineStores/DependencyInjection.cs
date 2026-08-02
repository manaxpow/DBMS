using FluentValidation;



public static class DependencyInjection
{
    public static IServiceCollection AddOnlineStores(this IServiceCollection services, IConfiguration configuration)
    {
        // JWT configuration
        services
            .AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Issuer),
                "JWT Issuer is required.")
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Audience),
                "JWT Audience is required.")
            .Validate(
                options => options.SecretKey.Length >= 32,
                "JWT SecretKey must contain at least 32 characters.")
            .ValidateOnStart();

        // Register repositories
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IStoreRepository, StoreRepository>();
        services.AddSingleton<IProductRepository, ProductRepository>();
        services.AddSingleton<ICustomerRepository, CustomerRepository>();
        services.AddSingleton<IOrderRepository, OrderRepository>();
        services.AddSingleton<IDiscountRepository, DiscountRepository>();
        services.AddSingleton<IDashboardRepository, DashboardRepository>();

        // Register services
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IStoreService, StoreService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<ICustomerExportService, CustomerExportService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IReportService, ReportService>();

        // Register validators
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}
