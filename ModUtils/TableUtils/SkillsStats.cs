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
    public enum SkillsStatsHook
    {
        BASIC,
        RANGED,
        SWORDS,
        [EnumMember(Value = "2H SWORDS")]
        TWOHANDEDSWORDS,
        [EnumMember(Value = "2H MACES")]
        TWOHANDEDMACES,
        [EnumMember(Value = "2H AXES")]
        TWOHANDEDAXES,
        AXES,
        MACES,
        STAVES,
        SHIELDS,
        DAGGERS,
        [EnumMember(Value = "DUAL WIELDING")]
        DUALWIELDING,
        SPEARS,
        COMBAT,
        ATHLETICS,
        SURVIVAL,
        PYRO,
        GEO,
        ELECTRO,
        ARMOR,
        [EnumMember(Value = "MAGIC MASTERY")]
        MAGICMASTERY,
        UNDEAD,
        [EnumMember(Value = "PROLOGUE STATUES")]
        PROLOGUESTATUES,
        [EnumMember(Value = "PROLOGUE ARCHON")]
        PROLOGUEARCHON,
        PROSELYTES,
        BRIGANDS,
        [EnumMember(Value = "ANCIENT TROLL")]
        ANCIENTTROLL,
        BEASTS,
        MANTICORE
    }
    
    public enum SkillsStatsTarget
    {
        [EnumMember(Value = "No Target")]
        NoTarget,
        [EnumMember(Value = "Target Object")]
        TargetObject,
        [EnumMember(Value = "Target Point")]
        TargetPoint,
        [EnumMember(Value = "Target Area")]
        TargetArea,
        [EnumMember(Value = "Target Ally")]
        TargetAlly
    }
    
    public enum SkillsStatsPattern
    {
        normal,
        five,
        line,
        circle,
        pyramid,
        pyramid_shift
    }

    public enum SkillsStatsValidator
    {
        [EnumMember(Value = "")]
        none,
        AVOID_TILEMARKS,
        DASH
    }
    
    public enum SkillsStatsClass
    {
        skill,
        spell,
        attack,
    }
    
    public enum SkillsStatsBranch
    {
        none, // For some reason the string none has to be written, rather than leaving the field empty. Inconsistent but it is what it is.
        ranged,
        sword,
        [EnumMember(Value = "2hsword")]
        two_handed_sword,
        [EnumMember(Value = "2hmace")]
        two_handed_mace,
        [EnumMember(Value = "2haxe")]
        two_handed_axe,
        axe,
        mace,
        staff,
        shield,
        dagger,
        dual,
        spear,
        combat,
        athletic,
        pyromancy,
        geomancy,
        electromancy,
        armor,
        magic_mastery,
        necromancy,
        sanguimancy
    }
    
    public enum SkillsStatsMetacategory
    {
        [EnumMember(Value = "")]
        none,
        weapon,
        utility
    }
    
    public static void InjectTableSkillsStats(
    SkillsStatsHook hook,
    string id,
    string? Object = null,
    SkillsStatsTarget Target = SkillsStatsTarget.NoTarget,
    string Range = "0",
    ushort KD = 0,
    ushort MP = 0,
    ushort Reserv = 0,
    ushort Duration = 0,
    byte AOE_Lenght = 0,
    byte AOE_Width = 0,
    bool is_movement = false,
    SkillsStatsPattern Pattern = SkillsStatsPattern.normal,
    SkillsStatsValidator Validators = SkillsStatsValidator.none,
    SkillsStatsClass Class = SkillsStatsClass.skill,
    bool Bonus_Range = false, // could be byte ? Not sure as only values are 0 and 1
    string? Starcast = null,
    SkillsStatsBranch Branch = SkillsStatsBranch.none,
    bool is_knockback = false,
    bool Crime = false,
    SkillsStatsMetacategory metacategory = SkillsStatsMetacategory.none,
    short FMB = 0,
    string AP = "x",
    bool Attack = false,
    bool Stance = false,
    bool Charge = false,
    bool Maneuver = false,
    bool Spell = false
        )
    {
        // Table filename
        const string tableName = "gml_GlobalScript_table_skills_stats";
        
        // Load table if it exists
        List<string> table = ThrowIfNull(ModLoader.GetTable(tableName));
        
        // Get first line of table, defining all columns. 
        string[] columnLine = table[0].Split(";");

        // Insert vanilla attributes at their respective indices in the new line, as an array
        string[] newlineArray = new string[columnLine.Length];
        newlineArray[Array.IndexOf(columnLine, "Object")] = Object;
        newlineArray[Array.IndexOf(columnLine, "Target")] = GetEnumMemberValue(Target);
        newlineArray[Array.IndexOf(columnLine, "Range")] = Range;
        newlineArray[Array.IndexOf(columnLine, "KD")] = KD;
        newlineArray[Array.IndexOf(columnLine, "MP")] = MP;
        newlineArray[Array.IndexOf(columnLine, "Reserv")] = Reserv;
        newlineArray[Array.IndexOf(columnLine, "Duration")] = Duration;
        newlineArray[Array.IndexOf(columnLine, "AOE_Lenght")] = AOE_Lenght;
        newlineArray[Array.IndexOf(columnLine, "AOE_Width")] = AOE_Width;
        newlineArray[Array.IndexOf(columnLine, "is_movement")] = is_movement;
        newlineArray[Array.IndexOf(columnLine, "Pattern")] = GetEnumMemberValue(Pattern);
        newlineArray[Array.IndexOf(columnLine, "Validators")] = GetEnumMemberValue(Validators);
        newlineArray[Array.IndexOf(columnLine, "Class")] = GetEnumMemberValue(Class);
        newlineArray[Array.IndexOf(columnLine, "Bonus_Range")] = Bonus_Range;
        newlineArray[Array.IndexOf(columnLine, "Starcast")] = Starcast;
        newlineArray[Array.IndexOf(columnLine, "Branch")] = GetEnumMemberValue(Branch);
        newlineArray[Array.IndexOf(columnLine, "is_knockback")] = is_knockback;
        newlineArray[Array.IndexOf(columnLine, "Crime")] = Crime;
        newlineArray[Array.IndexOf(columnLine, "metacategory")] = GetEnumMemberValue(metacategory);
        newlineArray[Array.IndexOf(columnLine, "FMB")] = FMB;
        newlineArray[Array.IndexOf(columnLine, "AP")] = AP;
        newlineArray[Array.IndexOf(columnLine, "Attack")] = Attack;
        newlineArray[Array.IndexOf(columnLine, "Stance")] = Stance;
        newlineArray[Array.IndexOf(columnLine, "Charge")] = Charge;
        newlineArray[Array.IndexOf(columnLine, "Maneuver")] = Maneuver;
        newlineArray[Array.IndexOf(columnLine, "Spell")] = Spell;
        
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
        // string newline = $"{id};{Object};{GetEnumMemberValue(Target)};{Range};{KD};{MP};{Reserv};{Duration};{AOE_Lenght};{AOE_Width};{(is_movement ? "1" : "0")};{Pattern};{GetEnumMemberValue(Validators)};{Class};{(Bonus_Range ? "1" : "0")};{Starcast};{GetEnumMemberValue(Branch)};{(is_knockback ? "1" : "0")};{(Crime ? "1" : "")};{GetEnumMemberValue(metacategory)};{FMB};{AP};{(Attack ? "1" : "")};{(Stance ? "1" : "")};{(Charge ? "1" : "")};{(Maneuver ? "1" : "")};{(Spell ? "1" : "")};";

        // Convert the new line to string form. 
        string newline = String.Join(";", newlineArray);

        // Find Hook
        string hookStr = "// " + GetEnumMemberValue(hook);
        (int ind, string? foundLine) = table.Enumerate().FirstOrDefault(x => x.Item2.Contains(hookStr));
        
        // Add line to table
        if (foundLine != null)
        {
            table.Insert(ind + 1, newline);
            ModLoader.SetTable(table, tableName);
            Log.Information($"Injected Skill Stat {id} into {tableName} under {hook}");
        }
        else
        {
            Log.Error($"Cannot find Hook {hook} in table {tableName}");
            throw new Exception($"Hook {hook} not found in table {tableName}");
        }
    }
}
/*
// Code used to generate newlineArray commands. I string used is the first line of gml_GlobalScript_table_skills_stats. 
using System;

namespace MyApplication
{
  class Program
  {
    static void Main(string[] args)
    {
    	string line = ";Object;Target;Range;KD;MP;Reserv;Duration;AOE_Lenght;AOE_Width;is_movement;Pattern;Validators;Class;Bonus_Range;Starcast;Branch;is_knockback;Crime;metacategory;FMB;AP;Attack;Stance;Charge;Maneuver;Spell;";
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
                if (columns[i] == "Target" || columns[i] == "Pattern" || columns[i] == "Validators" ||  columns[i] == "Class" || columns[i] == "Branch" || columns[i] == "metacategory")
                {
                    singleCommands[i] = "        newlineArray[Array.IndexOf(columnLine, \"" + columns[i] + "\")] = GetEnumMemberValue(" + columns[i] + ");";
                    addIndexCommands[i] = "newlineArray[indices[\"" + columns[i] + "\"]] = GetEnumMemberValue(" + columns[i] + ");";
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