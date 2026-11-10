using DonationHub.Domain.Results;
using DonationHub.Application.Features.Categories.Common;
using MediatR;

namespace DonationHub.Application.Features.Categories.GetCategoryById
{
    public record GetCategoryByIdQuery(Guid Id): IRequest<Result<CategoryResponse>>
    {
    }
}
