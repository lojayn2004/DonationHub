using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Common.Results;
using DonationHub.Application.Features.Categories.Common;
using DonationHub.Domain.Models.Categories;
using MediatR;

namespace DonationHub.Application.Features.Categories.CreateCatgory
{
    public class CreateCategoryCommandHandler(IUnitOfWork _unitOfWork) :
        IRequestHandler<CreateCategoryCommand, Result<CategoryResponse>>
    {
      

        async Task<Result<CategoryResponse>> IRequestHandler<CreateCategoryCommand, Result<CategoryResponse>>.Handle
            (CreateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var categoryRepo = _unitOfWork.GetRepository<Category>();

            var category = Category.CreateCategory(request.NameAr, request.NameEn, request.DescriptionAr, request.DescriptionEn, DateTime.UtcNow);

            categoryRepo.Add(category);
            await _unitOfWork.SavaChangesAsync();

            return Result<CategoryResponse>.Success(new CategoryResponse(category.Id, 
                category.NameAr, 
                category.NameEn,
                category.DescriptionAr, 
                category.DescriptionEn));

        }
    }
}
