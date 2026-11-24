using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Categories.Common;
using DonationHub.Application.Features.Categories.Specifications;
using DonationHub.Domain.Models.Categories;
using DonationHub.Domain.Results;
using MediatR;


namespace DonationHub.Application.Features.Categories.Queries.GetCategories
{
    internal class GetCategoriesQueryHandler(IQuery<Category, Category> _query, ICurrentLanguage _lang): IRequestHandler<GetCategoriesQuery, Result<IReadOnlyList<CategoryResponse>>>
    {
        public async Task<Result<IReadOnlyList<CategoryResponse>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {


            var categories = await _query.GetAllAsync(new CategorySpecification(), cancellationToken);
            var mappedCategories = categories.Select(c => c.ToLanguageResult(request.isAdmin, _lang.IsArabic)).ToList();
            return Result<IReadOnlyList<CategoryResponse>>.Success(mappedCategories);
        }
    }
}
