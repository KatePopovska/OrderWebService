using Microsoft.Extensions.Logging;

using OrderWebService.Domain.Constants;
using OrderWebService.Domain.Data;
using OrderWebService.Domain.Handlers;
using OrderWebService.Domain.Helpers;
using OrderWebService.Domain.Models;
using OrderWebService.Domain.Models.Events;

namespace OrderWebService.Domain.Commands
{
    internal class CreateOrderCommandHandler : BaseHandler<CreateOrderCommand, CreateOrderCommandResult>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        

        public CreateOrderCommandHandler(ApplicationDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
             ILogger<CreateOrderCommandHandler> logger) : base(logger)        
        {
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;
        }

        protected override async Task<CreateOrderCommandResult> HandleInternal(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderId = Guid.NewGuid();
            var now = _dateTimeProvider.Now;

            var order = new Order
            {
                Id = orderId,
                Details = request.OrderDetails,
                Status = OrderStatus.Created,
                CreatedAt = now,
                LastUpdatedAt = now,
                Events = new List<OrderEvent> {
                    new OrderEvent
                    {
                        EventType = EventTypes.OrderCreated,
                        OrderId = orderId,
                        Payload = new OrderCreatedEvent
                        {
                            Details = request.OrderDetails
                        },
                        EventTime = _dateTimeProvider.Now
                    }
                }
            };

            await _dbContext.Orders.AddAsync(order, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreateOrderCommandResult
            {
                OrderId = order.Id.ToString(),
            };
        }
    }
}
