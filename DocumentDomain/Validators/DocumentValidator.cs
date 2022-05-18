using FluentValidation;

namespace Document.Domain.Validators
{
    public class DocumentValidator : AbstractValidator<Entities.Document>
    {

        public DocumentValidator()
        {

            RuleFor(x => x.Number)
               .NotEmpty().WithMessage("Number field is required");

            RuleFor(x => x.Date)
               .NotNull().WithMessage("Date field is required");

            RuleFor(x => x.DocType)
               .NotNull().WithMessage("Document Type field is required")
               .IsInEnum().WithMessage("Invalid Type");

            RuleFor(x => x.Operation)
               .NotNull().WithMessage("Operation field is required")
               .IsInEnum().WithMessage("Invalid Type");

            RuleFor(x => x.Paid)
               .NotNull().WithMessage("Paid field is required");

            RuleFor(x => x.Description)
              .NotEmpty().WithMessage("Description field is required");

            RuleFor(x => x.Total)
                .NotNull().WithMessage("Total field is required")
                .GreaterThan(0).When(x => x.Operation == Entities.Enum.Operation.Entry, ApplyConditionTo.CurrentValidator).WithMessage("Total Amount must be positive!")
                .LessThan(0).When(x => x.Operation == Entities.Enum.Operation.Exit, ApplyConditionTo.CurrentValidator).WithMessage("Total Amount must be negative!");

        }

    }
}
