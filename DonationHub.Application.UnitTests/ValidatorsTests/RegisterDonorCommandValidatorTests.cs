using DonationHub.Application.Features.Auth.Commands.Register;
using FluentAssertions;
using FluentValidation.TestHelper;


namespace DonationHub.Application.UnitTests.Validators
{
    public class RegisterDonerCommandValidatorTests
    {
        private readonly RegisterDonerCommandValidator _validator = new();


        private static RegisterDonerCommand CreateValidCommand(string password) => new(
         "doner@example.com", password, "Test Doner", "testdoner", "en");

        [Fact]
        public void Password_WhenValid_ShouldNotHaveError()
        {
            var command = CreateValidCommand("ValidPass1");

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Password_WhenExactLength8_ShouldBeValid()
        {

            var command = CreateValidCommand("Abcdefg1");

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        [Theory]
        [InlineData("", "Password is required")]                      // empty
        [InlineData(null, "Password is required")]                    // null
        [InlineData("Ab1", "Password must be at least 8 characters")] // too short
        [InlineData("validpass1", "Password must contain an uppercase letter")]  // no upper
        [InlineData("VALIDPASS1", "Password must contain a lowercase letter")]   // no lower
        [InlineData("ValidPassword", "Password must contain a digit")]           // no digit
        public void Password_WhenBreaksPolicy_ShouldHaveExpectedError(
            string? password, string expectedMessage)
        {
            var command = CreateValidCommand(password!);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Password)
                  .WithErrorMessage(expectedMessage);
        }

      
        [Fact]
        public void Password_WhenMultipleRulesFail_ShouldReportAllFailures()
        {
            // "password" fails: min length, uppercase, digit (3 errors)
            var command = CreateValidCommand("password");

            var result = _validator.TestValidate(command);

            var passwordErrors = result.Errors
                .Where(e => e.PropertyName == nameof(RegisterDonerCommand.Password))
                .Select(e => e.ErrorMessage)
                .ToList();

            passwordErrors.Should().HaveCount(2);
           
            passwordErrors.Should().Contain("Password must contain an uppercase letter");
            passwordErrors.Should().Contain("Password must contain a digit");
        }

        
    }
}
