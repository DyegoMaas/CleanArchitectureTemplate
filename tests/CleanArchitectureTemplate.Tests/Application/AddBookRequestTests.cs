using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureTemplate.Application.Books;
using CleanArchitectureTemplate.Application.Books.AddBook;
using CleanArchitectureTemplate.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace CleanArchitectureTemplate.Tests
{
    public class AddBookRequestTests
    {
        [Fact]
        public async Task Should_save_an_empty_book_to_the_database()
        {
            var addBookRequest = new AddBookRequest
            {
                Name = "Fictional Book Name",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent justo nulla, pellentesque lacinia enim et, dictum finibus augue. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Morbi fringilla vestibulum ipsum. Nam auctor maximus magna, ac posuere odio dignissim at. Cras et pharetra nibh. Donec laoreet pellentesque finibus. Maecenas dictum elit vel eros semper pharetra. Sed commodo imperdiet dolor vitae fringilla. Aenean sit amet fermentum sem, id posuere sem. Phasellus tempus urna quis vulputate semper. Donec vestibulum sem ipsum, eget tincidunt mauris malesuada eget. Duis lacus nisl, facilisis ac erat vel, euismod tempus sapien. Pellentesque vitae sodales nisi. Sed feugiat justo tincidunt vehicula accumsan. Vestibulum vestibulum fringilla libero, id venenatis nibh venenatis ac. Pellentesque faucibus ut ex porttitor interdum.",
                Publisher = "Solar System Publishing Inc.",
                Author = "Evangeline Mustache",
                Origin =
                {
                    Planet = "Earth",
                    System = "Sol"
                },
                Edition = 1,
                GalacticYear = 10_001
            };

            var requestHandler = new AddBookRequestHandler();
            await requestHandler.Handle(addBookRequest, CancellationToken.None);

            var book = GetAll<Book>().First();
            book.Name.Should().Be(addBookRequest.Name);
        }

        private IList<T> GetAll<T>()
        {
            throw new NotImplementedException();
        }
    }
}