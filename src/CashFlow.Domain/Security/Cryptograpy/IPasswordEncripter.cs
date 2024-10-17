namespace CashFlow.Domain.Security.Cryptograpy;

public interface IPasswordEncripter
{
    string Encript(string password);

    bool Verify(string password, string passwordHash);
}
