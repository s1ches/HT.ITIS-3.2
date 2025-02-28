using GraphQL.HotChocolateExample.Data.Abstractions.Interfaces;

namespace GraphQL.HotChocolateExample.Data.Abstractions.BaseEntities;

public class EntityBase : IEntity
{
    public Guid Id { get; set; }
}