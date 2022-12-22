using App.DDD.Domain.Base.EntityBase;
using Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Interfaces.Base.Base
{
    public interface IServiceManagerBase
    {
        IlangDictionaryScopedService IlangDictionary { get; }

    }



    /// <summary>
    /// Репозиторий для работы с сущностями можно использовать не только для БД, для БД создается менедждер репозитоия см. DAL
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="Tid"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IRepositoryBaseAsync<TEntity, Tid, TDbContext>
                                                                where TEntity : IEntity<Tid>
                                                                where Tid : IComparable<Tid>, IEquatable<Tid>
    {
        /// <summary>
        /// возращает контекст базы данных
        /// </summary>
         TDbContext GetDbContext(CancellationToken token = default);


        Task<TEntity?> GetByIdAsync(TEntity TEntity, CancellationToken token = default);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default);
        Task<TEntity?> AddAsync(TEntity? entity, CancellationToken token = default);
        /// <summary>
        /// Добавляет много сущностей сразу
        /// </summary>
        /// <returns></returns>
        Task<bool> AddRangeAsync(IEnumerable<TEntity?> entity, CancellationToken token = default);

        Task<TEntity?> UpdateAsync(TEntity? entity, CancellationToken token = default);

        Task<TEntity?> DeleteAsync(TEntity? entity, CancellationToken token = default);
        /// <summary>
        /// Поиск сущности по ID
        /// </summary>
        /// <returns></returns>
        Task<TEntity?> FindAsync(Tid id, CancellationToken token = default);


        /// <summary>
        /// Фильтрует и возращает единственную сущность 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<TEntity?> FillterSingleAsync(Expression<Func<TEntity?, bool>> expression, CancellationToken token = default);
        /// <summary>
        /// возращает все найденные сущности 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<IQueryable<TEntity?>> FillterAllAsync(Expression<Func<TEntity, bool>> expression, CancellationToken token = default);

        /// <summary>
        /// Выполнить прямую команду sql
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task Execute(string command, params object[] parameters);
    }





    //public class GenericRepository<TEntity, TDbContext> : IGenericRepository<TEntity, TDbContext> where TEntity : class
    //{
    //    private readonly TDbContext _context; // EF db context class.
    //    private readonly DbSet<TEntity> _entityItems;

    //    protected GenericRepository(IDataContext context)
    //    {
    //        _context = context;
    //        _entityItems = _context.Set<TEntity>();
    //    }

    //    public async Task AddAsync(TEntity entity)
    //    {
    //        await _entityItems.AddAsync(entity);
    //    }

    //    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    //    {
    //        await _entityItems.AddRangeAsync(entities);
    //    }

    //    public async Task<IEnumerable<TEntity>> GetAllAsync()
    //    {
    //        return await _entityItems.ToListAsync();
    //    }

    //    public async Task<IEnumerable<TEntity>> GetByAsync(
    //        Expression<Func<TEntity, bool>> wherePredicate,
    //        params Expression<Func<TEntity, object>>[] includes)
    //    {
    //        var query = this.GetQuery(wherePredicate, includes);

    //        return await query.ToListAsync();
    //    }

    //    public async Task<TEntity> GetByIdAsync(int id)
    //    {
    //        return await _entityItems.FindAsync(id);
    //    }

    //    public async Task<IEnumerable<TRType>> GetByTypeAsync<TRType>(
    //        Expression<Func<TEntity, bool>> wherePredicate,
    //        Expression<Func<TEntity, TRType>> selectPredicate,
    //        params Expression<Func<TEntity, object>>[] includes)
    //    {
    //        var query = this.GetQuery(wherePredicate, includes);
    //        return await query.Select(selectPredicate).ToListAsync();
    //    }

    //    public void Remove(TEntity entity)
    //    {
    //        _entityItems.Remove(entity);
    //    }

    //    public void RemoveRange(IEnumerable<TEntity> entities)
    //    {
    //        _entityItems.RemoveRange(entities);
    //    }

    //    public virtual async Task SaveChangesAsync(int userId)
    //    {
    //        await _context.SaveChangesAsync(CancellationToken.None);
    //    }

    //    public void Update(TEntity entity)
    //    {
    //        var entry = _context.Entry(entity);
    //        entry.State = EntityState.Modified;
    //    }

    //    public void UpdateRange(IEnumerable<TEntity> entities)
    //    {
    //        _entityItems.UpdateRange(entities);
    //    }

    //    private IQueryable<TEntity> GetAllIncludes(Expression<Func<TEntity, object>>[] includes)
    //    {
    //        var queryable = _entityItems.AsQueryable();
    //        return includes.Aggregate(
    //            queryable,
    //            (current, includedProperty) => current.Include(includedProperty));
    //    }

    //    private IQueryable<TEntity> GetQuery(
    //        Expression<Func<TEntity, bool>> wherePredicate,
    //        Expression<Func<TEntity, object>>[] includes)
    //    {
    //        IQueryable<TEntity> query;
    //        if (includes != null)
    //        {
    //            var includedEntity = this.GetAllIncludes(includes);
    //            query = includedEntity.Where(wherePredicate);
    //        }
    //        else
    //        {
    //            query = _entityItems.Where(wherePredicate);
    //        }

    //        return query;
    //    }
    //}
}