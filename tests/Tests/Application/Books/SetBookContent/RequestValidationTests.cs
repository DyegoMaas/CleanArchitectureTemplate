using System;
using System.Threading.Tasks;
using Application.Books.SetBookContent;
using FluentAssertions;
using FluentValidation;
using Tests.TestsInfrasctructure;
using Xunit;

namespace Tests.Application.Books.SetBookContent
{
    public class RequestValidationTests : IntegrationTest
    {
        private readonly Guid _bookRegistryId;

        public RequestValidationTests()
        {
            RebuildDatabase(); // TODO should not be necessary for tests that do not use the database 
        }

        [Fact]
        public void Should_not_allow_empty_content()
        {
            var requestWithNoContent = new SetBookContentRequest
            {
                GalacticRegistryId = Guid.NewGuid(),
                Content = Array.Empty<byte>()
            };
            Func<Task> setBookContent = async () => await Handle<SetBookContentRequest, SetBookContentResponse>(requestWithNoContent);

            setBookContent.Should().ThrowAsync<ValidationException>().Result.Which
                .Errors.Should().HaveCount(1);
        }
    }
}