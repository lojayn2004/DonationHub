using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Common.Results;
using DonationHub.Application.Features.Categories.Common;
using DonationHub.Domain.Models.Categories;
using MediatR;

namespace DonationHub.Application.Features.Categories.UpdateCategory
{
    internal class UpdateCategoryCommandHandler(IUnitOfWork _unitOfWork) : IRequestHandler<UpdateCategoryCommand, Result<CategoryResponse>>
    {
        public async Task<Result<CategoryResponse>> Handle(UpdateCategoryCommand request, 
            CancellationToken cancellationToken)
        {
            var categoryRepo = _unitOfWork.GetRepository<Category>();
            var category = await categoryRepo.GetByIdAsync(request.Id);
            if(category == null)
                return Result<CategoryResponse>.Failure(new Error("CategoryNotFound", $"Category with id {request.Id} is not Found", ErrorType.NotFound));

            category.Update(request.NameAr, request.NameEn, request.DescriptionAr, request.DescriptionEn, DateTime.UtcNow);
            categoryRepo.Update(category);
            await _unitOfWork.SavaChangesAsync();
            return Result<CategoryResponse>.Success(new CategoryResponse(category.Id, 
                category.NameAr, 
                category.NameEn, 
                category.DescriptionAr, 
                category.DescriptionEn));
        }
    }
}
