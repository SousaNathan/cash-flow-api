using CashFlow.Communication.Requests;
using CashFlow.Exception.Resource;
using FluentValidation;

namespace CashFlow.Application.UseCases.Users.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(user => user.Name)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.NAME_EMPTY);

        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .When(user => !string.IsNullOrEmpty(user.Email), ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);
    }
}
