using GraphQL.Data;
using GraphQL.Data.DataSeed;
using GraphQL.GraphQL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddLogging()
    .AddRouting()
    .AddGraphQLCore()
    .AddGraphQLServer(disableDefaultSecurity: true)
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddMutationConventions()
    .AddFiltering()
    .AddSorting()
    .AddProjections();

var connectionString = builder.Configuration.GetSection("DbContextOptions").GetValue<string>("ConnectionString");
builder.Services.AddDbContext<DataContext>(opt => opt.UseNpgsql(connectionString));
builder.Services.AddScoped<IDataSeeder, DataSeeder>();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
dbContext.Database.Migrate();

var dataSeeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
dataSeeder.Seed();

app
    .UseRouting()
    .UseEndpoints(endpoints => endpoints.MapGraphQL());

app.Run();