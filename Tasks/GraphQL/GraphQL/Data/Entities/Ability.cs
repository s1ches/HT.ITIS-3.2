using GraphQL.Data.Abstractions.BaseEntities;

namespace GraphQL.Data.Entities;

public class Ability : EntityBase
{
    public string Name { get; set; } = null!;

    public List<Hero> Heroes { get; set; } = [];
}