
namespace DonationHub.Domain.Results.Errors
{
    public static  class CategoryErrors
    {
        public static readonly Error NotFound =
            new("Category.NotFound", "Category was not found.", ErrorType.NotFound);

        public static readonly Error Conflict =
            new Error("Category.HasCampaigns", " Cannot delete a category with linked campaigns", ErrorType.Conflict);
    
        
    
    }
}
