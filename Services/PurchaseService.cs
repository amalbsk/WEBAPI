using Webapi.DTO;
using Webapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class PurchaseService
{
    private readonly AppDbContext _context;

    public PurchaseService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result> PurchaseAsync(PurchaseDto purchaseDto, int customerId)
    {
        var item = await _context.Inventories.FindAsync(purchaseDto.ItemId);
        if (item == null || item.Quantity < purchaseDto.Quantity)
        {
            return new Result { Success = false, Message = "Insufficient stock or item not found." };
        }

        // Update stock
        item.Quantity -= purchaseDto.Quantity;

        // Create a new purchase entry
        var purchase = new Purchase
        {
            CustomerId = customerId,
            ItemId = purchaseDto.ItemId,
            Quantity = purchaseDto.Quantity,
            PurchaseDate = DateTime.UtcNow
        };

        _context.Purchases.Add(purchase); // Add purchase to the context

        await _context.SaveChangesAsync(); // Save changes to the database

        return new Result { Success = true, Message = "Purchase successful." };
    }

    public async Task<IEnumerable<PurchaseHistoryDto>> GetPurchaseHistoryAsync(int customerId)
    {
        return await _context.Purchases
            .Where(p => p.CustomerId == customerId)
            .Join(_context.Inventories,
                  purchase => purchase.ItemId,
                  inventory => inventory.ItemId,
                  (purchase, inventory) => new PurchaseHistoryDto
                  {
                      Id = purchase.Id,
                      CustomerId = purchase.CustomerId,
                      ItemId = purchase.ItemId,
                      Quantity = purchase.Quantity,
                      PurchaseDate = purchase.PurchaseDate,
                      ItemName = inventory.Name // Include item name
                  })
            .ToListAsync();
    }
}
