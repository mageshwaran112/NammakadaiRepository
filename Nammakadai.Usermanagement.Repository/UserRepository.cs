using Nammakadai.Core.Model;
using Nammakadai.Usermanagement.Repository.Interface;

namespace Nammakadai.Usermanagement.Repository
{
    public class UserRepository : IUserRepository
    {
        public UserRepository() 
        {
        
        }
        public Task<IEnumerable<User>> GetAllUsers(int id)
        {
            var users = new List<User>()
            {
                new User() { Id = 1 , UserName = "Nandhini"},
                new User() { Id = 2 , UserName = "Magesh"}
            };

            return Task.FromResult( users.AsEnumerable());

        }

    }
}
