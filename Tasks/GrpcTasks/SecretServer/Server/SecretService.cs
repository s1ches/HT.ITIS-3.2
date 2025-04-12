using Common.Grpc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace SecretServer.Server;

public class SecretService(ILogger<SecretService> logger) : SecretServer.SecretService.SecretServiceBase
{
    [Authorize]
    public override Task<Secret> GetSecret(Empty request, ServerCallContext context)
    {
        logger.LogInformation("User with name {0} getting his secret",
            context.GetHttpContext().User.Identity!.Name);

        return Task.FromResult(new Secret
        {
            SecretValue = "irek karimov is frontender".ToByteString()
        });
    }
}