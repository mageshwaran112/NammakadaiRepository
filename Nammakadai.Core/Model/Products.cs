namespace Nammakadai.Core.Model
{
    public class Products
    {
        public int ProductId { get; set; }
        public int SubCategoryId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal OriginalProductPrice { get; set; }
        public decimal? OfferProductPrice { get; set; }
    }
}
