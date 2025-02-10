using System;

namespace Webapi.DTO
{
    public class PurchaseHistoryDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string ItemName { get; set; } // New property for item name
    }
}
