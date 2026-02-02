using System.Collections.Concurrent;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data.SeedData;

// 1 StoreContext for all repository.
public class UnitOfWork(StoreContext context) : IUnitOfWork
{
    // ConcurrentDictionary prevent race condition when multiple thread access UOW
    private readonly ConcurrentDictionary<string, object> _repositories = [];
    public async Task<bool> Complete()
    {
        // .SaveChangeAsync = 1 transaction
        return await context.SaveChangesAsync() > 0;
    }

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        string type = typeof(TEntity).Name;
        
        return (IGenericRepository<TEntity>)_repositories.GetOrAdd(type, t =>
        {
            Type repoType = typeof(GenericRepository<>).MakeGenericType(typeof(TEntity));
            //Create an object that implement repo interface of type T with with context is pass to constructor param
            return Activator.CreateInstance(repoType, context) ?? throw new InvalidOperationException($"Could not create repository instance for {t}");
        });
    }
}
