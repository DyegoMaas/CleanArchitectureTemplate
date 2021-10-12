using System.Threading.Tasks;
using CleanArchitectureTemplate.Domain.Entities;

namespace CleanArchitectureTemplate.Domain.Repositories
{
    public interface IBooksRepository
    {
        public Task AddBook(Book book);
    }
}