using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DebugPerformanceSubMenu : DebugSubMenu
{
    public DebugPerformanceSubMenu(UIMenu debug, MenuPool menuPool, IActionable player) : base(debug, menuPool, player)
    {
    }
    public override void AddItems()
    {
        UIMenu OtherItemsMenu = MenuPool.AddSubMenu(Debug, "Performance Menu");
        OtherItemsMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Change various other items.";



        UIMenuNumericScrollerItem<int> logLevelChange = new UIMenuNumericScrollerItem<int>("Log Level", "Sets the log level of the mod", 0, 5, 1);
        logLevelChange.Value = EntryPoint.LogLevel;
        logLevelChange.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            EntryPoint.LogLevel = logLevelChange.Value;
        };
        OtherItemsMenu.AddItem(logLevelChange);

        UIMenuItem PrintEntities = new UIMenuItem("Print Persistent Entities", "Prints a list of all persistent and spawned entities to the log.");
        PrintEntities.Activated += (menu, item) =>
        {
            PrintPersistentEntities();
            menu.Visible = false;
        };
        OtherItemsMenu.AddItem(PrintEntities);

        UIMenuItem PrintEntities2 = new UIMenuItem("Print Entities", "Prints a list of all entities to the log.");
        PrintEntities2.Activated += (menu, item) =>
        {
            PrintAllEntities();
            menu.Visible = false;
        };
        OtherItemsMenu.AddItem(PrintEntities2);


        UIMenuListScrollerItem<ModTaskGroup> taskGroups = new UIMenuListScrollerItem<ModTaskGroup>("Enable Task Groups", "Enable or disable task groups.", EntryPoint.ModController.TaskGroups);
        taskGroups.Activated += (menu, item) =>
        {
            taskGroups.SelectedItem.IsRunning = !taskGroups.SelectedItem.IsRunning;
            taskGroups.Items = EntryPoint.ModController.TaskGroups;
            taskGroups.Reformat();
            Game.DisplaySubtitle($"{taskGroups.SelectedItem.ToString()}");
        };
        OtherItemsMenu.AddItem(taskGroups);

        UIMenuCheckboxItem runUI = new UIMenuCheckboxItem("Run UI", EntryPoint.ModController.RunUI);
        runUI.CheckboxEvent += (sender, Checked) =>
        {
            EntryPoint.ModController.RunUI = Checked;
            Game.DisplaySubtitle($"UI Running: {EntryPoint.ModController.RunUI}");
        };
        OtherItemsMenu.AddItem(runUI);

        UIMenuCheckboxItem runMenuOnly = new UIMenuCheckboxItem("Run Menu", EntryPoint.ModController.RunMenuOnly);
        runMenuOnly.CheckboxEvent += (sender, Checked) =>
        {
            EntryPoint.ModController.RunMenuOnly = Checked;
            Game.DisplaySubtitle($"UI Running: {EntryPoint.ModController.RunMenuOnly}");
        };
        OtherItemsMenu.AddItem(runMenuOnly);


        UIMenuCheckboxItem runVanilla = new UIMenuCheckboxItem("Run Vanilla", EntryPoint.ModController.RunVanilla);
        runVanilla.CheckboxEvent += (sender, Checked) =>
        {
            EntryPoint.ModController.RunVanilla = Checked;
            Game.DisplaySubtitle($"Vanilla Running: {EntryPoint.ModController.RunVanilla}");
        };
        OtherItemsMenu.AddItem(runVanilla);

        UIMenuCheckboxItem runInput = new UIMenuCheckboxItem("Run Input", EntryPoint.ModController.RunInput);
        runInput.CheckboxEvent += (sender, Checked) =>
        {
            EntryPoint.ModController.RunInput = Checked;
            Game.DisplaySubtitle($"Input Running: {EntryPoint.ModController.RunInput}");
        };
        OtherItemsMenu.AddItem(runInput);

        UIMenuCheckboxItem runOther = new UIMenuCheckboxItem("Run Other", EntryPoint.ModController.RunOther);
        runOther.CheckboxEvent += (sender, Checked) =>
        {
            EntryPoint.ModController.RunOther = Checked;
            Game.DisplaySubtitle($"Other Running: {EntryPoint.ModController.RunOther}");
        };
        OtherItemsMenu.AddItem(runOther);
    }

    private void PrintPersistentEntities()
    {
        int TotalEntities = 0;
        EntryPoint.WriteToConsole($"SPAWNED ENTITIES ===============================", 0);
        foreach (Entity ent in EntryPoint.SpawnedEntities)
        {
            if (ent.Exists())
            {
                TotalEntities++;
                EntryPoint.WriteToConsole($"SPAWNED ENTITY STILL EXISTS {ent.Handle} {ent.GetType()} {ent.Model.Name} Dead: {ent.IsDead} Position: {ent.Position}", 0);
            }
        }
        EntryPoint.WriteToConsole($"SPAWNED ENTITIES =============================== TOTAL: {TotalEntities}", 0);

        TotalEntities = 0;




        List<Entity> AllEntities = Rage.World.GetAllEntities().ToList();






        EntryPoint.WriteToConsole($"PERSISTENT ENTITIES ===============================", 0);
        foreach (Entity ent in AllEntities)
        {
            if (ent.Exists() && ent.IsPersistent)
            {
                TotalEntities++;
                EntryPoint.WriteToConsole($"PERSISTENT ENTITY STILL EXISTS {ent.Handle} {ent.GetType()}  {ent.Model.Name} Dead: {ent.IsDead} Position: {ent.Position}", 0);
            }
        }
        EntryPoint.WriteToConsole($"PERSISTENT ENTITIES =============================== TOTAL: {TotalEntities}", 0);
    }
    private void PrintAllEntities()
    {
        int TotalEntities = 0;
        List<Entity> AllEntities = Rage.World.GetAllEntities().ToList();
        int pedCount = 0;
        int vehCount = 0;
        int objectcount = 0;
        EntryPoint.WriteToConsole($"ENTITIES ===============================", 0);
        foreach (Entity ent in AllEntities)
        {
            if (ent.Exists())
            {
                if(ent.GetType() == typeof(Ped))
                {
                    pedCount++;
                }
                else if (ent.GetType() == typeof(Vehicle))
                {
                    vehCount++;
                }
                else if (ent.GetType() == typeof(Rage.Object))
                {
                    objectcount++;
                }
                TotalEntities++;
                EntryPoint.WriteToConsole($"ENTITY {ent.Handle} {ent.GetType()}  {ent.Model.Name} Dead: {ent.IsDead} Position: {ent.Position} Heading {ent.Heading}", 0);
            }
        }
        EntryPoint.WriteToConsole($"ENTITIES =============================== TOTAL: {TotalEntities} Ped:{pedCount} Veh:{vehCount} Obj:{objectcount}", 0);
    }
}

