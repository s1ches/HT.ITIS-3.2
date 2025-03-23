namespace AuthServer.Services.AccessTokenProvider;

public interface IAccessTokenProvider
{
    string GetAccessToken(string userName);
}