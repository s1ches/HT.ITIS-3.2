namespace Cassandra;

public record CassandraContextOptions
{
    public string Host { get; init; } = "127.0.0.1";
    public int Port { get; init; } = 9042;
    public string Keyspace { get; init; } = "your_keyspace";
}