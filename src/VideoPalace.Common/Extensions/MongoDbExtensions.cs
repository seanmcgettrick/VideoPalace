using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using VideoPalace.Common.Contracts;
using VideoPalace.Common.Repositories;
using VideoPalace.Common.Settings;

namespace VideoPalace.Common.Extensions;

public static class MongoDbExtensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration,
        string databaseName)
    {
        services.AddSingleton(sp =>
        {
            var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            var mongoClient = new MongoClient(mongoDbSettings!.ConnectionString);

            return mongoClient.GetDatabase(databaseName);
        });

        return services;
    }

    public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName)
        where T : IEntity
    {
        services.AddScoped<IRepository<T>>(sp =>
        {
            var database = sp.GetService<IMongoDatabase>();

            return new MongoRepository<T>(database!, collectionName);
        });

        return services;
    }
}