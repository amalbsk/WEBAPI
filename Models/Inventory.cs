using System.ComponentModel.DataAnnotations;

namespace Webapi.Models
{
    public class Inventory
    {
        [Key]
        public int ItemId { get; set; }
        [Required(ErrorMessage = "Item name is required")]
        [StringLength(100, ErrorMessage = "Item name can't be longer than 100 characters")]
        public string Name { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive number")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
    }
}
