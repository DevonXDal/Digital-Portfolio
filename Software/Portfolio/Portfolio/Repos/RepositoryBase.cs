﻿
using Portfolio.Data;
using Portfolio.Models.MainDb.NotMapped;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Portfolio.Repositories.Interfaces;

namespace Portfolio.Data
{
    /// <summary>
    /// The <c>RepositoryBase</c> class uses generics in order to manipulate data of the table specified during its creation.
    /// </summary>
    /// <typeparam name="EntityBase">The particular table's model that needs to have its data modified</typeparam>
    public class RepositoryBase<T>: IRepo<T> where T : EntityBase
    {
        protected ApplicationDbContext _context;
        protected DbSet<T> _dbSet;

        /// <summary>
        /// Creates an instance of the <c>RepositoryBase</c> class and provides it the database object
        /// </summary>
        /// <param name="context">The database object to be used to manipulate the database</param>
        public RepositoryBase(ApplicationDbContext context)
        {
            this._context = context;
            this._dbSet = context.Set<T>();
        }

        /// <summary>
        /// The entity to incorporate as an entry into the table specified by the type parameter
        /// </summary>
        /// <param name="entity">The entry to be inserted into the table</param>
        public virtual int Count()
        {
            return _dbSet.Count();
        }

        /// <summary>
        /// Reads the data from the database table specified by the type parameter
        /// </summary>
        /// <param name="filter">What to filter the table results by. Defaults to no filter used</param>
        /// <param name="orderBy">How the results returned are to be ordered. Defaults to ordered by ID</param>
        /// <returns>A list of results given the specified parameters</returns>
        public virtual IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        /// <summary>
        /// Gets all entries asynchronously.
        /// </summary>
        /// <returns>Every entry as a task</returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Grabs a particular entry in the table given its uniquely assigned ID
        /// </summary>
        /// <param name="id">The unique ID of the entry to find</param>
        /// <returns>The entry with that ID</returns>
        public virtual T GetByID(object id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Grabs a particular entry in the table given its uniquely assigned ID asynchronously
        /// </summary>
        /// <param name="id">The unique ID of the entry to find</param>
        /// <returns>A task with the entry from that ID</returns>
        public virtual async Task<T> GetByIDAsync(object id)
        {
            return await _dbSet.FindAsync(id).AsTask();
        }

        /// <summary>
        /// The entity to incorporate as an entry into the table specified by the type parameter
        /// </summary>
        /// <param name="entity">The entry to be inserted into the table</param>
        public virtual void Insert(T entity)
        {
            entity.Created = DateTime.UtcNow;
            entity.LastModified = entity.Created;
            entity.IsDeleted = false;
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Soft deletes an entry from the database based on its assigned ID
        /// </summary>
        /// <param name="id">The unique ID of the entry</param>
        public virtual void Delete(object id)
        {
            T entityToDelete = _dbSet.Find(id);
            entityToDelete.IsDeleted = true;
            entityToDelete.LastModified = DateTime.UtcNow;
            Update(entityToDelete);
        }

        /// <summary>
        /// Takes in an entity as a parameter to perform a soft delete operation on
        /// </summary>
        /// <param name="entityToDelete">The entity to be deleted</param>
        public virtual void Delete(T entityToDelete)
        {
            entityToDelete.IsDeleted = true;
            entityToDelete.LastModified = DateTime.UtcNow;
            Update(entityToDelete);
        }

        /// <summary>
        /// Updates the entity with the provided parameter
        /// </summary>
        /// <param name="entityToUpdate">The entity to be updated</param>
        public virtual void Update(T entityToUpdate)
        {
            entityToUpdate.LastModified = DateTime.UtcNow;
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}