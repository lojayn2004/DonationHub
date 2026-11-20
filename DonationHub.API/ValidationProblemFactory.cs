using DonationHub.Application.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DonationHub.API
{
    public class ValidationProblemFactory
    {

        public static IActionResult Create(ActionContext context)
        {
            NormalizeJsonErrors(context.ModelState);

            var details = context.ModelState
                .Where(kv => kv.Value?.Errors.Count > 0)
                .ToDictionary(
                    kv => kv.Key,
                    kv => kv.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

            var problemDetails = new ProblemDetailsDto
            {
                Title = "One or more validation errors occurred.",
                Code = "ValidationFailed",
                TraceId = context.HttpContext.TraceIdentifier,
                Status = StatusCodes.Status400BadRequest,
                Type = "https://httpstatuses.io/400",
                Details = details
            };

            return new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ContentTypes = { "application/problem+json" }
            };

        }




        private static void NormalizeJsonErrors(ModelStateDictionary modelState)
        {
            var jsonKeys = modelState.Keys
                .Where(k => k.StartsWith("$."))
                .ToList();

            foreach (var key in jsonKeys)
            {
                var fieldName = key.Substring(2);
                modelState.Remove(key);
                modelState.AddModelError(fieldName, "The provided value is not valid.");
            }
        }
    }
}

