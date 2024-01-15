using System.Text;
using System.Net.Http.Json;
using System.Net.Mime;

using Microsoft.AspNetCore.Mvc.Testing;
using Bogus;
using OrderWebService.Contracts.Http.Requests;
using Newtonsoft.Json;
using FluentAssertions;
using System.Net;
using OrderWebService.Contracts.Http.Responses;

namespace OrderWebService.IntegrationTests
{
    public class OrdersTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Faker _faker;

        public OrdersTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _faker = new Faker();
        }

        [Fact]
        public async Task CreatedOrderShouldDoIt()
        {
            //Arrange
            var client = _factory.CreateClient();
            var request = new CreateOrderRequest
            {
                Details = _faker.Lorem.Sentence()
            };

            using var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, MediaTypeNames.Application.Json);

            //Act
            using var response = await client.PostAsync("api/orders", content);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();

            var responseContent = await response.Content.ReadFromJsonAsync<CreateOrderResponse>();

            responseContent.Should().NotBeNull();
            responseContent.Id.Should().NotBeNullOrEmpty();
        }
    }
}
