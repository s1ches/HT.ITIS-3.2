namespace ClickHouse.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TableAttribute
    : Attribute
{
    public required string TableName { get; set; }

    public required string Engine { get; set; }
}