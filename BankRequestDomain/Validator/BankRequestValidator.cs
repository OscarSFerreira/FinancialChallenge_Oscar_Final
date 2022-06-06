using FluentValidation;

namespace BankRequest.Domain.Validator
{
    public class BankRequestValidator : AbstractValidator<Entities.BankRequest>
    {
        public BankRequestValidator()
        {
            RuleFor(x => x.Origin)
                .IsInEnum().WithMessage("The selected Origin is Invalid")
                .NotNull().When(x => x.OriginId != null, ApplyConditionTo.CurrentValidator).WithMessage("Origin can't be null when an OriginId is indicated!");

            RuleFor(x => x.OriginId)
                .NotNull().When(x => x.Origin == Entities.Enum.Origin.PurchaseRequest ||
                x.Origin == Entities.Enum.Origin.Document, ApplyConditionTo.CurrentValidator).WithMessage("OriginId can't be null!");

            RuleFor(x => x.Type)
                .NotNull().WithMessage("This field is required")
                .IsInEnum().WithMessage("The Type selected Invalid");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("The Description field is required");

            RuleFor(x => x.Amount)
                .NotNull().WithMessage("The Amount field is required")
                .LessThan(0).WithMessage("Payment option must have a negative Amount != 0")
                .When(x => x.Type == Entities.Enum.Type.Payment, ApplyConditionTo.CurrentValidator)
                .GreaterThan(0).WithMessage("Receive option must have a positive Amount != 0")
                .When(x => x.Type == Entities.Enum.Type.Receive, ApplyConditionTo.CurrentValidator);
        }
    }
}