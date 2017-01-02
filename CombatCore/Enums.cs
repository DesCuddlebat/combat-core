using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBot
{
    public enum Material
    {
        //Logs (Woodcutter)
        BirchLog = 0, OakLog = 1, SpurceLog = 2, DarkLog = 3,
        //Woods (Carpenter)
        BirchWood = 64, OakWood = 65, SpurceWood = 66, DarkWood = 67,
        //Furniture (Carpenter)

        //Fish (Fisher)
        Bass = 512, Cod = 513,

        //Ores (Miner)
        TinOre = 1024, CopperOre = 1025, IronOre = 1026, SilverOre = 1027, GoldOre = 1028, PlatinumOre = 1029,
        //Ingots (Smelter)
        TinIngot = 1088, CopperIngot = 1089, IronIngot = 1090, SilverIngot = 1091, GoldIngot = 1092, PlatinumIngot = 1093,
        //Gems (Extractor)
        Emerald = 1152, Ruby = 1153, Diamond = 1154, Sapphire = 1155,
        //Alloys (Alloyer)
        BronzeIngot = 1216,
        //Rocks (Digger)
        Marble = 1280, Limestone = 1281, Sandstone = 1282, Chalk = 1283,
        //Processed Rocks (Digger)
        ProcessedMarble = 1344, ProcessedLimestone = 1345, ProcessedSandstone = 1346, ProcessedChalk = 1347,

        //Herbs (Herbalist)
        Thyme = 1536, Basil = 1537, Mint = 1538,
        //Fruits (Fruit picker)
        Apple = 1600, Pear = 1601, Orange = 1602, Lemon = 1603,


        None = 65535
    }

    public enum Skill
    {
        Woodcutting = 0,
        Carpenting = 1,

        Fishing = 8,

        Mining = 16,
        Smelting = 17,
        GemExtracting = 18,
        Digging = 19,
        RockPolishing = 20,

        Herbalism = 24,
        FruitPicking = 25

    }

    public enum IOMode
    {
        Discord = 0,
        Client = 1,  //? Currently only Discord and Console (admin) is supported
        Web = 2,     //?
        Console = 3
    }

    public enum Process //processes that can run pararelly, among with player action.
    {
        Saw = 0,
        Cook = 8,
        Smelt = 16,
        Dry = 24
    }
}