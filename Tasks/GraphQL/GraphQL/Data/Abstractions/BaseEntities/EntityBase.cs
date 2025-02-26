using GraphQL.Data.Abstractions.Interfaces;

namespace GraphQL.Data.Abstractions.BaseEntities;

public class EntityBase : IEntity
{
    public Guid Id { get; set; }
}