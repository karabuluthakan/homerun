using System.Linq.Expressions;
using Domain.Entities;
using Domain.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.DataAccess.MongoDb;

public sealed class EventEntityMongoDbRepository : IEventEntityRepository
{
    private readonly IMongoCollection<EventEntity> _collection;
    private readonly ChallengeMongoClient _client;

    public EventEntityMongoDbRepository()
    {
        _client = new ChallengeMongoClient();
        var db = _client.GetDatabase("events");
        _collection = db.GetCollection<EventEntity>("eventEntities");
    }

    public async ValueTask<EventEntity?> GetAsync(
        Expression<Func<EventEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var cursor = await _collection.FindAsync(predicate, cancellationToken: cancellationToken);
        return await cursor.FirstOrDefaultAsync(cancellationToken);
    }

    public async ValueTask<List<EventEntity>> GetAllAsync(
        Expression<Func<EventEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _collection.AsQueryable();
        if (predicate is not null) query = query.Where(predicate);
        return await query.ToListAsync(cancellationToken: cancellationToken);
    }

    public async ValueTask<Exception?> AddAsync(
        EventEntity entity,
        CancellationToken cancellationToken = default)
    {
        var options = new InsertOneOptions { BypassDocumentValidation = true };
        var session = await _client.StartSessionAsync(cancellationToken: cancellationToken);
        entity.Id = ObjectId.GenerateNewId().ToString()!;
        try
        {
            await _collection.InsertOneAsync(entity, options, cancellationToken);
            await session.CommitTransactionAsync(cancellationToken);
            return null;
        }
        catch (Exception e)
        {
            await session.AbortTransactionAsync(cancellationToken);
            return e;
        }
    }

    public async ValueTask<Exception?> UpdateAsync(
        EventEntity entity,
        CancellationToken cancellationToken = default)
    {
        var session = await _client.StartSessionAsync(cancellationToken: cancellationToken);
        var predicate = Builders<EventEntity>.Filter.Eq(x => x.Id, entity.Id);
        var options = new FindOneAndReplaceOptions<EventEntity> { BypassDocumentValidation = true, IsUpsert = false };
        entity.Version += 1;
        try
        {
            await _collection.FindOneAndReplaceAsync(predicate, entity, options, cancellationToken);
            await session.CommitTransactionAsync(cancellationToken);
            return null;
        }
        catch (Exception e)
        {
            await session.AbortTransactionAsync(cancellationToken);
            return e;
        }
    }

    public async ValueTask<Exception?> DeleteAsync(
        Expression<Func<EventEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var session = await _client.StartSessionAsync(cancellationToken: cancellationToken);
        try
        {
            await _collection.FindOneAndDeleteAsync(predicate, cancellationToken: cancellationToken);
            await session.CommitTransactionAsync(cancellationToken);
            return null;
        }
        catch (Exception e)
        {
            await session.AbortTransactionAsync(cancellationToken);
            return e;
        }
    }
}