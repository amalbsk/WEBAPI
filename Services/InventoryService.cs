using Webapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class InventoryService
{
    private readonly AppDbContext _context;

    public InventoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Inventory>> GetAllItemsAsync()
    {
        return await _context.Inventories.ToListAsync();
    }

    public async Task<Inventory> GetItemByNameAsync(string name)
    {
        return await _context.Inventories.FirstOrDefaultAsync(i => i.Name == name);
    }
}
