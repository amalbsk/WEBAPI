/// <summary>
/// Data transfer object for purchase transactions
/// </summary>
public class PurchaseDto
{
    /// <summary>
    /// ID of the item being purchased
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// Quantity of the item being purchased
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Total price of the purchase
    /// </summary>
    public decimal TotalPrice { get; set; }
}
