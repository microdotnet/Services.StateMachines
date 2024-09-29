namespace MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo;

using MongoDB.Driver;

public interface IClientProvider
{
    IMongoClient GetClient(string connectionStringName);
}
