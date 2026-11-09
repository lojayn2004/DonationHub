using DonationHub.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace DonationHub.API.Controllers
{
    public class APIBaseController: ControllerBase
    {

        protected IActionResult GetProblemResult(Error error)
        {
            if (error.Type == ErrorType.Validation)
                return BadRequest(error.Message);
            if(error.Type == ErrorType.NotFound)
                return NotFound(error.Message);
            if(error.Type == ErrorType.UnAuthorized)
                return Unauthorized(error.Message);
            if(error.Type == ErrorType.Conflict)
                return Conflict(error.Message);
            if(error.Type == ErrorType.Forbidden)
                return Forbid(error.Message);
            if(error.Type == ErrorType.PayloadTooLarge)
                return new StatusCodeResult(StatusCodes.Status413PayloadTooLarge);
            if(error.Type == ErrorType.UnprocessableEntity)
                return UnprocessableEntity();
            return Problem(error.Message);
        }
    }
}
