namespace PosWebApplication.Models.Entities
{
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal GrandTotal => Items.Sum(i => i.LineTotal);
    }
}