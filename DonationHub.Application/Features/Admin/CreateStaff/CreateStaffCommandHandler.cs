using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Common.Results;
using DonationHub.Application.Features.Common;
using MediatR;


namespace DonationHub.Application.Features.Admin.CreateStaff
{
    internal class CreateStaffCommandHandler(IIdentityService _identityService): IRequestHandler<CreateStaffCommand, Result<CreateStaffResponse>>
    {
        public async Task<Result<CreateStaffResponse>> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
        {
            var registerCommand = new RegisterCommand(request.Email, request.Password, request.FullName, 
                request.UserName, request.PrefferedLanguage, request.Role);
            var user = await _identityService.CreateUserAsync(registerCommand);
            return Result<CreateStaffResponse>.Success(new CreateStaffResponse(user.Id, user.Email, user.UserName, user.FullName, user.PreferredLanguage));
        }
    }
}
