using Microsoft.OpenApi;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "Online Stores API",
                    Version = "v1"
                });

            options.AddSecurityDefinition(
                "Cookie",
                new OpenApiSecurityScheme
                {
                    Name = "accessToken",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Cookie,
                    Description =
                        "Enter the JWT access token cookie."
                });
        });

        return services;
    }
}
