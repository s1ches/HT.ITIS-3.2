using GraphQL.HotChocolateExample.Data;
using GraphQL.HotChocolateExample.Data.Entities;

namespace GraphQL.HotChocolateExample.GraphQL;

public class Query
{
    [UseOffsetPaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Hero> GetHeroes([Service] DataContext dbContext) => dbContext.Heroes;
    
    [UseOffsetPaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Ability> GetAbilities([Service] DataContext dbContext) => dbContext.Abilities;
}