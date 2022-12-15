using System.Linq.Expressions;

namespace Interfaces.Base.Base
{
    /// <summary>
    /// services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    /// если Вас не устроил  менеджер репозиториев можно использовать generic, и сущости в DI регистрировать способо выше, 
    /// но менеджер более собран и вы там получаете  доступ без регистрации в DI если к нему привыкнуть)  
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    /// <typeparam name="Tid"></typeparam>
    public interface IGenericRepository<TEntity, TDbContext,Tid> where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> wherePredicate, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> GetByIdAsync(Tid id);
        Task<IEnumerable<TRType>> GetByTypeAsync<TRType>(Expression<Func<TEntity, bool>> wherePredicate, Expression<Func<TEntity, TRType>> selectPredicate, params Expression<Func<TEntity, object>>[] includes);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);      
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        Task SaveChangesAsync(Tid userId);
    }
}