using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Auth.Commands.Login;
using FluentAssertions;
using Moq;


namespace DonationHub.Application.UnitTests.CommandTests
{
    public class LoginCommandTests
    {
        private readonly Mock<IIdentityService> _identityService = new();

        private readonly Mock<IJwtUtils> _jwtUtils = new();

        private readonly LoginCommandHandler _handler;


        public LoginCommandTests()
        {
            _handler = new LoginCommandHandler(_identityService.Object, _jwtUtils.Object);

        }

        [Fact]
        public async Task Handle_InValidEmailOrPassword_ReturnsInValidCredentials()
        {
            
            var loginCmd = new LoginCommand("lojaynk@gmail.com", "Suepr@Passwird123");

            _identityService.Setup(i => i.IsAccountLockedAsync(It.IsAny<string>())).ReturnsAsync(false);
            _identityService.Setup(i => i.ValidateCredentialsAsync((It.IsAny<string>()), (It.IsAny<string>())))
                .ReturnsAsync(false);

            var result = await _handler.Handle(loginCmd, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Type.Should().Be(Domain.Results.ErrorType.UnAuthorized);
            result.Error.Code.Should().Be("Auth.InvalidCredentials");


        }

    }
}
