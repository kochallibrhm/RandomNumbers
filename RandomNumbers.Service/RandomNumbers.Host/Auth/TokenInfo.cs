using Microsoft.AspNetCore.Http;

namespace RandomNumbers.Host.Auth;

public class TokenInfo : ITokenInfo
{
    public User User { get; set; } = new();

    public TokenInfo(IHttpContextAccessor httpContextAccessor)
    {
        try
        {
            string[]? tokenSplitValue = httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ");
            if (tokenSplitValue is null || tokenSplitValue.Length != 2)
                return;

            string? tokenValue = tokenSplitValue[1];
            if (tokenValue is null || tokenValue.Equals("null") || tokenValue.Equals("undefined"))
                return;

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken? securityToken = tokenHandler.ReadJwtToken(tokenValue);

            if (securityToken is null)
                return;

            string? userText = securityToken.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.User)?.Value;
            if (userText is not null)
                User = JsonSerializer.Deserialize<User>(userText) ?? new();
        }
        catch(Exception ex) {
            throw;
        }
    }
}
