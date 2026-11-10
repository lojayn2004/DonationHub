using DonationHub.Application.Common.Contracts;
using DonationHub.Domain.Results;
using DonationHub.Domain.Models.Categories;
using MediatR;
using DonationHub.Domain.Results.Errors;

namespace DonationHub.Application.Features.Categories.DeleteCategory
{
    public class DeleteCategoryCommandHandler(IUnitOfWork _unitOfWork): IRequestHandler<DeleteCategoryCommand, Result>
    {
        public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryRepo = _unitOfWork.GetRepository<Category>();
            var category = await categoryRepo.GetByIdAsync(request.Id, cancellationToken);
            if (category == null)
                return Result.Failure(CategoryErrors.NotFound);
            categoryRepo.Delete(category);
            await _unitOfWork.SavaChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
