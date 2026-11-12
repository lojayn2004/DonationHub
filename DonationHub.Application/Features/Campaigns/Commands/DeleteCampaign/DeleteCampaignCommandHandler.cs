using DonationHub.Application.Common.Contracts;
using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Results;
using DonationHub.Domain.Results.Errors;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.Commands.DeleteCampaign
{
    internal class DeleteCampaignCommandHandler(IUnitOfWork _unitOfWork): IRequestHandler<DeleteCampaignCommand, Result>
    {
        public async Task<Result> Handle(DeleteCampaignCommand request, 
        CancellationToken cancellationToken)
        {
            var campaignsRepo = _unitOfWork.GetRepository<Campaign>();

            var campaign = await campaignsRepo.GetByIdAsync(request.Id, cancellationToken);
            if(campaign == null) 
               return Result.Failure(CampaignErrors.NotFound);
            
            campaignsRepo.Delete(campaign);
            await _unitOfWork.SavaChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
