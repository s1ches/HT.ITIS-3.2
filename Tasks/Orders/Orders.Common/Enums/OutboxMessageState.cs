namespace Orders.Common.Enums;

public enum OutboxMessageState
{
    Pending = 0,
    Sending = 1,
    Sent = 2,
    Handled = 3
}