using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.ExceptionBase;
using CashFlow.Exception.Resource;

namespace CashFlow.Application.UseCases.Expenses.GetById;

internal class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
{
    private readonly IExpensesReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public GetExpenseByIdUseCase(
        IExpensesReadOnlyRepository repository,
        IMapper mapper,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseExpenseJson> Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();
        var expense = await _repository.GetById(loggedUser, id);

        return expense is null 
            ? throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND) 
            : _mapper.Map<ResponseExpenseJson>(expense);
    }
}
