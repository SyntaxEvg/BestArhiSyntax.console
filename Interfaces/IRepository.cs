using Interfaces.Base;
using System.Collections;
using System.Linq.Expressions;

namespace Interfaces
{
    public interface IRepositoryAsync<T,Tid> where T : class,IEntity<Tid>
    {
        /// <summary>
        /// Стандартый CRUD,работает как FluentInterface(цепочка...)
        /// </summary>
        /// <returns></returns>
        T? GetByIdAsync(T id, CancellationToken token = default);
        T? AddAsync(T? entity, CancellationToken token = default);  
        T? UpdateAsync(T? entity, CancellationToken token = default);
        T? DeleteAsync(T? entity, CancellationToken token = default);

        /// <summary>
        /// возращает контекст базы данных
        /// </summary>
        IQueryable<T?> QueryableAsync(CancellationToken token = default);

        /// <summary>
        /// возращает единственную сущность 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
       Task <T?> FillterSingleAsync(Expression<Func<T?,bool>> expression, CancellationToken token =default);

        /// <summary>
        /// возращает все найденные сущности 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<IEnumerable<T?>> FillterAllAsync(Expression<Func<T,bool>> expression, CancellationToken token = default);

        Task<IEnumerable<T>> GetAllAsync(CancellationToken token = default);
    } 
} 