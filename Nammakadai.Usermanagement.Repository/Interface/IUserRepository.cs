using Nammakadai.Core.Model;

namespace Nammakadai.Usermanagement.Repository.Interface
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<int?> SaveUserDetailAsync(User userRequest);
        Task SaveGenerateOTPAsync (UserOtpDetail userOtpDetail);
    }
}
