namespace Webapi.Services
{
    public interface ITokenService
    {
        string GenerateToken(string username);
    }
}

