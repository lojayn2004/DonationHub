using FluentValidation;

namespace DonationHub.Application.Features.Auth.Commands.Register
{
    public class RegisterDonerCommandValidator : AbstractValidator<RegisterDonerCommand>
    {
        public RegisterDonerCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
            
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(250);
            RuleFor(x => x.UserName).NotEmpty().MaximumLength(256);
            RuleFor(x => x.PrefferedLanguage).MaximumLength(3);


            RuleFor(x => x.Password)
                   .NotEmpty().WithMessage("Password is required")
                   .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                   .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter")
                   .Matches("[a-z]").WithMessage("Password must contain a lowercase letter")
                   .Matches("[0-9]").WithMessage("Password must contain a digit");
        }
    }
}
