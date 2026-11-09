
using DonationHub.Application.Common.Results;

namespace DonationHub.Application.Features.Categories.Common
{
    public static  class CategoryErrors
    {
        public static readonly Error NotFound =
            new("Category.NotFound", "Category was not found.", ErrorType.NotFound);

        public static readonly Error Conflict =
            new Error("Category.HasCampaigns", " Cannot delete a category with linked campaigns", ErrorType.Conflict);
    
        
    
    }
}
