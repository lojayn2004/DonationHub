﻿using DonationHub.Application.Behaviors;
using DonationHub.Domain.Results;
using FluentAssertions;
using FluentValidation;
using MediatR;

namespace DonationHub.Application.UnitTests;

public class ValidationBehaviorTests
{
    [Fact]
    public async Task Handle_WhenValidatorFails_ReturnsValidationFailureWithoutCallingHandler()
    {
        var behavior = new ValidationBehavior<TestRequest, Result>(
            [new TestRequestValidator()]);
        var handlerCalled = false;
        RequestHandlerDelegate<Result> next = _ =>
        {
            handlerCalled = true;
            return Task.FromResult(Result.Success());
        };

        var result = await behavior.Handle(
            new TestRequest(""),
            next,
            CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error!.Code.Should().Be("Validation.Failed");
        result.Error.Should().BeOfType<ErrorDetails>()
            .Which.Details.Should().ContainKey(nameof(TestRequest.Name))
            .WhoseValue.Should().Contain("Name is required");
        handlerCalled.Should().BeFalse();
    }

    private sealed record TestRequest(string Name);

    private sealed class TestRequestValidator : AbstractValidator<TestRequest>
    {
        public TestRequestValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                .WithMessage("Name is required");
        }
    }
}

