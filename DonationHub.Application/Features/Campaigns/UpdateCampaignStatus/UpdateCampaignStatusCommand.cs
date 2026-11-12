
using DonationHub.Domain.Results;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.UpdateCampaignStatus
{
    public record UpdateCampaignStatusCommand(Guid campaignId, string status): IRequest<Result>
    {
    }
}
