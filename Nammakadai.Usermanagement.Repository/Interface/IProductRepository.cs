using Nammakadai.Core.Model;

namespace Nammakadai.Usermanagement.Repository.Interface
{
    public interface IProductRepository
    {
        public Task<string> GetListItems();
    }
}
