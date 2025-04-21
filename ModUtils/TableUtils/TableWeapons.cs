using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using Serilog;

namespace ModShardLauncher;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "IdentifierTypo")]
public partial class Msl
{
    public enum WeaponsTier
    {
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
    
    /// <summary>
    /// <para>Enum used in Weapons table. </para>
    /// Defines the slot the weapon can be equipped in.
    /// </summary>
    public enum WeaponsSlot
    {
        sword,
        axe,
        mace,
        dagger,
        [EnumMember(Value = "2hsword")]
        twohandedsword,
        spear,
        [EnumMember(Value = "2haxe")]
        twohandedaxe,
        [EnumMember(Value = "2hmace")]
        twohandedmace,
        bow,
        crossbow,
        sling,
        [EnumMember(Value = "2hStaff")]
        twohandedstaff,
        chain,
        lute
    }

    /// <summary>
    /// <para>Enum used in Weapons table. </para>
    /// Defines the rarity of the weapon.
    /// </summary>
    public enum WeaponsRarity
    {
        Common,
        Unique
        // Legendary // Doesn't seem to be used post-RtR 
    }

    /// <summary>
    /// <para>Enum used in Weapons table. </para>
    /// Defines the material the weapon is made out of.
    /// </summary>
    public enum WeaponsMaterial
    {
        wood,
        metal,
        leather
    }

    /// <summary>
    /// <para>Enum used in Weapons table. </para>
    /// Defines the rarity of the weapon in description.
    /// </summary>
    public enum WeaponsTags
    {
        // SPECIAL
        unique,
        magic,
        special,
        [EnumMember(Value = "special exc")]
        specialexc,
        WIP,
        
        // ALDOR
        aldor,
        [EnumMember(Value = "aldor common")]
        aldorcommon,
        [EnumMember(Value = "aldor uncommon")]
        aldoruncommon,
        [EnumMember(Value = "aldor rare")]
        aldorrare,
        [EnumMember(Value = "aldor magic")]
        aldormagic,
        
        // FOREIGN
        fjall,
        elven,
        skadia,
        nistra
    }
    
    public static void InjectTableWeapons(
        // Would love to use a hook but devs fucked up the table's categories' names
        string name,
        WeaponsTier Tier,
        string id,
        WeaponsSlot Slot,
        WeaponsRarity rarity,
        WeaponsMaterial Mat,
        WeaponsTags tags,
        byte Rng = 1,
        int? Price = null,
        byte Markup = 1,
        short? MaxDuration = null,
        short? Armor_Piercing = null,
        short? Armor_Damage = null,
        short? Bodypart_Damage = null,
        short? Slashing_Damage = null,
        short? Piercing_Damage = null,
        short? Blunt_Damage = null,
        short? Rending_Damage = null,
        short? Fire_Damage = null,
        short? Shock_Damage = null,
        short? Poison_Damage = null,
        short? Caustic_Damage = null,
        short? Frost_Damage = null,
        short? Arcane_Damage = null,
        short? Unholy_Damage = null,
        short? Sacred_Damage = null,
        short? Psionic_Damage = null,
        short? FMB = null,
        short? Hit_Chance = null,
        short? CRT = null,
        short? CRTD = null,
        short? CTA = null,
        short? PRR = null,
        short? Block_Power = null,
        short? Block_Recovery = null,
        byte? Bleeding_Chance = null,
        byte? Daze_Chance = null,
        byte? Stun_Chance = null,
        byte? Knockback_Chance = null,
        byte? Immob_Chance = null,
        byte? Stagger_Chance = null,
        short? MP = null,
        short? MP_Restoration = null,
        short? Cooldown_Reduction = null,
        short? Skills_Energy_Cost = null,
        short? Spells_Energy_Cost = null,
        short? Magic_Power = null,
        short? Miscast_Chance = null,
        short? Miracle_Chance = null,
        short? Miracle_Power = null,
        short? Bonus_Range = null,
        short? max_hp = null,
        short? Health_Restoration = null,
        short? Healing_Received = null,
        short? Crit_Avoid = null,
        short? Fatigue_Gain = null,
        short? Lifesteal = null,
        short? Manasteal = null,
        short? Damage_Received = null,
        short? Pyromantic_Power = null,
        short? Geomantic_Power = null,
        short? Venomantic_Power = null,
        short? Electroantic_Power = null,
        short? Cryomantic_Power = null,
        short? Arcanistic_Power = null,
        short? Astromantic_Power = null,
        short? Psimantic_Power = null,
        short? Balance = null, // Could be byte ?
        string? upgrade = null,
        bool fireproof = false,
        bool NoDrop = false,
        Dictionary<string, string> additionalAttributes = new Dictionary<string, string>()
        )
    {
        // Table filename
        const string tableName = "gml_GlobalScript_table_weapons";

        // Load table if it exists
        List<string> table = ThrowIfNull(ModLoader.GetTable(tableName));

        // Get first line of table, defining all columns. 
        string[] columnLine = table[0].Split(";");

        // Insert vanilla attributes at their respective indices in the new line, as an array
        string[] newlineArray = new string[columnLine.Length];
        newlineArray[Array.IndexOf(columnLine, "name")] = name;
        newlineArray[Array.IndexOf(columnLine, "Tier")] = GetEnumMemberValue(Tier);
        newlineArray[Array.IndexOf(columnLine, "id")] = id;
        newlineArray[Array.IndexOf(columnLine, "Slot")] = GetEnumMemberValue(Slot);
        newlineArray[Array.IndexOf(columnLine, "rarity")] = GetEnumMemberValue(rarity);
        newlineArray[Array.IndexOf(columnLine, "Mat")] = GetEnumMemberValue(Mat);
        newlineArray[Array.IndexOf(columnLine, "Price")] = Price;
        newlineArray[Array.IndexOf(columnLine, "Markup")] = Markup;
        newlineArray[Array.IndexOf(columnLine, "MaxDuration")] = MaxDuration;
        newlineArray[Array.IndexOf(columnLine, "Rng")] = Rng;
        newlineArray[Array.IndexOf(columnLine, "Armor_Piercing")] = Armor_Piercing;
        newlineArray[Array.IndexOf(columnLine, "Armor_Damage")] = Armor_Damage;
        newlineArray[Array.IndexOf(columnLine, "Bodypart_Damage")] = Bodypart_Damage;
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
        newlineArray[Array.IndexOf(columnLine, "FMB")] = FMB;
        newlineArray[Array.IndexOf(columnLine, "Hit_Chance")] = Hit_Chance;
        newlineArray[Array.IndexOf(columnLine, "CRT")] = CRT;
        newlineArray[Array.IndexOf(columnLine, "CRTD")] = CRTD;
        newlineArray[Array.IndexOf(columnLine, "CTA")] = CTA;
        newlineArray[Array.IndexOf(columnLine, "PRR")] = PRR;
        newlineArray[Array.IndexOf(columnLine, "Block_Power")] = Block_Power;
        newlineArray[Array.IndexOf(columnLine, "Block_Recovery")] = Block_Recovery;
        newlineArray[Array.IndexOf(columnLine, "Bleeding_Chance")] = Bleeding_Chance;
        newlineArray[Array.IndexOf(columnLine, "Daze_Chance")] = Daze_Chance;
        newlineArray[Array.IndexOf(columnLine, "Stun_Chance")] = Stun_Chance;
        newlineArray[Array.IndexOf(columnLine, "Knockback_Chance")] = Knockback_Chance;
        newlineArray[Array.IndexOf(columnLine, "Immob_Chance")] = Immob_Chance;
        newlineArray[Array.IndexOf(columnLine, "Stagger_Chance")] = Stagger_Chance;
        newlineArray[Array.IndexOf(columnLine, "MP")] = MP;
        newlineArray[Array.IndexOf(columnLine, "MP_Restoration")] = MP_Restoration;
        newlineArray[Array.IndexOf(columnLine, "Cooldown_Reduction")] = Cooldown_Reduction;
        newlineArray[Array.IndexOf(columnLine, "Abilities_Energy_Cost")] = Abilities_Energy_Cost;
        newlineArray[Array.IndexOf(columnLine, "Skills_Energy_Cost")] = Skills_Energy_Cost;
        newlineArray[Array.IndexOf(columnLine, "Spells_Energy_Cost")] = Spells_Energy_Cost;
        newlineArray[Array.IndexOf(columnLine, "Magic_Power")] = Magic_Power;
        newlineArray[Array.IndexOf(columnLine, "Miscast_Chance")] = Miscast_Chance;
        newlineArray[Array.IndexOf(columnLine, "Miracle_Chance")] = Miracle_Chance;
        newlineArray[Array.IndexOf(columnLine, "Miracle_Power")] = Miracle_Power;
        newlineArray[Array.IndexOf(columnLine, "Bonus_Range")] = Bonus_Range;
        newlineArray[Array.IndexOf(columnLine, "max_hp")] = max_hp;
        newlineArray[Array.IndexOf(columnLine, "Health_Restoration")] = Health_Restoration;
        newlineArray[Array.IndexOf(columnLine, "Healing_Received")] = Healing_Received;
        newlineArray[Array.IndexOf(columnLine, "Crit_Avoid")] = Crit_Avoid;
        newlineArray[Array.IndexOf(columnLine, "Fatigue_Gain")] = Fatigue_Gain;
        newlineArray[Array.IndexOf(columnLine, "Lifesteal")] = Lifesteal;
        newlineArray[Array.IndexOf(columnLine, "Manasteal")] = Manasteal;
        newlineArray[Array.IndexOf(columnLine, "Damage_Received")] = Damage_Received;
        newlineArray[Array.IndexOf(columnLine, "Pyromantic_Power")] = Pyromantic_Power;
        newlineArray[Array.IndexOf(columnLine, "Geomantic_Power")] = Geomantic_Power;
        newlineArray[Array.IndexOf(columnLine, "Venomantic_Power")] = Venomantic_Power;
        newlineArray[Array.IndexOf(columnLine, "Electromantic_Power")] = Electromantic_Power;
        newlineArray[Array.IndexOf(columnLine, "Cryomantic_Power")] = Cryomantic_Power;
        newlineArray[Array.IndexOf(columnLine, "Arcanistic_Power")] = Arcanistic_Power;
        newlineArray[Array.IndexOf(columnLine, "Astromantic_Power")] = Astromantic_Power;
        newlineArray[Array.IndexOf(columnLine, "Psimantic_Power")] = Psimantic_Power;
        newlineArray[Array.IndexOf(columnLine, "Balance")] = Balance;
        newlineArray[Array.IndexOf(columnLine, "tags")] = GetEnumMemberValue(tags);
        newlineArray[Array.IndexOf(columnLine, "upgrade")] = upgrade;
        newlineArray[Array.IndexOf(columnLine, "fireproof")] = fireproof;
        
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

        // // Prepare line
        // string newline = $"{name};{GetEnumMemberValue(Tier)};{id};{GetEnumMemberValue(Slot)};{rarity};{Mat};{Price};{Markup};{MaxDuration};{Rng};;{Armor_Piercing};{Armor_Damage};{Bodypart_Damage};;{Slashing_Damage};{Piercing_Damage};{Blunt_Damage};{Rending_Damage};{Fire_Damage};{Shock_Damage};{Poison_Damage};{Caustic_Damage};{Frost_Damage};{Arcane_Damage};{Unholy_Damage};{Sacred_Damage};{Psionic_Damage};;{FMB};{Hit_Chance};{CRT};{CRTD};{CTA};{PRR};{Block_Power};{Block_Recovery};;{Bleeding_Chance};{Daze_Chance};{Stun_Chance};{Knockback_Chance};{Immob_Chance};{Stagger_Chance};;{MP};{MP_Restoration};{Cooldown_Reduction};{Skills_Energy_Cost};{Spells_Energy_Cost};{Magic_Power};{Miscast_Chance};{Miracle_Chance};{Miracle_Power};{Bonus_Range};;{max_hp};{Health_Restoration};{Healing_Received};{Crit_Avoid};{Fatigue_Gain};{Lifesteal};{Manasteal};{Damage_Received};;{Pyromantic_Power};{Geomantic_Power};{Venomantic_Power};{Electroantic_Power};{Cryomantic_Power};{Arcanistic_Power};{Astromantic_Power};{Psimantic_Power};;{Balance};{GetEnumMemberValue(tags)};{upgrade};{(fireproof ? 1 : "")};{(NoDrop ? 1 : "")};";
        
        // Convert the new line to string form. 
        string newline = String.Join(";", newlineArray);

        // Add line to table
        table.Add(newline);
        ModLoader.SetTable(table, tableName);
        Log.Information($"Injected Weapon {id} into table {tableName}");
    }
}
/*
// Code used to generate newlineArray commands. I string used is the first line of gml_GlobalScript_table_weapons. 
using System;

namespace MyApplication
{
  class Program
  {
    static void Main(string[] args)
    {
    	string line = "name;Tier;id;Slot;rarity;Mat;Price;Markup;MaxDuration;Rng;;Armor_Piercing;Armor_Damage;Bodypart_Damage;;Slashing_Damage;Piercing_Damage;Blunt_Damage;Rending_Damage;Fire_Damage;Shock_Damage;Poison_Damage;Caustic_Damage;Frost_Damage;Arcane_Damage;Unholy_Damage;Sacred_Damage;Psionic_Damage;;FMB;Hit_Chance;CRT;CRTD;CTA;PRR;Block_Power;Block_Recovery;;Bleeding_Chance;Daze_Chance;Stun_Chance;Knockback_Chance;Immob_Chance;Stagger_Chance;;MP;MP_Restoration;Cooldown_Reduction;Abilities_Energy_Cost;Skills_Energy_Cost;Spells_Energy_Cost;Magic_Power;Miscast_Chance;Miracle_Chance;Miracle_Power;Bonus_Range;;max_hp;Health_Restoration;Healing_Received;Crit_Avoid;Fatigue_Gain;Lifesteal;Manasteal;Damage_Received;;Pyromantic_Power;Geomantic_Power;Venomantic_Power;Electromantic_Power;Cryomantic_Power;Arcanistic_Power;Astromantic_Power;Psimantic_Power;;Balance;tags;upgrade;fireproof;";
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
                if (columns[i] == "Tier" || columns[i] == "Slot" || columns[i] == "rarity" ||  columns[i] == "Mat" || columns[i] == "tags")
                {
                    singleCommands[i] = "newlineArray[Array.IndexOf(columnLine, \"" + columns[i] + "\")] = GetEnumMemberValue(" + columns[i] + ");";
                    addIndexCommands[i] = "newlineArray[indices[\"" + columns[i] + "\"]] = GetEnumMemberValue(" + columns[i] + ");";
                }
                else if (columns[i] == "class")
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