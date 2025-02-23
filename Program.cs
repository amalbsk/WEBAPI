// Title        : Inventory Management Application API
// Author       : Amal Biju
// Created on   : 22/01/2025
// Updated on   : 05/02/2025
// Reviewed by  : Sabapathi Shanmugam
// Reviewed at  : 01/02/2025



// Description  : Main entry point for the Inventory Management API. 
//                Configures services, middleware, and application settings.

using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Webapi.Services;
using Webapi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Webapi.Utilities;  
using Serilog;

// Initialize the web application builder
var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

// Register core services
builder.Services.AddControllers(); // Add support for MVC controllers
builder.Services.AddEndpointsApiExplorer(); // Enable API endpoint exploration
builder.Services.AddSwaggerGen(); // Add Swagger documentation support
builder.Services.AddScoped<ITokenService, TokenService>(); // Register token service

// Configure Swagger documentation
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations(); // Enable Swagger annotations
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Inventory Management API",
        Version = "v1"
    });

    // Configure JWT authentication in Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and your JWT token. Example: Bearer eyJhbGciOiJIUzI1NiIs..."
    });

    // Add security requirement for JWT
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin() // Allow requests from any origin
               .AllowAnyMethod() // Allow any HTTP method
               .AllowAnyHeader(); // Allow any header
    });
});

// Configure AutoMapper for DTO mapping
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register application services
builder.Services.AddScoped<CustomerService>(); // Register customer service
builder.Services.AddScoped<PurchaseService>(); // Register purchase service
builder.Services.AddScoped<InventoryService>(); // Register inventory service

// Configure JWT Authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Validate token issuer
            ValidateAudience = true, // Validate token audience
            ValidateLifetime = true, // Validate token expiration
            ValidateIssuerSigningKey = true, // Validate signing key
            ValidIssuer = jwtSettings["Issuer"], // Set valid issuer
            ValidAudience = jwtSettings["Audience"], // Set valid audience
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])) // Set signing key
        };

        // Configure authentication events
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Authentication failed. Invalid or expired token.");
            },
            OnChallenge = context =>
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Access denied. Please provide a valid token.");
            }
        };
    });

// Register TokenService for JWT generation
builder.Services.AddSingleton<TokenService>();

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // Log to console
    .WriteTo.File("logs/webapi-.txt", rollingInterval: RollingInterval.Day) // Log to file with daily rotation
    .CreateLogger();

builder.Host.UseSerilog(); // Use Serilog for logging

// Build the application
var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger in development
    app.UseSwaggerUI(); // Enable Swagger UI
}

app.UseHttpsRedirection(); // Enforce HTTPS
app.UseMiddleware<Webapi.Middleware.ErrorHandlingMiddleware>(); // Add custom error handling
app.UseCors("AllowAll"); // Apply CORS policy

app.UseMiddleware<Webapi.Middleware.RequestLoggingMiddleware>(); // Add request logging
app.UseHttpsRedirection(); // Enforce HTTPS

app.UseAuthentication(); // Enable authentication
app.UseAuthorization(); // Enable authorization

app.MapControllers(); // Map controller routes

// Start the application
app.Run();
