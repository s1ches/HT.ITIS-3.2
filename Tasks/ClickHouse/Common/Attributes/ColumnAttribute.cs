namespace Common.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ColumnAttribute : Attribute
{
    public string Name { get; set; } = null!;
    
    public string? Type { get; set; }
}