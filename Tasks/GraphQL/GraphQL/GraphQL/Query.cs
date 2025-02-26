using GraphQL.Data;
using GraphQL.Data.Entities;

namespace GraphQL.GraphQL;

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