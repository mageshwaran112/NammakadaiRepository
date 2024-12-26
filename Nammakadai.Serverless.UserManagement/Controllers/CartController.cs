using Microsoft.AspNetCore.Mvc;
using Nammakadai.Core.Model;
using Nammakadai.UserManagement.BusinessLogic.Interface;

namespace Nammakadai.Serverless.UserManagement.Controllers
{
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private readonly ICartService _cartService;

        public CartController (ILogger<CartController> logger, ICartService cartService)
        {
            _logger = logger;
            _cartService = cartService;
        }

        [HttpPost]
        [Route(ApiRoutes.Cart.AddToCart)]
        public async Task AddCart (CartRequest cartRequest)
        {
            await _cartService.AddToCart(cartRequest);
        }

        [HttpGet]
        [Route(ApiRoutes.Cart.GetCartDetails)]
        public async Task<IActionResult> GetCartDetailsAsync(int userId)
        {
           var result = await _cartService.GetCartDetailsAsync(userId);
           return Ok(result);
        }
    }
}
