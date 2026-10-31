using Microsoft.AspNetCore.Mvc;

namespace DonationHub.API.Controllers
{
    public class APIBaseController: ControllerBase
    {

        protected IActionResult GetProblemResult()
        {
            return Ok();
        }
    }
}
