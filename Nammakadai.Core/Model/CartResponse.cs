using Newtonsoft.Json;

namespace Nammakadai.Core.Model
{
    public class CartResponse
    {
        [JsonProperty("CartId")]
        public int CartId { get; set; }
        [JsonProperty("ProductId")]
        public int ProductId { get; set; }
        [JsonProperty("ProductName")]
        public string ProductName { get; set; }
        [JsonProperty("Description")]
        public string? Description { get; set; }
        [JsonProperty("OriginalPrice")]
        public Decimal OriginalPrice { get; set; }
        [JsonProperty("OfferPrice")]
        public Decimal? OfferPrice { get; set; }
        [JsonProperty("StockAvailability")]
        public bool IsInStock { get; set; }
        [JsonProperty("Quantity")]
        public int Quantity { get; set; }
    }

    public class CartDetail    {
        [JsonProperty("cartdetails")]
        public List<CartResponse> CartDetails { get; set; }
        [JsonProperty("totalprice")]
        public decimal TotalPrice { get; set; }

    }
}
