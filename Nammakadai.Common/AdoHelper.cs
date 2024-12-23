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
        private void AttachParameters(NpgsqlCommand command, NpgsqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (NpgsqlParameter p in commandParameters)
                {

                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }

                        command.Parameters.Add(new() { Value = p.Value });
                    }
                }
            }
        }

        private void PrepareCommand(NpgsqlCommand command, NpgsqlConnection connection, NpgsqlTransaction transaction, CommandType commandType, string commandText, NpgsqlParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            // Set the command type
            command.CommandType = commandType;

            // Attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }
        public async Task<object?> ExecuteScalarAsync(string functionName, params NpgsqlParameter[]? commandParameters)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                if (connection == null) throw new ArgumentNullException("connection");

                NpgsqlCommand cmd = new NpgsqlCommand();
                bool mustCloseConnection = false;
                PrepareCommand(cmd, connection, (NpgsqlTransaction)null, CommandType.StoredProcedure, functionName, commandParameters, out mustCloseConnection);

                // Await the async execution
                object retval = await cmd.ExecuteScalarAsync();

                cmd.Parameters.Clear();

                if (mustCloseConnection)
                    await connection.CloseAsync();

                return retval;
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

             var   parameterPlaceholders = sqlParameters.Select(p => p.ParameterName).ToList();
  
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
