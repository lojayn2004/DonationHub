using DonationHub.Domain.Results;
using DonationHub.Application.Features.Categories.Common;
using MediatR;


namespace DonationHub.Application.Features.Categories.GetCategories
{
    public  record GetCategoriesQuery : IRequest<Result<IReadOnlyList<CategoryResponse>>>;
}
