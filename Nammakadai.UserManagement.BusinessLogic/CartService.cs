using Nammakadai.Core.Model;
using Nammakadai.Usermanagement.Repository;
using Nammakadai.Usermanagement.Repository.Interface;
using Nammakadai.UserManagement.BusinessLogic.Interface;
using Newtonsoft.Json;

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

        public async Task<List<CartResponse>> GetCartDetailsAsync(int userId)
        {
            string result = await _cartRepository.GetCartDetailsAsync(userId);
            return JsonConvert.DeserializeObject<List<CartResponse>>(result);
        }
    }
}
