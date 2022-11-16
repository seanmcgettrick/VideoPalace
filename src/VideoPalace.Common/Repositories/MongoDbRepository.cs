using System.Linq.Expressions;
using MongoDB.Driver;
using VideoPalace.Common.Contracts;

namespace VideoPalace.Common.Repositories;

public class MongoRepository<T> : IRepository<T> where T : IEntity
{
    private readonly IMongoCollection<T> _collection;
    private readonly FilterDefinitionBuilder<T> _filterDefinitionBuilder = Builders<T>.Filter;

    public MongoRepository(IMongoDatabase database, string collectionName) =>
        _collection = database.GetCollection<T>(collectionName);

    public async Task<IReadOnlyCollection<T>> GetAllAsync() =>
        await _collection.Find(_filterDefinitionBuilder.Empty).ToListAsync();

    public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter) =>
        await _collection.Find(filter).ToListAsync();

    public async Task<T?> GetAsync(Guid id) => await GetAsync(entity => entity.Id == id);

    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter) =>
        await _collection.Find(filter).FirstOrDefaultAsync();

    public async Task CreateAsync(T entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        await _collection.InsertOneAsync(entity);
    }

    public async Task BulkCreateAsync(IEnumerable<T> entities)
    {
        var createTasks = entities.Select(CreateAsync);

        await Task.WhenAll(createTasks);
    }

    public async Task UpdateAsync(T entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        var filter = _filterDefinitionBuilder.Eq(existingEntitty => existingEntitty.Id, entity.Id);

        await _collection.ReplaceOneAsync(filter, entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var filter = _filterDefinitionBuilder.Eq(entity => entity.Id, id);

        await _collection.DeleteOneAsync(filter);
    }
}