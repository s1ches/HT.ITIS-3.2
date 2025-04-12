namespace Saga.Auth.API.Models.Auth.Register;

public record RegisterRequest
{
    public string Email { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}