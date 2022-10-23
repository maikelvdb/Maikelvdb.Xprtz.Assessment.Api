using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Maikelvdb.Xprtz.Assessment.Api.Features.Shows.Commands
{
    public class UpdateShowCommandValidator : AbstractValidator<UpdateShowCommand>, IValidatorInterceptor
    {
        private readonly DataContext _context;

        public UpdateShowCommandValidator(DataContext context)
        {
            _context = context;

            RuleFor(c => c.ShowId).Cascade(CascadeMode.Stop).NotEmpty().MustAsync(BeExistingShowAsync).WithMessage("Show met opgegeven id bestaat niet");
        }

        private async Task<bool> BeExistingShowAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Set<Show>().AnyAsync(x => x.Id == id, cancellationToken);
        }

        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
        {
            var routeValue = actionContext.RouteData.Values["ShowId"];
            if (routeValue == null)
            {
                return commonContext;
            }

            var id = int.Parse(routeValue.ToString());
            (commonContext.InstanceToValidate as UpdateShowCommand)!.ShowId = id;

            return commonContext;
        }

        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
        {
            return result;
        }

    }
}
