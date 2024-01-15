using FluentValidation;

using OrderWebService.Contracts.Http.Requests;

namespace OrderWebService.Api.Validators
{
    internal class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(x => x.Details)
                .NotEmpty()
                .MaximumLength(2000);
        }
    }
}
