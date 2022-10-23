namespace Maikelvdb.Xprtz.Assessment.Api.Features.Shows.Commands
{
    public class DeleteShowCommandValidator : AbstractValidator<DeleteShowCommand>
    {
        private readonly DataContext _context;

        public DeleteShowCommandValidator(DataContext context)
        {
            _context = context;

            RuleFor(c => c.ShowId).Cascade(CascadeMode.Stop).NotEmpty().MustAsync(BeExistingShowAsync).WithMessage("Show met opgegeven id bestaat niet");
        }

        private async Task<bool> BeExistingShowAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Set<Show>().AnyAsync(x => x.Id == id, cancellationToken);
        }
    }
}
