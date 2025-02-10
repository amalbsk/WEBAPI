using Microsoft.AspNetCore.Mvc;
using Webapi.DTO;
using Webapi.Services;
using System.Threading.Tasks;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;
    private readonly InventoryService _inventoryService;
    private readonly PurchaseService _purchaseService;

    public CustomerController(CustomerService customerService, InventoryService inventoryService, PurchaseService purchaseService)
    {
        _customerService = customerService;
        _inventoryService = inventoryService;
        _purchaseService = purchaseService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CustomerRegisterDto registerDto)
    {
        var result = await _customerService.RegisterAsync(registerDto);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] CustomerLoginDto loginDto)
    {
        var result = await _customerService.LoginAsync(loginDto);
        if (!result.Success) return Unauthorized(result.Message);

        
        return Ok(new 
        {
            CustomerId = result.customerId, 
            success = result.Success,
            message = result.Message,
            username = loginDto.Username 
        });
    }

    [HttpGet("inventory")]
    public async Task<IActionResult> GetInventoryItems()
    {
        var items = await _inventoryService.GetAllItemsAsync();
        return Ok(items);
    }

    [HttpGet("inventory/search")] 
    public async Task<IActionResult> SearchItem(string name)
    {
        var item = await _inventoryService.GetItemByNameAsync(name);
        if (item == null) return NotFound("Item not found.");
        return Ok(item);
    }

    [HttpPost("purchase")]
    public async Task<IActionResult> Purchase([FromBody] PurchaseDto purchaseDto, int customerId)
    {
        var result = await _purchaseService.PurchaseAsync(purchaseDto, customerId);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Message);
    }

    [HttpGet("{customerId}/purchase-history")]
    public async Task<IActionResult> GetPurchaseHistory(int customerId)
    {
        var purchases = await _purchaseService.GetPurchaseHistoryAsync(customerId);
        if (purchases == null || !purchases.Any())
        {
            return NotFound("No purchase history found for this customer.");
        }
        return Ok(purchases);
    }
}
