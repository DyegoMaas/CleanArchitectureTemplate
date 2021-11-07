using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using MediatR;

namespace Application.Books.InsertBookMetadata
{
    public class InsertBookMetadataRequest : IRequest<InsertBookMetadataResponse>
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public AuthorLocation Origin { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public int GalacticYear { get; set; }
        
        public class AuthorLocation
        {
            public string Planet { get; set; }
            public string System { get; set; }
        }
    }

    public class InsertBookMetadataRequestHandler : IRequestHandler<InsertBookMetadataRequest, InsertBookMetadataResponse>
    {
        private readonly IBookMetadataRepository _bookMetadataRepository;

        public InsertBookMetadataRequestHandler(IBookMetadataRepository bookMetadataRepository)
        {
            _bookMetadataRepository = bookMetadataRepository;
        }

        public async Task<InsertBookMetadataResponse> Handle(InsertBookMetadataRequest request, CancellationToken cancellationToken)
        {
            var book = BookMetadata.Create(
                name: request.Name,
                description: request.Description,
                author: request.Author,
                origin: new GalacticMember(request.Origin.Planet, request.Origin.System),
                publisher: request.Publisher,
                galacticYear: request.GalacticYear
            );
            await _bookMetadataRepository.AddBook(book);
            
            return new InsertBookMetadataResponse
            {
                GalacticRegistryId = book.GalacticRegistryId
            };
        }
    }
}