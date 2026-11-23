using DonationHub.Application.Common.Contracts;
using DonationHub.Domain.Results;
using DonationHub.Application.Features.Categories.Common;
using DonationHub.Domain.Models.Categories;
using MediatR;


namespace DonationHub.Application.Features.Categories.Queries.GetCategories
{
    internal class GetCategoriesQueryHandler(IUnitOfWork _unitOfWork, ICurrentLanguage _lang): IRequestHandler<GetCategoriesQuery, Result<IReadOnlyList<CategoryResponse>>>
    {
        public async Task<Result<IReadOnlyList<CategoryResponse>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categoryRepo = _unitOfWork.GetRepository<Category>();

            var categories = await categoryRepo.GetAllAsync(cancellationToken);
            var mappedCategories = categories.Select(c => c.ToLanguageResult(request.isAdmin, _lang.IsArabic)).ToList();
            return Result<IReadOnlyList<CategoryResponse>>.Success(mappedCategories);
        }
    }
}
