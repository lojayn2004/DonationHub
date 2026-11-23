using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Campaigns.Common;
using DonationHub.Application.Features.Campaigns.Specifications;
using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Results;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.Queries.GetActiveCampaigns
{
    public class GetActiveCampaignsQueryHandler(
        IQuery<Campaign, MainInfoCampginDto> _query,
        ICurrentLanguage _lang,
        TimeProvider _timeProvider)
        : IRequestHandler<GetActiveCampaignsQuery, Result<IReadOnlyList<MainInfoCampginDto>>>
    {
        public async Task<Result<IReadOnlyList<MainInfoCampginDto>>> Handle(GetActiveCampaignsQuery request, 
                                                                            CancellationToken cancellationToken)
        {

            var campaigns = await _query.GetAllAsync(new ActiveCampaignsSpecification(true, _timeProvider), cancellationToken);
            var mappedCampaigns = campaigns.Select(s => s.ToLanguageResult(request.isAdmin, _lang.IsArabic)).ToList();

            return Result<IReadOnlyList<MainInfoCampginDto>>.Success(mappedCampaigns);
        }
    }
}
