using Nammakadai.Core.Model;
using Nammakadai.Usermanagement.Repository.Interface;
using Nammakadai.UserManagement.BusinessLogic.Interface;

namespace Nammakadai.UserManagement.BusinessLogic
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        public CartService(ICartRepository cartRepository) 
        {
            _cartRepository = cartRepository;
        }

        public async Task AddToCart(CartRequest cartRequest)
        {
            await _cartRepository.AddToCart(cartRequest);
        }
    }
}
