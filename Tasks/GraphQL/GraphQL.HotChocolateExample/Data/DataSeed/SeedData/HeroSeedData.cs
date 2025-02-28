using GraphQL.HotChocolateExample.Data.Entities;

namespace GraphQL.HotChocolateExample.Data.DataSeed.SeedData;

public static class HeroSeedData
{
    private static readonly Hero Geralt = new Hero
    {
        Id = Guid.Parse("D7DC0B95-A890-4CCE-8978-3E280BB3388C"),
        Name = "Geralt",
        Age = 100,
        Description = "Strong witcher from Wolf school",
        Abilities = [AbilitySeedData.WitcherSenses, AbilitySeedData.SwordMastery, AbilitySeedData.AlchemyExpertise]
    };

    private static readonly Hero Yennefer = new Hero
    {
        Id = Guid.Parse("A1B2C3D4-E5F6-7890-1234-56789ABCDEF0"),
        Name = "Yennefer",
        Age = 98,
        Description = "Powerful sorceress with raven-black hair and violet eyes",
        Abilities = [AbilitySeedData.Teleportation, AbilitySeedData.Fireball, AbilitySeedData.MindControl]
    };

    private static readonly Hero Ciri = new Hero
    {
        Id = Guid.Parse("B2C3D4E5-F678-9012-3456-789ABCDEF012"),
        Name = "Ciri",
        Age = 21,
        Description = "Child of Destiny, ashen-haired warrior with Elder Blood",
        Abilities = [AbilitySeedData.Blink, AbilitySeedData.ElderBlood, AbilitySeedData.Teleportation]
    };

    private static readonly Hero Triss = new Hero
    {
        Id = Guid.Parse("C3D4E5F6-7890-1234-5678-9ABCDEF01234"),
        Name = "Triss",
        Age = 37,
        Description = "Fiery-haired sorceress, expert in healing and alchemy",
        Abilities = [AbilitySeedData.Teleportation, AbilitySeedData.Fireball, AbilitySeedData.MindControl]
    };

    private static readonly Hero Vesemir = new Hero
    {
        Id = Guid.Parse("D4E5F678-9012-3456-789A-BCDEF0123456"),
        Name = "Vesemir",
        Age = 200,
        Description = "Oldest and wisest Witcher, mentor of Geralt",
        Abilities = [AbilitySeedData.WitcherSenses, AbilitySeedData.SwordMastery, AbilitySeedData.AlchemyExpertise]
    };

    private static readonly Hero Dandelion = new Hero
    {
        Id = Guid.Parse("E5F67890-1234-5678-9ABC-DEF012345678"),
        Name = "Dandelion",
        Age = 38,
        Description = "Famous bard, poet, and chronicler of Geralt's adventures",
        Abilities = [AbilitySeedData.SilverTongue]
    };

    private static readonly Hero Zoltan = new Hero
    {
        Id = Guid.Parse("F6789012-3456-789A-BCDE-F01234567890"),
        Name = "Zoltan",
        Age = 55,
        Description = "Gruff but kind-hearted dwarf, skilled warrior and drinker",
        Abilities = []
    };

    private static readonly Hero Regis = new Hero
    {
        Id = Guid.Parse("67890123-4567-89AB-CDEF-0123456789AB"),
        Name = "Regis",
        Age = 428,
        Description = "Higher vampire, philosopher, and skilled herbalist",
        Abilities = [AbilitySeedData.Regeneration, AbilitySeedData.VampireForm]
    };

    private static readonly Hero Letho = new Hero
    {
        Id = Guid.Parse("78901234-5678-9ABC-DEF0-1234567890AB"),
        Name = "Letho",
        Age = 45,
        Description = "Massive, ruthless witcher from the Viper school",
        Abilities = [AbilitySeedData.WitcherSenses, AbilitySeedData.SwordMastery, AbilitySeedData.AlchemyExpertise]
    };

    private static readonly Hero Emhyr = new Hero
    {
        Id = Guid.Parse("89012345-6789-ABCD-EF01-234567890ABC"),
        Name = "Emhyr",
        Age = 53,
        Description = "Emperor of Nilfgaard, known as the White Flame",
        Abilities = []
    };

    private static readonly Hero Keira = new Hero
    {
        Id = Guid.Parse("90123456-789A-BCDE-F012-34567890ABCD"),
        Name = "Keira",
        Age = 40,
        Description = "Enchanting sorceress, former advisor to King Foltest",
        Abilities = [AbilitySeedData.Teleportation, AbilitySeedData.Fireball, AbilitySeedData.MindControl]
    };

    public static readonly List<Hero> SeedHeroes =
    [
        Geralt,
        Yennefer,
        Ciri,
        Triss,
        Vesemir,
        Dandelion,
        Zoltan,
        Regis,
        Letho,
        Emhyr,
        Keira
    ];
}