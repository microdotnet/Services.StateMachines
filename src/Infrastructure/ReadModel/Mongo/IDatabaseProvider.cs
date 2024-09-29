namespace MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo;

using MongoDB.Driver;

public interface IDatabaseProvider
{
    IMongoDatabase GetDatabase(string connectionStringName, string databaseName);
}
