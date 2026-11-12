using FluentValidation;

namespace DonationHub.Application.Features.Categories.Commands.UpdateCategory
{
    internal class UpdateCategoryCommandValidator: AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {

            RuleFor(x => x.NameAr).NotEmpty().MaximumLength(200);
            RuleFor(x => x.NameEn).NotEmpty().MaximumLength(200);
            RuleFor(x => x.DescriptionAr).MaximumLength(4000);
            RuleFor(x => x.DescriptionEn).MaximumLength(4000);

        }
    }
}
