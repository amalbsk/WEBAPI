using Webapi.DTO;
using Webapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

/// <summary>
/// Service for handling customer-related business logic
/// </summary>
public class CustomerService
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of the CustomerService
    /// </summary>
    /// <param name="context">Database context for customer operations</param>
    public CustomerService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Registers a new customer
    /// </summary>
    /// <param name="registerDto">Customer registration data</param>
    /// <returns>Result of the registration operation</returns>
    public async Task<Result> RegisterAsync(CustomerRegisterDto registerDto)
    {
        // Check if customer exists
        var existingCustomer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Username == registerDto.Username);
        if (existingCustomer != null)
        {
            return new Result { Success = false, Message = "Username already exists." };
        }

        // Create new customer
        var customer = new Customer
        {
            Username = registerDto.Username,
            Password = registerDto.Password, 
            Email = registerDto.Email
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return new Result { Success = true, Message = "Registration successful." };
    }

    /// <summary>
    /// Authenticates a customer
    /// </summary>
    /// <param name="loginDto">Customer login credentials</param>
    /// <returns>Authentication result with customer details</returns>
    public async Task<Result> LoginAsync(CustomerLoginDto loginDto)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Username == loginDto.Username);

        if (customer == null || customer.Password != loginDto.Password)
        {
            return new Result { Success = false, Message = "Invalid username or password." };
        }

        return new Result { 
            Success = true, 
            customerId = customer.Id, 
            Message = "Login successful." 
        };
    }
}

/// <summary>
/// Represents the result of a customer operation
/// </summary>
public class Result
{
    /// <summary>
    /// ID of the customer
    /// </summary>
    public int customerId { get; set; }

    /// <summary>
    /// Indicates if the operation was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Message describing the result of the operation
    /// </summary>
    public string Message { get; set; }
}
