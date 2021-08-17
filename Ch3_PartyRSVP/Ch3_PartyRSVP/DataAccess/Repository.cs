/*
MIT License

Copyright(c) 2021 Kyle Givler
https://github.com/JoyfulReaper

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using Ch3_PartyRSVP.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ch3_PartyRSVP.DataAccess
{
    public abstract class Repository<T> : IRepository<T> where T: DatabaseEntity
    {
        protected string TableName { get; set; }
        private readonly ILogger<Repository<T>> _logger;
        private readonly IConfiguration _configuration;

        private Type _dbConnectionType = typeof(SqliteConnection);
        private string _connectionString;

        public Repository(ILogger<Repository<T>> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            string typeName = typeof(T).ToString();
            TableName = typeName.Substring(typeName.LastIndexOf('.') + 1);

            _connectionString = _configuration.GetConnectionString("Default");

            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
            SqlMapper.AddTypeHandler(new GuidHandler());
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }

        public abstract Task AddAsync(T entity);

        public abstract Task DeleteAsync(T entity);

        public abstract Task EditAsync(T entity);

        protected void Execute(string query, object parameters = null)
        {
            try
            {
                using (IDbConnection connection = (IDbConnection)Activator.CreateInstance(_dbConnectionType, _connectionString))
                {
                    connection.Execute(query, parameters);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repository: Execute Exception Thrown");
            }
        }

        protected async Task ExecuteAsync(string query, object parameters = null)
        {
            try
            {
                using (IDbConnection connection = (IDbConnection)Activator.CreateInstance(_dbConnectionType, _connectionString))
                {
                    await connection.ExecuteAsync(query, parameters);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repository: ExecuteAsync Exception Thrown");
            }
        }

        protected IEnumerable<T> Query<T>(string query, object parameters = null)
        {
            try
            {
                using (IDbConnection connection = (IDbConnection)Activator.CreateInstance(_dbConnectionType, _connectionString))
                {
                    return connection.Query<T>(query, parameters);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repository: Query Exception Thrown");
                return default;
            }
        }

        protected T QueryFirst<T>(string query, object parameters = null)
        {
            try
            {
                using (IDbConnection connection = (IDbConnection)Activator.CreateInstance(_dbConnectionType, _connectionString))
                {
                    return connection.QueryFirst<T>(query, parameters);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repository: QueryFirst Exception Thrown");
                return default;
            }
        }

        protected T QueryFirstOrDefault<T> (string query, object parameters = null)
        {
            try
            {
                using (IDbConnection connection = (IDbConnection)Activator.CreateInstance(_dbConnectionType, _connectionString))
                {
                    return connection.QueryFirstOrDefault<T>(query, parameters);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repository: QueryFirstOrDefault Exception Thrown");
                return default;
            }
        }

        protected T QuerySingle<T>(string query, object parameters = null)
        {
            try
            {
                using (IDbConnection connection = (IDbConnection)Activator.CreateInstance(_dbConnectionType, _connectionString))
                {
                    return connection.QuerySingle<T>(query, parameters);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repository: QuerySingle Exception Thrown");
                return default;
            }
        }

        protected T QuerySingleOrDefault<T>(string query, object parameters = null)
        {
            try
            {
                using (IDbConnection connection = (IDbConnection)Activator.CreateInstance(_dbConnectionType, _connectionString))
                {
                    return connection.QuerySingleOrDefault<T>(query, parameters);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repository: QuerySingleOrDefault Exception Thrown");
                return default;
            }
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(string query, object parameters = null)
        {
            try
            {
                using (IDbConnection connection = (IDbConnection)Activator.CreateInstance(_dbConnectionType, _connectionString))
                {
                    var result = await connection.QueryAsync<T>(query, parameters);
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repository: QueryAsync Exception Thrown");
                return default;
            }
        }

        protected async Task<T> QueryFirstAsync<T>(string query, object parameters = null)
        {
            try
            {
                using (IDbConnection connection = (IDbConnection)Activator.CreateInstance(_dbConnectionType, _connectionString))
                {
                    return await connection.QueryFirstAsync<T>(query, parameters);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repository: QueryFirstAsync Exception Thrown");
                return default;
            }
        }

        protected async Task<T> QueryFirstOrDefaultAsync<T>(string query, object parameters = null)
        {
            try
            {
                using (IDbConnection connection = (IDbConnection)Activator.CreateInstance(_dbConnectionType, _connectionString))
                {
                    return await connection.QueryFirstOrDefaultAsync<T>(query, parameters);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repository: QueryFirstOrDefaultAsync Exception Thrown");
                return default;
            }
        }

        protected async Task<T> QuerySingleAsync<T>(string query, object parameters = null)
        {
            try
            {
                using (IDbConnection connection = (IDbConnection)Activator.CreateInstance(_dbConnectionType, _connectionString))
                {
                    return await connection.QuerySingleAsync<T>(query, parameters);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repository: QuerySingleAsync Exception Thrown");
                return default;
            }
        }

        protected async Task<T> QuerySingleOrDefaultAsync<T>(string query, object parameters = null)
        {
            try
            {
                using (IDbConnection connection = (IDbConnection)Activator.CreateInstance(_dbConnectionType, _connectionString))
                {
                    return await connection.QuerySingleOrDefaultAsync<T>(query, parameters);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repository: QuerySingleOrDefaultAsync Exception Thrown");
                return default;
            }
        }
    }
}
