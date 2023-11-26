using System.Linq.Expressions;

namespace Molo.Application.Common.Interfaces
{
    public interface IMoloDbRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<T> Get(Expression<Func<T, bool>> predicate);
        Task<T> Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<T> GetById(Guid id);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(Guid id);
    }
}
