using System.ComponentModel.DataAnnotations;

namespace PosWebApplication.Models.DTOs
{
    public class CheckoutFormDTO
    {
        [Required(ErrorMessage = "Customer name is required.")]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [Display(Name = "Customer Email (Optional)")]
        public string? CustomerEmail { get; set; }
    }
}