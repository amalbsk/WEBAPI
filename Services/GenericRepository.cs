using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Webapi.Models;

namespace Webapi.Services
{
    /// <summary>
    /// Generic repository implementation for CRUD operations
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        /// <summary>
        /// Initializes a new instance of the GenericRepository
        /// </summary>
        /// <param name="context">The database context</param>
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Retrieves all entities
        /// </summary>
        /// <returns>List of entities</returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// Retrieves an entity by its ID
        /// <param name="id">The entity ID</param>
        /// <returns>The entity if found, otherwise null</returns>
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// Adds a new entity
        /// <param name="entity">The entity to add</param>
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        /// Updates an existing entity
        /// <param name="entity">The entity to update</param>
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        /// Deletes an entity by its ID
        /// <param name="id">The entity ID</param>
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
