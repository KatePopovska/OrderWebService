using FluentValidation;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using OrderWebService.Contracts.Http.Errors;
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
        private readonly IValidator<CreateOrderRequest> _validator;

        public OrdersController(IMediator mediator, IValidator<CreateOrderRequest> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateOrderCommandResult), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken = default)
        {

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToArray()
                });
            }

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
