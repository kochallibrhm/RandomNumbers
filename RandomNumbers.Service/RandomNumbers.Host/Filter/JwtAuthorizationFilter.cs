using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;
using System.Text;

namespace Core.Api.Filter;

public class JwtAuthorizationFilter : IAsyncAuthorizationFilter
{
    private ApplicationSettings applicationSettings { get; }
    private readonly JwtSecurityTokenHandler tokenHandler;
    private readonly RandomNumbers.Data.RandomNumbersContext _dbContext;

    public JwtAuthorizationFilter(ApplicationSettings applicationSettings, RandomNumbers.Data.RandomNumbersContext context)
    {
        this.applicationSettings = applicationSettings;
        this._dbContext = context;
        tokenHandler = new JwtSecurityTokenHandler();
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        ControllerActionDescriptor? actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
        if (actionDescriptor is null)
        {
            context.Result = GetErrorObject("ERR401005", ErrorMessages.ERR401005);
            return;
        }

        bool allowAnonymous = actionDescriptor.MethodInfo.GetCustomAttributes(inherit: true).Any(a => a is AllowAnonymousAttribute);

        if (allowAnonymous)
            return;

        string[]? token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ");

        if (token is null)
        {
            context.Result = GetErrorObject("ERR401000", ErrorMessages.ERR401000);
            return;
        }

        bool result = !CheckTokenFormat(context, token);
        if (result)
            return;

        result = !CheckTokenValidation(context, token[1]);
        if (result)
            return;

        await CheckLastPasswordChanged(context, token[1]);
    }

    private bool CheckTokenFormat(AuthorizationFilterContext context, string[] token)
    {
        if (token.Length != 2 || !token[0].Equals("Bearer"))
        {
            context.Result = GetErrorObject("ERR401001", ErrorMessages.ERR401001);
            return false;
        }

        return true;
    }

    private bool CheckTokenValidation(AuthorizationFilterContext context, string tokenValue)
    {
        SecurityToken? validatedToken = GetSecurityToken(tokenValue);

        if (validatedToken is null)
        {
            context.Result = GetErrorObject("ERR401002", ErrorMessages.ERR401002);
            return false;
        }

        return true;
    }

    private async Task<bool> CheckLastPasswordChanged(AuthorizationFilterContext context, string tokenValue)
    {
        JwtSecurityToken securityToken = tokenHandler.ReadJwtToken(tokenValue);
        string? userText = securityToken.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.User)?.Value;

        User? userModel = JsonSerializer.Deserialize<User>(userText ?? "");
        if (userModel is null)
        {
            context.Result = GetErrorObject("ERR401003", ErrorMessages.ERR401003);
            return false;
        }

        User? registeredUserModel = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userModel.Id);

        if (registeredUserModel is null)
        {
            context.Result = GetErrorObject("ERR401003", ErrorMessages.ERR401003);
            return false;
        }

        return true;
    }

    private SecurityToken? GetSecurityToken(string token)
    {
        try
        {
            var key = Encoding.ASCII.GetBytes(applicationSettings.Jwt.SecretKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out var validatedToken);

            return validatedToken;
        }
        catch
        {
            return null;
        }
    }

    private BadRequestObjectResult GetErrorObject(string errorCode, string errorMessage)
    {
        BadRequestObjectResult errorObject = new BadRequestObjectResult(new CustomException
        {
            ErrorCode = errorCode,
            ErrorMessage = errorMessage
        });

        return errorObject;
    }
}