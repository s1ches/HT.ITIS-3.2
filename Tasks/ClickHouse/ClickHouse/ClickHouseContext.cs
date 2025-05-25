using System.Reflection;
using ClickHouse.Client.ADO;
using ColumnAttribute = ClickHouse.Attributes.ColumnAttribute;
using TableAttribute = ClickHouse.Attributes.TableAttribute;

namespace ClickHouse;

public class ClickHouseContext(ClickHouseContextOptions options) : IDisposable, IAsyncDisposable
{
    private readonly ClickHouseConnection _connection = new(options.ConnectionString);

    public async Task CreateTableIfNotExistsAsync<TEntity>(CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var tableAttribute = typeof(TEntity).GetCustomAttribute<TableAttribute>();
        if (tableAttribute == null)
            throw new InvalidOperationException("The table attribute is missing.");

        var tableName = tableAttribute.TableName;
        var engine = tableAttribute.Engine;

        var columns = new List<string>();

        foreach (var property in typeof(TEntity)
                     .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                     .Where(p => p.IsDefined(typeof(ColumnAttribute))))
        {
            var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
            if (columnAttribute == null)
                throw new InvalidOperationException("The column attribute is missing.");

            columns.Add($"{columnAttribute.Name} {columnAttribute.Type}");
        }

        var sql = $"""
                   CREATE TABLE IF NOT EXISTS {tableName}
                   (
                       {string.Join(",\n", columns)}
                   )
                   ENGINE = {engine};
                   """;

        await using var command = _connection.CreateCommand();
        command.CommandText = sql;
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task DropTableIfExistsAsync<TEntity>(CancellationToken cancellationToken = default)
    {
        var tableAttribute = typeof(TEntity).GetCustomAttribute<TableAttribute>();
        if (tableAttribute == null)
            throw new InvalidOperationException("The table attribute is missing.");

        var tableName = tableAttribute.TableName;

        var sql = $"DROP TABLE IF EXISTS {tableName};";

        await using var command = _connection.CreateCommand();
        command.CommandText = sql;

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public void Dispose()
    {
        try
        {
            _connection.Dispose();
        }
        catch (NullReferenceException)
        {
            // Do nothing
        }
    }

    public async Task InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
    {
        var tableAttribute = typeof(TEntity).GetCustomAttribute<TableAttribute>();
        if (tableAttribute == null)
            throw new InvalidOperationException("The table attribute is missing.");

        var tableName = tableAttribute.TableName;

        var columnProps = typeof(TEntity)
            .GetProperties()
            .Where(p => p.IsDefined(typeof(ColumnAttribute)))
            .ToArray();

        var columnNames = columnProps.Select(p => p.GetCustomAttribute<ColumnAttribute>()!.Name).ToArray();

        var values = columnProps.Select(p =>
        {
            var val = p.GetValue(entity);
            if (val == null) return "NULL";

            return val switch
            {
                string s => $"'{s.Replace("'", "''")}'",
                DateTime dt => $"'{dt:yyyy-MM-dd HH:mm:ss}'",
                bool b => b ? "1" : "0",
                _ => val.ToString()
            };
        }).ToArray();

        var columnsString = string.Join(", ", columnNames);
        var valuesString = string.Join(", ", values);

        var sql = $"INSERT INTO {tableName} ({columnsString}) VALUES ({valuesString})";

        await using var command = _connection.CreateCommand();
        command.CommandText = sql;

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task RemoveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
    {
        var tableAttribute = typeof(TEntity).GetCustomAttribute<TableAttribute>();
        if (tableAttribute == null)
            throw new InvalidOperationException("The table attribute is missing.");

        var idProperty = typeof(TEntity)
            .GetProperties()
            .FirstOrDefault(p => p.GetCustomAttribute<ColumnAttribute>()?.Name == "Id");

        if (idProperty == null)
            throw new InvalidOperationException("The entity must have an Id column.");

        var idValue = idProperty.GetValue(entity);

        var whereClause = idValue switch
        {
            string s => $"Id = '{s.Replace("'", "''")}'",
            int or long or bool or double or float => $"Id = {idValue}",
            _ => throw new InvalidOperationException("Unsupported Id type.")
        };

        var sql = $"ALTER TABLE {tableAttribute.TableName} DELETE WHERE {whereClause}";

        await using var command = _connection.CreateCommand();
        command.CommandText = sql;

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<List<TEntity>> FindByAsync<TEntity>(
        string columnName,
        object value,
        CancellationToken cancellationToken = default)
        where TEntity : class, new()
    {
        var tableAttribute = typeof(TEntity).GetCustomAttribute<TableAttribute>();
        if (tableAttribute == null)
            throw new InvalidOperationException("Missing Table attribute.");

        string formattedValue = FormatValue(value);

        var sql = $"SELECT * FROM {tableAttribute.TableName} WHERE {columnName} = {formattedValue}";

        await using var command = _connection.CreateCommand();
        command.CommandText = sql;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var results = new List<TEntity>();
        while (await reader.ReadAsync(cancellationToken))
        {
            var entity = new TEntity();
            foreach (var property in typeof(TEntity).GetProperties())
            {
                var columnAttr = property.GetCustomAttribute<ColumnAttribute>();
                if (columnAttr == null) continue;

                var ordinal = reader.GetOrdinal(columnAttr.Name);
                var valueFromDb = reader.IsDBNull(ordinal) ? null : reader.GetValue(ordinal);
                property.SetValue(entity, valueFromDb);
            }

            results.Add(entity);
        }

        return results;
    }
    
    public async Task<List<TEntity>> FindByConditionsAsync<TEntity>(
        Dictionary<string, object> conditions,
        string logicalOperator = "AND", // или "OR"
        CancellationToken cancellationToken = default)
        where TEntity : class, new()
    {
        if (conditions.Count == 0)
            throw new ArgumentException("Conditions dictionary is empty.");

        var tableAttribute = typeof(TEntity).GetCustomAttribute<TableAttribute>();
        if (tableAttribute == null)
            throw new InvalidOperationException("Missing Table attribute.");

        var whereClauses = conditions.Select(kvp =>
            $"{kvp.Key} = {FormatValue(kvp.Value)}");

        var whereSql = string.Join($" {logicalOperator} ", whereClauses);
        var sql = $"SELECT * FROM {tableAttribute.TableName} WHERE {whereSql}";

        await using var command = _connection.CreateCommand();
        command.CommandText = sql;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var results = new List<TEntity>();
        while (await reader.ReadAsync(cancellationToken))
        {
            var entity = new TEntity();
            foreach (var property in typeof(TEntity).GetProperties())
            {
                var columnAttr = property.GetCustomAttribute<ColumnAttribute>();
                if (columnAttr == null) continue;

                var ordinal = reader.GetOrdinal(columnAttr.Name);
                var valueFromDb = reader.IsDBNull(ordinal) ? null : reader.GetValue(ordinal);
                property.SetValue(entity, valueFromDb);
            }

            results.Add(entity);
        }

        return results;
    }
    
    private static string FormatValue(object value)
    {
        return (value switch
        {
            string s => $"'{s.Replace("'", "''")}'",
            bool b => b ? "1" : "0",
            DateTime dt => $"'{dt:yyyy-MM-dd HH:mm:ss}'",
            null => "NULL",
            _ => value.ToString()
        })!;
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            await _connection.DisposeAsync();
        }
        catch (NullReferenceException)
        {
            // Do nothing
        }
    }
}