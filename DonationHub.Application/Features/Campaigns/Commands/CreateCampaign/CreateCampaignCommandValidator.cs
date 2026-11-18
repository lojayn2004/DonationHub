
using FluentValidation;
using MediatR;

namespace DonationHub.Application.Features.Campaigns.Commands.CreateCampaign
{
    public class CreateCampaignCommandValidator: AbstractValidator<CreateCampaignCommand>
    {
        public CreateCampaignCommandValidator()
        {
            RuleFor(c => c.goalAmount).GreaterThan(0).WithMessage("Goal Amount Must be Greater than zero");
            RuleFor(c => c.goalCurrency).NotEmpty().Length(3).WithMessage("Currencies are always 3 chars");
            RuleFor(c => c.titleAr).NotEmpty().MinimumLength(5).WithMessage("Title Min length is 5 chars");
            RuleFor(c => c.titleEn).NotEmpty().MinimumLength(5).WithMessage("Title Min length is 5 chars");
            RuleFor(c => c.descAr).NotEmpty().MinimumLength(5).WithMessage("Description Min length is 5 chars");
            RuleFor(c => c.descEn).NotEmpty().MinimumLength(5).WithMessage("Description Min length is 5 chars");


        }
    }
}


//                                             DateTimeOffset endDate,
//                                         