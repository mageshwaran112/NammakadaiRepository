using AutoMapper;
using Nammakadai.Common.GenerateSms;
using Nammakadai.Core.Model;
using Nammakadai.Usermanagement.Repository.Interface;
using Nammakadai.UserManagement.BusinessLogic.Interface;
namespace Nammakadai.UserManagement.BusinessLogic
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly OTPFunction _otpFunction;

        public UserService(IUserRepository userRepository,IMapper mapper,OTPFunction otpFunction)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _otpFunction = otpFunction;
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            var data = await _userRepository.GetUserByIdAsync(id);

            return data;

        }

        public async Task<int?> SaveUserDetailAsync(User userRequest)
        {
            int result = await _userRepository.SaveUserDetailAsync(userRequest) ?? 0;

            var otp = _otpFunction.SendOTP( userRequest.PhoneNumber);
            
            UserOtpDetail userOtpDetail = new UserOtpDetail
            {
                UserId = result,
                OTP = otp.ToString(),
            };
            await _userRepository.SaveGenerateOTPAsync(userOtpDetail);

            return result;
        }
    }
}
