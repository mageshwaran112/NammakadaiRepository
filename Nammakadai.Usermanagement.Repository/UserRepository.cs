using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Nammakadai.Common;
using Nammakadai.Common.Constants;
using Nammakadai.Core.Model;
using Nammakadai.Usermanagement.Repository.Interface;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Runtime.InteropServices;

namespace Nammakadai.Usermanagement.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDataBaseConfiguration _configuration;
        private string _connectionString;
        public UserRepository(IDataBaseConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString();
        }
        public async Task<User> GetUserById(int id)
        {
            AdoHelper adoHelper = new AdoHelper(_connectionString);

            var sqlParameters = new[]
            {
                new NpgsqlParameter("@User_ID", NpgsqlDbType.Integer) { Value = id }
            };

            return await adoHelper.QuerySingleOrDefaultAsync<User>(DBConstant.GetUserById, sqlParameters);
        }
    }
}
