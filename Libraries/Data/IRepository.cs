using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    /// <summary>
    /// Repository
    /// </summary>
    public interface IRepository<T>
    {
        /// <summary>
        /// Id bilgisine göre domain objesi döner.
        /// </summary>
        /// <param name="id">Id bilgisi.</param>
        Task<T> Get(Guid id);

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <returns>The query.</returns>
        /// <param name="sql">Sql.</param>
        Task<List<T>> GetQuery(string sql);

        /// <summary>
        /// Insert the specified entity.
        /// </summary>
        /// <returns>The ınsert.</returns>
        /// <param name="entity">Entity.</param>
        Guid Insert(T entity);

        /// <summary>
        /// Update the specified entity.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="entity">Entity.</param>
        bool Update(T entity);

        /// <summary>
        /// Delete the specified id and force.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="force">If set to <c>true</c> force.</param>
        bool Delete(Guid id, bool force = false);

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <returns>The query.</returns>
        /// <param name="sql">Sql.</param>
        Task<int> ExecuteQuery(string sql);

        /// <summary>
        /// Gets the stored procedure async.
        /// </summary>
        /// <returns>The stored procedure async.</returns>
        /// <param name="procedure_name">Procedure name.</param>
        /// <param name="parameters">Parameters.</param>
        Task<List<T>> GetStoredProcedureAsync(string procedure_name, Dictionary<string, object> parameters = null);

        /// <summary>
        /// Executes the stored procedure.
        /// </summary>
        /// <returns>The stored procedure.</returns>
        /// <param name="procedure_name">Procedure name.</param>
        /// <param name="parameters">Parameters.</param>
        Task ExecuteStoredProcedure(string procedure_name, Dictionary<string, object> parameters = null);
    }
}