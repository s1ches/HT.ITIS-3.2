namespace Orders.Common.Abstractions;

public interface IOutboxMessageEntity
{
    IOutboxMessage Message { get; }
}