using ClickHouse;
using ClickHouse.Example;

const string connectionString = "Host=localhost;Port=8123;Username=default;Password=;Database=default;";

var contextOptions = new ClickHouseContextOptions
{
    ConnectionString = connectionString
};

await using var context = new ClickHouseContext(contextOptions);
await context.CreateTableIfNotExistsAsync<Entity>(CancellationToken.None);

var entity = new Entity
{
    Id = 1,
    Value = "First entity"
};

await context.InsertAsync(entity, CancellationToken.None);

var find = await context.FindByAsync<Entity>(nameof(Entity.Id), 1, CancellationToken.None);
var findCond = await context.FindByConditionsAsync<Entity>(
    new Dictionary<string, object>
    {
        { "Id", 1 },
        { "Value", "First entity" }
    },
    logicalOperator: "AND",
    CancellationToken.None
);

await context.RemoveAsync(entity, CancellationToken.None);

await context.DropTableIfExistsAsync<Entity>(CancellationToken.None);