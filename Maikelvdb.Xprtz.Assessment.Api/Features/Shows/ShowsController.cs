using Maikelvdb.Xprtz.Assessment.Api.Features.Shows.Commands;
using Maikelvdb.Xprtz.Assessment.Api.Features.Shows.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Maikelvdb.Xprtz.Assessment.Api.Features.Shows
{
    [Route("api/[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShowsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<IList<ShowDto>> Get()
        {
            return _mediator.Send(new GetAllShowsQuery());
        }

        [HttpGet("Name")]
        public Task<ShowDto> GetByName(GetShowByNameQuery query)
        {
            return _mediator.Send(query);
        }

        [HttpPost]
        public Task<ShowDto> Create([FromBody] CreateShowCommand command)
        {
            return _mediator.Send(command);
        }

        [HttpPut("{ShowId}")]
        public Task<ShowDto> Update([FromBody] UpdateShowCommand command)
        {
            return _mediator.Send(command);
        }

        [HttpDelete("{ShowId}")]
        public Task Delete(DeleteShowCommand command)
        {
            return _mediator.Send(command);
        }
    }
}
