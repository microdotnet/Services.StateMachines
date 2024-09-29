namespace MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo;

using MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo.MachineDefinitions;
using MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo.MachineDetails;

using MongoDB.Driver;

public sealed class DefaultCollectionProvider : ICollectionProvider
{
    private const string MachinesCollectionName = "Machines";

    private const string MachineDefinitionsCollectionName = "MachineDefinitions";

    private readonly IDatabaseProvider databaseProvider;

    public DefaultCollectionProvider(IDatabaseProvider databaseProvider)
    {
        this.databaseProvider = databaseProvider ?? throw new System.ArgumentNullException(nameof(databaseProvider));
    }

    public IMongoCollection<MachineDetailsDto> Machines => this.GetCollection<MachineDetailsDto>(MachinesCollectionName);

    public IMongoCollection<MachineDefinitionDto> MachineDefinitions => this.GetCollection<MachineDefinitionDto>(MachineDefinitionsCollectionName);

    private IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        var database = this.databaseProvider.GetDatabase("ReadDB", "StateMachines");
        return database.GetCollection<T>(collectionName);
    }
}
