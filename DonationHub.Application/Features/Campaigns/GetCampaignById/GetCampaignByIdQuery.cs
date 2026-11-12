using DonationHub.Application.Features.Campaigns.Common;
using DonationHub.Domain.Results;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.GetCampaignById
{
    public record GetCampaignById(Guid id): IRequest<Result<CampaignDto?>>
    {
        
    }
}