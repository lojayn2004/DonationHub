using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Categories.Common;
using DonationHub.Application.Features.Categories.Specifications;
using DonationHub.Domain.Models.Categories;
using DonationHub.Domain.Results;
using DonationHub.Domain.Results.Errors;
using MediatR;

namespace DonationHub.Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler(IQuery<Category, Category> _query, ICurrentLanguage _lang) : IRequestHandler<GetCategoryByIdQuery, Result<CategoryResponse>>
    {
        public async Task<Result<CategoryResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
           
            var category = await _query.GetByIdAsync(request.Id, new CategorySpecification(), cancellationToken);
            if (category == null)
                return Result<CategoryResponse>.Failure(CategoryErrors.NotFound);

       
            return Result<CategoryResponse>.Success(category.ToLanguageResult(request.isAdmin, _lang.IsArabic));
        }
    
    }
}
