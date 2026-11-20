using DonationHub.Domain.Models.Campaigns;
using FluentValidation;

namespace DonationHub.Application.Features.Campaigns.Commands.UpdateCampaignStatus
{
    public class UpdateCommandStatusCommandValidator : AbstractValidator<UpdateCampaignStatusCommand>
    {
        public UpdateCommandStatusCommandValidator()
        {
            string[] allowed = Enum.GetNames(typeof(CampaignStatus));

            RuleFor(x => x.campaignId).NotEmpty();
            RuleFor(x => x.status)
                .Must(status => Enum.TryParse<CampaignStatus>(status, out var parsedStatus)
                    && Enum.IsDefined(parsedStatus))
                .WithMessage($"Status must be one of: {string.Join(", ", allowed)}");
            RuleFor(x => x.rowVersion).NotEmpty();
        }
    }
}
