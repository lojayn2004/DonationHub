using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Campaigns.Common;
using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Models.Categories;
using DonationHub.Domain.Models.Shared;
using DonationHub.Domain.Results;
using DonationHub.Domain.Results.Errors;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.CreateCampaign
{
    internal class CreateCampaignCommandHandler(IUnitOfWork _unitOfWork, 
        TimeProvider _timeProvider): IRequestHandler<CreateCampaignCommand, Result<CampaignDto>>
    {
        public async Task<Result<CampaignDto>> Handle(CreateCampaignCommand request, 
            CancellationToken cancellationToken)
        {
           
            var categoryRepo = _unitOfWork.GetRepository<Category>();
            var category = await categoryRepo.GetByIdAsync(request.categoryId,cancellationToken);

            if (category == null)
                return Result<CampaignDto>.Failure(CategoryErrors.NotFound);
            var campaignsRepo = _unitOfWork.GetRepository<Campaign>();

            Money goalAmount = new Money(request.goalAmount, "USD");
            Result<Campaign> campaignResult = Campaign.Create(request.titleAr, request.titleEn, request.descAr, request.descEn,
                goalAmount, request.endDate, request.categoryId, request.createdByUserId, _timeProvider.GetLocalNow()
                );

            if (!campaignResult.IsSuccess)
                return Result<CampaignDto>.Failure(campaignResult.Error);

            campaignsRepo.Add(campaignResult.Value);
            await _unitOfWork.SavaChangesAsync(cancellationToken);
            // TODO: CHANGE TO MAPSETR or AUTO MAPPER
            var campaignDto = new CampaignDto()
            {
                Id = campaignResult.Value.Id,
                TitleAr = campaignResult.Value.TitleAr,
                TitleEn = campaignResult.Value.TitleEn,
                DescriptionAr = campaignResult.Value.DescriptionAr,
                DescriptionEn = campaignResult.Value.DescriptionEn,
                GoalAmount = campaignResult.Value.GoalAmount.Amount,
                EndDate = campaignResult.Value.EndDate


            };
            return Result<CampaignDto>.Success(campaignDto);


        }
    }
}
