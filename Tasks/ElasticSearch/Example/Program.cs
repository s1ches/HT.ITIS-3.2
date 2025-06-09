using System.Text.RegularExpressions;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Bulk;
using Example;

var client = new ElasticsearchClient(new ElasticsearchClientSettings(new Uri("http://localhost:9200")));

const string text =
    @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras luctus velit at viverra eleifend. Etiam ac mi id lorem laoreet condimentum. Nullam tincidunt imperdiet ex, ac tempor nisl egestas quis. Proin tempor eget quam eget sollicitudin. Fusce quis lacus posuere, eleifend libero non, tristique libero. Etiam efficitur aliquam velit id sagittis. Ut venenatis sed nisl et tristique. Nam faucibus vehicula euismod. Praesent odio odio, vulputate vel eros sit amet, lobortis maximus tellus. Nam dui lectus, molestie quis turpis id, molestie ornare justo. Etiam eleifend iaculis feugiat. Suspendisse fringilla, metus a posuere molestie, dui ex mattis risus, ac egestas eros dui quis augue. Donec ut odio lacus.";

// Удаляем пунктуацию, разбиваем по пробелу
var words = Regex
    .Replace(text, @"[^\w\s]", "")
    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

var bulkRequest = new BulkRequest("lorem_words")
{
    Operations = []
};

foreach (var word in words)
{
    bulkRequest.Operations.Add(new BulkIndexOperation<WordDocument>(new WordDocument(word)));
}

var response = await client.BulkAsync(bulkRequest);

if (response.IsValidResponse)
{
    Console.WriteLine("Все слова успешно вставлены.");
}
else
{
    Console.WriteLine("Ошибка при bulk-запросе:");
    Console.WriteLine(response.DebugInformation);
}