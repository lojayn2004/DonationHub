using DonationHub.Application.Features.Campaigns.Common;
using DonationHub.Domain.Results;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.Commands.CreateCampaign
{
    public record CreateCampaignCommand(string titleAr, string titleEn,
                                              string descAr, string descEn,
                                              decimal goalAmount,
                                              DateTimeOffset endDate,
                                              Guid categoryId,
                                              Guid createdByUserId): IRequest<Result<CampaignDto>>
    {
    }
}
