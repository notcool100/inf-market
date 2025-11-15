using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using InfluencerMarketplace.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace InfluencerMarketplace.Infrastructure.Data
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly IConfiguration _configuration;
        protected readonly string _tableName;

        protected BaseRepository(IConfiguration configuration, string tableName)
        {
            _configuration = configuration;
            _tableName = tableName;
        }

        protected IDbConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            return new NpgsqlConnection(connectionString);
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            using var connection = CreateConnection();
            var sql = $"SELECT * FROM {_tableName} WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            using var connection = CreateConnection();
            var sql = $"SELECT * FROM {_tableName}";
            return await connection.QueryAsync<T>(sql);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(string sql, object parameters = null)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<T>(sql, parameters);
        }

        public virtual async Task<T> FirstOrDefaultAsync(string sql, object parameters = null)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }

        public abstract Task<Guid> CreateAsync(T entity);

        public abstract Task<bool> UpdateAsync(T entity);

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            using var connection = CreateConnection();
            var sql = $"DELETE FROM {_tableName} WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }
    }
}