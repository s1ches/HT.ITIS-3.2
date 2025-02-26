using GraphQL.Data.Abstractions.BaseEntities;

namespace GraphQL.Data.Entities;

public class Hero : EntityBase
{
    public string Name { get; set; } = null!;
    
    public int Age { get; set; }
    
    public string Description { get; set; } = null!;
    
    public ICollection<Ability> Abilities { get; set; } = [];
}