using Nammakadai.Core.Model;

namespace Nammakadai.Usermanagement.Repository.Interface
{
    public interface IOrderRepository
    {
        Task OrderPlacementAsync(OrderRequest orderRequest);
    }
}
