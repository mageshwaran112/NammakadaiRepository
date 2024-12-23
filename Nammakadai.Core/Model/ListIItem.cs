using Newtonsoft.Json;

namespace Nammakadai.Core.Model
{
    public class ListItemsModel
    {
        [JsonProperty("category_result")]
        public List<Category> Categories { get; set; }
        //public List<SubCategory> SubCategories { get; set; }
        //public List<Products> Products { get; set; }
    }
}
