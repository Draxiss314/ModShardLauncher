using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace ModShardLauncher
{ 
    public partial class Msl
    {
        public static void InjectTableNewColumn(string newEntry, string tablename, string insert = "", bool insertBehind = true, string overridePosition = false)
        {
            List<string> table = ThrowIfNull(ModLoader.GetTable(tableName));
            string[] columnLine = table[0].Split(";"); // The line which determine the column names

            // Add missing column. 
            if (columnLine.Contains(newEntry, StringComparison.Ordinal) != true) 
            {
                var updatedTable = new List<string>(table.Count) { Capacity = table.Count};

                int index = 0; // The insertion index

                if (insert == "") //Insert at the end if unspecified
                {
                    index = columnLine.Split(";").Length; 
                }
                else 
                { 
                    if (table[0].Contains(insert, StringComparison.Ordinal) != true)
                        throw new Exception("Error: String \"" + insert + "\" does not exist in gml_GlobalScript_table_items_stats.");

                    // By default, insert behind the targeted insertion entry. 
                    // Otherise, insert in front of that entry.
                    index = Array.FindIndex(columnLine, element => element == insert) + 1; 
                    if (insertBehind != true)
                    {
                        index -= 1;
                    }
                }
                for (int i = 0; i < table.Count; i++)
                {
                    string line = table[i];
                    string[] subs = line.Split(";");
                    Array.Resize(ref subs, subs.Length + 1);
                    Array.Copy(subs, index, subs, index + 1, subs.Length - index - 1);
                    if (i == 0)
                        subs[index] = newEntry;
                    else
                        subs[index] = "";
                    string newline = String.Join(";",subs);
                    updatedTable.Add(newline);
                }
                ModLoader.SetTable(updatedTable, tableName);
            }
            // Move an already-existing column entry back into position. 
            else if (overridePosition == true && columnLine.Contains(newEntry, StringComparison.Ordinal) == true && insert != "" && columnLine[Array.FindIndex(columnLine, element => element == insert) + 1] != newEntry) 
            {
                // Move the column position for all rows
                if (table[0].Contains(insert, StringComparison.Ordinal) != true)
                    throw new Exception("Error: String \"" + insert + "\" does not exist in gml_GlobalScript_table_items_stats.");
                current = Array.FindIndex(columnLine, element => element == newEntry);
                index = Array.FindIndex(columnLine, element => element == insert) + 1;

                for (int i = 0; i < table.Count; i++)
                {
                    string line = table[i];
                    string[] subs = line.Split(";");
                    string entryVal = subs[current];
                    subs = Array.FindAll(subs, e => e != entryVal); // Delete the column entry. 
                    Array.Resize(ref subs, subs.Length + 1); // Restore array size. This assumes there were no duplicate column entries. 
                    Array.Copy(subs, index, subs, index + 1, subs.Length - index - 1);
                    subs[index] = entryVal; // Move the entryVal to the new position. 
                    string newline = String.Join(";",subs);
                    updatedTable.Add(newline);
                }
                ModLoader.SetTable(updatedTable, tableName);
            }
        }

        private static string? GetEnumMemberValue<T>(this T value)
            where T : Enum
        {
            return typeof(T)
                .GetTypeInfo()
                .DeclaredMembers
                .SingleOrDefault(x => x.Name == value.ToString())?
                .GetCustomAttribute<EnumMemberAttribute>(false)?
                .Value ?? value.ToString();
        }
        
        // Tables left to do :
        // - ai
        // - supply / demand if necessary ?
    }
}
