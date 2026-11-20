using FluentValidation;

namespace DonationHub.Application.Features.Campaigns.Commands.UpdateCampaign
{

    public class UpdateCampaignCommandValidator: AbstractValidator<UpdateCampaignCommand>
    {
        public UpdateCampaignCommandValidator()
        {
            RuleFor(c => c.campaignId).NotEmpty();
            RuleFor(c => c.goalAmount)
                .GreaterThan(0).WithMessage("Goal Amount Must be Greater than zero");

            RuleFor(c => c.titleAr).NotEmpty().MinimumLength(5).MaximumLength(200);
            RuleFor(c => c.titleEn).NotEmpty().MinimumLength(5).MaximumLength(200);
            RuleFor(c => c.descAr).NotEmpty().MinimumLength(5).MaximumLength(4000);
            RuleFor(c => c.descEn).NotEmpty().MinimumLength(5).MaximumLength(4000);
            RuleFor(c => c.rowVersion).NotEmpty();
        }

    }

}