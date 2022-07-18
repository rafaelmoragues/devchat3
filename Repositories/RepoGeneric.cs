using devchat3.Data;
using System.Linq.Expressions;
using devchat3.Repositories.Interfaces;

namespace devchat3.Repositories
{
        public class RepoGeneric<T> : IRepoGeneric<T> where T : class
        {

            protected readonly Context context;

            public RepoGeneric(Context context)
            {
                this.context = context;
            }

            public IEnumerable<T> find(Expression<Func<T, bool>> predicate)
            {
                return context.Set<T>().Where(predicate);
            }

            public void Delete(int? id)
            {
                var entity = GetById(id);

                if (entity == null)
                {
                    throw new Exception("No se encontro objeto");
                }
                else
                {
                    context.Set<T>().Remove(entity);
                }

            }

            public IEnumerable<T> GetAll()
            {
                var aux = context.Set<T>().ToList();
                return aux;
            }

            public T GetById(int? id)
            {
                var aux = context.Set<T>().Find(id);
                return aux;
            }

            public void Insert(T entity)
            {
                context.Set<T>().Add(entity);
            }

            public void Update(T entity)
            {
                context.Set<T>().Update(entity);
            }


    }
    }

