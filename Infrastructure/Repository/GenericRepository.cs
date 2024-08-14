using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domains.Models.BaseEntity;
using Infrastructure.Data;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>  where TEntity : Entity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        public async Task<IQueryable<TEntity>> GetAllAsync()
        {
            return _dbSet; // _dbSet consists of queryable data
        }
        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public IQueryable<TEntity> GetQueryable()
        {
            return _dbSet.AsQueryable();              
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task SoftDeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                var property = entity.GetType().GetProperty("IsDeleted"); // we do gettype to get the type of the entity and then get the property of the entity
                if (property != null && property.PropertyType == typeof(bool)) // now we check if the property is not null and the property type is boolean, if boolean  then we set the value of the property to true 
                {
                    property.SetValue(entity, true); // using SetValue we set the value of the property to true
                    _context.Entry(entity).State = EntityState.Modified;

                }
                else
                {

                    _dbSet.Remove(entity); // if the property is not boolean then we remove the entity 
                }
                await _context.SaveChangesAsync();

            }
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.FirstOrDefaultAsync(predicate);
        }
    }
}