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
    public enum ArmorTier
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
    
    public enum ArmorHook
    {
        SHIELDS,
        HELMETS,
        CHESTPIECES,
        GLOVES,
        BOOTS,
        BELTS,
        RINGS,
        NECKLACES,
        CLOAKS
    }
    public enum ArmorSlot
    {
        shield,
        Head,
        Chest,
        Arms,
        Legs,
        Waist,
        Ring,
        Amulet,
        Back
    }
    
    public enum ArmorClass
    {
        Light,
        Medium,
        Heavy
    }
    
    public enum ArmorRarity
    {
        Common,
        Unique
        // Legendary // seemingly removed with RtR
    }
    
    public enum ArmorMaterial
    {
        wood,
        leather,
        metal,
        cloth,
        silver,
        gold,
        gem
    }
    
    public enum ArmorTags
    {
        aldor,
        fjall,
        elven,
        special,
        unique,
        skadia,
        nistra,
        WIP,
        magic,
        [EnumMember(Value = "special exc")]
        specialexc
    }

    public static void InjectTableArmor(
        ArmorHook hook,
        string name,
        ArmorTier Tier,
        string id,
        ArmorSlot Slot,
        ArmorClass Class,
        ArmorRarity rarity,
        ArmorMaterial Mat,
        ArmorTags tags,
        ushort MaxDuration,
        int Price, // could be nullable ? Only shackles have no price 
        float Markup = 1.0f,
        byte? DEF = null,
        byte? PRR = null,
        byte? Block_Power = null,
        short? Block_Recovery = null,
        short? EVS = null,
        byte? Crit_Avoid = null,
        short? FMB = null,
        short? Hit_Chance = null,
        short? Weapon_Damage = null,
        short? Armor_Piercing = null,
        short? Armor_Damage = null,
        short? CRT = null,
        short? CRTD = null,
        short? CTA = null,
        short? Damage_Received = null,
        short? Fortitude = null,
        short? MP = null,
        short? MP_Restoration = null,
        short? Skills_Energy_Cost = null,
        short? Spells_Energy_Cost = null,
        short? Magic_Power = null,
        short? Miscast_Chance = null,
        short? Miracle_Chance = null,
        short? Miracle_Power = null,
        short? Cooldown_Reduction = null,
        short? VSN = null,
        short? max_hp = null,
        short? Health_Restoration = null,
        short? Healing_Received = null,
        short? Lifesteal = null,
        short? Manasteal = null,
        short? Bonus_Range = null,
        short? Received_XP = null,
        short? Damage_Returned = null,
        short? Bleeding_Resistance = null,
        short? Knockback_Resistance = null,
        short? Stun_Resistance = null,
        short? Pain_Resistance = null,
        short? Fatigue_Gain = null,
        short? Physical_Resistance = null,
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
        short? Pyromantic_Power = null,
        short? Geomantic_Power = null,
        short? Venomantic_Power = null,
        short? Electromantic_Power = null,
        short? Cryomantic_Power = null,
        short? Arcanistic_Power = null,
        short? Astromantic_Power = null,
        short? Psimantic_Power = null,
        bool fireproof = false,
        bool IsOpen = false,
        bool NoDrop = false, // type unknown, assuming bool
        byte? fragment_cloth01 = null,
        byte? fragment_cloth02 = null,
        byte? fragment_cloth03 = null,
        byte? fragment_cloth04 = null,
        byte? fragment_leather01 = null,
        byte? fragment_leather02 = null,
        byte? fragment_leather03 = null,
        byte? fragment_leather04 = null,
        byte? fragment_metal01 = null,
        byte? fragment_metal02 = null,
        byte? fragment_metal03 = null,
        byte? fragment_metal04 = null,
        byte? fragment_gold = null,
        short? dur_per_frag = null,
        Dictionary<string, string> additionalAttributes = new Dictionary<string, string>()
        )
    {
        // Table filename
        const string tableName = "gml_GlobalScript_table_armor";
        
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
        newlineArray[Array.IndexOf(columnLine, "class")] = GetEnumMemberValue(Class); // Had to include specific exception for this one.
        newlineArray[Array.IndexOf(columnLine, "rarity")] = GetEnumMemberValue(rarity);
        newlineArray[Array.IndexOf(columnLine, "Mat")] = GetEnumMemberValue(Mat);
        newlineArray[Array.IndexOf(columnLine, "Price")] = Price;
        newlineArray[Array.IndexOf(columnLine, "Markup")] = Markup;
        newlineArray[Array.IndexOf(columnLine, "MaxDuration")] = MaxDuration;
        newlineArray[Array.IndexOf(columnLine, "DEF")] = DEF;
        newlineArray[Array.IndexOf(columnLine, "PRR")] = PRR;
        newlineArray[Array.IndexOf(columnLine, "Block_Power")] = Block_Power;
        newlineArray[Array.IndexOf(columnLine, "Block_Recovery")] = Block_Recovery;
        newlineArray[Array.IndexOf(columnLine, "EVS")] = EVS;
        newlineArray[Array.IndexOf(columnLine, "Crit_Avoid")] = Crit_Avoid;
        newlineArray[Array.IndexOf(columnLine, "FMB")] = FMB;
        newlineArray[Array.IndexOf(columnLine, "Hit_Chance")] = Hit_Chance;
        newlineArray[Array.IndexOf(columnLine, "Weapon_Damage")] = Weapon_Damage;
        newlineArray[Array.IndexOf(columnLine, "Armor_Piercing")] = Armor_Piercing;
        newlineArray[Array.IndexOf(columnLine, "Armor_Damage")] = Armor_Damage;
        newlineArray[Array.IndexOf(columnLine, "CRT")] = CRT;
        newlineArray[Array.IndexOf(columnLine, "CRTD")] = CRTD;
        newlineArray[Array.IndexOf(columnLine, "CTA")] = CTA;
        newlineArray[Array.IndexOf(columnLine, "Damage_Received")] = Damage_Received;
        newlineArray[Array.IndexOf(columnLine, "Fortitude")] = Fortitude;
        newlineArray[Array.IndexOf(columnLine, "MP")] = MP;
        newlineArray[Array.IndexOf(columnLine, "MP_Restoration")] = MP_Restoration;
        newlineArray[Array.IndexOf(columnLine, "Abilities_Energy_Cost")] = Abilities_Energy_Cost;
        newlineArray[Array.IndexOf(columnLine, "Skills_Energy_Cost")] = Skills_Energy_Cost;
        newlineArray[Array.IndexOf(columnLine, "Spells_Energy_Cost")] = Spells_Energy_Cost;
        newlineArray[Array.IndexOf(columnLine, "Magic_Power")] = Magic_Power;
        newlineArray[Array.IndexOf(columnLine, "Miscast_Chance")] = Miscast_Chance;
        newlineArray[Array.IndexOf(columnLine, "Miracle_Chance")] = Miracle_Chance;
        newlineArray[Array.IndexOf(columnLine, "Miracle_Power")] = Miracle_Power;
        newlineArray[Array.IndexOf(columnLine, "Cooldown_Reduction")] = Cooldown_Reduction;
        newlineArray[Array.IndexOf(columnLine, "VSN")] = VSN;
        newlineArray[Array.IndexOf(columnLine, "max_hp")] = max_hp;
        newlineArray[Array.IndexOf(columnLine, "Health_Restoration")] = Health_Restoration;
        newlineArray[Array.IndexOf(columnLine, "Healing_Received")] = Healing_Received;
        newlineArray[Array.IndexOf(columnLine, "Lifesteal")] = Lifesteal;
        newlineArray[Array.IndexOf(columnLine, "Manasteal")] = Manasteal;
        newlineArray[Array.IndexOf(columnLine, "Bonus_Range")] = Bonus_Range;
        newlineArray[Array.IndexOf(columnLine, "Received_XP")] = Received_XP;
        newlineArray[Array.IndexOf(columnLine, "Damage_Returned")] = Damage_Returned;
        newlineArray[Array.IndexOf(columnLine, "Bleeding_Resistance")] = Bleeding_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Knockback_Resistance")] = Knockback_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Stun_Resistance")] = Stun_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Pain_Resistance")] = Pain_Resistance;
        newlineArray[Array.IndexOf(columnLine, "Fatigue_Gain")] = Fatigue_Gain;
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
        newlineArray[Array.IndexOf(columnLine, "Pyromantic_Power")] = Pyromantic_Power;
        newlineArray[Array.IndexOf(columnLine, "Geomantic_Power")] = Geomantic_Power;
        newlineArray[Array.IndexOf(columnLine, "Venomantic_Power")] = Venomantic_Power;
        newlineArray[Array.IndexOf(columnLine, "Electromantic_Power")] = Electromantic_Power;
        newlineArray[Array.IndexOf(columnLine, "Cryomantic_Power")] = Cryomantic_Power;
        newlineArray[Array.IndexOf(columnLine, "Arcanistic_Power")] = Arcanistic_Power;
        newlineArray[Array.IndexOf(columnLine, "Astromantic_Power")] = Astromantic_Power;
        newlineArray[Array.IndexOf(columnLine, "Psimantic_Power")] = Psimantic_Power;
        newlineArray[Array.IndexOf(columnLine, "tags")] = GetEnumMemberValue(tags);
        newlineArray[Array.IndexOf(columnLine, "fireproof")] = fireproof;
        newlineArray[Array.IndexOf(columnLine, "IsOpen")] = IsOpen;
        newlineArray[Array.IndexOf(columnLine, "NoDrop")] = NoDrop;
        newlineArray[Array.IndexOf(columnLine, "fragment_cloth01")] = fragment_cloth01;
        newlineArray[Array.IndexOf(columnLine, "fragment_cloth02")] = fragment_cloth02;
        newlineArray[Array.IndexOf(columnLine, "fragment_cloth03")] = fragment_cloth03;
        newlineArray[Array.IndexOf(columnLine, "fragment_cloth04")] = fragment_cloth04;
        newlineArray[Array.IndexOf(columnLine, "fragment_leather01")] = fragment_leather01;
        newlineArray[Array.IndexOf(columnLine, "fragment_leather02")] = fragment_leather02;
        newlineArray[Array.IndexOf(columnLine, "fragment_leather03")] = fragment_leather03;
        newlineArray[Array.IndexOf(columnLine, "fragment_leather04")] = fragment_leather04;
        newlineArray[Array.IndexOf(columnLine, "fragment_metal01")] = fragment_metal01;
        newlineArray[Array.IndexOf(columnLine, "fragment_metal02")] = fragment_metal02;
        newlineArray[Array.IndexOf(columnLine, "fragment_metal03")] = fragment_metal03;
        newlineArray[Array.IndexOf(columnLine, "fragment_metal04")] = fragment_metal04;
        newlineArray[Array.IndexOf(columnLine, "fragment_gold")] = fragment_gold;

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
        // string newline = $"{name};{GetEnumMemberValue(Tier)};{id};{Slot};{Class};{rarity};{Mat};{Price};{Markup};{MaxDuration};{DEF};;{PRR};{Block_Power};{Block_Recovery};{EVS};{Crit_Avoid};{FMB};{Hit_Chance};{Weapon_Damage};{Armor_Piercing};{Armor_Damage};{CRT};{CRTD};{CTA};{Damage_Received};{Fortitude};;{MP};{MP_Restoration};{Skills_Energy_Cost};{Spells_Energy_Cost};{Magic_Power};{Miscast_Chance};{Miracle_Chance};{Miracle_Power};{Cooldown_Reduction};;{VSN};{max_hp};{Health_Restoration};{Healing_Received};{Lifesteal};{Manasteal};{Bonus_Range};{Received_XP};{Damage_Returned};;{Bleeding_Resistance};{Knockback_Resistance};{Stun_Resistance};{Pain_Resistance};{Fatigue_Gain};;{Physical_Resistance};{Nature_Resistance};{Magic_Resistance};;{Slashing_Resistance};{Piercing_Resistance};{Blunt_Resistance};{Rending_Resistance};{Fire_Resistance};{Shock_Resistance};{Poison_Resistance};{Caustic_Resistance};{Frost_Resistance};{Arcane_Resistance};{Unholy_Resistance};{Sacred_Resistance};{Psionic_Resistance};;{Pyromantic_Power};{Geomantic_Power};{Venomantic_Power};{Electromantic_Power};{Cryomantic_Power};{Arcanistic_Power};{Astromantic_Power};{Psimantic_Power};;{GetEnumMemberValue(tags)};{(fireproof ? "1" : "")};{(IsOpen ? "1" : "")};{(NoDrop ? "1" : "")};{fragment_cloth01};{fragment_cloth02};{fragment_cloth03};{fragment_cloth04};{fragment_leather01};{fragment_leather02};{fragment_leather03};{fragment_leather04};{fragment_metal01};{fragment_metal02};{fragment_metal03};{fragment_metal04};{fragment_gold};{dur_per_frag};";
        
        // Convert the new line to string form. 
        string newline = String.Join(";", newlineArray);

        // Find Meta Category in table
        string hookStr = "";
        
        if (hook == ArmorHook.CHESTPIECES)
            hookStr = "// " + GetEnumMemberValue(hook); // Necessary workaround for chestpieces as the devs messed up
        else
            hookStr = "[ " + GetEnumMemberValue(hook) + " ]";
        
        (int ind, string? foundLine) = table.Enumerate().FirstOrDefault(x => x.Item2.Contains(hookStr));
        
        // Add line to table
        if (foundLine != null)
        {
            table.Insert(ind + 1, newline);
            ModLoader.SetTable(table, tableName);
            Log.Information($"Injected Armor {id} into table {tableName} under {hook}");
        }
        else
        {
            Log.Error($"Cannot find hook {hook} in table {tableName}");
            throw new Exception($"Cannot find hook {hook} in table {tableName}");
        }
    }
}
/*
// Code used to generate newlineArray commands. I string used is the first line of gml_GlobalScript_table_armor. 
using System;

namespace MyApplication
{
  class Program
  {
    static void Main(string[] args)
    {
    	string line = "name;Tier;id;Slot;class;rarity;Mat;Price;Markup;MaxDuration;DEF;;PRR;Block_Power;Block_Recovery;EVS;Crit_Avoid;FMB;Hit_Chance;Weapon_Damage;Armor_Piercing;Armor_Damage;CRT;CRTD;CTA;Damage_Received;Fortitude;;MP;MP_Restoration;Abilities_Energy_Cost;Skills_Energy_Cost;Spells_Energy_Cost;Magic_Power;Miscast_Chance;Miracle_Chance;Miracle_Power;Cooldown_Reduction;;VSN;max_hp;Health_Restoration;Healing_Received;Lifesteal;Manasteal;Bonus_Range;Received_XP;Damage_Returned;;Bleeding_Resistance;Knockback_Resistance;Stun_Resistance;Pain_Resistance;Fatigue_Gain;;Physical_Resistance;Nature_Resistance;Magic_Resistance;;Slashing_Resistance;Piercing_Resistance;Blunt_Resistance;Rending_Resistance;Fire_Resistance;Shock_Resistance;Poison_Resistance;Caustic_Resistance;Frost_Resistance;Arcane_Resistance;Unholy_Resistance;Sacred_Resistance;Psionic_Resistance;;Pyromantic_Power;Geomantic_Power;Venomantic_Power;Electromantic_Power;Cryomantic_Power;Arcanistic_Power;Astromantic_Power;Psimantic_Power;;tags;fireproof;IsOpen;NoDrop;fragment_cloth01;fragment_cloth02;fragment_cloth03;fragment_cloth04;fragment_leather01;fragment_leather02;fragment_leather03;fragment_leather04;fragment_metal01;fragment_metal02;fragment_metal03;fragment_metal04;fragment_gold;";
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