using System.Threading.Tasks;
using CleanArchitectureTemplate.Application.Books.InsertBookMetadata;
using CleanArchitectureTemplate.Presentation.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureTemplate.Presentation.Controllers
{
    public class BookEditingController : ApiController
    {
        public BookEditingController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [Route("metadata")]
        public async Task<InsertBookMetadataResponse> InsertMetadata(InsertBookMetadataRequest request)
        {
            return await Mediator.Send(request);
        }
    }
}