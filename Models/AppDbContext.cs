using Microsoft.EntityFrameworkCore;
using Webapi.Models;
using System.ComponentModel.DataAnnotations;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public virtual DbSet<User> Users { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Customer> Customers { get; set; } // Added DbSet for Customer
    public DbSet<Purchase> Purchases { get; set; } // Added DbSet for Purchase
}

public class User
{
    [Required]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, ErrorMessage = "Username can't be longer than 50 characters")]
    [Display(Name = "Username", Description = "Enter your unique username.")]
    public string? Username { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
    [DataType(DataType.Password)] 
    [Display(Name = "Password", Description = "Enter a secure password.")]
    public string? Password { get; set; } 
}
