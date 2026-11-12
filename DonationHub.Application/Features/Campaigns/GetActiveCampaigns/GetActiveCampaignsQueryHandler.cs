using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Campaigns.Common;
using DonationHub.Application.Features.Campaigns.Specifications;
using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Results;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.GetActiveCampaigns
{
    public class GetActiveCampaignsQueryHandler(IQuery<Campaign, MainInfoCampginDto> _query)
        : IRequestHandler<GetActiveCampaignsQuery, Result<IReadOnlyList<MainInfoCampginDto>>>
    {
        public async Task<Result<IReadOnlyList<MainInfoCampginDto>>>
            Handle(GetActiveCampaignsQuery request, CancellationToken cancellationToken)
        {
            // TODOD: FIX TO TAKE CACNECLLATION TOKEN
            return Result<IReadOnlyList<MainInfoCampginDto>>.Success(
                await _query.GetAllAsync(new ActiveCampaignsSpecification(true)));
        }
    }
}
