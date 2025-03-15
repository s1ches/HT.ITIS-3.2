using Common.Grpc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using SecretClient;

using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(60));

using var authChannel = GrpcChannel.ForAddress("http://localhost:5211");
var authClient = new AuthService.AuthServiceClient(authChannel);

var credentials = new UserCredentials { UserName = "Irek Karimov Backend".ToByteString() };

var accessTokenResponse =
    await authClient.GetAccessTokenAsync(credentials, cancellationToken: cancellationTokenSource.Token);
var accessToken = accessTokenResponse.Jwt.ToStringUtf8();

if (!cancellationTokenSource.TryReset())
{
    cancellationTokenSource.Cancel();
    throw new OperationCanceledException();
}

using var secretChannel = GrpcChannel.ForAddress("http://localhost:5090");

var secretClient = new SecretService.SecretServiceClient(secretChannel);

var headers = new Metadata { { "Authorization", $"Bearer {accessToken}" } };
var secret = await secretClient.GetSecretAsync(new Empty(), headers, cancellationToken: cancellationTokenSource.Token);

Console.WriteLine("Secret of user with name {0} is {1}", credentials.UserName.ToStringUtf8(), secret.SecretValue.ToStringUtf8());