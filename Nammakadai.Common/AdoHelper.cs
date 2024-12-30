using Npgsql;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Data.SqlClient;
using System.Dynamic;
using System.Collections;

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


        //public async Task ExecuteNonQuery(string functionName, NpgsqlParameter[] sqlParameters)
        //{
        //    await OpenConnectionString();

        //    string query = GetFunctionParameter(functionName, sqlParameters);

        //    using (var command = new NpgsqlCommand(query, _connection))
        //    {
        //        if (sqlParameters != null)
        //        {
        //            command.Parameters.AddRange(sqlParameters);
        //        }

        //        var result = await command.ExecuteNonQueryAsync();

        //        command.Parameters.Clear();
        //    }

        //}

     
        /// <summary>
        /// Execute a NpgsqlCommand (that returns no resultset) against the specified NpgsqlConnection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new NpgsqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">A valid NpgsqlConnection</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
        /// <returns>An int representing the number of rows affected by the command</returns>
        public async Task<int>  ExecuteNonQuery(string commandText, params NpgsqlParameter[] commandParameters)
        {
            if (_connection == null) throw new ArgumentNullException("connection");

            // Create a command and prepare it for execution
            NpgsqlCommand cmd = new NpgsqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, _connection, (NpgsqlTransaction)null,CommandType.StoredProcedure, commandText, commandParameters, out mustCloseConnection);
            cmd.CommandTimeout = 300;
            // Finally, execute the command
            int retval = cmd.ExecuteNonQuery();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();
            if (mustCloseConnection)
                _connection.Close();
            return retval;
        }
  

        /// <summary>
        /// This method assigns an array of values to an array of SqlParameters
        /// </summary>
        /// <param name="commandParameters">Array of SqlParameters to be assigned values</param>
        /// <param name="parameterValues">Array of objects holding the values to be assigned</param>
        private void AssignParameterValues(NpgsqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                // Do nothing if we get no data
                return;
            }
            var pj = commandParameters.Where(x => x.Direction == ParameterDirection.Input).Count();
            // We must have the same number of values as we pave parameters to put them in
            if (commandParameters.Where(x => x.Direction == ParameterDirection.Input).Count() != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            // Iterate through the SqlParameters, assigning the values from the corresponding position in the 
            // value array
            for (int i = 0, j = commandParameters.Where(x => x.Direction == ParameterDirection.Input).Count(); i < j; i++)
            {
                // If the current array value derives from IDbDataParameter, then assign its Value property
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if (paramInstance.Value == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if (parameterValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
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
    /// <summary>
    /// PLPGSQLHelperParameterCache provides functions to leverage a  cache of procedure parameters, and the
    /// ability to discover parameters for stored procedures at run-time.
    /// </summary>
    public sealed class PLPGSQLHelperParameterCache
    {
        #region private methods, variables, and constructors

        //Since this class provides only static methods, make the default constructor private to prevent 
        //instances from being created with "new PLPGSQLHelperParameterCache()"
        private PLPGSQLHelperParameterCache() { }

        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// Resolve at run time the appropriate set of SqlParameters for a stored procedure
        /// </summary>
        /// <param name="connection">A valid NpgsqlConnection object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">Whether or not to include their return value parameter</param>
        /// <returns>The parameter array discovered.</returns>
		private static NpgsqlParameter[] DiscoverSpParameterSet(NpgsqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            NpgsqlCommand cmd = new NpgsqlCommand(spName, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            connection.Open();
            NpgsqlCommandBuilder.DeriveParameters(cmd);
            connection.Close();

            if (!includeReturnValueParameter)
            {
                cmd.Parameters.RemoveAt(0);
            }

            NpgsqlParameter[] discoveredParameters = new NpgsqlParameter[cmd.Parameters.Count];

            cmd.Parameters.CopyTo(discoveredParameters, 0);

            // Init the parameters with a DBNull value
            foreach (NpgsqlParameter discoveredParameter in discoveredParameters)
            {
                discoveredParameter.Value = DBNull.Value;
            }
            return discoveredParameters;
        }

        /// <summary>
        /// Deep copy of cached NpgsqlParameter array
        /// </summary>
        /// <param name="originalParameters"></param>
        /// <returns></returns>
        private static NpgsqlParameter[] CloneParameters(NpgsqlParameter[] originalParameters)
        {
            NpgsqlParameter[] clonedParameters = new NpgsqlParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (NpgsqlParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }

        #endregion private methods, variables, and constructors

        #region caching functions

        /// <summary>
        /// Add parameter array to the cache
        /// </summary>
        /// <param name="connectionString">A valid connection string for a NpgsqlConnection</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">An array of SqlParamters to be cached</param>
        public static void CacheParameterSet(string connectionString, string commandText, params NpgsqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            string hashKey = connectionString + ":" + commandText;

            paramCache[hashKey] = commandParameters;
        }

        /// <summary>
        /// Retrieve a parameter array from the cache
        /// </summary>
        /// <param name="connectionString">A valid connection string for a NpgsqlConnection</param>
        /// <param name="commandText">The stored procedure name or T-SQL command</param>
        /// <returns>An array of SqlParamters</returns>
        public static NpgsqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            string hashKey = connectionString + ":" + commandText;

            NpgsqlParameter[] cachedParameters = paramCache[hashKey] as NpgsqlParameter[];
            if (cachedParameters == null)
            {
                return null;
            }
            else
            {
                return CloneParameters(cachedParameters);
            }
        }

        #endregion caching functions

        #region Parameter Discovery Functions

        /// <summary>
        /// Retrieves the set of SqlParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a NpgsqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <returns>An array of SqlParameters</returns>
        public static NpgsqlParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return GetSpParameterSet(connectionString, spName, false);
        }

        /// <summary>
        /// Retrieves the set of SqlParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connectionString">A valid connection string for a NpgsqlConnection</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
        /// <returns>An array of SqlParameters</returns>
        public static NpgsqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                return GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
            }
        }

        /// <summary>
        /// Retrieves the set of SqlParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connection">A valid NpgsqlConnection object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <returns>An array of SqlParameters</returns>
        internal static NpgsqlParameter[] GetSpParameterSet(NpgsqlConnection connection, string spName)
        {
            return GetSpParameterSet(connection, spName, false);
        }

        /// <summary>
        /// Retrieves the set of SqlParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connection">A valid NpgsqlConnection object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
        /// <returns>An array of SqlParameters</returns>
        internal static NpgsqlParameter[] GetSpParameterSet(NpgsqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            using (NpgsqlConnection clonedConnection = (NpgsqlConnection)((ICloneable)connection).Clone())
            {
                return GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
            }
        }

        /// <summary>
        /// Retrieves the set of SqlParameters appropriate for the stored procedure
        /// </summary>
        /// <param name="connection">A valid NpgsqlConnection object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
        /// <returns>An array of SqlParameters</returns>
        private static NpgsqlParameter[] GetSpParameterSetInternal(NpgsqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            string hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

            NpgsqlParameter[] cachedParameters;

            cachedParameters = paramCache[hashKey] as NpgsqlParameter[];
            if (cachedParameters == null)
            {
                NpgsqlParameter[] spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
                paramCache[hashKey] = spParameters;
                cachedParameters = spParameters;
            }

            return CloneParameters(cachedParameters);
        }

        #endregion Parameter Discovery Functions

    }
}
