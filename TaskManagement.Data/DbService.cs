using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data.Entities;

namespace TaskManagement.Data
{
    public class DbService : IDbRepository
    {
        private readonly AppDbContext _context;

        public DbService(AppDbContext context)
        {
            _context = context;
        }

        public void LoadChildrenRecursively<T>(T parent) where T : TaskEntity
        {
            _context.Entry(parent)
                .Collection(t => t.Children)
                .Load();

            foreach (var child in parent.Children)
            {
                LoadChildrenRecursively(child);
            }
        }

        public IQueryable<T> Get<T>() where T : class, IEntity
        {
            return _context.Set<T>().AsQueryable();
        }

        public IQueryable<T> Get<T>(Expression<Func<T, bool>> expression) where T : class, IEntity
        {
            return _context.Set<T>().Where(expression).AsQueryable();
        }
        
        public async Task<T> Add<T>(T entity) where T : class, IEntity
        {
            var addedEntity = await _context.Set<T>().AddAsync(entity);
            return addedEntity.Entity;
        }

        public async Task<T> Update<T>(T entity) where T : class, IEntity
        {
            var updatedEntity = await Task.Run(() => _context.Set<T>().Update(entity));
            return updatedEntity.Entity;
        }

        public async Task<T> Delete<T>(Guid id) where T : class, IEntity
        {
            var entityForDelete = await _context.Set<T>().Where(t => t.Id == id).FirstOrDefaultAsync();
            return await Task.Run(() => _context.Set<T>().Remove(entityForDelete).Entity);
        }
        
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}