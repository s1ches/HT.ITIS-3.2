using GraphQL.HotChocolateExample.Subscriptions.GraphQL;
using GraphQL.HotChocolateExample.Subscriptions.Models;
using HotChocolate.Subscriptions;

namespace GraphQL.HotChocolateExample.Subscriptions.Services.Worker;

public class WorkerService(IServiceScopeFactory serviceScopeFactory, ILogger<WorkerService> logger) : BackgroundService
{
    private long _counter;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);

            _counter++;

            using (var scope = serviceScopeFactory.CreateScope())
            {
                var sender = scope.ServiceProvider.GetRequiredService<ITopicEventSender>();
                await sender.SendAsync(SubscriptionNames.BaseSubscriptionName, new CounterModel(_counter),
                    stoppingToken);
            }

            logger.LogInformation("Send counter value updated: {counterValue} to {topicName}", _counter,
                SubscriptionNames.BaseSubscriptionName);
        }
    }
}