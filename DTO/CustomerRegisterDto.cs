/// <summary>
/// Data transfer object for customer registration
/// </summary>
public class CustomerRegisterDto
{
    /// <summary>
    /// Username for the new account
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Password for the new account
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Email address for the new account
    /// </summary>
    public string Email { get; set; }
}
