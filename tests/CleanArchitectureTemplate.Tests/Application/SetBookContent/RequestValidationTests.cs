using System;
using System.Threading.Tasks;
using CleanArchitectureTemplate.Application.Books.SetBookContent;
using CleanArchitectureTemplate.Tests.TestsInfrasctructure;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace CleanArchitectureTemplate.Tests.Application.SetBookContent
{
    public class RequestValidationTests : IntegrationTest
    {
        private readonly Guid _bookRegistryId;

        public RequestValidationTests()
        {
            RebuildDatabase();
        }

        [Fact]
        public void Should_not_allow_empty_content()
        {
            var requestWithNoContent = new SetBookContentRequest
            {
                GalacticRegistryId = Guid.NewGuid(),
                Content = Array.Empty<byte>()
            };
            Func<Task> addBook = async () => await Handle<SetBookContentRequest, SetBookContentResponse>(requestWithNoContent);

            addBook.Should().ThrowAsync<ValidationException>().Result.Which
                .Errors.Should().HaveCount(1);
        }
    }
}