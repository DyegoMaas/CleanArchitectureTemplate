using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Repositories;
using Domain.Services;
using MediatR;

namespace Application.Books.SetBookContent
{
    public class SetBookContentRequest : IRequest<SetBookContentResponse>
    {
        public Guid GalacticRegistryId { get; set; }
        public byte[] Content { get; set; }
    }

    public class SetBookContentRequestHandler : IRequestHandler<SetBookContentRequest, SetBookContentResponse>
    {
        private readonly IUpdateBookReferenceService _updateBookReferenceService;
        private readonly IBookContentRepository _bookContentRepository;

        public SetBookContentRequestHandler(IUpdateBookReferenceService updateBookReferenceService, IBookContentRepository bookContentRepository)
        {
            _updateBookReferenceService = updateBookReferenceService;
            _bookContentRepository = bookContentRepository;
        }

        public async Task<SetBookContentResponse> Handle(SetBookContentRequest request, CancellationToken cancellationToken)
        {
            var libraryPath = await _bookContentRepository.StoreBookContent(request.Content, request.GalacticRegistryId, cancellationToken);
            await _updateBookReferenceService.UpdateBookContentReference(request.GalacticRegistryId, libraryPath);

            return new SetBookContentResponse(libraryPath.Path);
        }
    }
}