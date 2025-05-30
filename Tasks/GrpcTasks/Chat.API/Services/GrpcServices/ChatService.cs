using System.Collections.Concurrent;
using Chat.API.Data;
using Chat.API.Services.AccessTokenProvider;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Services.GrpcServices;

public class ChatService(
    ILogger<ChatService> logger,
    ChatDbContext dbContext,
    IAccessTokenProvider accessTokenProvider)
    : Chat.ChatBase
{
    private static readonly ConcurrentDictionary<string, IServerStreamWriter<Message>> UsersConnections = [];

    [Authorize]
    public override async Task<GetMessagesResponse> GetMessages(GetMessagesRequest request, ServerCallContext context)
    {
        logger.LogInformation("Get Messages From: {from}, to: {to}", request.From, request.To);

        var query = dbContext.Messages
            .OrderBy(x => x.Id)
            .Where(x => x.Id >= request.From && x.Id < request.To);

        var totalCount = await query.CountAsync(context.CancellationToken);
        var messages = await query
            .Select(x => new Message
            {
                Content = x.Content,
                MessageId = x.Id,
                UserName = x.UserName
            }).ToListAsync(context.CancellationToken);

        return new GetMessagesResponse
        {
            TotalCount = totalCount,
            Messages = { messages }
        };
    }

    public override Task<GetAccessTokenResponse> GetAccessToken(GetAccessTokenRequest request,
        ServerCallContext context)
    {
        logger.LogInformation("User with name: {name} trying to get access token", request.UserName);
        return Task.FromResult(new GetAccessTokenResponse
        {
            Jwt = accessTokenProvider.GetAccessToken(request.UserName)
        });
    }

    [Authorize]
    public override async Task<SendMessageResponse> SendMessage(SendMessageRequest request, ServerCallContext context)
    {
        var userName = context.GetHttpContext().User.Identity!.Name!;
        logger.LogInformation("User with name: {name} trying to send message", userName);

        if (string.IsNullOrWhiteSpace(request.Message))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Message cannot be empty"));

        var message = new Data.Entities.Message
        {
            Content = request.Message,
            UserName = userName,
        };

        var entry = await dbContext.Messages.AddAsync(message, context.CancellationToken);
        await dbContext.SaveChangesAsync(context.CancellationToken);

        var grpcMessageModel = new Message
        {
            MessageId = entry.Entity.Id,
            UserName = userName,
            Content = request.Message
        };

        var tasks = UsersConnections
            .Select(async connection =>
            {
                try
                {
                    await connection.Value.WriteAsync(grpcMessageModel, context.CancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                    UsersConnections.TryRemove(connection.Key, out _);
                }
            })
            .ToArray();

        await Task.WhenAll(tasks);

        logger.LogInformation("Message with id: {messageId} has been sent from user: {name}", entry.Entity.Id,
            userName);

        return new SendMessageResponse
        {
            MessageId = message.Id
        };
    }

    [Authorize]
    public override Task SubscribeOnNewMessages(Empty request, IServerStreamWriter<Message> responseStream,
        ServerCallContext context)
    {
        try
        {
            logger.LogInformation("Subscribing to new messages, user with name: {name}",
                context.GetHttpContext().User.Identity!.Name!);
            UsersConnections.TryAdd(context.GetHttpContext().User.Identity!.Name!, responseStream);
            while (!context.CancellationToken.IsCancellationRequested)
            {
                context.CancellationToken.ThrowIfCancellationRequested();
            }
        }
        catch (OperationCanceledException)
        {
            UsersConnections.TryRemove(context.GetHttpContext().User.Identity!.Name!, out _);
            logger.LogInformation("Subscription timeout for user: {name}",
                context.GetHttpContext().User.Identity!.Name!);
            throw;
        }

        return Task.CompletedTask;
    }
}