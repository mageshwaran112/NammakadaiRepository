using Nammakadai.Core.Model;

namespace Nammakadai.UserManagement.BusinessLosgic.Interface
{
    public interface IProductService
    {
        public Task<List<ListItemsModel>> GetListItems();
    }
}
