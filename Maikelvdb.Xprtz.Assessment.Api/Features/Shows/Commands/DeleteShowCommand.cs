using Microsoft.AspNetCore.Mvc;

namespace Maikelvdb.Xprtz.Assessment.Api.Features.Shows.Commands
{
    public class DeleteShowCommand : IRequest
    {
        [FromRoute]
        public int ShowId { get; set; }
    }
}
