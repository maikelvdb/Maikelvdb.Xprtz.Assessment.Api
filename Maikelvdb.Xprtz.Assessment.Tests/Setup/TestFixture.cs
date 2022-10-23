using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Maikelvdb.Xprtz.Assessment.Tests.Setup
{
    public abstract class TestFixture
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IMapper _mapper;

        public TestFixture()
        {
            var services = new ServiceCollection();
            services.AddAutoMapper(typeof(Api.Startup).Assembly);


            _serviceProvider = services.BuildServiceProvider();
            _mapper = _serviceProvider.GetRequiredService<IMapper>();
        }
    }
}
