using GraphQL.HotChocolateExample.Data.Entities;

namespace GraphQL.HotChocolateExample.Data.DataSeed.SeedData;

public class AbilitySeedData
{
    public static readonly Ability WitcherSenses = new Ability
    {
        Id = Guid.Parse("5DEC2A48-73D9-4A23-B1BF-7F3D64731A54"),
        Name = "Witcher Senses",
    };

    public static readonly Ability SwordMastery = new Ability
    {
        Id = Guid.Parse("CC31A9F0-E41D-4ECB-B3C9-7B26B93C3D03"),
        Name = "Sword Mastery",
    };

    public static readonly Ability AlchemyExpertise = new Ability
    {
        Id = Guid.Parse("AF49B9EA-70AC-4F8D-BB9B-3845DB26BBC8"),
        Name = "Alchemy Expertise",
    };

    public static readonly Ability Teleportation = new Ability
    {
        Id = Guid.Parse("6385902D-7D10-4CD9-B7A8-3622885CC235"),
        Name = "Teleportation",
    };

    public static readonly Ability Fireball = new Ability
    {
        Id = Guid.Parse("AFE26FC3-6503-436B-B0CC-E7B48ED9C70F"),
        Name = "Fireball",
    };

    public static readonly Ability MindControl = new Ability
    {
        Id = Guid.Parse("90F6A468-BAEF-4CD6-BF8C-76FF2E281C7E"),
        Name = "Mind Control",
    };

    public static readonly Ability Blink = new Ability
    {
        Id = Guid.Parse("DAAF3588-75FA-4571-AE04-00B230F8A1B5"),
        Name = "Blink",
    };

    public static readonly Ability ElderBlood = new Ability
    {
        Id = Guid.Parse("CEC8E207-0DE1-46B3-8683-3D98A81991CB"),
        Name = "Elder Blood",
    };

// 🎭 Герой: Dandelion
    public static readonly Ability SilverTongue = new Ability
    {
        Id = Guid.Parse("70393E52-775D-4E6C-A820-DEB3A4540B20"),
        Name = "Silver Tongue",
    };

// 🦇 Герой: Regis
    public static readonly Ability Regeneration = new Ability
    {
        Id = Guid.Parse("6BCD61A9-8B01-45CA-9395-AFADDCA8046E"),
        Name = "Regeneration",
    };

    public static readonly Ability VampireForm = new Ability
    {
        Id = Guid.Parse("B697A295-1BE4-411B-A704-ED1EF31F3B0A"),
        Name = "Vampire Form",
    };
    
// 🏰 Герой: Emhyr
    public static readonly Ability TacticalGenius = new Ability
    {
        Id = Guid.Parse("76BE2B1B-A241-49DC-8581-E5305114C1A7"),
        Name = "Tactical Genius",
    };
}