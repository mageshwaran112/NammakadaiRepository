namespace Nammakadai.Core.Model
{
    public class OrderRequest
    {
        public int UserId { get; set; }
        public List<ProductDetails> ProductDetails { get; set; }
    }

    public class ProductDetails
    {
        public int ProductIds { get; set; }
        public int Quantities { get; set; }
        public Decimal Price { get; set; }
    }
}
