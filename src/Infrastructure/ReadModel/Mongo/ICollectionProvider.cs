namespace MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo;

using MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo.MachineDetails;

using MongoDB.Driver;

public interface ICollectionProvider
{
    IMongoCollection<MachineDetailsDto> Machines { get; }
}
