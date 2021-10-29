using System.Threading.Tasks;
using CleanArchitectureTemplate.Application.Books.InsertBookMetadata;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Tests.TestsInfrasctructure;
using FluentAssertions;
using Xunit;

namespace CleanArchitectureTemplate.Tests.Application.InsertBookMetadata
{
    public class InsertBookMetadataRequestTests : IntegrationTest
    {
        public InsertBookMetadataRequestTests()
        {
            RebuildDatabase();
        }

        [Fact]
        public async Task Should_save_an_empty_book_to_the_database()
        {
            var addBookRequest = new InsertBookMetadataRequest
            {
                Name = "Fictional Book Name",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent justo nulla, pellentesque lacinia enim et, dictum finibus augue. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Morbi fringilla vestibulum ipsum. Nam auctor maximus magna, ac posuere odio dignissim at. Cras et pharetra nibh. Donec laoreet pellentesque finibus. Maecenas dictum elit vel eros semper pharetra. Sed commodo imperdiet dolor vitae fringilla. Aenean sit amet fermentum sem, id posuere sem. Phasellus tempus urna quis vulputate semper. Donec vestibulum sem ipsum, eget tincidunt mauris malesuada eget. Duis lacus nisl, facilisis ac erat vel, euismod tempus sapien. Pellentesque vitae sodales nisi. Sed feugiat justo tincidunt vehicula accumsan. Vestibulum vestibulum fringilla libero, id venenatis nibh venenatis ac. Pellentesque faucibus ut ex porttitor interdum.",
                Publisher = "Solar System Publishing Inc.",
                Author = "Evangeline Mustache",
                Origin = new InsertBookMetadataRequest.AuthorLocation
                {
                    Planet = "Earth",
                    System = "Sol"
                },
                GalacticYear = 10_001
            };

            var response = await Handle<InsertBookMetadataRequest, InsertBookMetadataResponse>(addBookRequest);

            var book = SideEffects.FromDatabase().GetDocument<BookMetadata>(x => x.GalacticRegistryId == response.GalacticRegistryId);
            book.Should().BeEquivalentTo(addBookRequest);
        }
    }
}