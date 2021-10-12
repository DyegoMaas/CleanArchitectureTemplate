using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace CleanArchitectureTemplate.Application.Books.AddBook
{
    public class AddBookRequest : IRequest<Unit>
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public AuthorLocation Origin { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public int Edition { get; set; }
        public int GalacticYear { get; set; }
        
        public class AuthorLocation
        {
            public string Planet { get; set; }
            public string System { get; set; }
        }
    }

    public class AddBookRequestHandler : IRequestHandler<AddBookRequest, Unit>
    {
        public Task<Unit> Handle(AddBookRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}