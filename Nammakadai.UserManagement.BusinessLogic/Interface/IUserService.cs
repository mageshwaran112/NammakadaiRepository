using Nammakadai.Core;
using Nammakadai.Core.Model;
namespace Nammakadai.UserManagement.BusinessLogic.Interface
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<int?> SaveUserDetailAsync(User userRequest);
    }
}
