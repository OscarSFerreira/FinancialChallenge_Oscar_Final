using BuyRequest.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BuyRequest.Domain.Validator
{
    public class BuyRequestValidator : AbstractValidator<Entities.BuyRequest>
    {

        public BuyRequestValidator()
        {
            RuleFor(x => x.Code)
               .NotNull().WithMessage("Code field is required");

            RuleFor(x => x.Date)
               .NotNull().WithMessage("Date field is required");

            RuleFor(x => x.ClientDescription)
               .NotEmpty().WithMessage("Client Description field is required");

            RuleFor(x => x.ClientEmail)
               .EmailAddress()
               .NotEmpty().WithMessage("Email field is required");

            RuleFor(p => p.ClientPhone)
               .NotEmpty()
               .NotNull().WithMessage("Phone Number is required.")
               .MinimumLength(9).WithMessage("Phone Number must have more than 9 characters.")
               .MaximumLength(11).WithMessage("Phone Number must have less than 11 characters.")
               .Matches(new Regex(@"(9[1236][0-9]) ?([0-9]{3}) ?([0-9]{3})")).WithMessage("PhoneNumber not valid");

            RuleFor(x => x.Status)
               .NotNull().WithMessage("Satus field is required")
               .IsInEnum().WithMessage("Invalid Status");

            RuleFor(x => x.ProductPrices)
               .NotNull().WithMessage("Product Price field is required");

            RuleFor(x => x.Discount)
               .NotNull().WithMessage("Discount field is required")
               .GreaterThanOrEqualTo(0).WithMessage("Discount field can't be negative.");

            RuleFor(x => x.CostPrice)
               .NotNull().WithMessage("Cost Value field is required");

            RuleFor(x => x.TotalPricing)
               .NotNull().WithMessage("Total Value field is required");

            RuleForEach(x => x.Products).SetValidator(new ProductRequestValidator());

            RuleFor(x => x.Products).Must(x => x.GroupBy(p => p.ProductCategory).Count() == 1)
                .WithMessage("There can only be one category of Products!");

            //RuleFor(x => x.Status).Must(x => )
        }
    }
}
