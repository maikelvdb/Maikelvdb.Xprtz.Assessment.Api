namespace Maikelvdb.Xprtz.Assessment.Api.Features.Shows.Commands
{
    public class UpdateShowCommandHandler : IRequestHandler<UpdateShowCommand, ShowDto>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UpdateShowCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ShowDto> Handle(UpdateShowCommand request, CancellationToken cancellationToken)
        {
            var existingShow = await _context.Set<Show>().SingleAsync(x => x.Id == request.ShowId, cancellationToken);

            _context.Entry(existingShow).CurrentValues.SetValues(request);
            existingShow.Genres = request.Genres;

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ShowDto>(existingShow);
        }
    }
}
