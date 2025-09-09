using ERestaurant.Application.Feartures.Materials.Dtos;
using FluentValidation;

namespace ERestaurant.Application.Feartures.Materials.Validators
{
    public class CreateUpdateMaterialDtoValidator : AbstractValidator<CreateUpdateMaterialDto>
    {
        public CreateUpdateMaterialDtoValidator()
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
