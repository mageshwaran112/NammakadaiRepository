using Nammakadai.Core.Model;
using Nammakadai.Usermanagement.Repository.Interface;
using Nammakadai.UserManagement.BusinessLosgic.Interface;
using Newtonsoft.Json;

namespace Nammakadai.UserManagement.BusinessLogic
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService ( IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ListItemsModel> GetListItems()
        {
            string productCategory = await _productRepository.GetListItems();
            return JsonConvert.DeserializeObject<ListItemsModel>(productCategory);
        }
    }
}
