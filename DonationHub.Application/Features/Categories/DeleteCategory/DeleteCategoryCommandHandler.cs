using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Common.Results;
using DonationHub.Domain.Models.Categories;
using MediatR;

namespace DonationHub.Application.Features.Categories.DeleteCategory
{
    public class DeleteCategoryCommandHandler(IUnitOfWork _unitOfWork): IRequestHandler<DeleteCategoryCommand, Result>
    {
        public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryRepo = _unitOfWork.GetRepository<Category>();
            var category = await categoryRepo.GetByIdAsync(request.Id, cancellationToken);
            if (category == null)
                return Result.Failure(new Error("CategoryNotFound", $"Category with id {request.Id} is not Found", ErrorType.NotFound));
            categoryRepo.Delete(category);
            await _unitOfWork.SavaChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
