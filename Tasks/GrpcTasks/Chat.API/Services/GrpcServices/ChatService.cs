using System.Text.Json;
using Chat.API.Data;
using Chat.API.Services.AccessTokenProvider;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Chat.API.Services.GrpcServices;

public class ChatService(
    ILogger<ChatService> logger,
    ChatDbContext dbContext,
    IDistributedCache cache,
    IAccessTokenProvider accessTokenProvider)
    : Chat.ChatBase
{
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
                Message_ = x.Content,
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
        
        var message = new Data.Entities.Message
        {
            Content = request.Message,
            UserName = userName,
        };

        await dbContext.Messages.AddAsync(message, context.CancellationToken);
        await cache.SetStringAsync(message.Id.ToString(), JsonSerializer.Serialize(new Message
            {
                MessageId = message.Id,
                UserName = userName,
                Message_ = message.Content
            }),
            context.CancellationToken);

        await dbContext.SaveChangesAsync(context.CancellationToken);
        
        logger.LogInformation("Message with id: {messageId} has been sent from user: {name}", message.Id, userName);
        
        return new SendMessageResponse
        {
            MessageId = message.Id
        };
    }

    [Authorize]
    public override async Task SubscribeOnNewMessages(Empty request, IServerStreamWriter<Message> responseStream,
        ServerCallContext context)
    {
        var userName = context.GetHttpContext().User.Identity!.Name!;
        logger.LogInformation("User with name: {name} subscribe on new messages", userName);
        
        var lastMessageId = await dbContext.Messages
            .OrderByDescending(x => x.Id)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(context.CancellationToken);

        if (lastMessageId == 0)
            lastMessageId = 1;
        
        while (!context.CancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("Checking for new messages");
            var cacheMessage = await cache.GetStringAsync((lastMessageId+1).ToString());
            
            if (cacheMessage == null)
            {
                logger.LogInformation("New messages not found found");
                await Task.Delay(TimeSpan.FromSeconds(0.25), context.CancellationToken);
                continue;
            }
            
            logger.LogInformation("New messages found, sending new message with id: {id}", lastMessageId + 1);
            
            var message = JsonSerializer.Deserialize<Message>(cacheMessage)!;
            await responseStream.WriteAsync(message, context.CancellationToken);
            lastMessageId = message.MessageId;
        }
    }
}