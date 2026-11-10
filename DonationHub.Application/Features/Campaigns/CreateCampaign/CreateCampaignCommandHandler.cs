using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Common.Results;
using DonationHub.Application.Features.Campaigns.Common;
using DonationHub.Application.Features.Categories.Common;
using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Models.Categories;
using DonationHub.Domain.Models.Shared;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.CreateCampaign
{
    internal class CreateCampaignCommandHandler(IUnitOfWork _unitOfWork, 
        TimeProvider _timeProvider): IRequestHandler<CreateCampaignCommand, Result<CampaignDto>>
    {
        public async Task<Result<CampaignDto>> Handle(CreateCampaignCommand request, 
            CancellationToken cancellationToken)
        {
            var campaignsRepo = _unitOfWork.GetRepository<Campaign>();
            var categoryRepo = _unitOfWork.GetRepository<Category>();

            var category = await categoryRepo.GetByIdAsync(request.categoryId,cancellationToken);

            if (category == null)
                return Result<CampaignDto>.Failure(CategoryErrors.NotFound);
            Money goalAmount = new Money(request.goalAmount, "USD");
            Campaign campaign = Campaign.Create(request.titleAr, request.titleEn, request.descAr, request.descEn,
                goalAmount, request.endDate, request.categoryId, request.createdByUserId, _timeProvider.GetLocalNow()
                );

            campaignsRepo.Add(campaign);
            await _unitOfWork.SavaChangesAsync(cancellationToken);
            var campaignDto = new CampaignDto()
            {
                Id = campaign.Id,
                TitleAr = campaign.TitleAr,
                TitleEn = campaign.TitleEn,
                DescriptionAr = campaign.DescriptionAr,
                DescriptionEn = campaign.DescriptionEn,
                GoalAmount = campaign.GoalAmount.Amount,
                EndDate = campaign.EndDate


            };
            return Result<CampaignDto>.Success(campaignDto);


        }
    }
}
