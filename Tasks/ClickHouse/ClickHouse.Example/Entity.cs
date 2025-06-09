using Common.Attributes;

namespace ClickHouse.Example;

[Table(TableName = "Entities", Engine = $"MergeTree ORDER BY {nameof(Id)}")]
public class Entity
{
    [Column(Name = nameof(Id), Type = "Int32")]
    public int Id { get; set; }

    [Column(Name = nameof(Value), Type = "String")]
    public string Value { get; set; } = null!;
}