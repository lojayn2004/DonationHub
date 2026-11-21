using DonationHub.Domain.Results;
using FluentValidation;
using MediatR;
using System.Reflection;
namespace DonationHub.Application.Behaviors
{
  
    public class ValidationBehavior<TRequest,TResponse>(IEnumerable<IValidator<TRequest>> validators) :
        IPipelineBehavior<TRequest,TResponse> 
                  where TRequest: notnull
                  where TResponse: Result
                                              
    {
        public async Task<TResponse> Handle(TRequest request, 
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
          
            if (!validators.Any())
                return await next();
            var context = new ValidationContext<TRequest>(request);

            var results = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

          
            var errors = results
                  .SelectMany(r => r.Errors)
                  .GroupBy(f => f.PropertyName)
                  .ToDictionary(
                            g => g.Key,
                            g => g.Select(f => f.ErrorMessage).Distinct().ToArray());

            if (errors.Count == 0)
                return await next();


            var error = ErrorDetails.Validation(errors);   
            return HandleValidationFailure<TResponse>(error);
        }



        private static TResponse HandleValidationFailure<TResponse>(Error error) where TResponse : Result
        {
            var responseType = typeof(TResponse);
            if (responseType.IsGenericType &&
                responseType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var payloadType = responseType.GetGenericArguments()[0];
                var closedResult = typeof(Result<>).MakeGenericType(payloadType);

                var failure = closedResult.GetMethod(
                    nameof(Result.Failure),
                    BindingFlags.Public | BindingFlags.Static,
                    binder: null,
                    types: new[] { typeof(Error) },
                    modifiers: null);

                return (TResponse)failure!.Invoke(null, new object[] { error })!;
            }
            return (TResponse)Result.Failure(error);
        }

    }
}
