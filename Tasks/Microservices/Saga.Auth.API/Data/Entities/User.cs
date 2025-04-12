namespace Saga.Auth.API.Data.Entities;

public class User
{
    public long Id { get; set; }
    
    public string Email { get; set; } = string.Empty;
    
    public string PasswordHash { get; set; } = string.Empty;
}