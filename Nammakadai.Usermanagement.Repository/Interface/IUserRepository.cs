using Nammakadai.Core.Model;
using System.Data;

namespace Nammakadai.Usermanagement.Repository.Interface
{
    public interface IUserRepository
    {
        Task<User> GetUserById(int id);
    }
}
