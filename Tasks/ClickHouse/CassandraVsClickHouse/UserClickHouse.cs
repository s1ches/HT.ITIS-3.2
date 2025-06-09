using Common.Attributes;

namespace CassandraVsClickHouse;

[Table(TableName = "users", Engine = $"MergeTree ORDER BY {nameof(Id)}")]
public class UserClickHouse : User
{
    [Column(Name = "Id", Type = "Int32")]
    public override int Id { get; set; }

    [Column(Name = "Name", Type = "String")]
    public override string Name { get; set; } = null!;

    [Column(Name = "Email", Type = "String")]
    public override string Email { get; set; } = null!;
}