using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Repositories;
using CleanArchitectureTemplate.Domain.ValueObjects;
using MediatR;

namespace CleanArchitectureTemplate.Application.Books.InsertBookMetadata
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
        private readonly IBooksRepository _booksRepository;

        public InsertBookMetadataRequestHandler(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        public async Task<InsertBookMetadataResponse> Handle(InsertBookMetadataRequest request, CancellationToken cancellationToken)
        {
            var book = Book.Create(
                name: request.Name,
                description: request.Description,
                author: request.Author,
                origin: new GalacticBody(request.Origin.Planet, request.Origin.System),
                publisher: request.Publisher,
                galacticYear: request.GalacticYear
            );
            await _booksRepository.AddBook(book);
            
            return new InsertBookMetadataResponse
            {
                GalacticRegistryId = book.GalacticRegistryId
            };
        }
    }
}