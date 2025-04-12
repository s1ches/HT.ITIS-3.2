using AuthServer.Services.AccessTokenProvider;
using Common.Grpc;
using Grpc.Core;

namespace AuthServer.Server;

public class AuthService(IAccessTokenProvider accessTokenProvider)
    : AuthServer.AuthService.AuthServiceBase
{
    public override Task<AccessToken> GetAccessToken(UserCredentials request, ServerCallContext context)
    {
        var accessToken = accessTokenProvider.GetAccessToken(request.UserName.ToStringUtf8());
        
        return Task.FromResult(new AccessToken
        {
            Jwt = accessToken.ToByteString()
        });
    }
}