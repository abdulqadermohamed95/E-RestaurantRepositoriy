using ERestaurant.Application.Feartures.Combos.Dtos;
using FluentValidation;

namespace ERestaurant.Application.Feartures.Combos.Validators
{
    public class CreateUpdateComboDtoValidator : AbstractValidator<CreateUpdateComboDto>
    {
        public CreateUpdateComboDtoValidator()
        {
            RuleFor(x => x.NameAr)
                .NotEmpty().WithMessage("Arabic name is required")
                .MaximumLength(100);

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero");

            RuleFor(x => x.Tax)
                .GreaterThan(0).WithMessage("Tax cannot be negative");
        }
    }
}
