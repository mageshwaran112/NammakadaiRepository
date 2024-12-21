using Nammakadai.Common;
using Nammakadai.Common.Constants;
using Nammakadai.Core.Model;
using Nammakadai.Usermanagement.Repository.Interface;

namespace Nammakadai.Usermanagement.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDataBaseConfiguration _configuration;
        private string _connectionString;
        public ProductRepository(IDataBaseConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString();
        }

      public async Task<List<ListItem>> GetListItems()
{
    AdoHelper adoHelper = new AdoHelper(_connectionString);

    // Fetch raw data from the database
    var result = await adoHelper.ExecuteReaderAsync<dynamic>(DBConstant.GetListItem, null);

    // Directly map the result to ListItem
    var listItem = new ListItem
    {
        Categories = result
            .Where(r => r.categoryid != null)
            .Select(r => new Category
            {
                CategoryId = (int)r.categoryid,
                CategoryName = (string)r.categoryname
            })
            .ToList(),

        SubCategories = result
            .Where(r => r.subcategoryid != null)
            .Select(r => new SubCategory
            {
                SubCategoryId = (int)r.subcategoryid,
                SubCategoryName = (string)r.subcategoryname
            })
            .ToList(),

        Products = result
            .Where(r => r.productid != null)
            .Select(r => new Products
            {
                ProductId = (int)r.productid,
                ProductName = (string)r.productname,
                ProductDescription = (string)r.productdescription,
                OriginalProductPrice = (decimal)r.originalproductprice,
                OfferProductPrice = r.offerproductprice != null ? (decimal)r.offerproductprice : (decimal?)null
            })
            .ToList()
    };

    // Return the ListItem containing Categories, SubCategories, and Products
    return new List<ListItem> { listItem };
}


    }
}
