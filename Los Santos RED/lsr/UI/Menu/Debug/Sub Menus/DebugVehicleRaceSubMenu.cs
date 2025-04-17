using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DebugVehicleRaceSubMenu : DebugSubMenu
{
    private List<VehicleRace> PossibleRaces = new List<VehicleRace>();
    private IVehicleRaces VehicleRaces;
    private IEntityProvideable World;
    private ModDataFileManager ModDataFileManager;
    private IInteractionable Interactionable;
    private bool IsWritingPosition;
    private UIMenu DebugRaceSubMenu;

    public DebugVehicleRaceSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, IVehicleRaces vehicleRaces, IEntityProvideable world, ModDataFileManager modDataFileManager, IInteractionable interactionable) : base(debug, menuPool, player)
    {
        VehicleRaces = vehicleRaces;
        World = world;
        ModDataFileManager = modDataFileManager;
        Interactionable = interactionable;
    }

    public override void AddItems()
    {
        SubMenu = MenuPool.AddSubMenu(Debug, "Racing Menu");
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Do vehicle racing";
        SubMenu.SetBannerType(EntryPoint.LSRedColor);
        CreateMenu();
    }
    public override void Update()
    {
        DebugRaceSubMenu.Clear();
        VehicleRacesMenu vehicleRaceMenu = new VehicleRacesMenu(MenuPool, DebugRaceSubMenu, null, ModDataFileManager.VehicleRaces, ModDataFileManager.PlacesOfInterest, World, Interactionable, false, null, ModDataFileManager.DispatchableVehicles, null, null, -1, ModDataFileManager.DispatchablePeople);
        vehicleRaceMenu.Setup();
    }
    private void CreateMenu()
    {
        SubMenu.Clear();

        DebugRaceSubMenu = MenuPool.AddSubMenu(SubMenu, "Setup Debug Race");
        SubMenu.SetBannerType(EntryPoint.LSRedColor);
        VehicleRacesMenu vehicleRaceMenu = new VehicleRacesMenu(MenuPool, DebugRaceSubMenu, null, ModDataFileManager.VehicleRaces, ModDataFileManager.PlacesOfInterest, World, Interactionable, false, null, ModDataFileManager.DispatchableVehicles, null, null, -1, ModDataFileManager.DispatchablePeople);
        vehicleRaceMenu.Setup();


        UIMenuNumericScrollerItem<int> LogRaceStart = new UIMenuNumericScrollerItem<int>("Log Race Starting Pos", "",0,10,1);
        LogRaceStart.Value = 0;
        LogRaceStart.Activated += (sender, selectedItem) =>
        {
            LogStartingPos(LogRaceStart.Value);
            Game.DisplaySubtitle("Added Starting Position");
        };
        SubMenu.AddItem(LogRaceStart);
        UIMenuNumericScrollerItem<int> LogRaceCheckpoint = new UIMenuNumericScrollerItem<int>("Log Race Checkpoint", "", 0, 20, 1);
        LogRaceCheckpoint.Value = 0;
        LogRaceCheckpoint.Activated += (sender, selectedItem) =>
        {
            LogCheckpointPos(LogRaceCheckpoint.Value);
            Game.DisplaySubtitle("Added Checkpoint");
        };    
        SubMenu.AddItem(LogRaceCheckpoint);


        UIMenuItem addLineBreak = new UIMenuItem("Line Break", "Start a the specific race.");
        addLineBreak.Activated += (sender, selectedItem) =>
        {
            AddLineBreak();
            Game.DisplaySubtitle("Added Line Break");
        };
        SubMenu.AddItem(addLineBreak);
    }

    private void AddLineBreak()
    {
        IsWritingPosition = true;
        WriteToTrackData($"                               ");
        IsWritingPosition = false;
    }

    private void LogStartingPos(int ordering)
    {
        IsWritingPosition = true;
        Vector3 pos = Game.LocalPlayer.Character.Position;
        float Heading = Game.LocalPlayer.Character.Heading;
        WriteToTrackData($"new VehicleRaceStartingPosition({ordering}, new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), {Heading}f),");
        IsWritingPosition = false;
    }

    private void LogCheckpointPos(int ordering)
    {
        IsWritingPosition = true;
        Vector3 pos = Game.LocalPlayer.Character.Position;
        float Heading = Game.LocalPlayer.Character.Heading;
        WriteToTrackData($"new VehicleRaceCheckpoint({ordering}, new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f)),");
        IsWritingPosition = false;
    }

    private void WriteToTrackData(String TextToLog)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(TextToLog + System.Environment.NewLine);
        File.AppendAllText("Plugins\\LosSantosRED\\" + "TrackData.txt", sb.ToString());
        sb.Clear();
    }


}

