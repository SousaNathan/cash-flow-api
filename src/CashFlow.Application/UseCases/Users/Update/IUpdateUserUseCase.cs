using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Users.Update;

public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUserJson request);
}
