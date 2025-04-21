using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using Serilog;

namespace ModShardLauncher;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public partial class Msl
{
    public enum MobsStatsTier
    {
        [EnumMember(Value = "0")]
        Tier0,
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

    public enum MobsStatsType
    {
        undead,
        human,
        spectre,
        spawner,
        vampire,
        beast,
        arachnid,
        elf,
        dwarf
    }
    
    public enum MobsStatsFaction
    {
        Undead,
        Vampire,
        Brigand,
        omnivore,
        carnivore,
        herbivore,
        GrandMagistrate,
        RottenWillow,
        Mercenary,
        Neutral
    }
    
    public enum MobsStatsPattern
    {
        Melee,
        Ranger,
        Mage
    }

    public enum MobsStatsCategory
    {
        [EnumMember(Value = "")]
        none,
        Expendable,
        Tech,
        Fighter,
        Ranger,
        Mage,
        Support,
        Tank,
        Assassin,
        Boss
    }

    public enum MobsStatsWeapon
    {
        claws,
        axe,
        dagger,
        spear,
        sword,
        mace,
        bow,
        crossbow,
        [EnumMember(Value = "2hmace")]
        twohandedmace,
        [EnumMember(Value = "2haxe")]
        twohandedaxe,
        [EnumMember(Value = "2hsword")]
        twohandedsword,
        [EnumMember(Value = "2hStaff")]
        twohandedstaff,
        fangs,
        sting,
        antlers,
        fists
    }

    public enum MobsStatsArmor
    {
        Light,
        Medium,
        Heavy
    }
    
    public enum MobsStatsSize
    {
        tiny,
        small,
        medium,
        large,
        giant
    }

    public enum MobsStatsMatter
    {
        flesh,
        bones,
        ectoplasm,
        ooze,
        chitin,
        other
    }
    
    public static void InjectTableMobsStats(
        string name,
        MobsStatsTier tier,
        MobsStatsType type,
        MobsStatsFaction faction,
        MobsStatsPattern pattern,
        MobsStatsWeapon weapon,
        MobsStatsArmor armor,
        MobsStatsSize size,
        short HP,
        byte Hit_Chance,
        byte STR,
        byte AGL,
        byte Vitality,
        byte PRC,
        byte WIL,
        short XP,
        MobsStatsMatter matter,
        MobsStatsCategory category1 = MobsStatsCategory.none,
        MobsStatsCategory category2 = MobsStatsCategory.none,
        string? ID = null,
        short? MP = null,
        ushort? Morale = null,
        byte? VIS = null,
        byte? Head_DEF = null,
        byte? Body_DEF = null,
        byte? Arms_DEF = null,
        byte? Legs_DEF = null,
        short? EVS = null,
        byte? PRR = null,
        byte? Block_Power = null,
        byte? Block_Recovery = null,
        byte? Crit_Avoid = null,
        byte? CRT = null,
        byte? CRTD = null,
        short? CTA = null,
        short? FMB = null,
        short? Magic_Power = null,
        short? Miscast_Chance = null,
        byte? Miracle_Chance = null,
        byte? Miracle_Power = null,
        byte? MP_Restoration = null,
        short? Cooldown_Reduction = null,
        short? Fortitude = null,
        short? Health_Restoration = null,
        byte? Healing_Received = null,
        byte? Lifesteal = null,
        byte? Manasteal = null,
        short? Bleeding_Resistance = null,
        short? Knockback_Resistance = null,
        short? Stun_Resistance = null,
        short? Pain_Resistance = null,
        short? Bleeding_Chance = null,
        short? Daze_Chance = null,
        short? Stun_Chance = null,
        short? Knockback_Chance = null,
        short? Immob_Chance = null,
        short? Stagger_Chance = null,
        float? STRk = null,
        float? AGLk = null,
        float? Vitalityk = null,
        float? PRCk = null,
        float? WILk = null,
        float? Checksum = null,
        short? Bonus_Range = null,
        byte? Avoiding_Chance = null,
        byte? Damage_Returned = null,
        short? Damage_Received = null, // unknown type, assuming short
        byte? Head = null,
        byte? Torso = null,
        byte? Left_Leg = null,
        byte? Right_Leg = null,
        byte? Left_Hand = null,
        byte? Right_Hand = null,
        ushort? IP = null,
        byte? Threat_Time = null,
        byte? Bodypart_Damage = null,
        byte? Armor_Piercing = null,
        byte? DMG_Sum = null,
        byte? Slashing_Damage = null,
        byte? Piercing_Damage = null,
        byte? Blunt_Damage = null,
        byte? Rending_Damage = null,
        byte? Fire_Damage = null,
        byte? Shock_Damage = null,
        byte? Poison_Damage = null,
        byte? Caustic_Damage = null,
        byte? Frost_Damage = null,
        byte? Arcane_Damage = null,
        byte? Unholy_Damage = null,
        byte? Sacred_Damage = null,
        byte? Psionic_Damage = null,
        short? Physical_Resistance = null,
        short? Natural_Resistance = null,
        short? Magical_Resistance = null,
        short? Slashing_Resistance = null,
        short? Piercing_Resistance = null,
        short? Blunt_Resistance = null,
        short? Rending_Resistance = null,
        short? Fire_Resistance = null,
        short? Shock_Resistance = null,
        short? Poison_Resistance = null,
        short? Frost_Resistance = null,
        short? Caustic_Resistance = null,
        short? Arcane_Resistance = null,
        short? Unholy_Resistance = null,
        short? Sacred_Resistance = null,
        short? Psionic_Resistance = null,
        bool canBlock = false,
        bool canDisarm = false,
        bool canSwim = false,
        byte Swimming_Cost = 1,
        byte? achievement = null,
        Dictionary<string, string> additionalAttributes = new Dictionary<string, string>()
        )
    {
        // Aliasing "tier" so that it's got the correct capitalization. 
        MobsStatsTier Tier = tier;

        // Table filename
        const string tableName = "gml_GlobalScript_table_mobs_stats";
        
        // Load table if it exists
        List<string> table = ThrowIfNull(ModLoader.GetTable(tableName));

        // Get first line of table, defining all columns. 
        string[] columnLine = table[0].Split(";");

        // Insert vanilla attributes at their respective indices in the new line, as an array
        string[] newlineArray = new string[columnLine.Length];
        newlineArray[Array.IndexOf(columnLine, "name")] = name;
        newlineArray[Array.IndexOf(columnLine, "Tier")] = GetEnumMemberValue(Tier);
        newlineArray[Array.IndexOf(columnLine, "ID")] = ID;
        newlineArray[Array.IndexOf(columnLine, "type")] = GetEnumMemberValue(type);
        newlineArray[Array.IndexOf(columnLine, "faction")] = GetEnumMemberValue(faction);
        newlineArray[Array.IndexOf(columnLine, "pattern")] = GetEnumMemberValue(pattern);
        newlineArray[Array.IndexOf(columnLine, "category1")] = GetEnumMemberValue(category1);
        newlineArray[Array.IndexOf(columnLine, "category2")] = GetEnumMemberValue(category2);
        newlineArray[Array.IndexOf(columnLine, "weapon")] = GetEnumMemberValue(weapon);
        newlineArray[Array.IndexOf(columnLine, "armor")] = GetEnumMemberValue(armor);
        newlineArray[Array.IndexOf(columnLine, "size")] = GetEnumMemberValue(size);
        newlineArray[Array.IndexOf(columnLine, "matter")] = GetEnumMemberValue(matter);
        newlineArray[Array.IndexOf(columnLine, "VIS")] = VIS;
        newlineArray[Array.IndexOf(columnLine, "XP")] = XP;
        newlineArray[Array.IndexOf(columnLine, "HP")] = HP;
        newlineArray[Array.IndexOf(columnLine, "MP")] = MP;
        newlineArray[Array.IndexOf(columnLine, "Head_DEF")] = Head_DEF;
        newlineArray[Array.IndexOf(columnLine, "Body_DEF")] = Body_DEF;
        newlineArray[Array.IndexOf(columnLine, "Arms_DEF")] = Arms_DEF;
        newlineArray[Array.IndexOf(columnLine, "Legs_DEF")] = Legs_DEF;
        newlineArray[Array.IndexOf(columnLine, "Hit_Chance")] = Hit_Chance;
        newlineArray[Array.IndexOf(columnLine, "EVS")] = EVS;
        newlineArray[Array.IndexOf(columnLine, "PRR")] = PRR;
        newlineArray[Array.IndexOf(columnLine, "Block_Power")] = Block_Power;
        newlineArray[Array.IndexOf(columnLine, "Block_Recovery")] = Block_Recovery;
        newlineArray[Array.IndexOf(columnLine, "Crit_Avoid")] = Crit_Avoid;
        newlineArray[Array.IndexOf(columnLine, "CRT")] = CRT;
        newlineArray[Array.IndexOf(columnLine, "CRTD")] = CRTD;
        newlineArray[Array.IndexOf(columnLine, "CTA")] = CTA;
        newlineArray[Array.IndexOf(columnLine, "FMB")] = FMB;
        newlineArray[Array.IndexOf(columnLine, "Magic_Power")] = Magic_Power;
        newlineArray[Array.IndexOf(columnLine, "Miscast_Chance")] = Miscast_Chance;
        newlineArray[Array.IndexOf(columnLine, "Miracle_Chance")] = Miracle_Chance;
        newlineArray[Array.IndexOf(columnLine, "Miracle_Power")] = Miracle_Power;
        newlineArray[Array.IndexOf(columnLine, "MP_Restoration")] = MP_Restoration;
        newlineArray[Array.IndexOf(columnLine, "Cooldown_Reduction")] = Cooldown_Reduction;
        newlineArray[Array.IndexOf(columnLine, "Fortitude")] = Fortitude;
        newlineArray[Array.IndexOf(columnLine, "Health_Restoration")] = Health_Restoration;
        newlineArray[Array.IndexOf(columnLine, "Healing_Received")] = Healing_Received;
        newlineArray[Array.IndexOf(columnLine, "Lifesteal")] = Lifesteal;
        newlineArray[Array.IndexOf(columnLine, "Manasteal")] = Manasteal;
        newlineArray[Array.IndexOf(columnLine, "Bleeding_Resistance")] = Bleeding_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Knockback_Resistance")] = Knockback_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Stun_Resistance")] = Stun_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Pain_Resistance")] = Pain_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Bleeding_Chance")] = Bleeding_Chance;
        newlineArray[Array.IndexOf(columnLine, "Daze_Chance")] = Daze_Chance;
        newlineArray[Array.IndexOf(columnLine, "Stun_Chance")] = Stun_Chance;
        newlineArray[Array.IndexOf(columnLine, "Knockback_Chance")] = Knockback_Chance;
        newlineArray[Array.IndexOf(columnLine, "Immob_Chance")] = Immob_Chance;
        newlineArray[Array.IndexOf(columnLine, "Stagger_Chance")] = Stagger_Chance;
        newlineArray[Array.IndexOf(columnLine, "STR k")] = STRk;
        newlineArray[Array.IndexOf(columnLine, "AGL k")] = AGLk;
        newlineArray[Array.IndexOf(columnLine, "Vitality k")] = Vitalityk;
        newlineArray[Array.IndexOf(columnLine, "PRC k")] = PRCk;
        newlineArray[Array.IndexOf(columnLine, "WIL k")] = WILk;
        newlineArray[Array.IndexOf(columnLine, "Checksum")] = Checksum;
        newlineArray[Array.IndexOf(columnLine, "STR")] = STR;
        newlineArray[Array.IndexOf(columnLine, "AGL")] = AGL;
        newlineArray[Array.IndexOf(columnLine, "Vitality")] = Vitality;
        newlineArray[Array.IndexOf(columnLine, "PRC")] = PRC;
        newlineArray[Array.IndexOf(columnLine, "WIL")] = WIL;
        newlineArray[Array.IndexOf(columnLine, "Bonus_Range")] = Bonus_Range;
        newlineArray[Array.IndexOf(columnLine, "Avoiding_Chance")] = Avoiding_Chance;
        newlineArray[Array.IndexOf(columnLine, "Damage_Returned")] = Damage_Returned;
        newlineArray[Array.IndexOf(columnLine, "Damage_Received")] = Damage_Received;
        newlineArray[Array.IndexOf(columnLine, "Head")] = Head;
        newlineArray[Array.IndexOf(columnLine, "Torso")] = Torso;
        newlineArray[Array.IndexOf(columnLine, "Left_Leg")] = Left_Leg;
        newlineArray[Array.IndexOf(columnLine, "Right_Leg")] = Right_Leg;
        newlineArray[Array.IndexOf(columnLine, "Left_Hand")] = Left_Hand;
        newlineArray[Array.IndexOf(columnLine, "Right_Hand")] = Right_Hand;
        newlineArray[Array.IndexOf(columnLine, "IP")] = IP;
        newlineArray[Array.IndexOf(columnLine, "Morale")] = Morale;
        newlineArray[Array.IndexOf(columnLine, "Threat_Time")] = Threat_Time;
        newlineArray[Array.IndexOf(columnLine, "Bodypart_Damage")] = Bodypart_Damage;
        newlineArray[Array.IndexOf(columnLine, "Armor_Piercing")] = Armor_Piercing;
        newlineArray[Array.IndexOf(columnLine, "DMG Sum")] = DMG_Sum;
        newlineArray[Array.IndexOf(columnLine, "Slashing_Damage")] = Slashing_Damage;
        newlineArray[Array.IndexOf(columnLine, "Piercing_Damage")] = Piercing_Damage;
        newlineArray[Array.IndexOf(columnLine, "Blunt_Damage")] = Blunt_Damage;
        newlineArray[Array.IndexOf(columnLine, "Rending_Damage")] = Rending_Damage;
        newlineArray[Array.IndexOf(columnLine, "Fire_Damage")] = Fire_Damage;
        newlineArray[Array.IndexOf(columnLine, "Shock_Damage")] = Shock_Damage;
        newlineArray[Array.IndexOf(columnLine, "Poison_Damage")] = Poison_Damage;
        newlineArray[Array.IndexOf(columnLine, "Caustic_Damage")] = Caustic_Damage;
        newlineArray[Array.IndexOf(columnLine, "Frost_Damage")] = Frost_Damage;
        newlineArray[Array.IndexOf(columnLine, "Arcane_Damage")] = Arcane_Damage;
        newlineArray[Array.IndexOf(columnLine, "Unholy_Damage")] = Unholy_Damage;
        newlineArray[Array.IndexOf(columnLine, "Sacred_Damage")] = Sacred_Damage;
        newlineArray[Array.IndexOf(columnLine, "Psionic_Damage")] = Psionic_Damage;
        newlineArray[Array.IndexOf(columnLine, "Physical_Resistance")] = Physical_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Natural_Resistance")] = Natural_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Magical_Resistance")] = Magical_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Slashing_Resistance")] = Slashing_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Piercing_Resistance")] = Piercing_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Blunt_Resistance")] = Blunt_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Rending_Resistance")] = Rending_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Fire_Resistance")] = Fire_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Shock_Resistance")] = Shock_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Poison_Resistance")] = Poison_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Frost_Resistance")] = Frost_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Caustic_Resistance")] = Caustic_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Arcane_Resistance")] = Arcane_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Unholy_Resistance")] = Unholy_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Sacred_Resistance")] = Sacred_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Psionic_Resistance")] = Psionic_Resistance;
        newlineArray[Array.IndexOf(columnLine, "canBlock")] = canBlock;
        newlineArray[Array.IndexOf(columnLine, "canDisarm")] = canDisarm;
        newlineArray[Array.IndexOf(columnLine, "canSwim")] = canSwim;
        newlineArray[Array.IndexOf(columnLine, "Swimming_Cost")] = Swimming_Cost;
        newlineArray[Array.IndexOf(columnLine, "achievement")] = achievement;
        newlineArray[Array.IndexOf(columnLine, "trophy")] = trophy;

        // // Prepare line
        // string newline = $"{name};{GetEnumMemberValue(tier)};{ID};{type};{faction};{pattern};;{GetEnumMemberValue(category1)};{GetEnumMemberValue(category2)};{GetEnumMemberValue(weapon)};{armor};{size};{matter};{VIS};;{XP};{HP};{MP};{Head_DEF};{Body_DEF};{Arms_DEF};{Legs_DEF};;{Hit_Chance};{EVS};{PRR};{Block_Power};{Block_Recovery};{Crit_Avoid};{CRT};{CRTD};{CTA};{FMB};;{Magic_Power};{Miscast_Chance};{Miracle_Chance};{Miracle_Power};;{MP_Restoration};{Cooldown_Reduction};{Fortitude};{Health_Restoration};{Healing_Received};{Lifesteal};{Manasteal};;{Bleeding_Resistance};{Knockback_Resistance};{Stun_Resistance};{Pain_Resistance};;{Bleeding_Chance};{Daze_Chance};{Stun_Chance};{Knockback_Chance};{Immob_Chance};{Stagger_Chance};;{STRk};{AGLk};{Vitalityk};{PRCk};{WILk};{Checksum};;{STR};{AGL};{Vitality};{PRC};{WIL};;{Bonus_Range};{Avoiding_Chance};{Damage_Returned};{Damage_Received};;{Head};{Torso};{Left_Leg};{Right_Leg};{Left_Hand};{Right_Hand};;{IP};{Morale};{Threat_Time};;{Bodypart_Damage};{Armor_Piercing};{DMG_Sum};{Slashing_Damage};{Piercing_Damage};{Blunt_Damage};{Rending_Damage};{Fire_Damage};{Shock_Damage};{Poison_Damage};{Caustic_Damage};{Frost_Damage};{Arcane_Damage};{Unholy_Damage};{Sacred_Damage};{Psionic_Damage};;{Physical_Resistance};{Natural_Resistance};{Magical_Resistance};;{Slashing_Resistance};{Piercing_Resistance};{Blunt_Resistance};{Rending_Resistance};{Fire_Resistance};{Shock_Resistance};{Poison_Resistance};{Frost_Resistance};{Caustic_Resistance};{Arcane_Resistance};{Unholy_Resistance};{Sacred_Resistance};{Psionic_Resistance};;{(canBlock ? "1": "")};{(canDisarm ? "1": "")};{(canSwim ? "1": "")};{Swimming_Cost};{achievement};";
        
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

        // Add line to table
        table.Add(newline);
        ModLoader.SetTable(table, tableName);
        Log.Information($"Injected Mob Stat {name} into table");
    }
}
/*
// Code used to generate newlineArray commands. I string used is the first line of gml_GlobalScript_table_mobs_stats.
// Caught an inconsistency in spelling of "tier" vs. "Tier". Added an alias in the code to maintain backwards-compatibility.
using System;

namespace MyApplication
{
  class Program
  {
    static void Main(string[] args)
    {
    	string line = "name;Tier;ID;type;faction;pattern;;category1;category2;weapon;armor;size;matter;VIS;;XP;HP;MP;Head_DEF;Body_DEF;Arms_DEF;Legs_DEF;;Hit_Chance;EVS;PRR;Block_Power;Block_Recovery;Crit_Avoid;CRT;CRTD;CTA;FMB;;Magic_Power;Miscast_Chance;Miracle_Chance;Miracle_Power;;MP_Restoration;Cooldown_Reduction;Fortitude;Health_Restoration;Healing_Received;Lifesteal;Manasteal;;Bleeding_Resistance;Knockback_Resistance;Stun_Resistance;Pain_Resistance;;Bleeding_Chance;Daze_Chance;Stun_Chance;Knockback_Chance;Immob_Chance;Stagger_Chance;;STR k;AGL k;Vitality k;PRC k;WIL k;Checksum;;STR;AGL;Vitality;PRC;WIL;;Bonus_Range;Avoiding_Chance;Damage_Returned;Damage_Received;;Head;Torso;Left_Leg;Right_Leg;Left_Hand;Right_Hand;;IP;Morale;Threat_Time;;Bodypart_Damage;Armor_Piercing;DMG Sum;Slashing_Damage;Piercing_Damage;Blunt_Damage;Rending_Damage;Fire_Damage;Shock_Damage;Poison_Damage;Caustic_Damage;Frost_Damage;Arcane_Damage;Unholy_Damage;Sacred_Damage;Psionic_Damage;;Physical_Resistance;Natural_Resistance;Magical_Resistance;;Slashing_Resistance;Piercing_Resistance;Blunt_Resistance;Rending_Resistance;Fire_Resistance;Shock_Resistance;Poison_Resistance;Frost_Resistance;Caustic_Resistance;Arcane_Resistance;Unholy_Resistance;Sacred_Resistance;Psionic_Resistance;;canBlock;canDisarm;canSwim;Swimming_Cost;achievement;trophy;";
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
                if (columns[i] == "Tier" || columns[i] == "type" || columns[i] == "faction" ||  columns[i] == "pattern" || columns[i] == "weapon" || columns[i] == "armor" || columns[i] == "size" || columns[i] == "matter" || columns[i] == "category1" || columns[i] == "category2")
                {
                    singleCommands[i] = "        newlineArray[Array.IndexOf(columnLine, \"" + columns[i] + "\")] = GetEnumMemberValue(" + columns[i] + ");";
                    addIndexCommands[i] = "newlineArray[indices[\"" + columns[i] + "\"]] = GetEnumMemberValue(" + columns[i] + ");";
                }
                else if (columns[i] == "STR k" || columns[i] == "AGL k" || columns[i] == "Vitality k" || columns[i] == "PRC k" || columns[i] == "WIL k")
                {
                	singleCommands[i] = "        newlineArray[Array.IndexOf(columnLine, \"" + columns[i] + "\")] = " + columns[i].Replace(" ","") + ";";
                    addIndexCommands[i] = "newlineArray[indices[\"" + columns[i] + "\"]] = " + columns[i].Replace(" ","") + ";";
                }
                else if (columns[i] == "DMG Sum")
                {
                	singleCommands[i] = "        newlineArray[Array.IndexOf(columnLine, \"" + columns[i] + "\")] = " + columns[i].Replace(" ","_") + ";";
                    addIndexCommands[i] = "newlineArray[indices[\"" + columns[i] + "\"]] = " + columns[i].Replace(" ","_") + ";";
                }
                else if (columns[i] == "class")
                {
                    singleCommands[i] = "        newlineArray[Array.IndexOf(columnLine, \"" + columns[i] + "\")] = GetEnumMemberValue(Class);";
                    addIndexCommands[i] = "newlineArray[indices[\"" + columns[i] + "\"]] = GetEnumMemberValue(Class));";
                }
                else
                {
                    singleCommands[i] = "        newlineArray[Array.IndexOf(columnLine, \"" + columns[i] + "\")] = " + columns[i] + ";";
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