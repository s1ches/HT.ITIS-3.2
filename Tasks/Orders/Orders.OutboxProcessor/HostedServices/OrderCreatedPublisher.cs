using MassTransit;
using Microsoft.EntityFrameworkCore;
using Orders.Common.Enums;
using Orders.Management.Data;

namespace Orders.OutboxProcessor.HostedServices;

public class OrderCreatedPublisher(ILogger<OrderCreatedPublisher> logger, IServiceScopeFactory serviceScopeFactory)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Order created publisher started a new iteration");

            using var scope = serviceScopeFactory.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            await using var dataContext = serviceProvider.GetRequiredService<DataContext>();

            try
            {
                logger.LogInformation("Getting created orders messages");
                var messages = await dataContext.OrderCreatedMessages
                    .Where(x => x.State == OutboxMessageState.Pending)
                    .Take(50)
                    .ToListAsync(cancellationToken: stoppingToken);

                if (!messages.Any())
                {
                    logger.LogInformation("No orders created");
                    await Task.Delay(5000, stoppingToken);
                    continue;
                }

                messages.ForEach(x => x.State = OutboxMessageState.Sending);

                {
                    await using var transaction = await dataContext.Database.BeginTransactionAsync(stoppingToken);
                    try
                    {
                        dataContext.OrderCreatedMessages.UpdateRange(messages);
                        await dataContext.SaveChangesAsync(stoppingToken);
                        await transaction.CommitAsync(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync(stoppingToken);
                        logger.LogError(ex, "An error occurred while updating orders messages state to Sending");
                        throw;
                    }
                }
                
                logger.LogInformation("Orders created messages statuses updated to Sending");
                logger.LogInformation("Sending orders created messages");
                var publisher = serviceProvider.GetRequiredService<IPublishEndpoint>();
                await publisher.PublishBatch(messages, cancellationToken: stoppingToken);
                
                logger.LogInformation("Orders created messages sent");

                {
                    messages.ForEach(x => x.State = OutboxMessageState.Sent);
                    await using var transaction = await dataContext.Database.BeginTransactionAsync(stoppingToken);
                    try
                    {
                        dataContext.OrderCreatedMessages.UpdateRange(messages);
                        await dataContext.SaveChangesAsync(stoppingToken);
                        await transaction.CommitAsync(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync(stoppingToken);
                        logger.LogError(ex, "An error occurred while updating orders messages state to Sent");
                        throw;
                    }
                }
                
                logger.LogInformation("Orders created messages statuses updated to Sent");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}