namespace MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo;

using MongoDB.Driver;

public sealed class DefaultDatabaseProvider : IDatabaseProvider
{
    private readonly IClientProvider clientProvider;

    public DefaultDatabaseProvider(IClientProvider clientProvider)
    {
        this.clientProvider = clientProvider;
    }

    public IMongoDatabase GetDatabase(string connectionStringName, string databaseName)
    {
        var client = this.clientProvider.GetClient(connectionStringName);
        return client.GetDatabase(databaseName);
    }
}
