using GraphQL.HotChocolateExample.Subscriptions.GraphQL;
using GraphQL.HotChocolateExample.Subscriptions.Services.Worker;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddLogging()
    .AddRouting()
    .AddGraphQLCore()
    .AddGraphQLServer(disableDefaultSecurity: true)
    .AddQueryType<Query>()
    .AddSubscriptionType<Subscription>()
    .AddInMemorySubscriptions();

builder.Services.AddHostedService<WorkerService>();

var app = builder.Build();

app
    .UseRouting()
    .UseEndpoints(endpoints => endpoints.MapGraphQL());

app.RunWithGraphQLCommands(args);