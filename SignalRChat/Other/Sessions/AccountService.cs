namespace SignalRChat.Other.Sessions;

public class AccountService : IAccountService
{
    public Members Login(string email, string password)
    {
        using AppDbContext db = new AppDbContext();
        return db.Members.SingleOrDefault(x => x.Email == email && x.Password == password);
    }
}