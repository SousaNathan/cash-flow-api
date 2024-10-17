using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public RegisterExpenseUseCase(
        IExpensesWriteOnlyRepository repository, 
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseExpenseJson> Execute(RequestExpenseJson request)
    {
        Validate(request);

        var loggerUser = await _loggedUser.Get();
        var expense = _mapper.Map<Expense>(request);
        expense.UserId = loggerUser.Id;

        await _repository.Add(expense);
        await _unitOfWork.Commit();

        return _mapper.Map<ResponseExpenseJson>(expense);
    }

    private void Validate(RequestExpenseJson request)
    {
        var validate = new ExpenseValidator()
            .Validate(request);

        if (!validate.IsValid)
        {
            var errorMessages = validate.Errors
            .Select(v => v.ErrorMessage)
            .ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
