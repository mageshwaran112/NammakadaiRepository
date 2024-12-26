using Nammakadai.Core.Model;

namespace Nammakadai.Usermanagement.Repository.Interface
{
    public interface ICartRepository
    {
        Task AddToCart(CartRequest cartRequest);
        Task<string> GetCartDetailsAsync(int userId);
    }
}
