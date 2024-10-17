using CashFlow.Communication.Requests;
using CashFlow.Exception.Resource;
using FluentValidation;

namespace CashFlow.Application.UseCases.Users;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty()
                .WithMessage(ResourceErrorMessages.NAME_EMPTY);

        RuleFor(e => e.Email)
            .NotEmpty()
                .WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress()
                .WithMessage(ResourceErrorMessages.EMAIL_INVALID);

        RuleFor(e => e.Password)
            .SetValidator(new PasswordValidator<RequestRegisterUserJson>());
    }
}
