using System.Linq.Expressions;

namespace devchat3.Repositories.Interfaces
{
    public interface IRepoGeneric<T> where T : class
    {
            IEnumerable<T> GetAll();
            T GetById(int? id);
            void Insert(T entity);
            void Update(T entity);
            void Delete(int? id);
            IEnumerable<T> find(Expression<Func<T, bool>> predicate);

        
    }
}
