using Nammakadai.Core;
using Nammakadai.Core.Model;
namespace Nammakadai.UserManagement.BusinessLogic.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers(int id);
    }
}
