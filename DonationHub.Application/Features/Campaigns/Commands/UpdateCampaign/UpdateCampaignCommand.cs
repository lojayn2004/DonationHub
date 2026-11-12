using DonationHub.Domain.Results;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.Commands.UpdateCampaign
{

    public record UpdateCampaignCommand(Guid campaignId, string titleAr, string titleEn,
                                              string descAr, string descEn,
                                              decimal goalAmount,
                                              DateTimeOffset endDate,
                                              Guid categoryId): IRequest<Result>
    {

    }

}