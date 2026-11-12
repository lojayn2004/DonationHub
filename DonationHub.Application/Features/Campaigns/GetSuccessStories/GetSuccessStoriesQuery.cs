using DonationHub.Application.Features.Campaigns.Common;
using DonationHub.Domain.Results;
using MediatR;


namespace DonationHub.Application.Features.Campaigns.GetSuccessStories
{
    public class GetSuccessStoriesQuery : IRequest<Result<IReadOnlyList<MainInfoCampginDto>>>
    {
    }
}
