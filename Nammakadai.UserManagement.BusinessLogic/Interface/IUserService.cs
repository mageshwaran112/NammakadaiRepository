using Nammakadai.Core;
using Nammakadai.Core.Model;
namespace Nammakadai.UserManagement.BusinessLogic.Interface
{
    public interface IUserService
    {
        Task<User> GetUserById(int id);
    }
}
