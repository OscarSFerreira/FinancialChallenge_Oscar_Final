using FluentValidation;

namespace Product.Domain.Validators
{
    public class ProductValidator : AbstractValidator<Entities.Product>
    {

        public ProductValidator()
        {
            RuleFor(x => x.Description)
               .NotEmpty().WithMessage("Description field is required");

            RuleFor(x => x.ProductCategory)
               .NotNull().WithMessage("Operation field is required")
               .IsInEnum().WithMessage("Invalid Product Category");

            RuleFor(x => x.GTIN)
              .NotEmpty().WithMessage("GTIN field is required");
        }

    }
}
