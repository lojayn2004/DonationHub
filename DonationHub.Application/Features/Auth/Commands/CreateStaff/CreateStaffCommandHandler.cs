using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Auth.Common;
using DonationHub.Domain.Results;
using DonationHub.Domain.Results.Errors;
using MediatR;


namespace DonationHub.Application.Features.Auth.Commands.CreateStaff
{
    internal class CreateStaffCommandHandler(IIdentityService _identityService, TimeProvider _timeProvider): IRequestHandler<CreateStaffCommand, Result<CreateStaffResponse>>
    {
        public async Task<Result<CreateStaffResponse>> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
        {
            if(await _identityService.EmailExistsAsync(request.Email))
                return Result<CreateStaffResponse>.Failure(AuthErrors.EmailAlreadyExists);

            var registerCommand = new RegisterCommand(request.Email, request.Password, request.FullName, 
                request.UserName, request.PrefferedLanguage, request.Role);

            var userResult = await _identityService.CreateUserAsync(registerCommand, _timeProvider.GetUtcNow().DateTime);
            if (!userResult.IsSuccess)
                return Result<CreateStaffResponse>.Failure(userResult.Error!);

            var user = userResult.Value!;
            return Result<CreateStaffResponse>.Success(new CreateStaffResponse(user.Id, user.FullName, user.UserName, user.Email, user.PreferredLanguage));
        }
    }
}
