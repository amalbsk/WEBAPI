using System;
using System.ComponentModel.DataAnnotations;

namespace Webapi.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public DateTime PurchaseDate { get; set; }

        // Navigation property
        public Customer Customer { get; set; }
    }
}
