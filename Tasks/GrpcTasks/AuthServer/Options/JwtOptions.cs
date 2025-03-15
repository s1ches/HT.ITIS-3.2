namespace AuthServer.Options;

public class JwtOptions : Common.Options.JwtOptions
{
    public int AccessTokenLifetimeMinutes { get; set; } = 30;
}