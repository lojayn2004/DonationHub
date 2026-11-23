using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Campaigns.Common;
using DonationHub.Application.Features.Campaigns.Specifications;
using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Results;
using MediatR;


namespace DonationHub.Application.Features.Campaigns.Queries.GetSuccessStories
{
    internal class GetSuccessStoriesQueryHandler(IQuery<Campaign, MainInfoCampginDto> _query, 
        ICurrentLanguage _lang,
        TimeProvider _timeProvider) : IRequestHandler<GetSuccessStoriesQuery, Result<IReadOnlyList<MainInfoCampginDto>>>
    {
        public async Task<Result<IReadOnlyList<MainInfoCampginDto>>> Handle(GetSuccessStoriesQuery request, 
            CancellationToken cancellationToken)
        {

            var campaigns = await _query.GetAllAsync(new ActiveCampaignsSpecification(false, _timeProvider), cancellationToken);
            var mappedCampaigns = campaigns.Select(s => s.ToLanguageResult(request.isAdmin, _lang.IsArabic)).ToList();

            return Result<IReadOnlyList<MainInfoCampginDto>>.Success(mappedCampaigns);
        }
    }
}
