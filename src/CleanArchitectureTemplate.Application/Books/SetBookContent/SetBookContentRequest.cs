using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureTemplate.Domain.Repositories;
using MediatR;

namespace CleanArchitectureTemplate.Application.Books.SetBookContent
{
    public class SetBookContentRequest : IRequest<SetBookContentResponse>
    {
        public Guid GalacticRegistryId { get; set; }
        public byte[] Content { get; set; }
    }

    public class SetBookContentRequestHandler : IRequestHandler<SetBookContentRequest, SetBookContentResponse>
    {
        private readonly IBookMetadataRepository _bookMetadataRepository;
        private readonly IBookContentRepository _bookContentRepository;

        public SetBookContentRequestHandler(IBookMetadataRepository bookMetadataRepository, IBookContentRepository bookContentRepository)
        {
            _bookMetadataRepository = bookMetadataRepository;
            _bookContentRepository = bookContentRepository;
        }

        public async Task<SetBookContentResponse> Handle(SetBookContentRequest request, CancellationToken cancellationToken)
        {
            var libraryPath = _bookContentRepository.StoreBookContent(request.Content, request.GalacticRegistryId);

            var bookMetadata = await _bookMetadataRepository.GetBookAsync(request.GalacticRegistryId);
            bookMetadata.StoreFileLocation(libraryPath);

            return new SetBookContentResponse(libraryPath.Path);
        }
    }
}