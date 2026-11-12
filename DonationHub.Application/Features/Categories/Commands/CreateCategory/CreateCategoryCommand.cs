using DonationHub.Domain.Results;
using DonationHub.Application.Features.Categories.Common;
using MediatR;

namespace DonationHub.Application.Features.Categories.Commands.CreateCategory
{


    public record CreateCategoryCommand(string NameAr, 
                string NameEn,
                string? DescriptionAr,
                string? DescriptionEn):
        IRequest<Result<CategoryResponse>>
    {
    }
}
