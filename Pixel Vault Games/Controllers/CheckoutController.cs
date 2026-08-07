using Microsoft.AspNetCore.Mvc;
using PosWebApplication.Models.DTOs;
using PosWebApplication.Models.Entities;
using PosWebApplication.Repositories;
using System.Transactions;

namespace PosWebApplication.Controllers
{
    public class CheckoutController : Controller
    {
        // US-05: Render Checkout Form
        public IActionResult Index()
        {
            var cart = CartRepository.ActiveCart;

            // Empty Cart Validation: Cannot proceed if 0 items
            if (!cart.Items.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty. Please add products before checking out.";
                return RedirectToAction("Index", "Catalog");
            }

            return View(new CheckoutFormDTO());
        }

        // US-05: Process Checkout Submission
        [HttpPost]
        public IActionResult ProcessCheckout(CheckoutFormDTO dto)
        {
            var cart = CartRepository.ActiveCart;

            // 1. Server-side validation check for empty cart[cite: 1]
            if (!cart.Items.Any())
            {
                ModelState.AddModelError("", "Cannot process checkout because your shopping cart is empty.");
            }

            // 2. Validate Form DTO via Data Annotations[cite: 1]
            if (!ModelState.IsValid)
            {
                return View("Index", dto);
            }

            // 3. Create Transaction Record[cite: 1]
            int newTransactionId = TransactionRepository.Transactions.Count + 1;

            var transaction = new PosWebApplication.Models.Entities.Transaction
            {
                TransactionId = newTransactionId,
                CustomerName = dto.CustomerName,
                Date = DateTime.Now,
                TotalAmount = cart.GrandTotal,
                PurchasedItems = cart.Items.Select(item => new CartItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };

            // 4. Deduct Stock permanently from Product Repository[cite: 1]
            foreach (var cartItem in cart.Items)
            {
                var product = ProductRepository.Products.FirstOrDefault(p => p.Id == cartItem.ProductId);
                if (product != null)
                {
                    product.StockQuantity -= cartItem.Quantity;
                }
            }

            // 5. Store Transaction and Clear Cart[cite: 1]
            TransactionRepository.AddTransaction(transaction);
            CartRepository.ClearCart();

            TempData["SuccessMessage"] = $"Sale #{transaction.TransactionId} completed successfully!";
            return RedirectToAction("Details", new { id = transaction.TransactionId });
        }

        // US-06: Sales History Overview
        public IActionResult History()
        {
            var transactions = TransactionRepository.Transactions;
            return View(transactions);
        }

        // US-06: Detailed Transaction Receipt Page
        public IActionResult Details(int id)
        {
            var transaction = TransactionRepository.Transactions.FirstOrDefault(t => t.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }
    }
}