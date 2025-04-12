using MassTransit;
using Microsoft.EntityFrameworkCore;
using Orders.Common.Messages;
using Orders.Delivery.Data;
using Orders.Delivery.Data.Enums;

namespace Orders.Delivery.HostedServices;

public class CreatedOrderConsumer(DataContext dataContext, ILogger<CreatedOrderConsumer> logger)
    : IConsumer<OrderCreatedMessage>
{
    public async Task Consume(ConsumeContext<OrderCreatedMessage> context)
    {
        try
        {
            logger.LogInformation("Got new message with id {id}", context.Message.Id);

            if (await dataContext.Deliveries.AnyAsync(x => x.OrderId == context.Message.OrderId))
            {
                logger.LogInformation("Delivery with OrderId {id} already created", context.Message.OrderId);
                return;
            }

            var deliveryId = Guid.NewGuid();
            var delivery = new Data.Entities.Delivery
            {
                Id = deliveryId,
                Address = context.Message.Address,
                CustomerId = context.Message.CustomerId,
                ProductId = context.Message.ProductId,
                OrderId = context.Message.OrderId,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                DeliveryState = DeliveryState.Registered,
                ProductsCount = context.Message.ProductsCount
            };

            dataContext.Add(delivery);

            var random = new Random();
            var needToThrowException = random.Next(0, 4) == 3;

            if (needToThrowException)
            {
                logger.LogError("Delivery for order message with id {id} has not been created", context.Message.Id);
                throw new Exception($"Unexpected error for message with id {context.Message.Id}");
            }

            await dataContext.SaveChangesAsync(context.CancellationToken);
            logger.LogInformation("Delivery from message with id {messageId} has been created, delivery with id {id}",
                context.Message.Id, deliveryId);
            await context.NotifyConsumed(TimeSpan.Zero, nameof(CreatedOrderConsumer));
        }
        catch (Exception ex)
        {
            await context.NotifyFaulted(TimeSpan.Zero, nameof(CreatedOrderConsumer), ex);
            throw;
        }
    }
}