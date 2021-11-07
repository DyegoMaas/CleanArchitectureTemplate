using System;
using System.Threading.Tasks;
using Domain.ValueObjects;

namespace Domain.Services
{
    public interface IUpdateBookReferenceService
    {
        Task UpdateBookContentReference(Guid galacticRegistryId, LibraryPath libraryPath);
    }
}