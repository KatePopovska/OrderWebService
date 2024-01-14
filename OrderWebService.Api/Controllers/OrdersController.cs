using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using OrderWebService.Contracts.Http.Requests;
using OrderWebService.Contracts.Http.Responses;
using OrderWebService.Domain.Commands;

namespace OrderWebService.Api.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateOrderCommandResult), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateOrderCommand
            {
                OrderDetails = request.Details
            };

            var result = await _mediator.Send(command, cancellationToken);

            return Created($"/api/orders/{result.OrderId}", new CreateOrderResponse
            {
                Id = result.OrderId
            });
        }
    }
}
