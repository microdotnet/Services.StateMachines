namespace MicroDotNet.Services.StateMachines.Infrastructure.ReadModel.Mongo;

using MongoDB.Driver;

public abstract class MongoCollectionRepositoryBase<TPayload, TId>
    where TPayload : class
{
    protected MongoCollectionRepositoryBase()
    {
    }

    protected async Task CreateItemAsync(TPayload item, CancellationToken cancellationToken)
    {
        var collection = this.GetCollection();
        await collection.InsertOneAsync(
            item,
            null,
            cancellationToken)
            .ConfigureAwait(false);
    }

    protected async Task<TPayload?> GetItemAsync(TId itemId, CancellationToken cancellationToken)
    {
        var collection = this.GetCollection();
        var filterBuilder = Builders<TPayload>.Filter;
        var filter = this.FilterById(filterBuilder, itemId);
        var searchOptions = new FindOptions<TPayload>()
        {
            Limit = 1,
        };
        var itemsMatched = await collection.FindAsync(filter, searchOptions, cancellationToken)
            .ConfigureAwait(false);
        var items = await itemsMatched
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        if (items.Count == 0)
        {
            return null;
        }

        var found = items[0];
        return found;
    }

    protected abstract IMongoCollection<TPayload> GetCollection();

    protected abstract FilterDefinition<TPayload> FilterById(
        FilterDefinitionBuilder<TPayload> filterDefinitionBuilder,
        TId id);
}