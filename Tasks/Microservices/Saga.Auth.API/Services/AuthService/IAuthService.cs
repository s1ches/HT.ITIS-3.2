using Saga.Auth.API.Models.Auth.Register;

namespace Saga.Auth.API.Services.AuthService;

public interface IAuthService
{
    Task<long> CreateUserAsync(RegisterRequest registerRequest, CancellationToken cancellationToken = default);
    
    Task<bool> TryRoollbackCreateUserAsync(long userId, CancellationToken cancellationToken = default);
}