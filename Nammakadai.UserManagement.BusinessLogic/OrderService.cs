using Nammakadai.Core.Model;
using Nammakadai.Usermanagement.Repository.Interface;
using Nammakadai.UserManagement.BusinessLogic.Interface;

namespace Nammakadai.UserManagement.BusinessLogic
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task OrderPlacementAsync(OrderRequest orderRequest)
        {
            await _orderRepository.OrderPlacementAsync(orderRequest);
        }
    }
}
