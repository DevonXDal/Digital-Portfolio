using System.Linq.Expressions;

namespace Portfolio.Repositories.Interfaces
{
    /// <summary>
    /// IRepo serves the basis for any database accessing repositories providing the key methods needed to perform CRUD opeations
    /// on the database entities.
    /// </summary>
    /// <typeparam name="T">The database model type that CRUD will be used with the database to access</typeparam>
    public interface IRepo<T> where T : class
    {
        /// <summary>
        /// The entity to incorporate as an entry into the table specified by the type parameter
        /// </summary>
        /// <param name="entity">The entry to be inserted into the table</param>
        public int Count();

        /// <summary>
        /// Reads the data from the database table specified by the type parameter
        /// </summary>
        /// <param name="filter">What to filter the table results by. Defaults to no filter used</param>
        /// <param name="orderBy">How the results returned are to be ordered. Defaults to ordered by ID</param>
        /// <returns>A list of results given the specified parameters</returns>
        public IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        /// <summary>
        /// Gets all entries asynchronously.
        /// </summary>
        /// <returns>Every entry as a task</returns>
        public Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Grabs a particular entry in the table given its uniquely assigned ID
        /// </summary>
        /// <param name="id">The unique ID of the entry to find</param>
        /// <returns>The entry with that ID</returns>
        public T GetByID(object id);

        /// <summary>
        /// Grabs a particular entry in the table given its uniquely assigned ID asynchronously
        /// </summary>
        /// <param name="id">The unique ID of the entry to find</param>
        /// <returns>A task with the entry from that ID</returns>
        public Task<T> GetByIDAsync(object id);

        /// <summary>
        /// The entity to incorporate as an entry into the table specified by the type parameter
        /// </summary>
        /// <param name="entity">The entry to be inserted into the table</param>
        public void Insert(T entity);

        /// <summary>
        /// Soft deletes an entry from the database based on its assigned ID
        /// </summary>
        /// <param name="id">The unique ID of the entry</param>
        public void Delete(object id);

        /// <summary>
        /// Takes in an entity as a parameter to perform a soft delete operation on
        /// </summary>
        /// <param name="entityToDelete">The entity to be deleted</param>
        public void Delete(T entityToDelete);

        /// <summary>
        /// Updates the entity with the provided parameter
        /// </summary>
        /// <param name="entityToUpdate">The entity to be updated</param>
        public void Update(T entityToUpdate);
    }
}
