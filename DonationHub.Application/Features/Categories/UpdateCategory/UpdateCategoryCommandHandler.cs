using DonationHub.Application.Common.Contracts;
using DonationHub.Domain.Results;
using DonationHub.Application.Features.Categories.Common;
using DonationHub.Domain.Models.Categories;
using MediatR;
using DonationHub.Domain.Results.Errors;

namespace DonationHub.Application.Features.Categories.UpdateCategory
{
    internal class UpdateCategoryCommandHandler(IUnitOfWork _unitOfWork, TimeProvider _timeProvider) : IRequestHandler<UpdateCategoryCommand, Result<CategoryResponse>>
    {
        public async Task<Result<CategoryResponse>> Handle(UpdateCategoryCommand request, 
            CancellationToken cancellationToken)
        {
            var categoryRepo = _unitOfWork.GetRepository<Category>();
            var category = await categoryRepo.GetByIdAsync(request.Id, cancellationToken);
            if(category == null)
                return Result<CategoryResponse>.Failure(CategoryErrors.NotFound);

            category.Update(request.NameAr, request.NameEn, request.DescriptionAr,
                request.DescriptionEn, _timeProvider.GetUtcNow());
            categoryRepo.Update(category);
            await _unitOfWork.SavaChangesAsync(cancellationToken);
            return Result<CategoryResponse>.Success(new CategoryResponse(category.Id, 
                category.NameAr, 
                category.NameEn, 
                category.DescriptionAr, 
                category.DescriptionEn));
        }
    }
}
