using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Common.Results;
using DonationHub.Application.Features.Categories.Common;
using DonationHub.Domain.Models.Categories;
using MediatR;


namespace DonationHub.Application.Features.Categories.GetCategories
{
    internal class GetCategoriesQueryHandler(IUnitOfWork _unitOfWork): IRequestHandler<GetCategoriesQuery, Result<IReadOnlyList<CategoryResponse>>>
    {
        public async Task<Result<IReadOnlyList<CategoryResponse>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categoryRepo = _unitOfWork.GetRepository<Category>();

            var categories = await categoryRepo.GetAllAsync(cancellationToken);
            var mappedCategories = categories.Select(c => new CategoryResponse(c.Id, c.NameAr, c.NameEn, c.DescriptionAr, c.DescriptionEn)).ToList();
            return Result<IReadOnlyList<CategoryResponse>>.Success(mappedCategories);
        }
    }
}
