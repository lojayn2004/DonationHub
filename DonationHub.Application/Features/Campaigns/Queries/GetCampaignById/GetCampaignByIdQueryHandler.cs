using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Campaigns.Common;
using DonationHub.Application.Features.Campaigns.Specifications;
using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Results;
using DonationHub.Domain.Results.Errors;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.Queries.GetCampaignById
{

    public class GetCampaignByIdQueryHandler(IQuery<Campaign, CampaignDto> _query,
        TimeProvider _timeProvider): IRequestHandler<GetCampaignById, Result<CampaignDto>>
    {
        public async Task<Result<CampaignDto>> Handle(GetCampaignById request, CancellationToken cancellationToken)
        {
            
            var campaignDto = await  _query.GetByIdAsync(request.id, 
                new CampaignWithCategorySpecification(_timeProvider), cancellationToken);
        
            if(campaignDto == null)
                return Result<CampaignDto>.Failure(CampaignErrors.NotFound);

            return Result<CampaignDto>.Success(campaignDto!);
        }
    }
}