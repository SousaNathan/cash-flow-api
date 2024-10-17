using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class ExpensesRepository : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository, IExpensesUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;

    public ExpensesRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Expense expense)
    {
        await _dbContext.Expenses.AddAsync(expense);
    }

    public async Task<List<Expense>> GetAll(User user)
    {
        return await _dbContext.Expenses
            .AsNoTracking()
            .Where(e => e.UserId == user.Id)
            .ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(User user, long id)
    {
        return await GetFullExpanse()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);
    }

    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(User user, long id)
    {
        return await GetFullExpanse()
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);
    }

    public async Task Delete(long id)
    {
        var expense = await _dbContext.Expenses.FirstAsync(e => e.Id == id);

        _dbContext.Expenses.Remove(expense);
    }

    public void Update(Expense expense)
    {
        _dbContext.Update(expense);
    }

    private IIncludableQueryable<Expense, ICollection<Tag>> GetFullExpanse()
    {
        return _dbContext.Expenses
            .Include(e => e.Tags);
    }
}
