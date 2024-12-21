using Nammakadai.Core.Model;
using Nammakadai.Usermanagement.Repository.Interface;
using Nammakadai.UserManagement.BusinessLosgic.Interface;

namespace Nammakadai.UserManagement.BusinessLogic
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService ( IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ListItem>> GetListItems()
        {
            return await _productRepository.GetListItems();
        }
    }
}
