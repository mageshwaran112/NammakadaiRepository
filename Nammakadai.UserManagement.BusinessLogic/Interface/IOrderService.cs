using Nammakadai.Core.Model;

namespace Nammakadai.UserManagement.BusinessLogic.Interface
{
    public interface IOrderService
    {
        Task OrderPlacementAsync (OrderRequest orderRequest);
    }
}
