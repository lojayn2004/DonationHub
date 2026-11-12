using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Campaigns.Common;
using DonationHub.Application.Features.Campaigns.Specifications;
using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Results;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.Queries.GetCampaignById
{

    public class GetCampaignByIdQueryHandler(IQuery<Campaign, CampaignDto> _query): IRequestHandler<GetCampaignById, Result<CampaignDto?>>
    {
        public async Task<Result<CampaignDto?>> Handle(GetCampaignById request, CancellationToken cancellationToken)
        {
            // maloosh lazama request.Id 3ayza ashelo
            var campaignDto = await  _query.GetByIdAsync(request.id, new CampaignWithCategorySpecification(request.id));
            return Result<CampaignDto?>.Success(campaignDto);
        }
    }
}