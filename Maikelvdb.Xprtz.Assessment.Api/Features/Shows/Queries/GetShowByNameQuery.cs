using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Maikelvdb.Xprtz.Assessment.Api.Features.Shows.Queries
{
    public class GetShowByNameQuery : IRequest<ShowDto>
    {
        [BindRequired,
            FromQuery(Name = "ShowName")]
        public string Name { get; set; }
    }
}
