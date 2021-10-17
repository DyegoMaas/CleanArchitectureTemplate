using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace CleanArchitectureTemplate.Application.Books.SetBookContent
{
    public class SetBookContentRequest : IRequest<Unit>
    {
        public Guid BookId { get; set; }
        public byte[] Content { get; set; }
    }

    public class SetBookContentRequestHandler : IRequestHandler<SetBookContentRequest, Unit>
    {
        public Task<Unit> Handle(SetBookContentRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}