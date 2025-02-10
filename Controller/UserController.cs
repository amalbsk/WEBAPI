using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Webapi.Services;
using Webapi.DTO;
using Swashbuckle.AspNetCore.Annotations;

namespace Webapi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public UserController(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // Endpoint for user registration
        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register a new user", Description = "Provide username and password to register.")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (_context.Users.Any(u => u.Username == userDto.Username))
                    return BadRequest("User already exists");

                // Create a new User object from the DTO
                var user = new User
                {
                    Username = userDto.Username,
                    Password = userDto.Password
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok("User registered successfully");
            }
            catch (Exception exception)
            {
                return StatusCode(500, $"Internal server error: {exception.Message}");
            }
        }

        // Endpoint for user login and JWT generation
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Authenticate user and generate JWT", Description = "Provide username and password for login.")]
        public IActionResult Login([FromBody] UserLoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);



                var user = _context.Users.SingleOrDefault(u => u.Username == loginDto.Username && u.Password == loginDto.Password);
                if (user == null)
                    return Unauthorized("Invalid credentials");

                // Generate JWT Token using the TokenService
                var token = _tokenService.GenerateToken(user.Username);

                return Ok(new { Token = token });
            }
            catch (Exception exception)
            {
                return StatusCode(500, $"Internal server error: {exception.Message}");
            }
        }

        // Protected endpoint to test authorization
        [Authorize]
        [HttpGet("protected")]
        public IActionResult Protected()
        {
            var username = User.Identity?.Name;
            return Ok($"You are authorized! Welcome, {username}.");
        }
    }

}
