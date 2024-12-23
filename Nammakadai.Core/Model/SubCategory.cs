using Newtonsoft.Json;

namespace Nammakadai.Core.Model
{
    public class SubCategory
    {
        [JsonProperty("subcategoryid")]
        public int SubCategoryId { get; set; }

        [JsonProperty("subcategoryname")]
        public string SubCategoryName { get; set; }

        [JsonProperty("categoryid")]
        public int CategoryId { get; set; }
    }
}
