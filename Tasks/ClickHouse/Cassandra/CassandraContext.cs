using System.Reflection;
using Common.Attributes;

namespace Cassandra;

public class CassandraContext : IDisposable
{
    private readonly ISession _session;

    public CassandraContext(CassandraContextOptions options)
    {
        var keyspace = options.Keyspace;

        var cluster = Cluster.Builder()
            .AddContactPoint(options.Host)
            .WithPort(options.Port)
            .Build();

        var sysSession = cluster.Connect();

        var createKeyspaceCql = $"CREATE KEYSPACE IF NOT EXISTS {keyspace} " +
                                "WITH replication = { 'class' : 'SimpleStrategy', 'replication_factor' : 1 };";
        sysSession.Execute(createKeyspaceCql);

        _session = cluster.Connect(keyspace);
    }

    public async Task InsertAsync<TEntity>(TEntity entity)
    {
        var tableAttr = typeof(TEntity).GetCustomAttribute<TableAttribute>() 
                        ?? throw new InvalidOperationException("Missing Table attribute.");
        var props = typeof(TEntity)
            .GetProperties()
            .Where(p => p.IsDefined(typeof(ColumnAttribute)))
            .ToArray();

        var columns = props.Select(p => p.GetCustomAttribute<ColumnAttribute>()!.Name).ToArray();
        var values = props.Select(p => p.GetValue(entity)).ToArray();

        var placeholders = string.Join(", ", columns.Select(_ => "?"));
        var columnsJoined = string.Join(", ", columns);
        var cql = $"INSERT INTO {tableAttr.TableName} ({columnsJoined}) VALUES ({placeholders})";

        var statement = (await _session.PrepareAsync(cql)).Bind(values);
        await _session.ExecuteAsync(statement);
    }

    public async Task RemoveAsync<TEntity>(TEntity entity)
    {
        var tableAttr = typeof(TEntity).GetCustomAttribute<TableAttribute>() 
                        ?? throw new InvalidOperationException("Missing Table attribute.");

        var idProp = typeof(TEntity).GetProperties()
            .FirstOrDefault(p => p.GetCustomAttribute<ColumnAttribute>()?.Name == "Id");
        if (idProp == null)
            throw new InvalidOperationException("Missing Id column.");

        var idValue = idProp.GetValue(entity);
        if (idValue == null)
            throw new InvalidOperationException("Id value is null.");

        var cql = $"DELETE FROM {tableAttr.TableName} WHERE Id = ?";
        var statement = (await _session.PrepareAsync(cql)).Bind(idValue);
        await _session.ExecuteAsync(statement);
    }

    public async Task<List<TEntity>> FindByAsync<TEntity>(string columnName, object value)
        where TEntity : class, new()
    {
        var tableAttr = typeof(TEntity).GetCustomAttribute<TableAttribute>()
                        ?? throw new InvalidOperationException("Missing Table attribute.");

        var cql = $"SELECT * FROM {tableAttr.TableName} WHERE {columnName} = ?";
        var statement = (await _session.PrepareAsync(cql)).Bind(value);

        var result = await _session.ExecuteAsync(statement);
        var rows = result.GetRows();

        var props = typeof(TEntity).GetProperties()
            .Where(p => p.IsDefined(typeof(ColumnAttribute)))
            .ToArray();

        var list = new List<TEntity>();
        foreach (var row in rows)
        {
            var entity = new TEntity();
            foreach (var prop in props)
            {
                var columnAttr = prop.GetCustomAttribute<ColumnAttribute>()!;
                var colName = columnAttr.Name;

                if (!row.IsNull(colName))
                {
                    var val = row.GetValue<object>(colName);
                    prop.SetValue(entity, val);
                }
            }
            
            list.Add(entity);
        }

        return list;
    }
    
    public async Task CreateTableIfNotExistsAsync<TEntity>(CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var tableAttribute = typeof(TEntity).GetCustomAttribute<TableAttribute>();
        if (tableAttribute == null)
            throw new InvalidOperationException("The table attribute is missing.");

        var tableName = tableAttribute.TableName;

        var columns = new List<string>();
        string? primaryKey = null;

        foreach (var property in typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
            if (columnAttribute == null) continue;

            columns.Add($"{columnAttribute.Name} {columnAttribute.Type}");

            if (columnAttribute.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
            {
                primaryKey = columnAttribute.Name;
            }
        }

        if (primaryKey == null)
            throw new InvalidOperationException("No primary key defined in entity.");

        var cql = $"""
                   CREATE TABLE IF NOT EXISTS {tableName} (
                       {string.Join(",\n", columns)},
                       PRIMARY KEY ({primaryKey})
                   );
                   """;

        await _session.ExecuteAsync(new SimpleStatement(cql));
    }

    public async Task DropTableIfExistsAsync<TEntity>(CancellationToken cancellationToken = default)
    {
        var tableAttribute = typeof(TEntity).GetCustomAttribute<TableAttribute>();
        if (tableAttribute == null)
            throw new InvalidOperationException("The table attribute is missing.");

        var tableName = tableAttribute.TableName;

        var cql = $"DROP TABLE IF EXISTS {tableName};";

        await _session.ExecuteAsync(new SimpleStatement(cql));
    }

    public void Dispose()
    {
        try
        {
            _session.Dispose();
            GC.SuppressFinalize(this);
        }
        catch (NullReferenceException)
        {
            // Do nothing
        }
    }
}