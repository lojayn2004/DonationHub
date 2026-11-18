using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Categories.Common;
using DonationHub.Domain.Models.Categories;
using MediatR;
using DonationHub.Domain.Results;

namespace DonationHub.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler(IUnitOfWork _unitOfWork, 
        TimeProvider _timeProvider) :
        IRequestHandler<CreateCategoryCommand, Result<CategoryResponse>>
    {
      

        async Task<Result<CategoryResponse>> IRequestHandler<CreateCategoryCommand, Result<CategoryResponse>>.Handle
            (CreateCategoryCommand request,
            CancellationToken cancellationToken)
        {
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
