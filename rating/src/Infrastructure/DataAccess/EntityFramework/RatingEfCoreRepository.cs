using System.Linq.Expressions;
using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

// ReSharper disable All

namespace Infrastructure.DataAccess.EntityFramework;

public sealed class RatingEfCoreRepository : IRatingRepository
{
    private readonly IDbContextFactory<ChallengeDbContext> _dbContextFactory;

    public RatingEfCoreRepository(IDbContextFactory<ChallengeDbContext> dbContextFactory)
    {
        ArgumentNullException.ThrowIfNull(dbContextFactory);
        _dbContextFactory = dbContextFactory;
    }

    public async ValueTask<RatingEntity?> GetAsync(
        Expression<Func<RatingEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        using (var dbContext = _dbContextFactory.CreateDbContext())
        {
            return await dbContext.Ratings
                .AsNoTracking()
                .Where(predicate)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }

    public async ValueTask<List<RatingEntity>> GetAllAsync(
        Expression<Func<RatingEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        using (var dbContext = _dbContextFactory.CreateDbContext())
        {
            var query = dbContext.Ratings.AsNoTracking();

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            return await query.ToListAsync(cancellationToken);
        }
    }

    public async ValueTask<Exception?> AddAsync(
        RatingEntity entity,
        CancellationToken cancellationToken = default)
    {
        using (var dbContext = _dbContextFactory.CreateDbContext())
        {
            var transaction = dbContext.Database.BeginTransaction();
            try
            {
                var entry = await dbContext.AddAsync(entity, cancellationToken);
                entry.State = EntityState.Added;
                transaction.Commit();
                return null;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return e;
            }
        }
    }

    public ValueTask<Exception?> UpdateAsync(
        RatingEntity entity,
        CancellationToken cancellationToken = default)
    {
        Exception? exception;
        using (var dbContext = _dbContextFactory.CreateDbContext())
        {
            var transaction = dbContext.Database.BeginTransaction();
            try
            {
                dbContext.Set<RatingEntity>().Update(entity);
                transaction.Commit();
                exception = null;
            }
            catch (Exception e)
            {
                exception = e;
                transaction.Rollback();
            }
        }

        return ValueTask.FromResult<Exception?>(exception);
    }

    public ValueTask<Exception?> DeleteAsync(
        Expression<Func<RatingEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        Exception? exception;
        using (var dbContext = _dbContextFactory.CreateDbContext())
        {
            var transaction = dbContext.Database.BeginTransaction();
            try
            {
                var entities = dbContext.Ratings.Where(predicate);
                dbContext.RemoveRange(entities);
                exception = null;
            }
            catch (Exception e)
            {
                exception = e;
                transaction.Rollback();
            }
        }

        return ValueTask.FromResult<Exception?>(exception);
    }

    public async ValueTask<Exception?> UpdateScoreAsync(Guid id, int score)
    {
        Exception? exception;
        using (var dbContext = _dbContextFactory.CreateDbContext())
        {
            var transaction = dbContext.Database.BeginTransaction();
            try
            {
                var result = await dbContext.Ratings
                    .Where(x => x.Id.Equals(id))
                    .ExecuteUpdateAsync(x => x.SetProperty(c => c.Score, score));

                if (result > 0)
                {
                    transaction.Commit();
                    exception = null;
                }
                else
                {
                    throw new Exception($"{id} is not updated");
                }
            }
            catch (Exception e)
            {
                exception = e;
                transaction.Rollback();
            }
        }

        return exception;
    }
}