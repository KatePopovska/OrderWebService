using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;

using FluentAssertions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Moq;
using OrderWebService.Domain.Commands;
using OrderWebService.Domain.Constants;
using OrderWebService.Domain.Data;
using OrderWebService.Domain.Helpers;
using OrderWebService.Domain.Models;
using OrderWebService.Domain.Models.Events;

namespace OrderWebService.UnitTests.Commands
{
    public class CreateOrderCommandTest : IAsyncDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mock<IDateTimeProvider> _dateTimeProvider;

        private readonly IRequestHandler<CreateOrderCommand, CreateOrderCommandResult> _handler;

        private readonly Faker _faker;

        public CreateOrderCommandTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "OrderWebService")
                .Options;

            _dbContext = new ApplicationDbContext(options);

            _dateTimeProvider = new Mock<IDateTimeProvider>();

            _handler = new CreateOrderCommandHandler(_dbContext, _dateTimeProvider.Object);

            _faker = new Faker();
        }

        [Fact]
        public async Task HandleShouldCreateOrder()
        {
            //Arrange
            var command = new CreateOrderCommand
            {
                OrderDetails = _faker.Lorem.Sentence()
            };

            var now = new DateTime(2024, 1, 14, 0, 0, 0, DateTimeKind.Utc);
            _dateTimeProvider.SetupGet(d => d.Now).Returns(now);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            Guid.TryParse(result.OrderId, out var orderId).Should().BeTrue();

            var order = await _dbContext.Orders
                .Include(o => o.Events)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            order.Should().NotBeNull();
            order.Details.Should().Be(command.OrderDetails);
            order.Status.Should().Be(OrderStatus.Created);
            order.CreatedAt.Should().Be(now);
            order.LastUpdatedAt.Should().Be(now);
            order.Events.Should().HaveCount(1);

            var @event = order.Events.First();

            @event.EventType.Should().Be(EventTypes.OrderCreated);
            @event.OrderId.Should().Be(orderId);
            @event.EventTime.Should().Be(now);
            @event.Payload.Should().Be(new OrderCreatedEvent
            {
                Details = command.OrderDetails
            });
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }


        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                await _dbContext.DisposeAsync();
            }
        }
    }
}
