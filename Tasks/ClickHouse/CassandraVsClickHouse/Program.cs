using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace CassandraVsClickHouse;

[MemoryDiagnoser]
public class DatabaseBenchmark
{
    private CassandraContext _cassandraContext = null!;
    private ClickHouseContext _clickHouseContext = null!;
    private List<User> _testData = null!;
    private const int RecordCount = 2000;

    [GlobalSetup]
    public async Task Setup()
    {
        _cassandraContext = new CassandraContext(new CassandraContextOptions
        {
            Host = "127.0.0.1",
            Port = 9042,
            Keyspace = "test"
        });

        _clickHouseContext = new ClickHouseContext(new ClickHouseContextOptions
        {
            ConnectionString = "Host=localhost;Port=8123;Username=default;Password=;Database=default;"
        });

        await _clickHouseContext.CreateTableIfNotExistsAsync<UserClickHouse>();
        await _cassandraContext.CreateTableIfNotExistsAsync<UserCassandra>();

        _testData = Enumerable.Range(0, RecordCount).Select(i => new User
        {
            Id = i,
            Name = "Name_" + i,
            Email = $"email{i}@test.com"
        }).ToList();
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _clickHouseContext.DropTableIfExistsAsync<UserClickHouse>();
        await _cassandraContext.DropTableIfExistsAsync<UserCassandra>();
    }

    [Benchmark]
    public async Task Cassandra_Insert()
    {
        foreach (var user in _testData)
        {
            await _cassandraContext.InsertAsync(user);
        }
    }

    [Benchmark]
    public async Task ClickHouse_Insert()
    {
        foreach (var user in _testData)
        {
            await _clickHouseContext.InsertAsync(user);
        }
    }

    [Benchmark]
    public async Task Cassandra_Find()
    {
        var email = _testData[100].Email;
        var result = await _cassandraContext.FindByAsync<UserCassandra>("Email", email);
    }

    [Benchmark]
    public async Task ClickHouse_Find()
    {
        var email = _testData[100].Email;
        var result = await _clickHouseContext.FindByAsync<UserClickHouse>("Email", email);
    }

    [Benchmark]
    public async Task Cassandra_Remove()
    {
        foreach (var user in _testData.Take(10))
        {
            await _cassandraContext.RemoveAsync(user);
        }
    }

    [Benchmark]
    public async Task ClickHouse_Remove()
    {
        foreach (var user in _testData.Take(10))
        {
            await _clickHouseContext.RemoveAsync(user);
        }
    }

    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<DatabaseBenchmark>();
    }
}