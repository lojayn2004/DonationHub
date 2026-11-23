using DonationHub.Application.Features.Campaigns.Common;
using DonationHub.Domain.Results;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.Queries.GetActiveCampaigns
{
    public record GetActiveCampaignsQuery(bool isAdmin) : IRequest<Result<IReadOnlyList<MainInfoCampginDto>>>
    {
    }
}
