using PosWebApplication.Models.Entities;

namespace PosWebApplication.Repositories
{
    public static class ProductRepository
    {
        public static List<Product> Products { get; private set; } = new List<Product>
        {
            new Product { Id = 1, Name = "Super Mario Bros. (NES)", Price = 49.99m, StockQuantity = 5 },
            new Product { Id = 2, Name = "The Legend of Zelda (NES)", Price = 89.99m, StockQuantity = 2 },
            new Product { Id = 3, Name = "Sonic the Hedgehog (Genesis)", Price = 34.99m, StockQuantity = 8 },
            new Product { Id = 4, Name = "Chrono Trigger (SNES)", Price = 199.99m, StockQuantity = 1 },
            new Product { Id = 5, Name = "Pokemon Red Version (GB)", Price = 79.99m, StockQuantity = 4 },
            new Product { Id = 6, Name = "Castlevania: SOTN (PS1)", Price = 120.00m, StockQuantity = 0 },
            new Product { Id = 7, Name = "N64 Console (Charcoal)", Price = 149.99m, StockQuantity = 3 },
            new Product { Id = 8, Name = "GameCube Controller", Price = 45.00m, StockQuantity = 6 }
        };
    }
}