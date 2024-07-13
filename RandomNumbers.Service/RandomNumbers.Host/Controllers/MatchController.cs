using RandomNumbers.Host.Auth;
using RandomNumbers.Host.MediatR.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace RandomNumbers.Host.Controllers
{
    public class MatchController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetMatches()
        {
            var response = await Mediator.Send(new GetMatchesRequest());
            return Ok(response);
        }

        [HttpPost("play")]
        public async Task<IActionResult> PlayMatch([FromBody] PlayMatchRequest request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }
    }
}
