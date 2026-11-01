using DonationHub.API.Dtos.Auth;
using DonationHub.Application.Features.Auth.Login;
using DonationHub.Application.Features.Auth.Refresh;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DonationHub.API.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController(ISender _mediator) : APIBaseController
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var command = new RegisterDonerCommand(dto.Email, dto.Password, dto.FullName, dto.UserName, dto.PreferredLanguage);
            var result = await _mediator.Send(command);
            if(!result.IsSuccess)
                return GetProblemResult();
            
            return Ok(result.Value);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var command = new LoginCommand(dto.Email, dto.Password);
            var result = await _mediator.Send(command);
            if(!result.IsSuccess)
                return GetProblemResult();
            
            return Ok(result.Value);
        }

        [HttpPost("me")]
        public async Task<IActionResult> GetMe()
        {
            // Implementation for fetching current user information
            return Ok();
        }
    }
}
