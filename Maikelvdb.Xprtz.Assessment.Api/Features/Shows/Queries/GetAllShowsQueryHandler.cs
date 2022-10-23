using Microsoft.Extensions.Caching.Memory;

namespace Maikelvdb.Xprtz.Assessment.Api.Features.Shows.Queries
{
    public class GetAllShowsQueryHandler : IRequestHandler<GetAllShowsQuery, IList<ShowDto>>
    {
        private const string CacheKey = "AllShows";
        private readonly IMemoryCache _cache;

        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetAllShowsQueryHandler(DataContext dataContext, IMapper mapper, IMemoryCache cacheProvider)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _cache = cacheProvider;
        }

        public async Task<IList<ShowDto>> Handle(GetAllShowsQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(CacheKey, out IList<ShowDto> cachedShows))
            {
                return cachedShows;
            }

            var shows = await _dataContext.Set<Show>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var mappedShows = _mapper.Map<IList<ShowDto>>(shows);
            _cache.Set(CacheKey, mappedShows, TimeSpan.FromHours(1));

            return mappedShows;
        }
    }
}
