using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.ExceptionBase;
using CashFlow.Exception.Resource;

namespace CashFlow.Application.UseCases.Expenses.Update;

public class UpdateExpenseUseCase : IUpdateExpenseUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExpensesUpdateOnlyRepository _repository;
    private readonly ILoggedUser _loggeUser;

    public UpdateExpenseUseCase(
        IMapper mapper, 
        IUnitOfWork unitOfWork, 
        IExpensesUpdateOnlyRepository repository,
        ILoggedUser loggedUser)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _loggeUser = loggedUser;
    }

    public async Task Execute(long id, RequestExpenseJson request)
    {
        Validate(request);

        var loggedUser = await _loggeUser.Get();
        var expense = await _repository.GetById(loggedUser, id);
        
        if (expense is null || expense.UserId != loggedUser.Id)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        expense.Tags.Clear();

        _mapper.Map(request, expense);
        _repository.Update(expense);

        await _unitOfWork.Commit();
    }

    public void Validate(RequestExpenseJson request)
    {
        var validator = new ExpenseValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors
                .Select(e => e.ErrorMessage)
                .ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
