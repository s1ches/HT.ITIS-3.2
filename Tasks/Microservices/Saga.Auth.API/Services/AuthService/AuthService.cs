using Saga.Auth.API.Models.Auth.Register;

namespace Saga.Auth.API.Services.AuthService;

public class AuthService : IAuthService
{
    public async Task<long> CreateUserAsync(RegisterRequest registerRequest,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> TryRoollbackCreateUserAsync(long userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}