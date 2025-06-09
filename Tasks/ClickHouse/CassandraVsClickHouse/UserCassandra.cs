using Common.Attributes;

namespace CassandraVsClickHouse;

[Table(TableName = "users", Engine = $"MergeTree ORDER BY {nameof(Id)}")]
public class UserCassandra : User
{
    [Column(Name = "Id", Type = "int")]
    public override int Id { get; set; }

    [Column(Name = "Name", Type = "text")]
    public override string Name { get; set; } = null!;

    [Column(Name = "Email", Type = "text")]
    public override string Email { get; set; } = null!;
}