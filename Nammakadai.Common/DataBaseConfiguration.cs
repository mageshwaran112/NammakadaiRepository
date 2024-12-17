using Microsoft.Extensions.Configuration;

namespace Nammakadai.Common
{
    public interface IDataBaseConfiguration
    {
        string GetConnectionString();
    }
    public class DataBaseConfiguration : IDataBaseConfiguration
    {
        private readonly  IConfiguration _configuration;
        public DataBaseConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString()
        {
            return _configuration.GetConnectionString("DBConnection");
        }
    }
}
