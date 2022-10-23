using Microsoft.Extensions.Caching.Memory;

namespace Maikelvdb.Xprtz.Assessment.Api.Features.Shows.Queries
{
    public class GetShowByNameQueryHandler : IRequestHandler<GetShowByNameQuery, ShowDto>
    {
        private const string CacheKeyPrefix = "AllShows";
        private readonly IMemoryCache _cache;

        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GetShowByNameQueryHandler(DataContext dataContext, IMapper mapper, IMemoryCache cache)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<ShowDto> Handle(GetShowByNameQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue($"{CacheKeyPrefix}_{request.Name}", out ShowDto cacheShow))
            {
                return cacheShow;
            }

            var show = await _dataContext.Set<Show>()
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Name.ToLower() == request.Name.ToLower(), cancellationToken);

            var mappedShow = _mapper.Map<ShowDto>(show);
            _cache.Set($"{CacheKeyPrefix}_{request.Name}", mappedShow, TimeSpan.FromHours(1));

            return mappedShow;
        }
    }
}
