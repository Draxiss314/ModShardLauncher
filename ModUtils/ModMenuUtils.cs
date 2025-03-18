using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using Serilog;
using UndertaleModLib;
using UndertaleModLib.Models;

namespace ModShardLauncher;

public enum UIComponentType
{
    ComboBox,
    CheckBox,
    Slider,
}
public class UIComponent
{
    public UIComponentType CompomentType { get; }
    public string Name { get; }
    public string AssociatedGlobal { get; }
    public (int, int) SliderValues { get; }
    private string[] DropDownValues { get; } = Array.Empty<string>();
    public int DefaultValue { get; }
    public bool OnlyInMainMenu { get; }
    public UIComponent(string name, string associatedGlobal, UIComponentType compomentType, int defaultValue, bool onlyInMainMenu = false)
    {
        CompomentType = compomentType;
        Name = name;
        AssociatedGlobal = associatedGlobal;
        DefaultValue = defaultValue;
        OnlyInMainMenu = onlyInMainMenu;
        switch(CompomentType)
        {
            case UIComponentType.ComboBox:
            case UIComponentType.Slider:
                Log.Error($"Incorrect use of UIComponent, you cannot define a {compomentType} using these parameters.");
                throw new ValueUnavailableException();

            default:
                break;
        }
    }
    public UIComponent(string name, string associatedGlobal, UIComponentType compomentType, string[] dropDownValues, bool onlyInMainMenu = false)
    {
        CompomentType = compomentType;
        DropDownValues = dropDownValues;
        Name = name;
        AssociatedGlobal = associatedGlobal;
        OnlyInMainMenu = onlyInMainMenu;
        switch(CompomentType)
        {
            case UIComponentType.CheckBox:
            case UIComponentType.Slider:
                Log.Error($"Incorrect use of UIComponent, you cannot define a {compomentType} using these parameters.");
                throw new ValueUnavailableException();

            default:
                break;
        }
    }
    public UIComponent(string name, string associatedGlobal, UIComponentType compomentType, (int, int) sliderValues, int defaultValue, bool onlyInMainMenu = false)
    {
        CompomentType = compomentType;
        SliderValues = sliderValues;
        Name = name;
        AssociatedGlobal = associatedGlobal;
        DefaultValue = defaultValue;
        OnlyInMainMenu = onlyInMainMenu;
        switch(CompomentType)
        {
            case UIComponentType.CheckBox:
            case UIComponentType.ComboBox:
                Log.Error($"Incorrect use of UIComponent, you cannot define a {compomentType} using these parameters.");
                throw new ValueUnavailableException();

            default:
                break;
        }
    }
    public void Apply(string sectionName, int index)
    {
        switch(CompomentType)
        {
            case UIComponentType.CheckBox:
                AddCheckBox(Name, AssociatedGlobal, DefaultValue, sectionName, index);
                break;
                
            case UIComponentType.ComboBox:
                AddDropDown(AssociatedGlobal, DropDownValues, index);
                break;

            case UIComponentType.Slider:
                AddSlider(AssociatedGlobal, SliderValues, DefaultValue, sectionName, index);
                break;
        }
    }
    private static void AddCheckBox(string name, string associatedGlobal, int defaultValue, string sectionName, int index)
    {
        UndertaleGameObject checkbox = Msl.AddObject($"o_msl_component_{index}", "s_point", "o_checkbox", isVisible:true, isAwake:true);
        Msl.AddNewEvent(checkbox, @$"event_inherited()
ini_open(""msl_menu_mod.ini"");
global.{associatedGlobal} = ini_read_real(""{sectionName}"", ""{associatedGlobal}"", {defaultValue})
ini_close();
text = ""{name}""
selected = global.{associatedGlobal}", EventType.Create, 0);
        Msl.AddNewEvent(checkbox, $"event_inherited()\nglobal.{associatedGlobal} = selected", EventType.Other, 11);
    }
    private static void AddDropDown(string associatedGlobal, string[] dropDownValues, int index)
    {
        UndertaleGameObject checkbox = Msl.AddObject($"o_msl_component_{index}", "s_point", "o_combobox", isVisible:true, isAwake:true);
 
        string tmp = "";
        foreach(string value in dropDownValues)
        {
            tmp += $"\nds_list_add(optionsNamesList, \"{value}\")";
            tmp += $"\nds_list_add(optionsValuesList, \"{value}\")";
        }
        Msl.AddNewEvent(checkbox, $"global.{associatedGlobal} = ds_list_find_value(optionsValuesList, optionIndex)\nevent_user(14)", EventType.Other, 11);
        string other24 = $@"event_inherited()
{tmp}
if (variable_global_exists(""{associatedGlobal}""))
{{
    optionIndex = ds_list_find_index(optionsValuesList, global.{associatedGlobal})
}}
if ((optionIndex == -1))
{{
    optionIndex = 0;
    global.{associatedGlobal} = ds_list_find_value(optionsValuesList, 0)
}}
        ";
        Msl.AddNewEvent(checkbox, other24, EventType.Other, 24);
    }
    private static void AddSlider(string associatedGlobal, (int, int) sliderValues, int defaultValue, string sectionName, int index)
    {
        UndertaleGameObject slider = Msl.AddObject($"o_msl_component_{index}", "s_music_slide", "o_slider", isVisible:true, isAwake:true);
        Msl.AddNewEvent(
            slider, 
            @$"ini_open(""msl_menu_mod.ini"");
global.{associatedGlobal} = math_round(ini_read_real(""{sectionName}"", ""{associatedGlobal}"", {defaultValue}))
ini_close();

event_inherited()
positionPercent =  scr_convertToNewRange(global.{associatedGlobal}, 0, 1, {sliderValues.Item1}, {sliderValues.Item2})
valueMin = {sliderValues.Item1}
valueMax = {sliderValues.Item2}
scr_guiInteractiveStateUpdate(id, 14, 25)
scr_guiPositionOffsetUpdate(id, scr_convertToNewRange(positionPercent, positionMin, positionMax, 0, 1))
scr_guiLayoutOffsetUpdate(id, 0, -2)", 
            EventType.Create, 0
        );
        Msl.AddNewEvent(slider, $"global.{associatedGlobal} = math_round(scr_convertToNewRange(positionPercent, valueMin, valueMax, 0, 1))", EventType.Other, 10);
        Msl.AddNewEvent(slider, @"event_inherited()
with (guiParent)
{
    valueLeft = math_round(scr_convertToNewRange(other.positionPercent, other.valueMin, other.valueMax, 0, 1)) 
}",
            EventType.Other, 11);
        
        Msl.AddNewEvent(slider, "event_inherited()", EventType.Other, 25);
    }
}
internal class Menu
{
    public string Name { get; }
    public UIComponent[] Components { get; }
    public Menu(string name, UIComponent[] components)
    {
        Name = name;
        Components = components;
    }
}
public static partial class Msl
{
    internal static void CreateMenu(List<Menu> menus)
    {
        UndertaleGameObject menu = AddObject("o_msl_menu_mod", "s_settings_button_down", "o_settings_tab", isVisible:true, isAwake:true);
        AddNewEvent(menu, $"event_inherited()\ntext = \"MODS\"", EventType.Create, 0);

        string injectedOther10 = "";
        string injectedOther11 = "";
        string injectedOther12 = "";
        string injectedOther13 = "";
        string tmp = "";

        int index = 0;
        foreach(Menu m in menus)
        {
        
            injectedOther10 += $@"_sectionContainer = scr_guiCreateContainer(rightContainer, o_guiContainerEmpty, depth, 0, 0, 2)
with (scr_guiCreateSimple(_sectionContainer, o_settings_menu_header, (depth - 1)))
{{
    text = ""{m.Name}""
    image_xscale = _headerWidth
    image_yscale = 26
    scr_guiSizeUpdate(id, image_xscale, image_yscale)
}}
_buttonsContainer = scr_guiCreateContainer(_sectionContainer, o_guiContainerEmpty, depth, 0, 0, 2)
scr_guiContainerSpaceUpdate(_buttonsContainer, 5, 0, 0, 0)
scr_guiContainerChildrenSpaceUpdate(_buttonsContainer, 0, 5)";

            injectedOther11 += $"ini_section_delete(\"{m.Name}\")";

            foreach(UIComponent component in m.Components)
            {
                component.Apply(m.Name, index);
                switch(component.CompomentType)
                {
                    case UIComponentType.CheckBox:
                        tmp = $"scr_guiCreateCheckbox(_buttonsContainer, o_msl_component_{index}, (depth - 1));";
                        injectedOther12 += $"\nini_write_real(\"{m.Name}\", \"{component.AssociatedGlobal}\", global.{component.AssociatedGlobal})";
                        injectedOther13 += $"\nglobal.{component.AssociatedGlobal} = ini_read_real(\"{m.Name}\", \"{component.AssociatedGlobal}\", {component.DefaultValue})";
                    break;

                    case UIComponentType.ComboBox:
                        tmp = $"\nscr_guiCreateCombobox(_buttonsContainer, o_msl_component_{index}, (depth - 1), 0, 0, \"{component.Name}\")";
                        injectedOther12 += $"\nini_write_string(\"{m.Name}\", \"{component.AssociatedGlobal}\", global.{component.AssociatedGlobal})";
                        injectedOther13 += $"\nglobal.{component.AssociatedGlobal} = ini_read_string(\"{m.Name}\", \"{component.AssociatedGlobal}\", \"{component.DefaultValue}\")";
                    break;
                    
                    case UIComponentType.Slider:
                        tmp = @$"
_container = scr_guiCreateContainer(_buttonsContainer, o_guiContainerEmpty, depth)
scr_guiSizeUpdate(_container, _headerWidth, 14)
scr_guiCreateText(_container, (depth - 1), 0, 2, ""{component.Name}"", make_colour_rgb(149, 121, 106))
with (scr_guiCreateContainer(_container, o_music_bar, (depth - 1), 118, 2))
{{
    slider = scr_guiCreateInteractive(id, o_msl_component_{index}, (depth - 1))
    valueRight = {component.SliderValues.Item2}
    valueLeft = scr_convertToNewRange(slider.positionPercent, {component.SliderValues.Item1}, {component.SliderValues.Item2}, 0, 1)
}}";  
                        injectedOther12 += $"\nini_write_real(\"{m.Name}\", \"{component.AssociatedGlobal}\", global.{component.AssociatedGlobal})";
                        injectedOther13 += $"\nglobal.{component.AssociatedGlobal} = math_round(ini_read_real(\"{m.Name}\", \"{component.AssociatedGlobal}\", {component.DefaultValue}))";
                    break;
                }
                if (component.OnlyInMainMenu)
                {
                    injectedOther10 += @$"if (room == global.mainMenuRoom)
{{
{tmp}
}}
else
{{
scr_guiCreateText(_buttonsContainer, (depth - 1), 0, 0, ""{component.Name} can only be changed from the main menu"", make_colour_rgb(149, 121, 106))
}}";
                }
                else
                {
                    injectedOther10 += tmp;
                }
                index++;
            }
        }

        string other10 = $@"event_inherited();
with (guiParent)
{{
    scr_guiContainerSpaceUpdate(rightContainer, 5, 5, 5, 5)
    scr_guiContainerChildrenSpaceUpdate(rightContainer, 0, 10)
    var _headerWidth = (rightContainerWidth - 10)
    var _sectionContainer;
    var _buttonsContainer;
    var _container;
    {injectedOther10}
}}
        ";
        AddNewEvent(menu, other10, EventType.Other, 10);

        string other11 = $@"event_inherited();
ini_open(""msl_menu_mod.ini"");
{injectedOther11}
ini_close();
event_user(3);
        ";
        AddNewEvent(menu, other11, EventType.Other, 11);

        string other12 = $@"event_inherited();
with (o_checkbox) 
{{
    event_user(1)
}}
with (o_combobox)
{{
    event_user(1)
}}
with (o_slider)
{{
    event_user(0)
}}
ini_open(""msl_menu_mod.ini"");
{injectedOther12}
ini_close();
        ";
        AddNewEvent(menu, other12, EventType.Other, 12);
        
        string other13 = $@"event_inherited();
ini_open(""msl_menu_mod.ini"");
{injectedOther13}
ini_close();
";
        AddNewEvent(menu, other13, EventType.Other, 13);

        LoadGML("gml_Object_o_settings_menu_Create_0")
            .Apply(x => InsertNewMenu(x, "o_msl_menu_mod"))
            .Save();
    }
    private static IEnumerable<string> InsertNewMenu(IEnumerable<string> lines, string name)
    {
        foreach(string line in lines)
        {
            if (line.Contains("var _tabButtonsArray = "))
            {
                yield return line[..line.IndexOf("]")] + $", {name}];";
            }
            else
            {
                yield return line;
            }
        }
    }
    public static void AddMenu(string name, params UIComponent[] components)
    {
        string tmp = "";
        foreach(UIComponent component in components)
        {
            switch(component.CompomentType)
            {
                case UIComponentType.CheckBox:
                case UIComponentType.Slider:
                    tmp += @$"global.{component.AssociatedGlobal} = ini_read_real(""{name}"", ""{component.AssociatedGlobal}"", {component.DefaultValue})";
                break;

                case UIComponentType.ComboBox:
                    tmp += @$"global.{component.AssociatedGlobal} = ini_read_string(""{name}"", ""{component.AssociatedGlobal}"", ""{component.DefaultValue}"")";
                break;
            }
           
        }
        string txtGlobalCreation = $@"ini_open(""msl_menu_mod.ini"");
{tmp}
ini_close();";

        // add global if needed
        if (!ModLoader.Data.GameObjects.Select(x => x.Name.Content).Contains("o_msl_initializer"))
        {
            UndertaleGameObject ob = AddObject("o_msl_initializer", isPersistent:true);
            AddNewEvent(ob, txtGlobalCreation, EventType.Create, 0);
            // initializer in START room
            UndertaleRoom room = GetRoom("START");
            room.AddGameObject("Instances", ob);
        }
        else
        {
            LoadGML(EventName("o_msl_initializer", EventType.Create, 0))
                .MatchAll()
                .InsertBelow(txtGlobalCreation)
                .Save();
        }
        
        ModLoader.AddMenu(name, components);
    }
}