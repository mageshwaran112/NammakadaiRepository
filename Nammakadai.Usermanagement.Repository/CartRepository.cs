using Nammakadai.Common;
using Nammakadai.Common.Constants;
using Nammakadai.Core.Model;
using Nammakadai.Usermanagement.Repository.Interface;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;

namespace Nammakadai.Usermanagement.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly IDataBaseConfiguration _configuration;
        private string _connectionString;
        public CartRepository(IDataBaseConfiguration configuration) 
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString();
        }

        public async Task AddToCart(CartRequest cartRequest)
        {
            AdoHelper adoHelper = new AdoHelper(_connectionString);

            var sqlParameter = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@in_productid",NpgsqlDbType.Integer){Value = cartRequest.ProductId},
                new NpgsqlParameter("@in_quantity",NpgsqlDbType.Integer){Value = cartRequest.Quantity},
                new NpgsqlParameter("@in_userid",NpgsqlDbType.Integer){Value = cartRequest.UserId}
            };

            await adoHelper.ExecuteNonQuery(DBConstant.AddCart, sqlParameter);
        }

        public async Task<string> GetCartDetailsAsync(int userId)
        {
            AdoHelper adoHelper = new AdoHelper(_connectionString);

            var sqlParameter = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@in_userid",NpgsqlDbType.Integer){ Value = userId},
            };

            var result = await adoHelper.ExecuteScalarAsync(DBConstant.GetCartDetail, sqlParameter);
            return result.ToString();
        }
    }
}
