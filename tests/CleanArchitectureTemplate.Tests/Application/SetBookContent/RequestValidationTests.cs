﻿using System;
using System.Threading.Tasks;
using CleanArchitectureTemplate.Application.Books.InsertBookMetadata;
using CleanArchitectureTemplate.Application.Books.SetBookContent;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.ValueObjects;
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
            
            var bookMetadata = BookMetadata.Create(
                name: "Fictional Book Name",
                description: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent justo nulla, pellentesque lacinia enim et, dictum finibus augue. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Morbi fringilla vestibulum ipsum. Nam auctor maximus magna, ac posuere odio dignissim at. Cras et pharetra nibh. Donec laoreet pellentesque finibus. Maecenas dictum elit vel eros semper pharetra. Sed commodo imperdiet dolor vitae fringilla. Aenean sit amet fermentum sem, id posuere sem. Phasellus tempus urna quis vulputate semper. Donec vestibulum sem ipsum, eget tincidunt mauris malesuada eget. Duis lacus nisl, facilisis ac erat vel, euismod tempus sapien. Pellentesque vitae sodales nisi. Sed feugiat justo tincidunt vehicula accumsan. Vestibulum vestibulum fringilla libero, id venenatis nibh venenatis ac. Pellentesque faucibus ut ex porttitor interdum.",
                author: "Evangeline Mustache",
                origin: new GalacticMember("Earth", "Sol"),
                publisher: "Solar System Publishing Inc.",
                galacticYear: 10_001
            );
            Seed.DatabaseDocument.InsertDocument(bookMetadata);

            _bookRegistryId = bookMetadata.GalacticRegistryId;
        }

        [Fact]
        public void Should_not_allow_empty_content()
        {
            var requestWithNoContent = new SetBookContentRequest
            {
                GalacticRegistryId = _bookRegistryId,
                Content = Array.Empty<byte>()
            };
            Func<Task> addBook = async () => await Handle<SetBookContentRequest, SetBookContentResponse>(requestWithNoContent);

            addBook.Should().ThrowAsync<ValidationException>().Result.Which
                .Errors.Should().HaveCount(1);
        }
    }
}