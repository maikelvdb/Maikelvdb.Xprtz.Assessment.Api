namespace Maikelvdb.Xprtz.Assessment.Api.Features.Shows.Commands
{
    public class CreateShowCommandHandler : IRequestHandler<CreateShowCommand, ShowDto>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CreateShowCommandHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ShowDto> Handle(CreateShowCommand request, CancellationToken cancellationToken)
        {
            var show = _mapper.Map<Show>(request);

            _context.Add(show);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ShowDto>(show);
        }
    }
}
