using FluentValidation;

namespace DonationHub.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {

            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.NameAr).NotEmpty().MaximumLength(200);
            RuleFor(x => x.NameEn).NotEmpty().MaximumLength(200);
            RuleFor(x => x.DescriptionAr).NotEmpty().MaximumLength(4000);
            RuleFor(x => x.DescriptionEn).NotEmpty().MaximumLength(4000);

        }
    }
}
