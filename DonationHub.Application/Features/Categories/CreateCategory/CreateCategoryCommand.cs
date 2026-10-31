using DonationHub.Application.Common.Results;
using DonationHub.Application.Features.Categories.Common;
using MediatR;

namespace DonationHub.Application.Features.Categories.CreateCatgory
{


    public record CreateCategoryCommand(string NameAr, 
                string NameEn,
                string? DescriptionAr,
                string? DescriptionEn):
        IRequest<Result<CategoryResponse>>
    {
    }
}
