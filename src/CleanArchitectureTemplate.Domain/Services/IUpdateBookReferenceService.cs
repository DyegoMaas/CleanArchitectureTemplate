using System;
using System.Threading.Tasks;
using CleanArchitectureTemplate.Domain.ValueObjects;

namespace CleanArchitectureTemplate.Domain.Services
{
    public interface IUpdateBookReferenceService
    {
        Task UpdateBookContentReference(Guid galacticRegistryId, LibraryPath libraryPath);
    }
}