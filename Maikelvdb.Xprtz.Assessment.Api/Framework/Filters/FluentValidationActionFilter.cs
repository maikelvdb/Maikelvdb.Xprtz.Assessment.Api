using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.AspNetCore;

namespace Maikelvdb.Xprtz.Assessment.Api.Framework.Filters
{
    public class FluentValidationActionFilter : IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public FluentValidationActionFilter(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.Count == 0)
            {
                await next();
                return;
            }

            var allErrors = new Dictionary<string, object>();
            foreach (var argument in context.ActionArguments)
            {
                if (argument.Value == null)
                {
                    continue;
                }

                var typeOfValue = argument.Value.GetType();
                var isClass = typeOfValue.IsClass;
                if (!isClass)
                {
                    continue;
                }

                if (argument.Value is not IBaseRequest)
                {
                    continue;
                }

                var genericType = typeof(FluentValidation.IValidator<>).MakeGenericType(typeOfValue);
                if (_serviceProvider.GetService(genericType) is not FluentValidation.IValidator validator)
                {
                    continue;
                }

                var validationContext = (IValidationContext)new ValidationContext<object>(argument.Value);
                var valueValidatorInterceptor = validator is IValidatorInterceptor ? (IValidatorInterceptor)validator : null;
                if (valueValidatorInterceptor != null)
                {
                    validationContext = valueValidatorInterceptor.BeforeAspNetValidation(context, validationContext);
                }

                var result = await validator.ValidateAsync(validationContext);
                if (valueValidatorInterceptor != null)
                {
                    result = valueValidatorInterceptor.AfterAspNetValidation(context, validationContext, result);
                }

                if (result.IsValid)
                {
                    continue;
                }

                var errorDictionary = new Dictionary<string, string>(result.Errors.Count + 1);
                foreach (var e in result.Errors)
                {
                    errorDictionary[e.PropertyName] = e.ErrorMessage;
                }

                allErrors.Add(argument.Key, errorDictionary);
            }

            if (allErrors.Count > 0)
            {
                context.Result = new ObjectResult(new { Errors = allErrors })
                {
                    StatusCode = 400,
                };

                return;
            }

            await next();
        }
    }
}
