using PosWebApplication.Models.Entities;

namespace PosWebApplication.Repositories
{
    public static class CartRepository
    {
        public static ShoppingCart ActiveCart { get; private set; } = new ShoppingCart();

        public static void ClearCart()
        {
            ActiveCart = new ShoppingCart();
        }
    }
}