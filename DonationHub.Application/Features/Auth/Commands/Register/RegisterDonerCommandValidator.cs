using FluentValidation;

namespace DonationHub.Application.Features.Auth.Commands.Register
{
    public class RegisterDonerCommandValidator : AbstractValidator<RegisterDonerCommand>
    {
        public RegisterDonerCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(250);
            RuleFor(x => x.UserName).NotEmpty().MaximumLength(256);
            RuleFor(x => x.PrefferedLanguage).MaximumLength(3);
        }
    }
}
