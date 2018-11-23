using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Core.ConfigReader;
using Dapper;
using Dapper.Contrib.Extensions;
using Model;
using static Dapper.SqlMapper;

namespace Data
{
    /// <summary>
    /// Dapper repository.
    /// </summary>
    public class DapperRepository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// The config.
        /// </summary>
        readonly IConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Data.DapperRepository`1"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        public DapperRepository(IConfig config)
        {
            _config = config;
            _connection.ConnectionString = _config.GetValue<string>("SqlConnectionString");
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>The connection.</value>
        IDbConnection _connection => new SqlConnection(_config.GetValue<string>("SqlConnectionString"));

        /// <summary>
        /// Id bilgisine göre domain objesi döner.
        /// </summary>
        /// <param name="id">Id bilgisi.</param>
        public async Task<T> Get(Guid id)
        {
            T result;
            using (IDbConnection con = _connection)
            {
                result = await con.QueryFirstOrDefaultAsync<T>("SELECT * FROM " + typeof(T).Name + " (NOLOCK) WHERE Id = @Id", new { @Id = id });
            }
            return result;
        }

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <returns>The query.</returns>
        /// <param name="sql">Sql.</param>
        public async Task<List<T>> GetQuery(string sql)
        {
            using (IDbConnection con = _connection)
            {
                SqlConnection.ClearAllPools();
                var result = await con.QueryAsync<T>(sql);
                con.Close();
                con.Dispose();
                if (result != null && result.Any())
                {
                    return result.ToList();
                }
                return null;
            }
        }

        /// <summary>
        /// Insert the specified entity.
        /// </summary>
        /// <returns>The ınsert.</returns>
        /// <param name="entity">Entity.</param>
        public Guid Insert(T entity)
        {
            using (IDbConnection con = _connection)
            {
                con.Insert(entity);
                return (entity as BaseEntity).Id;
            }
        }

        /// <summary>
        /// Update the specified entity.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="entity">Entity.</param>
        public bool Update(T entity)
        {
            using (IDbConnection con = _connection)
            {
                return con.Update(entity);
            }
        }

        /// <summary>
        /// Delete the specified id and force.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="force">If set to <c>true</c> force.</param>
        public bool Delete(Guid id, bool force = false)
        {
            var entity = Get(id).Result as BaseEntity;
            if (entity == null)
            {
                return false;
            }

            if (!force)
            {
                entity.Deleted = true;
                return Update(entity as T);
            }

            using (IDbConnection con = _connection)
            {
                return con.Delete(entity as T);
            }
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <returns>The query.</returns>
        /// <param name="sql">Sql.</param>
        public async Task<int> ExecuteQuery(string sql)
        {
            using (IDbConnection con = _connection)
            {
                SqlConnection.ClearAllPools();
                var result = await con.ExecuteAsync(sql);
                con.Close();
                con.Dispose();
                return result;
            }
        }

        /// <summary>
        /// Executes the stored procedure.
        /// </summary>
        /// <returns>The stored procedure.</returns>
        /// <param name="procedure_name">Procedure name.</param>
        /// <param name="parameters">Parameters.</param>
        public async Task ExecuteStoredProcedure(string procedure_name, Dictionary<string, object> parameters = null)
        {
            using (IDbConnection con = _connection)
            {
                SqlConnection.ClearAllPools();
                if (parameters == null)
                {
                    await con.ExecuteAsync(procedure_name, null, null, 65530, CommandType.StoredProcedure);
                }
                else
                {
                    var ps = new DynamicParameters();
                    foreach (var p in parameters)
                    {
                        ps.Add(p.Key, p.Value);
                    }
                    await con.ExecuteAsync(procedure_name, ps, null, 65530, CommandType.StoredProcedure);
                }
                con.Close();
                con.Dispose();
            }
        }

        /// <summary>
        /// Gets the stored procedure async.
        /// </summary>
        /// <returns>The stored procedure async.</returns>
        /// <param name="procedure_name">Procedure name.</param>
        /// <param name="parameters">Parameters.</param>
        public async Task<List<T>> GetStoredProcedureAsync(string procedure_name, Dictionary<string, object> parameters = null)
        {
            IEnumerable<T> list;
            using (IDbConnection con = _connection)
            {
                SqlConnection.ClearAllPools();
                if (parameters == null)
                {
                    list = await con.QueryAsync<T>(procedure_name, null, null, 65530, CommandType.StoredProcedure);
                }
                else
                {
                    var ps = new DynamicParameters();
                    foreach (var p in parameters)
                    {
                        ps.Add(p.Key, p.Value);
                    }
                    list = await con.QueryAsync<T>(procedure_name, ps, null, 65530, CommandType.StoredProcedure);
                }
                con.Close();
                con.Dispose();
            }
            return list.ToList();
        }
    }
}