using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Categories.Common;
using DonationHub.Application.Features.Categories.Specifications;
using DonationHub.Domain.Models.Categories;
using MediatR;
using DonationHub.Domain.Results;
using DonationHub.Domain.Results.Errors;

namespace DonationHub.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler(
        IUnitOfWork _unitOfWork,
        IQuery<Category, CategoryResponse> _categoryQuery,
        TimeProvider _timeProvider) :
        IRequestHandler<CreateCategoryCommand, Result<CategoryResponse>>
    {
        public async Task<Result<CategoryResponse>> Handle
            (CreateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var existingCategories = await _categoryQuery.GetAllAsync(
                new CategoryNameSpecification(request.NameAr, request.NameEn));
            if (existingCategories.Count > 0)
                return Result<CategoryResponse>.Failure(CategoryErrors.AlreadyExists);

            var categoryRepo = _unitOfWork.GetRepository<Category>();

            var category = Category.CreateCategory(request.NameAr, request.NameEn, 
                request.DescriptionAr, request.DescriptionEn, _timeProvider.GetUtcNow());

            categoryRepo.Add(category);
            await _unitOfWork.SavaChangesAsync(cancellationToken);

            return Result<CategoryResponse>.Success(new CategoryResponse(category.Id, 
                category.NameAr, 
                category.NameEn,
                category.DescriptionAr, 
                category.DescriptionEn));

        }
    }
}
