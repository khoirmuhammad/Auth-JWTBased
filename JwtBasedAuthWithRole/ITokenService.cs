namespace JwtBasedAuthWithRole
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, string audience, User user);
    }
}
