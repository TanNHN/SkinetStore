using System.Collections.Concurrent;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data.SeedData;

public class UnitOfWork(StoreContext context) : IUnitOfWork
{
    private readonly ConcurrentDictionary<string, object> _repositories = [];
    public async Task<bool> Complete()
    {
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
