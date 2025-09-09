using ERestaurant.Application.Feartures.Orders.Dtos;
using FluentValidation;

namespace ERestaurant.Application.Feartures.Orders.Validators
{
    public class CreateUpdateOrderDtoValidator : AbstractValidator<CreateUpdateOrderDto>
    {
        public CreateUpdateOrderDtoValidator()
        {
            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Customer name is required")
                .MaximumLength(150);

            RuleFor(x => x.CustomerMobile)
                .NotEmpty().WithMessage("Customer mobile is required")
                .MaximumLength(20);

            RuleFor(x => x.OrderItems)
                .NotEmpty().WithMessage("Order must contain at least one item");
        }
    }
}
