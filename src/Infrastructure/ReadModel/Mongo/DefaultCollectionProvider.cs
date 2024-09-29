namespace MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo;

using MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo.MachineDetails;

using MongoDB.Driver;

public sealed class DefaultCollectionProvider : ICollectionProvider
{
    public IMongoCollection<MachineDetailsDto> Machines => throw new NotImplementedException();
}
