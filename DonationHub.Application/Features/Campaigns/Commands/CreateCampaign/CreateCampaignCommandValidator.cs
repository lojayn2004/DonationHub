using FluentValidation;

namespace DonationHub.Application.Features.Campaigns.Commands.CreateCampaign
{
    public class CreateCampaignCommandValidator: AbstractValidator<CreateCampaignCommand>
    {
        public CreateCampaignCommandValidator()
        {
            RuleFor(c => c.goalAmount).GreaterThan(0).WithMessage("Goal Amount Must be Greater than zero");

        }
    }
}
