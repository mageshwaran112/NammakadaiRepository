using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<IEnumerable<Core.Model.User>>> GetAll(int id)
        {
            var users = await _userService.GetAllUsers(id);
            return Ok(users);
        }

        [HttpPost]
        [Route(ApiRoutes.User.GetUsers)]
        public async Task<ActionResult<bool>> ValidateUser([FromBody] string phoneNumber)
        {
            var users = await _userService.ValidateUser(phoneNumber);
            return Ok(users);
        }

    }
}
