using Nammakadai.Core.Model;

namespace Nammakadai.Usermanagement.Repository.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers(int id);
    }
}
