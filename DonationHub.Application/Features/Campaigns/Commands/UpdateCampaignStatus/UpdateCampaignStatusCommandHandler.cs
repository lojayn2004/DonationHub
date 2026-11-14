using DonationHub.Application.Common.Contracts;
using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Results;
using DonationHub.Domain.Results.Errors;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.Commands.UpdateCampaignStatus
{
    public class UpdateCampaignStatusCommandHandler(IUnitOfWork _unitOfWork,  
                                                    TimeProvider _timeProvider,
                                                    IConcurrencyHandler<Campaign> _concurrencyHandler) :
        IRequestHandler<UpdateCampaignStatusCommand, Result>
    {
        public async Task<Result> Handle(UpdateCampaignStatusCommand request, CancellationToken cancellationToken)
        {
            var campaignsRepo = _unitOfWork.GetRepository<Campaign>();
            var campaign = await campaignsRepo.GetByIdAsync(request.campaignId, cancellationToken);
            if (campaign == null)
                return Result.Failure(CampaignErrors.NotFound);
            var status = Enum.Parse<CampaignStatus>(request.status);


            var updateStatusResult = campaign.UpdateCampaignStatus(status, _timeProvider.GetLocalNow());

            if (!updateStatusResult.IsSuccess)
                return Result.Failure(updateStatusResult.Error);
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
