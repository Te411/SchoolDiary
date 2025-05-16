using System.Linq.Expressions;

namespace MyWebApplicationServer.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetById(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> Add(T entity);
        Task Update(T entity);
        Task Delete(Guid id);
    }
}
