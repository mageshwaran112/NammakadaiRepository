using Microsoft.AspNetCore.Mvc;
using Nammakadai.Core.Model;
using Nammakadai.UserManagement.BusinessLogic.Interface;

namespace Nammakadai.Serverless.UserManagement.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController ( ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpPost]
        [Route(ApiRoutes.Order.OrderPlacement)]
        public async Task OrderPlacement ([FromBody]  OrderRequest orderRequest)
        {
            await _orderService.OrderPlacementAsync (orderRequest);
        }
    }
}
