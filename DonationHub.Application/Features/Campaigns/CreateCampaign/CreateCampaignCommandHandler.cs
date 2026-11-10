
using DonationHub.Application.Common.Contracts;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.CreateCampaign
{
    internal class CreateCampaignCommandHandler(IUnitOfWork _unitOfWork, 
        TimeProvider _timeProvider): IRequestHandler<CreateCampaignCommand, Guid>
    {
        public Task<Guid> Handle(CreateCampaignCommand request, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
