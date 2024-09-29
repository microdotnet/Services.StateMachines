namespace MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo;

using MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo.MachineDetails;

using MongoDB.Driver;

public sealed class DefaultCollectionProvider : ICollectionProvider
{
    private const string MachinesCollectionName = "Machines";

    private readonly IDatabaseProvider databaseProvider;

    public DefaultCollectionProvider(IDatabaseProvider databaseProvider)
    {
        this.databaseProvider = databaseProvider ?? throw new System.ArgumentNullException(nameof(databaseProvider));
    }

    public IMongoCollection<MachineDetailsDto> Machines => this.GetCollection<MachineDetailsDto>(MachinesCollectionName);

    private IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        var database = this.databaseProvider.GetDatabase("ReadDB", "StateMachines");
        return database.GetCollection<T>(collectionName);
    }
}
