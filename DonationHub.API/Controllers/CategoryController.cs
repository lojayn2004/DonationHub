using DonationHub.API.Dtos.Categories;
using DonationHub.Application.Features.Categories.CreateCatgory;
using DonationHub.Application.Features.Categories.DeleteCategory;
using DonationHub.Application.Features.Categories.GetCategories;
using DonationHub.Application.Features.Categories.GetCategoryById;
using DonationHub.Application.Features.Categories.UpdateCategory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DonationHub.API.Controllers
{
    [ApiController]
    [Route("api/v1/categories")]
    public class CategoryController(ISender _mediator) : APIBaseController
    {

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create(CreateCategoryRequest request, CancellationToken ct)
        {
            var command = new CreateCategoryCommand(request.NameAr,
                request.NameEn,
                request.DescriptionAr,
                request.DescriptionEn);

            var result = await _mediator.Send(command, ct);
            if (!result.IsSuccess)
                return GetProblemResult(result.Error!);


            return Created($"/api/v1/categories/{result.Value!.Id}", result.Value);
        }
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateCategoryRequest request, Guid id, CancellationToken ct)
        {
            var command = new UpdateCategoryCommand(id, request.NameAr,
                request.NameEn,
                request.DescriptionAr,
                request.DescriptionEn);

            var result = await _mediator.Send(command, ct);
            if (!result.IsSuccess)
                return GetProblemResult(result.Error!);

            return Ok(result.Value);


        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var command = new DeleteCategoryCommand(id);

            var result = await _mediator.Send(command, ct);
            if (!result.IsSuccess)
                return GetProblemResult(result.Error!);

            return NoContent();
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken ct)
        {
            var query = new GetCategoryByIdQuery(id);

            var result = await _mediator.Send(query, ct);
            if (!result.IsSuccess)
                return GetProblemResult(result.Error!);

            return Ok(result.Value);
        }


        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken ct)
        {
            var query = new GetCategoriesQuery();

            var result = await _mediator.Send(query, ct);
            if (!result.IsSuccess)
                return GetProblemResult(result.Error!);

            return Ok(result.Value);
        }
    }
}

           