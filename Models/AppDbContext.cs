using Microsoft.EntityFrameworkCore;
using Webapi.Models;
using System.ComponentModel.DataAnnotations;

/// Represents the database context for the application
public class AppDbContext : DbContext
{
    /// Initializes a new instance of the AppDbContext
    /// <param name="options">The options to be used by the DbContext</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// Gets or sets the Users DbSet
    public virtual DbSet<User> Users { get; set; }

    /// Gets or sets the Inventories DbSet
    public DbSet<Inventory> Inventories { get; set; }

    /// Gets or sets the Customers DbSet
    public DbSet<Customer> Customers { get; set; }

    /// Gets or sets the Purchases DbSet
    public DbSet<Purchase> Purchases { get; set; }
}

/// Represents a user in the system
public class User
{
    /// Gets or sets the user's unique identifier
    [Required]
    public int Id { get; set; }

    /// Gets or sets the user's username
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, ErrorMessage = "Username can't be longer than 50 characters")]
    [Display(Name = "Username", Description = "Enter your unique username.")]
    public string? Username { get; set; }

    /// Gets or sets the user's password
    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Description = "Enter a secure password.")]
    public string? Password { get; set; }
}
