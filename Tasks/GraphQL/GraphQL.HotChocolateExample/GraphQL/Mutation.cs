using GraphQL.HotChocolateExample.Data;
using GraphQL.HotChocolateExample.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.HotChocolateExample.GraphQL;

public class Mutation
{
    public async Task<Guid> AddAbility([Service] DataContext dbContext, string abilityName,
        CancellationToken cancellationToken)
    {
        var abilityEntry = dbContext.Abilities.Add(new Ability
        {
            Name = abilityName
        });

        await dbContext.SaveChangesAsync(cancellationToken);
        return abilityEntry.Entity.Id;
    }

    public async Task<bool> RemoveAbility([Service] DataContext dbContext, Guid abilityId,
        CancellationToken cancellationToken)
    {
        var ability = await dbContext.Abilities.FirstOrDefaultAsync(x => x.Id == abilityId, cancellationToken);

        if (ability == null)
            return true;
        
        dbContext.Abilities.Remove(ability);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}