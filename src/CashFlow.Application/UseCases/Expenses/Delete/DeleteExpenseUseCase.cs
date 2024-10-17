using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.ExceptionBase;
using CashFlow.Exception.Resource;

namespace CashFlow.Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpensesReadOnlyRepository _repositoryReadOnly;
    private readonly IExpensesWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggeUser;

    public DeleteExpenseUseCase(
        IExpensesReadOnlyRepository repositoryReadOnly,
        IExpensesWriteOnlyRepository repository, 
        IUnitOfWork unitOfWork,
        ILoggedUser loggedUser)
    {
        _repositoryReadOnly = repositoryReadOnly;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _loggeUser = loggedUser;
    }

    public async Task Execute(long id)
    {
        var loggedUser = await _loggeUser.Get();
        var expense = await _repositoryReadOnly.GetById(loggedUser, id);

        if (expense is null)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        await _repository.Delete(id);
        await _unitOfWork.Commit();
    }
}
