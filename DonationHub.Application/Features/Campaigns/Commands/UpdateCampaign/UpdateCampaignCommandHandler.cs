using DonationHub.Application.Common.Contracts;
using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Results;
using DonationHub.Domain.Results.Errors;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.Commands.UpdateCampaign
{

    public class UpdateCampaignHandler(IUnitOfWork _unitOfWork, 
                                      TimeProvider _timeProvider, 
                                      IConcurrencyHandler<Campaign> _concurrencyHandler): 
        IRequestHandler<UpdateCampaignCommand, Result>
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
            _concurrencyHandler.HandleConcurrentUpdate(campaign, request.rowVersion);



            var result = await _unitOfWork.SavaChangesAsync(cancellationToken);

            if (!result.IsSuccess)
            {
                if (result.Error.Type == ErrorType.Conflict)
                    return Result.Failure(CampaignErrors.ConcurrencyConflict);
                return Result.Failure(result.Error);
            }

            return Result.Success();
        }
    }
}