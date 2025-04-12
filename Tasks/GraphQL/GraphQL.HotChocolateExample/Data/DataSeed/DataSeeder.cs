using GraphQL.HotChocolateExample.Data.DataSeed.SeedData;

namespace GraphQL.HotChocolateExample.Data.DataSeed;

public class DataSeeder(DataContext dataContext) : IDataSeeder
{
    public void Seed()
    {
        if (!dataContext.Heroes.Any())
        {
            dataContext.AddRange(HeroSeedData.SeedHeroes);
            dataContext.SaveChanges();
        }
    }
}