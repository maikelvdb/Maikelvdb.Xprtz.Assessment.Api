namespace Maikelvdb.Xprtz.Assessment.Api.Features.Shows.Commands
{
    public class DeleteShowCommandHandler : IRequestHandler<DeleteShowCommand>
    {
        private readonly DataContext _context;

        public DeleteShowCommandHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteShowCommand request, CancellationToken cancellationToken)
        {
            var existingShow = await _context.Set<Show>().SingleAsync(x => x.Id == request.ShowId, cancellationToken);
            existingShow.IsArchived = true;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
