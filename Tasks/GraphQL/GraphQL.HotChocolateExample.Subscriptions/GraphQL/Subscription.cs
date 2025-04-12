using GraphQL.HotChocolateExample.Subscriptions.Models;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;

namespace GraphQL.HotChocolateExample.Subscriptions.GraphQL;

public class Subscription
{
    public ValueTask<ISourceStream<CounterModel>> SubscribeToCounterUpdate([Service] ITopicEventReceiver receiver,
        CancellationToken cancellationToken = default)
        => receiver.SubscribeAsync<CounterModel>(SubscriptionNames.BaseSubscriptionName, cancellationToken);

    [Subscribe(With = nameof(SubscribeToCounterUpdate))]
    public CounterModel OnCounterUpdate([EventMessage] CounterModel counterModel)
        => counterModel;
}