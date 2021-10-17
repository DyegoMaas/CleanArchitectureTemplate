using System;
using System.Text;
using System.Threading.Tasks;
using CleanArchitectureTemplate.Application.Books.SetBookContent;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.ValueObjects;
using CleanArchitectureTemplate.Tests.TestsInfrasctructure;
using FluentAssertions;
using MediatR;
using Xunit;

namespace CleanArchitectureTemplate.Tests.Application.SetBookContent
{
    public class SetBookContentRequestTests : IntegrationTest
    {
        public SetBookContentRequestTests()
        {
            RebuildDatabase();
        }

        [Fact]
        public async Task Should_create_a_new_book_in_the_file_system_when_it_does_not_exist()
        {
            var book = Book.Create(
                name: "Fictional Book Name",
                description: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent justo nulla, pellentesque lacinia enim et, dictum finibus augue. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Morbi fringilla vestibulum ipsum. Nam auctor maximus magna, ac posuere odio dignissim at. Cras et pharetra nibh. Donec laoreet pellentesque finibus. Maecenas dictum elit vel eros semper pharetra. Sed commodo imperdiet dolor vitae fringilla. Aenean sit amet fermentum sem, id posuere sem. Phasellus tempus urna quis vulputate semper. Donec vestibulum sem ipsum, eget tincidunt mauris malesuada eget. Duis lacus nisl, facilisis ac erat vel, euismod tempus sapien. Pellentesque vitae sodales nisi. Sed feugiat justo tincidunt vehicula accumsan. Vestibulum vestibulum fringilla libero, id venenatis nibh venenatis ac. Pellentesque faucibus ut ex porttitor interdum.",
                author: "Evangeline Mustache",
                origin: new GalacticMember("Earth", "Sol"),
                publisher: "Solar System Publishing Inc.",
                galacticYear: 10_001
            );
            Seed.DatabaseDocument.InsertDocument(book);
            
            var request = new SetBookContentRequest
            {
                BookId = book.GalacticRegistryId,
                Content = ToBinaryArray(FakeBook.Content)
            };
            await Handle<SetBookContentRequest, Unit>(request);

            var file = SideEffects.OverFileSystem.LoadFileAsBinary(PathFor(book.GalacticRegistryId));
            file.Should().BeEquivalentTo(request.Content);
        }

        private string PathFor(Guid bookGalacticRegistryId)
        {
            throw new NotImplementedException();
        }

        private static byte[] ToBinaryArray(string content) => Encoding.UTF8.GetBytes(content);
    }
}