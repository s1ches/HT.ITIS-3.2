using Orders.Common.Enums;

namespace Orders.Common.Abstractions;

public interface IOutboxMessage
{
    Guid Id { get; set; }

    OutboxMessageState State { get; set; }

    DateTime UpdateDate { get; set; }

    DateTime CreateDate { get; set; }
}