using DonationHub.Application.Features.Campaigns.Common;
using DonationHub.Domain.Results;
using MediatR;


namespace DonationHub.Application.Features.Campaigns.Queries.GetSuccessStories
{
    public record GetSuccessStoriesQuery(bool isAdmin) : IRequest<Result<IReadOnlyList<MainInfoCampginDto>>>
    {
    }
}
