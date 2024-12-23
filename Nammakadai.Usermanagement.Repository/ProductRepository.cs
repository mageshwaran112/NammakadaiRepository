using Nammakadai.Common;
using Nammakadai.Common.Constants;
using Nammakadai.Core.Model;
using Nammakadai.Usermanagement.Repository.Interface;
using Newtonsoft.Json;

namespace Nammakadai.Usermanagement.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDataBaseConfiguration _configuration;
        private string _connectionString;
        public ProductRepository(IDataBaseConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString();
        }

        public async Task<string> GetListItems()
        {
            AdoHelper adoHelper = new AdoHelper(_connectionString);
            var result = await adoHelper.ExecuteScalarAsync(DBConstant.GetCategoryDetails, null);
            return result.ToString();
        }


    }
}
