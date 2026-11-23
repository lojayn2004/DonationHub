using DonationHub.Domain.Results;
using DonationHub.Application.Features.Categories.Common;
using MediatR;


namespace DonationHub.Application.Features.Categories.Queries.GetCategories
{
    public  record GetCategoriesQuery(bool isAdmin): IRequest<Result<IReadOnlyList<CategoryResponse>>>;
}
