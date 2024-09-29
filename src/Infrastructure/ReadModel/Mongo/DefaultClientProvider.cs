namespace MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo;

using System.Configuration;

using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

public sealed class DefaultClientProvider : IClientProvider
{
    private static readonly SemaphoreSlim GetClientSemaphore = new(1);

    private readonly Dictionary<string, IMongoClient> clientsCache = new();

    private readonly IConfiguration configuration;

    public DefaultClientProvider(IConfiguration configuration)
    {
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public IMongoClient GetClient(string connectionStringName)
    {
        if (this.clientsCache.TryGetValue(connectionStringName, out var value))
        {
            return value;
        }

        GetClientSemaphore.Wait();
        if (!this.clientsCache.TryGetValue(connectionStringName, out value))
        {
            var client = this.CreateClient(connectionStringName);
            value = client;
            this.clientsCache.Add(connectionStringName, value);
        }

        GetClientSemaphore.Release();
        return value!;
    }

    private IMongoClient CreateClient(string connectionStringName)
    {
        if (string.IsNullOrEmpty(connectionStringName))
        {
            throw new ArgumentNullException(nameof(connectionStringName));
        }

        var connectionString = this.configuration.GetConnectionString(connectionStringName);
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ConfigurationErrorsException(
                $"Connection string named '{connectionStringName}' is empty. Non-empty connection string expected");
        }

        return new MongoClient(connectionString);
    }
}
