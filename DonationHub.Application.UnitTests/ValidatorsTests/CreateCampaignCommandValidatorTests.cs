using DonationHub.Application.Features.Campaigns.Commands.CreateCampaign;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Time.Testing;

namespace DonationHub.Application.UnitTests.Validators
{
    public class CreateCampaignCommandValidatorTests
    {
        private readonly CreateCampaignCommandValidator _validator = new();

        private readonly FakeTimeProvider _time =
           new(new DateTimeOffset(2026, 11, 17, 0, 0, 0, TimeSpan.Zero));



        [Fact]

        public void Validator_WhenCategoryIdEmpty_GivesError()
        {
            var cmd = new CreateCampaignCommand(
                 titleAr: "حملة",
                 titleEn: "Campaign",
                 descAr: "وصف",
                 descEn: "Description",
                 goalAmount: 10000,
                 goalCurrency: "EGP",
                 endDate: _time.GetUtcNow().AddDays(30),
                 categoryId: Guid.Empty,
                 createdByUserId: Guid.NewGuid());

            var result = _validator.TestValidate(cmd);

            result.ShouldHaveValidationErrorFor(cmd => cmd.categoryId)
                  .WithErrorMessage("Category Id cannot be empty");

        }


        [Fact]
        public void Validator_WhenGoalAmountLessThanOrEqualZero_GivesError()
        {
            var cmd = new CreateCampaignCommand(
                 titleAr: "حملة",
                 titleEn: "Campaign",
                 descAr: "وصف",
                 descEn: "Description",
                 goalAmount: 0,
                 goalCurrency: "EGP",
                 endDate: _time.GetUtcNow().AddDays(30),
                 categoryId: Guid.Empty,
                 createdByUserId: Guid.NewGuid());

            var result = _validator.TestValidate(cmd);

            result.ShouldHaveValidationErrorFor(cmd => cmd.goalAmount)
                .WithErrorMessage("Goal Amount Must be Greater than zero");

        }

    }
}
