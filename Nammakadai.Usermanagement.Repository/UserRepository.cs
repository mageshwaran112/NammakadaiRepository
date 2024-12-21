using Nammakadai.Common;
using Nammakadai.Common.Constants;
using Nammakadai.Core.Model;
using Nammakadai.Usermanagement.Repository.Interface;
using Npgsql;
using NpgsqlTypes;

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

        /// <summary>
        /// Get User Details using with UserId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User> GetUserByIdAsync(int id)
        {
            AdoHelper adoHelper = new AdoHelper(_connectionString);

            var sqlParameters = new[]
            {
                 new NpgsqlParameter("@User_ID", NpgsqlDbType.Integer) { Value = id }
            };

            var result = await adoHelper.ExecuteReaderAsync<User>(DBConstant.GetUserByIdAsync, sqlParameters);

            return result?.FirstOrDefault() as User;
        }

        /// <summary>
        /// Register UserDetails using PhoneNumber and Profile Edit.
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        public async Task<int?> SaveUserDetailAsync(User userRequest)
        {
            AdoHelper adoHelper = new AdoHelper(_connectionString);

            var sqlParameter = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@in_username",NpgsqlDbType.Varchar){Value =  userRequest.UserName},
                new NpgsqlParameter("@in_mailid",NpgsqlDbType.Varchar){Value =  userRequest.UserMail},
                new NpgsqlParameter("@in_phonenumber",NpgsqlDbType.Varchar){Value =  userRequest.PhoneNumber},
            };

            var result = Convert.ToInt32(await adoHelper.ExecuteScalarAsync(DBConstant.RegisterUser, sqlParameter));
            return result;
        }

        public async Task SaveGenerateOTPAsync(UserOtpDetail userOtpDetail)
        {
            AdoHelper adoHelper = new AdoHelper(_connectionString);

            var sqlParameter = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@In_UserId",NpgsqlDbType.Integer){Value = userOtpDetail.UserId},
                new NpgsqlParameter("@In_OTP",NpgsqlDbType.Varchar){Value = userOtpDetail.OTP}
            };

            await adoHelper.ExecuteNonQuery(DBConstant.SaveOTP, sqlParameter);
        }

    }
}
