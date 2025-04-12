using GraphQL.HotChocolateExample.Cache.GraphQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddLogging()
    .AddRouting()
    .AddGraphQLCore()
    .AddGraphQLServer(disableDefaultSecurity: true)
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddMutationConventions();

builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

app
    .UseRouting()
    .UseEndpoints(endpoints => endpoints.MapGraphQL());

app.RunWithGraphQLCommands(args);