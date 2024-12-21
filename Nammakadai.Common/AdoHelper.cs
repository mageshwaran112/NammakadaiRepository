using Npgsql;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Data.SqlClient;
using System.Dynamic;

namespace Nammakadai.Common
{
    public class AdoHelper
    {
        private readonly string _connectionString;
        private NpgsqlConnection _connection;
        private NpgsqlTransaction? _transaction;
        private bool disposed = false;

        public AdoHelper(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new NpgsqlConnection(connectionString);
        }
        ~AdoHelper()
        {
            Dispose(false);
        }
        public void Dispose() 
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task OpenConnectionString()
        {
            if (_connection is null)
            {
                _connection = new NpgsqlConnection(_connectionString);
            }
            if(_connection.State != ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }
        }

        public async Task CloseConnectionString()
        {
            if (_connection is null)
            {
                return;
            }
            if (_connection.State != ConnectionState.Closed)
            {
                await _connection.CloseAsync();
            }
        }

        public async Task BeginTransactionAsync()
        {
            if(_transaction is null)
            {
                _transaction = (NpgsqlTransaction)await _connection.BeginTransactionAsync();
            }
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction is not null)
            {
                await _transaction.CommitAsync();
            }
        }
        public async Task<T?> GetDataAsync<T>(string storeProcedure, NpgsqlParameter[] sqlParameters) where T : class, new()
        {
            await OpenConnectionString();

            using var sqlCommand = new NpgsqlCommand($"SELECT * FROM {storeProcedure}({string.Join(", ", sqlParameters.Select(p => p.ParameterName))})", _connection)
            {
                CommandType = CommandType.Text,
                Transaction = _transaction
            };

            if (sqlParameters != null)
            {
                sqlCommand.Parameters.AddRange(sqlParameters);
            }

            using NpgsqlDataAdapter sqlDataAdapter = new NpgsqlDataAdapter(sqlCommand);

            if (typeof(T) == typeof(DataSet))
            {
                var ds = new DataSet();
                sqlDataAdapter.Fill(ds);
                sqlCommand.Parameters.Clear();

                return ds as T;
            }
            else if (typeof(T) == typeof(DataTable))
            {
                var dt = new DataTable();
                sqlDataAdapter.Fill(dt);
                sqlCommand.Parameters.Clear();

                return dt as T;
            }

            return null; 
        }
        public DataTable ExecuteQuery(string query)
        {
            DataTable dataTable = new DataTable();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }

            return dataTable;
        }

        public async Task<List<T>> ExecuteReaderAsync<T>(string functionName, NpgsqlParameter[]? sqlParameters) where T : class, new()
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = sqlParameters == null || sqlParameters.Length == 0
                ? $"SELECT * FROM {functionName}()"
                : $"SELECT * FROM {functionName}({string.Join(",", sqlParameters.Select(p => p.ParameterName))})";

            await using var command = new NpgsqlCommand(query, connection)
            {
                CommandType = CommandType.Text
            };

            if (sqlParameters != null)
            {
                command.Parameters.AddRange(sqlParameters);
            }

            await using var reader = await command.ExecuteReaderAsync();
            var results = new List<T>();

            while (await reader.ReadAsync())
            {
                var instance = new T();
                foreach (var property in typeof(T).GetProperties())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                    {
                        property.SetValue(instance, reader[property.Name]);
                    }
                }
                results.Add(instance);
            }

            return results;
        }


        public async Task<object> ExecuteScalarAsync(string functionName, NpgsqlParameter[] sqlParameters)
        {
            await OpenConnectionString();

            string query = GetFunctionParameter(functionName, sqlParameters);
            
            using (var command = new NpgsqlCommand(query, _connection))
            {
                if (sqlParameters != null)
                {
                    command.Parameters.AddRange(sqlParameters);
                }

                var result = await command.ExecuteScalarAsync();

                return result;
            }

        }
        public async Task ExecuteNonQuery(string functionName, NpgsqlParameter[] sqlParameters)
        {
            await OpenConnectionString();

            string query = GetFunctionParameter(functionName, sqlParameters);

            using (var command = new NpgsqlCommand(query, _connection))
            {
                if (sqlParameters != null)
                {
                    command.Parameters.AddRange(sqlParameters);
                }

                var result = await command.ExecuteNonQueryAsync();

                command.Parameters.Clear();
            }

        }

        private string GetFunctionParameter (string functionName, NpgsqlParameter[] sqlParameters)
        {
            var parameterPlaceholders = sqlParameters.Select(p => p.ParameterName).ToList();

            string query = $"SELECT {functionName}({string.Join(",", parameterPlaceholders)})";

            return query;
        }
        protected virtual void Dispose (bool disposing)
        {
            if(!disposed)
            {
                if(disposing)
                {
                    if(_transaction != null)
                    {
                        _transaction.Dispose();
                    }
                    if(_connection != null)
                    {
                        if(_connection.State != ConnectionState.Closed)
                        {
                            _connection.Close();
                        }
                        _connection.Dispose();
                    }
                }
                disposed = true;
            }
        }
    }
}
