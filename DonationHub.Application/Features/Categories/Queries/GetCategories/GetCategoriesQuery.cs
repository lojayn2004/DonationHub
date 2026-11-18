using DonationHub.Domain.Results;
using DonationHub.Application.Features.Categories.Common;
using MediatR;


namespace DonationHub.Application.Features.Categories.Queries.GetCategories
{
    public  record GetCategoriesQuery : IRequest<Result<IReadOnlyList<CategoryResponse>>>;
}
