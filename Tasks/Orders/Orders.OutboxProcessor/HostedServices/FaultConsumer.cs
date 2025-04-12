using MassTransit;
using Microsoft.EntityFrameworkCore;
using Orders.Common.Enums;
using Orders.Common.Messages;
using Orders.Management.Data;

namespace Orders.OutboxProcessor.HostedServices;

public class FaultConsumer(DataContext dataContext, ILogger<FaultConsumer> logger)
    : IConsumer<Fault<OrderCreatedMessage>>
{
    public async Task Consume(ConsumeContext<Fault<OrderCreatedMessage>> context)
    {
        while (!context.CancellationToken.IsCancellationRequested)
        {
            try
            {
                logger.LogWarning("Received OrderCreatedMessage error with message id {id}",
                    context.Message.Message.Id);
                
                var message = await dataContext.OrderCreatedMessages
                    .FirstAsync(x => x.Id == context.Message.Message.Id);

                message.State = OutboxMessageState.Pending;
                
                dataContext.Update(message);
                await dataContext.SaveChangesAsync(context.CancellationToken);
                
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }
    }
}