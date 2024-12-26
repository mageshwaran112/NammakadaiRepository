namespace Nammakadai.ApiRoutes
{
    public static class User
    {
        public const string GetUsers = "api/User/GetUsersList/{id}";
        public const string SaveUser = "api/User/SaveUser";
    }
    public static class Product
    {
        public const string GetListItem = "api/Product/GetListItem";
    }
    public static class Cart
    {
        public const string AddToCart = "api/Cart/AddToCart";
    }
}
