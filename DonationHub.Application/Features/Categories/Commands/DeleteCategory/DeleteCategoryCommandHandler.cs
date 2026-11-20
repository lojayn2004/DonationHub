using DonationHub.Application.Common.Contracts;
using DonationHub.Domain.Results;
using DonationHub.Domain.Models.Categories;
using MediatR;
using DonationHub.Domain.Results.Errors;
using DonationHub.Application.Features.Categories.Specifications;

namespace DonationHub.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler(IUnitOfWork _unitOfWork): IRequestHandler<DeleteCategoryCommand, Result>
    {
        public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryRepo = _unitOfWork.GetRepository<Category>();
            var category = await categoryRepo.GetByIdAsync(
                request.Id,
                new CategoryWithCampaignsSpecification(),
                cancellationToken);
            if (category == null)
                return Result.Failure(CategoryErrors.NotFound);

            if (!category.CanBeDeleted())
                return Result.Failure(CategoryErrors.Conflict);

            categoryRepo.Delete(category);
            await _unitOfWork.SavaChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
