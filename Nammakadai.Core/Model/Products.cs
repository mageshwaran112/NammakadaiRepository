using Newtonsoft.Json;

namespace Nammakadai.Core.Model
{
    public class Products
    {
        [JsonProperty("productid")]
        public int ProductId { get; set; }

        [JsonProperty("subcategoryid")]
        public int SubCategoryId { get; set; }

        [JsonProperty("productname")]
        public string ProductName { get; set; }

        [JsonProperty("")]
        public string? ProductDescription { get; set; }

        [JsonProperty("originalprice")]
        public decimal OriginalProductPrice { get; set; }

        [JsonProperty("offerprice")]
        public decimal? OfferProductPrice { get; set; }

        [JsonProperty("stockquantity")]
        public decimal? StockQuantity { get; set; }
    }
}
