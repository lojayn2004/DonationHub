using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Common.Results;
using DonationHub.Application.Features.Categories.Common;
using DonationHub.Domain.Models.Categories;
using MediatR;

namespace DonationHub.Application.Features.Categories.GetCategoryById
{
    internal class GetCategoryByIdQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetCategoryByIdQuery, Result<CategoryResponse>>
    {
        public async Task<Result<CategoryResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var categoryRepo = _unitOfWork.GetRepository<Category>();
            var category = await categoryRepo.GetByIdAsync(request.Id, cancellationToken);
            if (category == null)
                return Result<CategoryResponse>.Failure(new Error("CategoryNotFound", $"Category with id {request.Id} is not Found", ErrorType.NotFound));

       
            return Result<CategoryResponse>.Success(new CategoryResponse(category.Id,
                category.NameAr,
                category.NameEn,
                category.DescriptionAr,
                category.DescriptionEn));
        }
    
    }
}
