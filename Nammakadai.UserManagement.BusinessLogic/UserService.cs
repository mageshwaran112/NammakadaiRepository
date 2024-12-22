using Nammakadai.Core.Model;
using Nammakadai.UserManagement.BusinessLogic.Interface;
using Nammakadai.Usermanagement.Repository.Interface;
using Vonage.Messaging;
using Vonage.Request;
using Vonage;
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
        public async Task<bool> ValidateUser(string phoneNumber)
        {
            var credentials = Credentials.FromApiKeyAndSecret("your_api_key", "your_api_secret");
            var client = new VonageClient(credentials);

            var response = client.SmsClient.SendAnSmsAsync(new SendSmsRequest
            {
                To = "recipient_phone_number",
                From = "your_brand_name",
                Text = "Hello from Vonage!"
            });
            return true;
        }
    }
}
