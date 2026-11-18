using DonationHub.Domain.Results;
using MediatR;

namespace DonationHub.Application.Features.Categories.Commands.DeleteCategory
{
    public record DeleteCategoryCommand(Guid Id): IRequest<Result>
    {
    }
}
