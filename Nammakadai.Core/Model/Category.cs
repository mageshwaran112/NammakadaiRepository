using Newtonsoft.Json;

namespace Nammakadai.Core.Model
{
    public class Category
    {
        [JsonProperty("categoryid")]
        public int CategoryId { get; set; }

        [JsonProperty("categoryname")]
        public string CategoryName { get; set; }
    }
}
