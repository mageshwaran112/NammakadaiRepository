using Microsoft.AspNetCore.Mvc;
using Nammakadai.Core.Model;
using Nammakadai.UserManagement.BusinessLogic.Interface;

namespace Nammakadai.Serverless.UserManagement.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        [Route(ApiRoutes.User.GetUsers)]
        public async Task<ActionResult<User>> GetAll(int id)
        {
            var users = await _userService.GetUserByIdAsync(id);
            return Ok(users);
        }

        [HttpPost]
        [Route(ApiRoutes.User.SaveUser)]
        public async Task<IActionResult> SaveUser(User userRequest)
        {
            var result = await _userService.SaveUserDetailAsync(userRequest);           
            return Ok(result);
        }
    }
}
