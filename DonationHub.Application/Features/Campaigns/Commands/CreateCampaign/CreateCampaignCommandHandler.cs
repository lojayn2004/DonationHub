using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Campaigns.Common;
using DonationHub.Application.Features.Campaigns.Specifications;
using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Models.Categories;
using DonationHub.Domain.Models.Shared;
using DonationHub.Domain.Results;
using DonationHub.Domain.Results.Errors;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.Commands.CreateCampaign
{
    public class CreateCampaignCommandHandler(IUnitOfWork _unitOfWork, 
        TimeProvider _timeProvider, IQuery<Campaign, CampaignDto> _query): IRequestHandler<CreateCampaignCommand, Result<CampaignDto>>
    {
        public async Task<Result<CampaignDto>> Handle(CreateCampaignCommand request, 
            CancellationToken cancellationToken)
        {
           
            var categoryRepo = _unitOfWork.GetRepository<Category>();
            var category = await categoryRepo.GetByIdAsync(request.categoryId,cancellationToken);

            if (category == null)
                return Result<CampaignDto>.Failure(CategoryErrors.NotFound);
            var campaignsRepo = _unitOfWork.GetRepository<Campaign>();

            Money goalAmount = new Money(request.goalAmount, request.goalCurrency);
            Result<Campaign> campaignResult = Campaign.Create(request.titleAr, request.titleEn, request.descAr, request.descEn,
                goalAmount, request.endDate, request.categoryId, request.createdByUserId, _timeProvider.GetUtcNow()
                );

            if (!campaignResult.IsSuccess)
                return Result<CampaignDto>.Failure(campaignResult.Error);

            campaignsRepo.Add(campaignResult.Value);
            await _unitOfWork.SavaChangesAsync(cancellationToken);

            
            var campaignDto = await _query.GetByIdAsync(campaignResult.Value.Id,
                new CampaignWithCategorySpecification(_timeProvider), cancellationToken);
            return Result<CampaignDto>.Success(campaignDto);


        }
    }
}
