using Microsoft.AspNetCore.Mvc;
using PosWebApplication.Models.DTOs;
using PosWebApplication.Models.Entities;
using PosWebApplication.Repositories;

namespace PosWebApplication.Controllers
{
    public class CatalogController : Controller
    {
        // US-01: Display all available products
        public IActionResult Index()
        {
            var products = ProductRepository.Products;
            return View(products);
        }

        // US-02: Add product to cart using AddToCartDTO
        [HttpPost]
        public IActionResult AddToCart(AddToCartDTO dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid submission. Please enter a valid quantity.";
                return RedirectToAction("Index");
            }

            var product = ProductRepository.Products.FirstOrDefault(p => p.Id == dto.ProductId);
            if (product == null)
            {
                return NotFound();
            }

            // Validate requested quantity against stock
            var existingCartItem = CartRepository.ActiveCart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
            int currentCartQuantity = existingCartItem?.Quantity ?? 0;
            int totalRequested = currentCartQuantity + dto.Quantity;

            if (totalRequested > product.StockQuantity)
            {
                TempData["ErrorMessage"] = $"Cannot add item. Only {product.StockQuantity - currentCartQuantity} more in stock!";
                return RedirectToAction("Index");
            }

            // Update cart logic
            if (existingCartItem != null)
            {
                existingCartItem.Quantity = totalRequested;
            }
            else
            {
                CartRepository.ActiveCart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = dto.Quantity
                });
            }

            TempData["SuccessMessage"] = $"Added '{product.Name}' to cart!";
            return RedirectToAction("Index");
        }
    }
}