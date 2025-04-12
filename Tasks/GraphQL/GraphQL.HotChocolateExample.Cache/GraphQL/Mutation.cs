using System.Text.Json;
using GraphQL.HotChocolateExample.Cache.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace GraphQL.HotChocolateExample.Cache.GraphQL;

public class Mutation
{
    public async Task<HumanModel> AddHuman([Service] IDistributedCache cache, string name)
    {
        var humanModel = new HumanModel { Name = name };
        var serializedHuman = JsonSerializer.Serialize(humanModel);
        await cache.SetStringAsync(name, serializedHuman);
        return humanModel;
    }
}