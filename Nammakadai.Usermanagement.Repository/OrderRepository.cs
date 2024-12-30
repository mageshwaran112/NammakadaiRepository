using Nammakadai.Common;
using Nammakadai.Common.Constants;
using Nammakadai.Core.Model;
using Nammakadai.Usermanagement.Repository.Interface;
using Newtonsoft.Json;
using Npgsql;

namespace Nammakadai.Usermanagement.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDataBaseConfiguration _configuration;
        private string _connectionString;

        public OrderRepository(IDataBaseConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString();
        }
        public async Task OrderPlacementAsync(OrderRequest orderRequest)
        {
            AdoHelper adoHelper = new AdoHelper(_connectionString);

            var sqlParameter = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@ip_payload",NpgsqlTypes.NpgsqlDbType.Text){Value = JsonConvert.SerializeObject(orderRequest)}
            };

            var result = await adoHelper.ExecuteNonQuery(DBConstant.OrderPlacement, sqlParameter);

        }
    }
}
