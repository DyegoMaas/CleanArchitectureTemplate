using System;
using System.Threading.Tasks;
using Application.Books.SetBookContent;
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