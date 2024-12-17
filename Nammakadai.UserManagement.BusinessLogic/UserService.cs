using Nammakadai.Core.Model;
using Nammakadai.UserManagement.BusinessLogic.Interface;
using Nammakadai.Usermanagement.Repository.Interface;
using AutoMapper;
using System.Data;
namespace Nammakadai.UserManagement.BusinessLogic
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
           
        }
        public async Task<User> GetUserById(int id)
        {
            var data = await _userRepository.GetUserById(id);

            return data;

        }
    }
}
