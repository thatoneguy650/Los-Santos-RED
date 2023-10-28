using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DebugTimeSubMenu : DebugSubMenu
{
    private ITimeControllable Time;
    public DebugTimeSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, ITimeControllable time) : base(debug, menuPool, player)
    {
        Time = time;
    }
    public override void AddItems()
    {
        UIMenu TimeItems = MenuPool.AddSubMenu(Debug, "Time Menu");
        TimeItems.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Change various time items.";
        UIMenuNumericScrollerItem<int> FastForwardTime = new UIMenuNumericScrollerItem<int>("Fast Forward Time", "Fast forward time.", 1, 24, 1) { Formatter = v => v + " Hours" };
        FastForwardTime.Activated += (menu, item) =>
        {
            Time.FastForward(FastForwardTime.Value);
            menu.Visible = false;
        };
        TimeItems.AddItem(FastForwardTime);
        UIMenuItem SetDateToToday = new UIMenuItem("Set Game Date Current", "Sets the game date the same as system date");
        SetDateToToday.Activated += (menu, item) =>
        {
            Time.SetDateToToday();
            menu.Visible = false;
        };
        TimeItems.AddItem(SetDateToToday);
        UIMenuNumericScrollerItem<int> SetDateYear = new UIMenuNumericScrollerItem<int>("Set Game Date Year", "Sets the game date to the year selected (August 1st at 1 PM)", 1970, 2030, 1);
        SetDateYear.Activated += (menu, item) =>
        {
            DateTime toSet = new DateTime(SetDateYear.Value, 8, 1, 13, 0, 0);
            Time.SetDateTime(toSet);
            Game.DisplayHelp($"Date Set to {toSet}");
            menu.Visible = false;
        };
        TimeItems.AddItem(SetDateYear);



        UIMenuNumericScrollerItem<int> AdvanceHours = new UIMenuNumericScrollerItem<int>("Advance Hours", "Moves the game time forwards by the set hours", 1, 48, 1);
        AdvanceHours.Activated += (menu, item) =>
        {
            Time.SetDateTime(Time.CurrentDateTime.AddHours(AdvanceHours.Value));
            Game.DisplayHelp($"Date Set to {Time.CurrentDateTime.AddHours(AdvanceHours.Value)}");
            menu.Visible = false;
        };
        TimeItems.AddItem(AdvanceHours);

        UIMenuNumericScrollerItem<int> AdvanceDays = new UIMenuNumericScrollerItem<int>("Advance Days", "Moves the game time forwards by the set days", 1, 48, 1);
        AdvanceDays.Activated += (menu, item) =>
        {
            Time.SetDateTime(Time.CurrentDateTime.AddDays(AdvanceDays.Value));
            Game.DisplayHelp($"Date Set to {Time.CurrentDateTime.AddDays(AdvanceDays.Value)}");
            menu.Visible = false;
        };
        TimeItems.AddItem(AdvanceDays);
    }
}

