using Abstractions.Source;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sources.Mssql;

namespace Cli.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.TryAddTransient<IDbReader, Reader>();

        return services;
    }
}