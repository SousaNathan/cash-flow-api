using CashFlow.Communication.Requests;
using CashFlow.Exception.Resource;
using FluentValidation;

namespace CashFlow.Application.UseCases.Expenses;

public class ExpenseValidator : AbstractValidator<RequestExpenseJson>
{
    public ExpenseValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.TITLE_REQUIRED);

        RuleFor(e => e.Amount)
            .GreaterThan(0)
            .WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO);

        RuleFor(e => e.Date)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(ResourceErrorMessages.EXPENSES_CANNOT_BE_FOR_THE_FUTURE);

        RuleFor(e => e.PaymentType)
            .IsInEnum()
            .WithMessage(ResourceErrorMessages.PAYMENT_TYPE_INVALID);

        RuleFor(e => e.Tags)
            .ForEach(rule =>
            {
                rule.IsInEnum()
                    .WithMessage(ResourceErrorMessages.TAG_TYPE_NOT_SUPORTED);
            });
    }
}
