namespace Chat.API.Services.AccessTokenProvider;

public interface IAccessTokenProvider
{
    string GetAccessToken(string userName);
}