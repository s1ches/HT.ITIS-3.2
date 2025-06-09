using Common.Attributes;

namespace CassandraVsClickHouse;

[Table(TableName = "users")]
public class User
{
    [Column(Name = "Id")]
    public virtual int Id { get; set; }

    [Column(Name = "Name")]
    public virtual string Name { get; set; } = null!;

    [Column(Name = "Email")]
    public virtual string Email { get; set; } = null!;
}