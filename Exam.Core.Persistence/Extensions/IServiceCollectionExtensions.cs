using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Exam.Core.Persistence.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddMySqlContext<TContext>(this IServiceCollection services, string databaseName, IConfiguration config)
        where TContext : DbContext
    {
        var connectionString = config.GetConnectionString("MySQL");
        var version = new MySqlServerVersion("8.0.26");

        services.AddDbContext<DbContext, TContext>(options =>
        {
            options.UseMySql($"{connectionString};Database={databaseName}", version, options =>
            {
                options.EnableRetryOnFailure();
            });
        });

        return services;
    }
}