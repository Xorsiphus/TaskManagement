using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskManagement.Data.Entities;

namespace TaskManagement.Data
{
    public interface IDbRepository
    {
        IQueryable<T> Get<T>() where T : class, IEntity;
        
        IQueryable<T> Get<T>(Expression<Func<T, bool>> expression) where T : class, IEntity;

        Task<T> Add<T>(T entity) where T : class, IEntity;

        Task<T> Update<T>(T entity) where T : class, IEntity;
        
        Task<T> Delete<T>(Guid id) where T : class, IEntity;

        Task Save();
    }
}