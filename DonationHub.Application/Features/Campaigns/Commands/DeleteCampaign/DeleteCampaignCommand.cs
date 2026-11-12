using DonationHub.Domain.Results;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.Commands.DeleteCampaign
{
    
    public record DeleteCampaignCommand(Guid Id) : IRequest<Result>
    {
        
    }
}