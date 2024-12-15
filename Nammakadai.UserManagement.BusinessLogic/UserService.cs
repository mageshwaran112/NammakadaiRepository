using Nammakadai.Core.Model;
using Nammakadai.UserManagement.BusinessLogic.Interface;
using Nammakadai.Usermanagement.Repository.Interface;
namespace Nammakadai.UserManagement.BusinessLogic
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<User>> GetAllUsers(int id)
        {
            return await _userRepository.GetAllUsers(id);
        }
    }
}
