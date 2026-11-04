using DonationHub.Application.Features.Auth.Login;
using DonationHub.Application.Features.Auth.Refresh;
using Microsoft.AspNetCore.Authorization;
using DonationHub.API.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using DonationHub.Application.Features.Auth.GetMe;
using DonationHub.Application.Features.Auth.Logout;

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
            if (!string.IsNullOrEmpty(result.Value.RefreshToken))
                SetRefreshTokenCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiration);


            return Ok(result.Value);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var command = new LoginCommand(dto.Email, dto.Password);
            var result = await _mediator.Send(command);
            if(!result.IsSuccess)
                return GetProblemResult();

           if (!string.IsNullOrEmpty(result.Value.RefreshToken))
                SetRefreshTokenCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiration);
           

            return Ok(result.Value);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                return Unauthorized(new { message = "Refresh token is missing." });
            
            var command = new RefreshTokenCommand(refreshToken);
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return GetProblemResult();
            if (!string.IsNullOrEmpty(result.Value.RefreshToken))
                SetRefreshTokenCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiration);
            return Ok(result.Value);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                return Unauthorized(new { message = "Refresh token is missing." });
            var command = new LogoutCommand(refreshToken);
            var result = await _mediator.Send(command);
            if(!result.IsSuccess)
                return GetProblemResult();
            Response.Cookies.Delete("refreshToken");
            return Ok(new { message = "Logged out successfully." });
        }



        [Authorize]
        [HttpGet("me")]
        
        public async Task<IActionResult> GetMe()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if(string.IsNullOrEmpty(userEmail))
                return Unauthorized(new { message = "Invalid Credentials" });

            var query = new GetProfileQuery(userEmail);
            var result = await _mediator.Send(query);
            if(!result.IsSuccess)
                return GetProblemResult();
            return Ok(result.Value);
        }



        private void SetRefreshTokenCookie(string refreshToken, 
            DateTime expiresAt)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expiresAt.ToUniversalTime(),
                Secure = true,
                SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
