using Microsoft.AspNetCore.Mvc;
using PosWebApplication.Models.DTOs;
using PosWebApplication.Repositories;

namespace PosWebApplication.Controllers
{
    public class CartController : Controller
    {
        // US-03: View active shopping cart
        public IActionResult Index()
        {
            var cart = CartRepository.ActiveCart;
            return View(cart);
        }

        // US-03: Update item quantity in cart using UpdateCartDTO
        [HttpPost]
        public IActionResult UpdateQuantity(UpdateCartDTO dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid quantity provided.";
                return RedirectToAction("Index");
            }

            var product = ProductRepository.Products.FirstOrDefault(p => p.Id == dto.ProductId);
            var cartItem = CartRepository.ActiveCart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);

            if (product == null || cartItem == null)
            {
                return NotFound();
            }

            // Validate against static product inventory stock
            if (dto.Quantity > product.StockQuantity)
            {
                TempData["ErrorMessage"] = $"Cannot update quantity. Only {product.StockQuantity} available in stock!";
                return RedirectToAction("Index");
            }

            cartItem.Quantity = dto.Quantity;
            TempData["SuccessMessage"] = $"Updated quantity for '{cartItem.ProductName}'.";
            return RedirectToAction("Index");
        }

        // US-04: Remove item from cart
        [HttpPost]
        public IActionResult RemoveItem(int productId)
        {
            var cartItem = CartRepository.ActiveCart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem != null)
            {
                CartRepository.ActiveCart.Items.Remove(cartItem);
                TempData["SuccessMessage"] = $"Removed '{cartItem.ProductName}' from cart.";
            }

            return RedirectToAction("Index");
        }
    }
}