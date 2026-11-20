using FluentValidation;

namespace DonationHub.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().MaximumLength(200);
            RuleFor(x => x.NameEn).NotEmpty().MaximumLength(200);
            RuleFor(x => x.DescriptionAr).NotEmpty().MaximumLength(4000);
            RuleFor(x => x.DescriptionEn).NotEmpty().MaximumLength(4000);
        }
    }
  
}
