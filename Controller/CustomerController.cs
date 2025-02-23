using Microsoft.AspNetCore.Mvc;
using Webapi.DTO;
using Webapi.Services;
using System.Threading.Tasks;
using System.Linq;


/// Controller for handling customer-related operations

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;
    private readonly InventoryService _inventoryService;
    private readonly PurchaseService _purchaseService;


    /// Initializes a new instance of the CustomerController

    /// <param name="customerService">Service for customer operations</param>
    /// <param name="inventoryService">Service for inventory operations</param>
    /// <param name="purchaseService">Service for purchase operations</param>
    public CustomerController(CustomerService customerService, InventoryService inventoryService, PurchaseService purchaseService)
    {
        _customerService = customerService;
        _inventoryService = inventoryService;
        _purchaseService = purchaseService;
    }


    /// Registers a new customer

    /// <param name="registerDto">Customer registration data</param>
    /// <returns>Result of the registration operation</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CustomerRegisterDto registerDto)
    {
        var result = await _customerService.RegisterAsync(registerDto);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result);
    }


    /// Authenticates a customer

    /// <param name="loginDto">Customer login credentials</param>
    /// <returns>Authentication result with customer details</returns>
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


    /// Retrieves all inventory items

    /// <returns>List of inventory items</returns>
    [HttpGet("inventory")]
    public async Task<IActionResult> GetInventoryItems()
    {
        var items = await _inventoryService.GetAllItemsAsync();
        return Ok(items);
    }


    /// Searches for an inventory item by name

    /// <param name="name">Name of the item to search for</param>
    /// <returns>Matching inventory item or not found</returns>
    [HttpGet("inventory/search")] 
    public async Task<IActionResult> SearchItem(string name)
    {
        var item = await _inventoryService.GetItemByNameAsync(name);
        if (item == null) return NotFound("Item not found.");
        return Ok(item);
    }


    /// Processes a purchase transaction

    /// <param name="purchaseDto">Purchase details</param>
    /// <param name="customerId">ID of the purchasing customer</param>
    /// <returns>Result of the purchase operation</returns>
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

    /// Retrieves purchase history for a customer

    /// <param name="customerId">ID of the customer</param>
    /// <returns>List of purchase records</returns>
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
