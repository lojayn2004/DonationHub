using DonationHub.Application.Features.Campaigns.Commands.CreateCampaign;
using DonationHub.Application.Features.Campaigns.Commands.DeleteCampaign;
using DonationHub.Application.Features.Campaigns.Commands.UpdateCampaign;
using DonationHub.Application.Features.Campaigns.Commands.UpdateCampaignStatus;
using DonationHub.Application.Features.Campaigns.Queries.GetActiveCampaigns;
using DonationHub.Application.Features.Campaigns.Queries.GetCampaignById;
using DonationHub.Application.Features.Campaigns.Queries.GetSuccessStories;
using DonationHub.API.Dtos.Campaigns;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace DonationHub.API.Controllers
{
    [ApiController]
    [Route("api/v1/campaign")]
    public class CampaignsController(ISender _mediator): APIBaseController
    {
        [HttpPost] 
        public async Task<IActionResult> Create(CreateCampaignDto request, CancellationToken ct)
        {
            //var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userId = new Guid("1F12D3D7-4978-433B-44AE-08DF3F161831");
            var createCampaignCommand =
                new CreateCampaignCommand(request.TitleAr, request.TitleEn, request.DescriptionAr, request.DescriptionEn,
                request.GoalAmount, request.GoalCurrency, request.EndDate, request.CategoryId, userId);

            var result = await  _mediator.Send(createCampaignCommand, ct);

            if (!result.IsSuccess)
                return GetProblemResult(result.Error);

            return Created($"/api/campaign/{result.Value!.Id}", result.Value);



        }


        [HttpPut("{id:guid}")]
        //[Authorize(Roles = "Admin")]
        // may change his to dto
        public async Task<IActionResult> Update(UpdateCampaignCommand request, Guid id, CancellationToken ct)
        {
            var command = new UpdateCampaignCommand(id, request.titleAr,
                request.titleEn,
                request.descAr,
                request.descEn, request.goalAmount, request.endDate, request.categoryId, request.rowVersion);

            var result = await _mediator.Send(command, ct);
            
            if (!result.IsSuccess)
                return GetProblemResult(result.Error!);

            return Ok();


        }

        [HttpDelete("{id:guid}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var command = new DeleteCampaignCommand(id);

            var result = await _mediator.Send(command, ct);
            if (!result.IsSuccess)
                return GetProblemResult(result.Error!);

            return NoContent();
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken ct)
        {
            var query = new GetCampaignById(id);

            var result = await _mediator.Send(query, ct);
            
            if (!result.IsSuccess)
                return GetProblemResult(result.Error!);

            return Ok(result.Value);
        }


        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken ct)
        {
            var query = new GetActiveCampaignsQuery();

            var result = await _mediator.Send(query, ct);
            if (!result.IsSuccess)
                return GetProblemResult(result.Error!);

            return Ok(result.Value);
        }

        [HttpGet("success-stories")]
        public async Task<IActionResult> GetSuccessStories(CancellationToken ct)
        {
            var query = new GetSuccessStoriesQuery();

            var result = await _mediator.Send(query, ct);
            if (!result.IsSuccess)
                return GetProblemResult(result.Error!);

            return Ok(result.Value);
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateStatusDto updateStatusDto, CancellationToken ct)
        {

            var command = new UpdateCampaignStatusCommand(id, updateStatusDto.status, updateStatusDto.rowVersion);
            var result = await _mediator.Send(command, ct);
            if (!result.IsSuccess)
                return GetProblemResult(result.Error!);

            return Ok();
        }

        

    }
}
