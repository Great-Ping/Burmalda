namespace Burmalda.Entities.Users;

public class UserAccount(decimal balance)
{
    public decimal Balance { get; set; } = balance;
}