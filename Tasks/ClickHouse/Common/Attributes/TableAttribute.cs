namespace Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TableAttribute : Attribute
{
    public string TableName { get; set; } = null!;
    
    public string? Engine { get; set; }
}