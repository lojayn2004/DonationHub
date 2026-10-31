

using DonationHub.Application.Common.Results;
using MediatR;

namespace DonationHub.Application.Features.Categories.DeleteCategory
{
    public record DeleteCategoryCommand(Guid Id): IRequest<Result>
    {
    }
}
