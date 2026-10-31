using DonationHub.Application.Common.Results;
using DonationHub.Application.Features.Categories.Common;
using MediatR;

namespace DonationHub.Application.Features.Categories.GetCategoryById
{
    public record GetQueryByIdQuery(Guid Id): IRequest<Result<CategoryResponse>>
    {
    }
}
