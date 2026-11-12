using DonationHub.Application.Common.Contracts;
using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Results;
using DonationHub.Domain.Results.Errors;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.UpdateCampaign
{

    public class UpdateCampaignHandler(IUnitOfWork _unitOfWork, TimeProvider _timeProvider): IRequestHandler<UpdateCampaignCommand, Result>
    {
        public async Task<Result> Handle(UpdateCampaignCommand request, 
            CancellationToken cancellationToken)
        {
            var campaignRepo = _unitOfWork.GetRepository<Campaign>();

            var campaign = await campaignRepo.GetByIdAsync(request.campaignId, cancellationToken);

            if (campaign == null)
                return Result.Failure(CampaignErrors.NotFound);

            Result updateResult = campaign.Update(request.titleAr, request.titleEn, request.descAr, request.descEn,
                request.goalAmount, request.endDate, request.categoryId, _timeProvider.GetLocalNow());


            if (!updateResult.IsSuccess)
                return Result.Failure(updateResult.Error);
            campaignRepo.Update(campaign);
            await _unitOfWork.SavaChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}