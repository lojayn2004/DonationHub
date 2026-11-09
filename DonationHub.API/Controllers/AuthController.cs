using DonationHub.API.Dtos.Auth;
using DonationHub.Application.Features.Auth.GetMe;
using DonationHub.Application.Features.Auth.Login;
using DonationHub.Application.Features.Auth.Logout;
using DonationHub.Application.Features.Auth.Refresh;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DonationHub.API.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController(ISender _mediator) : APIBaseController
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto, CancellationToken ct)
        {
            var command = new RegisterDonerCommand(dto.Email, dto.Password, dto.FullName, dto.UserName, dto.PreferredLanguage);
            var result = await _mediator.Send(command, ct);
            if(!result.IsSuccess)
                return GetProblemResult(result.Error!);

            if (!string.IsNullOrEmpty(result.Value.RefreshToken))
                SetRefreshTokenCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiration);


            return Ok(result.Value);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto, CancellationToken ct)
        {
            var command = new LoginCommand(dto.Email, dto.Password);
            var result = await _mediator.Send(command, ct);
            if(!result.IsSuccess)
                return GetProblemResult(result.Error!);

           if (!string.IsNullOrEmpty(result.Value.RefreshToken))
                SetRefreshTokenCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiration);
           

            return Ok(result.Value);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(CancellationToken ct)
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                return Unauthorized(new { message = "Refresh token is missing." });
            
            var command = new RefreshTokenCommand(refreshToken);
            var result = await _mediator.Send(command, ct);
            if (!result.IsSuccess)
                return GetProblemResult(result.Error!);
            if (!string.IsNullOrEmpty(result.Value.RefreshToken))
                SetRefreshTokenCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiration);
            return Ok(result.Value);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                return Unauthorized(new { message = "Refresh token is missing." });
            var command = new LogoutCommand(refreshToken);
            var result = await _mediator.Send(command, ct);
            if(!result.IsSuccess)
                return GetProblemResult(result.Error!);
            Response.Cookies.Delete("refreshToken");
            return Ok(new { message = "Logged out successfully." });
        }



        [Authorize]
        [HttpGet("me")]
        
        public async Task<IActionResult> GetMe(CancellationToken ct)
        {
           
            string userEmail = User.FindFirstValue(ClaimTypes.Email);
            Console.WriteLine("User Email: " + userEmail);

            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new { message = "Invalid Credentials" });

            var query = new GetProfileQuery(userEmail);
            var result = await _mediator.Send(query, ct);
            if(!result.IsSuccess)
                return GetProblemResult(result.Error!);
            return Ok(result.Value);
        }



        private void SetRefreshTokenCookie(string refreshToken,
            DateTimeOffset expiresAt)
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
