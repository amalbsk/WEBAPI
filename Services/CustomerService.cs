using Webapi.DTO;
using Webapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class CustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context)
    {
        _context = context;
    }

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

    public async Task<Result> LoginAsync(CustomerLoginDto loginDto)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Username == loginDto.Username);

        if (customer == null || customer.Password != loginDto.Password) // Check password directly
        {
            return new Result { Success = false, Message = "Invalid username or password." };
        }

        // Generate JWT token (implementation not shown here)
        // var token = GenerateJwtToken(customer);

        return new Result { Success = true, customerId = customer.Id , Message = "Login successful." /*, Token = token */ };
    }
}

public class Result
{
    public int customerId { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    // public string Token { get; set; } // Uncomment if using JWT
}
