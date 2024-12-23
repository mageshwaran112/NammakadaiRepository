using Nammakadai.Common;
using Nammakadai.Common.Constants;
using Nammakadai.Core.Model;
using Nammakadai.Usermanagement.Repository.Interface;
using Newtonsoft.Json;

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

        public async Task<List<ListItemsModel>> GetListItems()
        {
            AdoHelper adoHelper = new AdoHelper(_connectionString);
            List<ListItemsModel> responce = new ();

            // Fetch raw data from the database
            var result = await adoHelper.ExecuteScalarAsync(DBConstant.GetCategoryDetails, null);

            var listitem = JsonConvert.DeserializeObject<List<Category>>(result.ToString());
          
            // responce= listitem;


            if (result != null)
            {
                //var row = result[0]; // Assuming result is a list of rows
                //var categoryJson = row["category_result"].ToString();
                //var subCategoryJson = row["subcategory_result"].ToString();
                //var productJson = row["product_result"].ToString();

                // Deserialize JSON to objects
                //Categories = JsonConvert.DeserializeObject<List<Category>>(categoryJson);
                //SubCategories = JsonConvert.DeserializeObject<List<SubCategory>>(subCategoryJson);
                //Products = JsonConvert.DeserializeObject<List<Products>>(productJson);
            }

            // Directly map the result to ListItem
            //var listItem = new ListItem
            //{
            //    Categories = result
            //        .Where(r => r.categoryid != null)
            //        .Select(r => new Category
            //        {
            //            CategoryId = (int)r.categoryid,
            //            CategoryName = (string)r.categoryname
            //        })
            //        .ToList(),

            //    SubCategories = result
            //        .Where(r => r.subcategoryid != null)
            //        .Select(r => new SubCategory
            //        {
            //            SubCategoryId = (int)r.subcategoryid,
            //            SubCategoryName = (string)r.subcategoryname
            //        })
            //        .ToList(),

            //    Products = result
            //        .Where(r => r.productid != null)
            //        .Select(r => new Products
            //        {
            //            ProductId = (int)r.productid,
            //            ProductName = (string)r.productname,
            //            ProductDescription = (string)r.productdescription,
            //            OriginalProductPrice = (decimal)r.originalproductprice,
            //            OfferProductPrice = r.offerproductprice != null ? (decimal)r.offerproductprice : (decimal?)null
            //        })
            //        .ToList()
            //};

            // Return the ListItem containing Categories, SubCategories, and Products
            return null;
        }


    }
}
