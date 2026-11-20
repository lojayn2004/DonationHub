using DonationHub.API.Dtos.Campaigns;
using DonationHub.Application.Features.Campaigns.Commands.CreateCampaign;
using DonationHub.Application.Features.Campaigns.Commands.DeleteCampaign;
using DonationHub.Application.Features.Campaigns.Commands.UpdateCampaign;
using DonationHub.Application.Features.Campaigns.Commands.UpdateCampaignStatus;
using DonationHub.Application.Features.Campaigns.Common;
using DonationHub.Application.Features.Campaigns.Queries.GetActiveCampaigns;
using DonationHub.Application.Features.Campaigns.Queries.GetCampaignById;
using DonationHub.Application.Features.Campaigns.Queries.GetSuccessStories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DonationHub.API.Controllers
{
    [ApiController]
    [Route("api/v1/campaign")]
    public class CampaignsController(ISender _mediator): APIBaseController
    {
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken ct)
        {
            var query = new GetCampaignById(id);

            var result = await _mediator.Send(query, ct);

            if (!result.IsSuccess)
                return GetProblemResult(result.Error!);

            return Ok(ToPublicResponse(result.Value!));
        }


        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken ct)
        {
            var query = new GetActiveCampaignsQuery();

            var result = await _mediator.Send(query, ct);
            if (!result.IsSuccess)
                return GetProblemResult(result.Error!);

            return Ok(result.Value!.Select(ToPublicSummaryResponse));
        }

        [HttpGet("success-stories")]
        public async Task<IActionResult> GetSuccessStories(CancellationToken ct)
        {
            var query = new GetSuccessStoriesQuery();

            var result = await _mediator.Send(query, ct);
            if (!result.IsSuccess)
                return GetProblemResult(result.Error!);

            return Ok(result.Value!.Select(ToPublicSummaryResponse));
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
     
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var command = new DeleteCampaignCommand(id);

            var result = await _mediator.Send(command, ct);
            if (!result.IsSuccess)
                return GetProblemResult(result.Error!);

            return NoContent();
        }

       

        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, UpdateStatusDto updateStatusDto, CancellationToken ct)
        {

            var command = new UpdateCampaignStatusCommand(id, updateStatusDto.status, updateStatusDto.rowVersion);
            var result = await _mediator.Send(command, ct);
            if (!result.IsSuccess)
                return GetProblemResult(result.Error!);

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateCampaignDto request, CancellationToken ct)
        {
            var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var createCampaignCommand =
                new CreateCampaignCommand(request.TitleAr, request.TitleEn, request.DescriptionAr, request.DescriptionEn,
                request.GoalAmount, request.GoalCurrency, request.EndDate, request.CategoryId, userId);

            var result = await _mediator.Send(createCampaignCommand, ct);

            if (!result.IsSuccess)
                return GetProblemResult(result.Error);

            return Created($"/api/campaign/{result.Value!.Id}", ToPublicResponse(result.Value));



        }


        private object ToPublicResponse(CampaignDto campaign) => new
        {
            campaign.Id,
            Title = IsArabicRequest ? campaign.TitleAr : campaign.TitleEn,
            Description = IsArabicRequest ? campaign.DescriptionAr : campaign.DescriptionEn,
            campaign.GoalAmount,
            campaign.RaisedAmount,
            campaign.EndDate,
            campaign.CategoryId,
            campaign.CategoryName,
            campaign.RowVersion,
            campaign.Status,
            campaign.CreatedByUserId,
            campaign.ProgressPercent,
            campaign.DaysLeft
        };

        private object ToPublicSummaryResponse(MainInfoCampginDto campaign) => new
        {
            campaign.Id,
            Title = IsArabicRequest ? campaign.TitleAr : campaign.TitleEn,
            Description = IsArabicRequest ? campaign.DescriptionAr : campaign.DescriptionEn,
            campaign.RaisedAmount,
            campaign.GoalAmount,
            campaign.EndDate,
            campaign.ProgressPercent,
            campaign.DaysLeft
        };

    }
}
