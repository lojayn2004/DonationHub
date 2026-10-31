using DonationHub.API.Dtos.Categories;
using DonationHub.Application.Features.Categories.CreateCatgory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DonationHub.API.Controllers
{
    [Route("api/v1/categories")]
    public class CategoryController(ISender _mediator): APIBaseController
    {


        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryRequest request, CancellationToken ct)
        {
            var command = new CreateCategoryCommand(request.NameAr,
                request.NameEn,
                request.DescriptionAr,
                request.DescriptionEn);

            var result = await _mediator.Send(command, ct);
            if (result.IsFailure)
               return GetProblemResult();


            return Created($"/api/v1/categories/{result.Value!.Id}", result.Value);
        }

    }
}

