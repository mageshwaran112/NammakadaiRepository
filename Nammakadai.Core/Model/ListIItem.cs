using Newtonsoft.Json;

namespace Nammakadai.Core.Model
{
    public class ListItemsModel
    {
        [JsonProperty("categories")]
        public List<Category> Categories { get; set; }

        [JsonProperty("subcategories")]
        public List<SubCategory> SubCategories { get; set; }

        [JsonProperty("products")]
        public List<Products> Products { get; set; }
    }
}
