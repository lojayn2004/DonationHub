using DonationHub.Domain.Results;
using DonationHub.Application.Features.Categories.Common;
using MediatR;

namespace DonationHub.Application.Features.Categories.Queries.GetCategoryById
{
    public record GetCategoryByIdQuery(Guid Id, bool isAdmin): IRequest<Result<CategoryResponse>>
    {
    }
}
