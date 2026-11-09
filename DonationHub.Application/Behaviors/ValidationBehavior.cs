using DonationHub.Application.Common.Results;
using FluentValidation;
using MediatR;
namespace DonationHub.Application.Behaviors
{
    public static class ValidationErrors {
        public static Error Falied = new("Validation.Failed", "Input Validation Faliure", ErrorType.Validation);
    }
    // applies validation before reaching the handler
    public class ValidationBehavior<TRequest,TResponse>(IEnumerable<IValidator<TRequest>> validators) :
        IPipelineBehavior<TRequest, Result<TResponse>> where TRequest: notnull
    {
        public async Task<Result<TResponse>> Handle(TRequest request, 
            RequestHandlerDelegate<Result<TResponse>> next,
            CancellationToken cancellationToken)
        {
            // if their is no validations
            if (!validators.Any())
                return await next();
            var context = new ValidationContext<TRequest>(request);

            var results = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = results.SelectMany(r => r.Errors).Where(e => e is not null).ToList();
            if (failures.Count != 0)
                return Result<TResponse>.Failure(ValidationErrors.Falied);
                
            return await next();
        }
    }
}
