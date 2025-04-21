using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;

namespace ModShardLauncher;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "IdentifierTypo")]

public partial class Msl
{
    public enum ItemStatsTier
    {
        [EnumMember(Value = "")]
        none,
        [EnumMember(Value = "1")]
        Tier1,
        [EnumMember(Value = "2")]
        Tier2,
        [EnumMember(Value = "3")]
        Tier3,
        [EnumMember(Value = "4")]
        Tier4,
        [EnumMember(Value = "5")]
        Tier5
    }
    
    public enum ItemStatsCategory
    {
        [EnumMember(Value = "")]
        none,
        medicine,
        beverage,
        junk,
        tool,
        drug,
        alcohol,
        ammo,
        valuable,
        ingredient,
        food,
        trophy,
        commodity,
        material,
        additive,
        resource,
        upgrade,
        flag,
        bag,
        quest,
        scroll,
        book,
        treasure,
        recipe,
        schematic
    }

    public enum ItemStatsSubcategory
    {
        [EnumMember(Value = "")]
        none,
        hide,
        ingredient,
        alchemy,
        gem,
        potion,
        meat,
        meat_large,
        fish,
        vegetable,
        fruit,
        berry,
        herb,
        mushroom,
        dairy,
        pastry,
        dish,
        quest,
        bird,
        treatise
    }
    
    public enum ItemStatsMaterial
    {
        cloth,
        glass,
        organic,
        metal,
        wood,
        leather,
        gold,
        pottery,
        gem,
        silver,
        stone,
        paper
    }
    
    public enum ItemStatsWeight
    {
        Light,
        Medium,
        [EnumMember(Value = "Very Light")]
        VeryLight,
        Heavy,
        Net
    }

    public enum ItemStatsTags
    {
        [EnumMember(Value = "")]
        // special
        none,
        special,
        WIP,
        
        // rarities
        common,
        uncommon,
        rare,
        unique,
        
        // elven
        elven,
        [EnumMember(Value = "elven common")]
        elvencommon,
        [EnumMember(Value = "elven uncommon")]
        elvenuncommon,
        [EnumMember(Value = "elven rare")]
        elvenrare,
        
        // animals
        [EnumMember(Value = "common animal")]
        commonanimal,
        [EnumMember(Value = "uncommon animal")]
        uncommonanimal,
        [EnumMember(Value = "rare animal")]
        rareanimal,
        
        // alchemy
        alchemy,
        [EnumMember(Value = "common alchemy")]
        commonalchemy,
        [EnumMember(Value = "uncommon alchemy")]
        uncommonalchemy,
        
        // food
        [EnumMember(Value = "common raw")]
        commonraw,
        [EnumMember(Value = "uncommon raw")]
        uncommonraw,
        [EnumMember(Value = "rare raw")]
        rareraw,
        [EnumMember(Value = "common cooked")]
        commoncooked,
        [EnumMember(Value = "uncommon cooked")]
        uncommoncooked,
        [EnumMember(Value = "rare cooked")]
        rarecooked,
        
        // dungeons
        crypt,
        catacombs,
        bastion
    }

    public static void InjectTableItemStats(
        string id,
        ItemStatsMaterial Material,
        ItemStatsWeight Weight,
        int? Price = null,
        int? EffPrice = null,
        ItemStatsTier tier = ItemStatsTier.none,
        ItemStatsCategory Cat = ItemStatsCategory.none,
        ItemStatsSubcategory Subcat = ItemStatsSubcategory.none,
        ushort? Fresh = null,
        ushort? Duration = null,
        ushort? Stacks = null,
        short? Hunger = null,
        float? Hunger_Change = null,
        short? Hunger_Resistance = null,
        short? Thirsty = null,
        float? Thirst_Change = null,
        short? Immunity = null,
        float? Immunity_Change = null,
        short? Intoxication = null,
        float? Toxicity_Change = null,
        short? Toxicity_Resistance = null, //Could be ushort ?
        short? Pain = null,
        float? Pain_Change = null,
        short? Pain_Resistance = null, // Could be ushort ?
        short? Pain_Limit = null,
        short? Morale = null,
        float? Morale_Change = null,
        short? Sanity = null,
        float? Sanity_Change = null,
        short? Condition = null,
        short? max_hp = null, // Could be ushort ?
        short? max_hp_res = null, // Could be ushort ?
        short? Health_Restoration = null, // Could be float ?
        short? Healing_Received = null,
        short? max_mp = null, // Could be ushort ?
        short? max_mp_res = null, // Could be ushort ?
        short? MP_Restoration = null,
        short? MP_turn = null, // Could be ushort ?
        short? Fatigue = null,
        float? Fatigue_Change = null,
        short? Fatigue_Gain = null, // Could be ushort ?
        short? Received_XP = null,
        short? Cooldown_Reduction = null,
        short? Weapon_Damage = null,
        short? Hit_Chance = null, // type unknown, assuming short
        short? FMB = null,
        short? CRTD = null, // Could be ushort ?
        short? Fortitude = null, // Could be ushort ?
        short? VSN = null, // type unknown, assuming short
        short? Bleeding_Resistance = null,
        short? Knockback_Resistance = null,
        short? Stun_Resistance = null,
        short? Physical_Resistance = null, // type unknown, assuming short
        short? Nature_Resistance = null,
        short? Magic_Resistance = null,
        short? Slashing_Resistance = null,
        short? Piercing_Resistance = null,
        short? Blunt_Resistance = null,
        short? Rending_Resistance = null,
        short? Fire_Resistance = null,
        short? Shock_Resistance = null,
        short? Poison_Resistance = null,
        short? Caustic_Resistance = null,
        short? Frost_Resistance = null,
        short? Arcane_Resistance = null,
        short? Unholy_Resistance = null,
        short? Sacred_Resistance = null,
        short? Psionic_Resistance = null,
        short? Bleeding_Resistance_2 = null,
        short? Nausea_Chance = null, // Could be ushort ?
        short? Poisoning_Chance = null, // Could be ushort ?
        short? Poisoning_Duration = null,
        bool purse = false,
        bool bottle = false,
        string? upgrade = null, // Could be an enum, keeping as a string for custom upgrades I guess
        short? fodder = null, // Could be ushort ?
        short? stack = null, // Could be ushort ?
        bool fireproof = false,
        bool dropsOnce = false,
        ItemStatsTags tags = ItemStatsTags.none,
        Dictionary<string, string> additionalAttributes = new Dictionary<string, string>()
        )
    {
        // Table filename
        const string tableName = "gml_GlobalScript_table_items_stats";
        
        // Load table if it exists
        List<string> table = ThrowIfNull(ModLoader.GetTable(tableName));

        // Get first line of table, defining all columns. 
        string[] columnLine = table[0].Split(";");

        // Insert vanilla attributes at their respective indices in the new line, as an array
        string[] newlineArray = new string[columnLine.Length];
        newlineArray[Array.IndexOf(columnLine, "id")] = id;
        newlineArray[Array.IndexOf(columnLine, "Price")] = Price;
        newlineArray[Array.IndexOf(columnLine, "EffPrice")] = EffPrice;
        newlineArray[Array.IndexOf(columnLine, "tier")] = GetEnumMemberValue(tier);
        newlineArray[Array.IndexOf(columnLine, "Cat")] = GetEnumMemberValue(Cat);
        newlineArray[Array.IndexOf(columnLine, "Subcat")] = GetEnumMemberValue(Subcat);
        newlineArray[Array.IndexOf(columnLine, "Material")] = GetEnumMemberValue(Material);
        newlineArray[Array.IndexOf(columnLine, "Weight")] = GetEnumMemberValue(Weight);
        newlineArray[Array.IndexOf(columnLine, "Fresh")] = Fresh;
        newlineArray[Array.IndexOf(columnLine, "Duration")] = Duration;
        newlineArray[Array.IndexOf(columnLine, "Stacks")] = Stacks;
        newlineArray[Array.IndexOf(columnLine, "Hunger")] = Hunger;
        newlineArray[Array.IndexOf(columnLine, "Hunger_Change")] = Hunger_Change;
        newlineArray[Array.IndexOf(columnLine, "Hunger_Resistance")] = Hunger_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Thirsty")] = Thirsty;
        newlineArray[Array.IndexOf(columnLine, "Thirst_Change")] = Thirst_Change;
        newlineArray[Array.IndexOf(columnLine, "Immunity")] = Immunity;
        newlineArray[Array.IndexOf(columnLine, "Immunity_Change")] = Immunity_Change;
        newlineArray[Array.IndexOf(columnLine, "Intoxication")] = Intoxication;
        newlineArray[Array.IndexOf(columnLine, "Toxicity_Change")] = Toxicity_Change;
        newlineArray[Array.IndexOf(columnLine, "Toxicity_Resistance")] = Toxicity_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Pain")] = Pain;
        newlineArray[Array.IndexOf(columnLine, "Pain_Change")] = Pain_Change;
        newlineArray[Array.IndexOf(columnLine, "Pain_Resistance")] = Pain_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Pain_Limit")] = Pain_Limit;
        newlineArray[Array.IndexOf(columnLine, "Morale")] = Morale;
        newlineArray[Array.IndexOf(columnLine, "Morale_Change")] = Morale_Change;
        newlineArray[Array.IndexOf(columnLine, "Sanity")] = Sanity;
        newlineArray[Array.IndexOf(columnLine, "Sanity_Change")] = Sanity_Change;
        newlineArray[Array.IndexOf(columnLine, "Condition")] = Condition;
        newlineArray[Array.IndexOf(columnLine, "max_hp")] = max_hp;
        newlineArray[Array.IndexOf(columnLine, "max_hp_res")] = max_hp_res;
        newlineArray[Array.IndexOf(columnLine, "Health_Restoration")] = Health_Restoration;
        newlineArray[Array.IndexOf(columnLine, "Healing_Received")] = Healing_Received;
        newlineArray[Array.IndexOf(columnLine, "max_mp")] = max_mp;
        newlineArray[Array.IndexOf(columnLine, "max_mp_res")] = max_mp_res;
        newlineArray[Array.IndexOf(columnLine, "MP_Restoration")] = MP_Restoration;
        newlineArray[Array.IndexOf(columnLine, "MP_turn")] = MP_turn;
        newlineArray[Array.IndexOf(columnLine, "Fatigue")] = Fatigue;
        newlineArray[Array.IndexOf(columnLine, "Fatigue_Change")] = Fatigue_Change;
        newlineArray[Array.IndexOf(columnLine, "Fatigue_Gain")] = Fatigue_Gain;
        newlineArray[Array.IndexOf(columnLine, "Received_XP")] = Received_XP;
        newlineArray[Array.IndexOf(columnLine, "Cooldown_Reduction")] = Cooldown_Reduction;
        newlineArray[Array.IndexOf(columnLine, "Weapon_Damage")] = Weapon_Damage;
        newlineArray[Array.IndexOf(columnLine, "Hit_Chance")] = Hit_Chance;
        newlineArray[Array.IndexOf(columnLine, "FMB")] = FMB;
        newlineArray[Array.IndexOf(columnLine, "CRTD")] = CRTD;
        newlineArray[Array.IndexOf(columnLine, "Fortitude")] = Fortitude;
        newlineArray[Array.IndexOf(columnLine, "VSN")] = VSN;
        newlineArray[Array.IndexOf(columnLine, "Bleeding_Resistance")] = Bleeding_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Knockback_Resistance")] = Knockback_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Stun_Resistance")] = Stun_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Physical_Resistance")] = Physical_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Nature_Resistance")] = Nature_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Magic_Resistance")] = Magic_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Slashing_Resistance")] = Slashing_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Piercing_Resistance")] = Piercing_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Blunt_Resistance")] = Blunt_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Rending_Resistance")] = Rending_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Fire_Resistance")] = Fire_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Shock_Resistance")] = Shock_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Poison_Resistance")] = Poison_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Caustic_Resistance")] = Caustic_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Frost_Resistance")] = Frost_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Arcane_Resistance")] = Arcane_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Unholy_Resistance")] = Unholy_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Sacred_Resistance")] = Sacred_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Psionic_Resistance")] = Psionic_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Nausea_Chance")] = Nausea_Chance;
        newlineArray[Array.IndexOf(columnLine, "Poisoning_Chance")] = Poisoning_Chance;
        newlineArray[Array.IndexOf(columnLine, "Poisoning_Duration")] = Poisoning_Duration;
        newlineArray[Array.IndexOf(columnLine, "purse")] = purse;
        newlineArray[Array.IndexOf(columnLine, "bottle")] = bottle;
        newlineArray[Array.IndexOf(columnLine, "upgrade")] = upgrade;
        newlineArray[Array.IndexOf(columnLine, "fodder")] = fodder;
        newlineArray[Array.IndexOf(columnLine, "stack")] = stack;
        newlineArray[Array.IndexOf(columnLine, "fireproof")] = fireproof;
        newlineArray[Array.IndexOf(columnLine, "dropsOnce")] = dropsOnce;
        newlineArray[Array.IndexOf(columnLine, "tags")] = GetEnumMemberValue(tags);

        // If additional attributes are included, check to see if get they exist in the current table and add their indices. 
        // We assume that any unused additional attributes are 'deactivated'; that the modder will have inserted all necessesary columns beforehand. 
        // If your mod is dependent on another mod, you should still use InjectTableNewColumn to maintain load order agnosticism. Simply tell it not to overwrite positional values.
        // This way, we never have to resize the newlineArray
        if (additionalAttributes.Length > 0)
        {
            foreach(string key in columnLine)
            {
                if (additionalAttributes.ContainsKey(key))
                {
                    newlineArray[Array.IndexOf(columnLine, entry)] = additionalAttributes[entry]; // Worst-case scenario, redundant entries are overwritten. 
                }
            }
        }

        // Convert the new line to string form. 
        string newline = String.Join(";", newlineArray);

        // Add line to table
        table.Add(newline);
        ModLoader.SetTable(table, tableName);
    }
}
/*
// Code used to generate newlineArray commands. I string used is the first line of gml_GlobalScript_table_items_stats. 
using System;

namespace MyApplication
{
  class Program
  {
    static void Main(string[] args)
    {
    	string line = "id;;Price;EffPrice;tier;Cat;Subcat;Material;Weight;;Fresh;Duration;Stacks;;Hunger;Hunger_Change;Hunger_Resistance;;Thirsty;Thirst_Change;;Immunity;Immunity_Change;;Intoxication;Toxicity_Change;Toxicity_Resistance;;Pain;Pain_Change;Pain_Resistance;Pain_Limit;;Morale;Morale_Change;Sanity;Sanity_Change;;Condition;max_hp;max_hp_res;Health_Restoration;Healing_Received;;max_mp;max_mp_res;MP_Restoration;MP_turn;;Fatigue;Fatigue_Change;Fatigue_Gain;;Received_XP;Cooldown_Reduction;Weapon_Damage;Hit_Chance;FMB;CRTD;Fortitude;VSN;;Bleeding_Resistance;Knockback_Resistance;Stun_Resistance;;Physical_Resistance;Nature_Resistance;Magic_Resistance;Slashing_Resistance;Piercing_Resistance;Blunt_Resistance;Rending_Resistance;Fire_Resistance;Shock_Resistance;Poison_Resistance;Caustic_Resistance;Frost_Resistance;Arcane_Resistance;Unholy_Resistance;Sacred_Resistance;Psionic_Resistance;;Nausea_Chance;Poisoning_Chance;Poisoning_Duration;;purse;bottle;upgrade;fodder;stack;fireproof;dropsOnce;tags;";
        string[] columns = line.Split(";");
        columns = Array.FindAll(columns, e => e != "");
        string[] dictcolumns = new string[columns.Length];
        string[] addIndexCommands = new string[columns.Length];
        // newlineArray[Array.IndexOf(columnLine,"name")] = name;
        string[] singleCommands = new string[columns.Length];
        for (int i = 0; i < columns.Length; i++)
        {
            if (columns[i].Length > 0)
            {
                dictcolumns[i] = "{\""+ columns[i] + "\", Array.IndexOf(columnLine,\"" + columns[i] + "\")}";
                if (columns[i] == "Material" || columns[i] == "Weight" || columns[i] == "tier" ||  columns[i] == "Cat" ||  columns[i] == "Subcat" || columns[i] == "tags")
                {
                    singleCommands[i] = "newlineArray[Array.IndexOf(columnLine, \"" + columns[i] + "\")] = GetEnumMemberValue(" + columns[i] + ");";
                    addIndexCommands[i] = "newlineArray[indices[\"" + columns[i] + "\"]] = GetEnumMemberValue(" + columns[i] + ");";
                }
                else if (columns[i] == "class") // Copy-pasted from the armor table command generator. Figure it's worth keeping around. 
                {
                    singleCommands[i] = "newlineArray[Array.IndexOf(columnLine, \"" + columns[i] + "\")] = GetEnumMemberValue(Class);";
                    addIndexCommands[i] = "newlineArray[indices[\"" + columns[i] + "\"]] = GetEnumMemberValue(Class));";
                }
                else
                {
                    singleCommands[i] = "newlineArray[Array.IndexOf(columnLine, \"" + columns[i] + "\")] = " + columns[i] + ";";
                    addIndexCommands[i] = "newlineArray[indices[\"" + columns[i] + "\"]] = " + columns[i] + ";";
                }
            }
        }
        
        string newline1 = String.Join(",", dictcolumns);
        string newline2 = String.Join("\n", addIndexCommands);
        string newline3 = String.Join("\n", singleCommands);

        // Console.WriteLine(newline1);
        // Console.WriteLine(newline2);
        Console.WriteLine(newline3);
    }
  }
}*/