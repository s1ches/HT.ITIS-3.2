using System.Text.Json;
using GraphQL.HotChocolateExample.Cache.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace GraphQL.HotChocolateExample.Cache.GraphQL;

public class Query
{
    public async Task<HumanModel?> GetHuman([Service] IDistributedCache cache, string key)
    {
        var serializedHuman = await cache.GetStringAsync(key);
        
        if(serializedHuman == null)
            return null;
        
        var human = JsonSerializer.Deserialize<HumanModel>(serializedHuman);
        
        if(human == null)
            throw new InvalidDataException("Human cannot be deserialized.");
        
        return human;
    }
}