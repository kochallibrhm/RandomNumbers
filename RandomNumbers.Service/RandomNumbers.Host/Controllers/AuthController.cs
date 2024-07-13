using RandomNumbers.Host.MediatR.Handlers;

namespace RandomNumbers.Host.Controllers;

public class AuthController : BaseController
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateUserRequest request)
    {
        var response = await Mediator.Send(request);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var response = await Mediator.Send(request);
        return Ok(response);
    }
}
