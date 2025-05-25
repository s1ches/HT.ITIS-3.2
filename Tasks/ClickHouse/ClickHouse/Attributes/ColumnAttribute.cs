namespace ClickHouse.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ColumnAttribute : Attribute
{
    public required string Name { get; set; }
    
    public required string Type { get; set; }
}