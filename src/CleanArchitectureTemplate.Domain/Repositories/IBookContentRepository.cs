using System;
using CleanArchitectureTemplate.Domain.ValueObjects;

namespace CleanArchitectureTemplate.Domain.Repositories
{
    public interface IBookContentRepository
    
    {
        LibraryPath StoreBookContent(byte[] bookContent, Guid requestGalacticRegistryId);
    }
}