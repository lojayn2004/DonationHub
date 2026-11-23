using DonationHub.Application.Features.Campaigns.Common;
using DonationHub.Domain.Results;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.Queries.GetCampaignById
{
    public record GetCampaignById(Guid id, bool isAdmin): IRequest<Result<CampaignDto>>
    {
        
    }
}