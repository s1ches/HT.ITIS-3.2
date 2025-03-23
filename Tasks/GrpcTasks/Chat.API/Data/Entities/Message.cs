using System.ComponentModel.DataAnnotations;

namespace Chat.API.Data.Entities;

public class Message
{
    [Key]
    public long Id { get; set; }
    
    public string UserName { get; set; } = null!;
    
    public string Content { get; set; } = null!;
}