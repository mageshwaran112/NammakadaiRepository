using Microsoft.AspNetCore.Mvc;
using Nammakadai.Core.Model;
using Nammakadai.UserManagement.BusinessLogic.Interface;
using Nammakadai.UserManagement.BusinessLosgic.Interface;

namespace Nammakadai.Serverless.UserManagement.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<UserController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet]
        [Route(ApiRoutes.Product.GetListItem)]
        public async Task<ActionResult<ListItem>> GetAllListItem()
        {
            var list = await _productService.GetListItems();
            return Ok(list);
        }
    }
}
