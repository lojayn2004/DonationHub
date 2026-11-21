using DonationHub.Domain.Results;
using Microsoft.AspNetCore.Mvc;
using DonationHub.Application.Common.Dtos;

namespace DonationHub.API.Controllers
{
    public class APIBaseController: ControllerBase
    {
        protected bool IsArabicRequest
        {
            get
            {
                var language = Request.Headers.AcceptLanguage.ToString()
                    .Split(',')[0]
                    .Split(';')[0]
                    .Trim();

                return language.Equals("ar", StringComparison.OrdinalIgnoreCase) ||
                    language.StartsWith("ar-", StringComparison.OrdinalIgnoreCase);
            }
        }

        protected IActionResult GetProblemResult(Error error)
        {
            var errorDetails = error as ErrorDetails;



            var problemDetails = new ProblemDetailsDto()
            {
                Title  = error.Message,
                Code = error.Code,
                TraceId = HttpContext.TraceIdentifier,
                Status = (int) error.Type,
                Type = $"https://httpstatuses.io/{(int) error.Type}"

            };
            if(errorDetails is not null)
            {
                problemDetails.Details = errorDetails.Details;
            }
            return new ObjectResult(problemDetails)
            {
                StatusCode = (int)error.Type,
                ContentTypes = { "application/problem+json" }
            };


        }
    }

   
}
