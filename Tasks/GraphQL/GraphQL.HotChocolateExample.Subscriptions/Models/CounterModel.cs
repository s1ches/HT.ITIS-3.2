namespace GraphQL.HotChocolateExample.Subscriptions.Models;

public class CounterModel
{
    public CounterModel()
    {
    }

    public CounterModel(long counterValue)
    {
        CounterNumber = counterValue;
    }
    
    public long CounterNumber { get; set; }
}