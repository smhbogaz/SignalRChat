namespace SignalRChat.Other.Interfaces;

public interface IAccountService
{
    public Members Login(string email, string password);
}