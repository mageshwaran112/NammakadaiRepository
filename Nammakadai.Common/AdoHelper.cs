using Npgsql;
using System.Data;

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

        public async Task<T?> ExecuteReaderAsync<T>(string functionName, NpgsqlParameter[] sqlParameters) where T : class, new()
        {
            await OpenConnectionString();

            using var sqlCommand = new NpgsqlCommand($"SELECT * FROM {functionName}({string.Join("", sqlParameters.Select(p => p.ParameterName))})", _connection)
            {
                CommandType = CommandType.Text,
                Transaction = _transaction
            };

            if (sqlParameters != null)
            {
                sqlCommand.Parameters.AddRange(sqlParameters);
            }

            using var reader = await sqlCommand.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var instance = new T();
                foreach (var property in typeof(T).GetProperties())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                    {
                        property.SetValue(instance, reader[property.Name]);
                    }
                }
                return instance;
            }

            return null;
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
