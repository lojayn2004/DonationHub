using DonationHub.API.Dtos.Auth;
using DonationHub.Application.Features.Auth.CreateStaff;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DonationHub.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController(ISender _mediator): APIBaseController
    {
        [HttpPost("staff")]
        public async Task<IActionResult> CreateStaffUser(CreateStaffUserDto dto, CancellationToken ct)
        {
            var command = new CreateStaffCommand(dto.Email, dto.Password, dto.FullName, 
                dto.UserName, dto.PreferredLanguage, dto.Role);
            var result = await _mediator.Send(command, ct);
            if (!result.IsSuccess)
                return GetProblemResult(result.Error!);
            return Ok(result.Value);
        }

    }
}
