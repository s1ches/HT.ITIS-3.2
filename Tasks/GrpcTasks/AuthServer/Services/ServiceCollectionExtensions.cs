using AuthServer.Services.AccessTokenProvider;

namespace AuthServer.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IAccessTokenProvider, AccessTokenProvider.AccessTokenProvider>();
    }
}