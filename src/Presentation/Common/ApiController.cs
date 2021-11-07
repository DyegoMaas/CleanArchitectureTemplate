using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Common
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : ControllerBase
    {
        protected IMediator Mediator { get; }

        protected ApiController(IMediator mediator)
        {
            Mediator = mediator;
        }
    }
}