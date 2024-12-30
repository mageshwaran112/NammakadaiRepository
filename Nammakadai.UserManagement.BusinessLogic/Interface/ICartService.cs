using Nammakadai.Core.Model;

namespace Nammakadai.UserManagement.BusinessLogic.Interface
{
    public interface ICartService
    {
        Task AddToCart (CartRequest cartRequest);
        Task<CartDetail> GetCartDetailsAsync(int userId);
    }
}
