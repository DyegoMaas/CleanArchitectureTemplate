using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CleanArchitectureTemplate.Application.Books.InsertBookMetadata;
using CleanArchitectureTemplate.Application.Books.SetBookContent;
using CleanArchitectureTemplate.Presentation.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureTemplate.Presentation.Controllers
{
    [Route("api/book-editing")]
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
        
        [HttpPost]
        [HttpPut]
        [Route("{galacticRegistryId}/content")]
        public async Task<SetBookContentResponse> SetContent([FromRoute]Guid galacticRegistryId, [FromBody]BookContentRequest bookContent)
        {
            await using var memoryStream = new MemoryStream();
            memoryStream.Write(Encoding.UTF8.GetBytes(bookContent.Content));
            memoryStream.Seek(0, SeekOrigin.Begin);
            
            return await Mediator.Send(new SetBookContentRequest
            {
                GalacticRegistryId = galacticRegistryId,
                Content = memoryStream.ToArray()
            });
        }

        public class BookContentRequest
        {
            public string Content { get; set; }
        }
    }
}