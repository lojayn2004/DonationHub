using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Common.Results;
using DonationHub.Application.Features.Auth.Common;
using MediatR;


namespace DonationHub.Application.Features.Auth.CreateStaff
{
    internal class CreateStaffCommandHandler(IIdentityService _identityService): IRequestHandler<CreateStaffCommand, Result<CreateStaffResponse>>
    {
        public async Task<Result<CreateStaffResponse>> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
        {
            if(await _identityService.EmailExistsAsync(request.Email))
                return Result<CreateStaffResponse>.Failure(AuthErrors.EmailAlreadyExists);

            var registerCommand = new RegisterCommand(request.Email, request.Password, request.FullName, 
                request.UserName, request.PrefferedLanguage, request.Role);

            var userResult = await _identityService.CreateUserAsync(registerCommand);
            if (!userResult.IsSuccess)
                return Result<CreateStaffResponse>.Failure(userResult.Error!);

            var user = userResult.Value!;
            return Result<CreateStaffResponse>.Success(new CreateStaffResponse(user.Id, user.Email, user.UserName, user.FullName, user.PreferredLanguage));
        }
    }
}
