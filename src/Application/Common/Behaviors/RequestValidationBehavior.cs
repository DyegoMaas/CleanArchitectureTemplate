using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Application.Common.Behaviors
{
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (!_validators.Any()) return next();

            var validationContext = new ValidationContext<TRequest>(request);

            var validationFailures = _validators
                .Select(v => v.Validate(validationContext))
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (validationFailures.Count != 0)
                throw new ValidationException(validationFailures);

            return next();
        }
    }
}