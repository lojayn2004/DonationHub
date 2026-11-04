using DonationHub.API.Dtos.Auth;
using DonationHub.Application.Features.Admin.CreateStaff;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DonationHub.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController(ISender _mediator): APIBaseController
    {
        [HttpPost("staff")]
        public async Task<IActionResult> CreateStaffUser(CreateStaffUserDto dto)
        {
            var command = new CreateStaffCommand(dto.Email, dto.Password, dto.FullName, dto.UserName, dto.PreferredLanguage, dto.Role);
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return GetProblemResult();
            return Ok(result.Value);
        }

    }
}
