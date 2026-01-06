using Rage;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


public class DisplayInteract : InteriorInteract
{
    private UIMenu DisplayMainMenu;
    private MenuPool MenuPool;

    public DisplayInteract() 
    { 

    }

    public DisplayInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {
        AutoCamera = false;
    }

    [XmlIgnore]
    public Residence DisplayLocation { get; set; }
    public CabinetData CabinetData { get; set; }


    public override void OnInteract()
    {
        Interior.IsMenuInteracting = true;
        Interior?.RemoveButtonPrompts();
        RemovePrompt();
        SetupCamera(false);
        if (!WithWarp)
        {
            if (!MoveToPosition())
            {
                Interior.IsMenuInteracting = false;
                Game.DisplayHelp("Interact Failed");
                LocationCamera?.StopImmediately(true);
                return;
            }
        }
        Player.InteriorManager.OnStartedInteriorInteract();


        CreateTrophyMenu();

        Interior.IsMenuInteracting = false;
        LocationCamera?.ReturnToGameplay(true);
        LocationCamera?.StopImmediately(true);
        Player.InteriorManager.OnEndedInteriorInteract();
    }
    public override void AddPrompt()
    {
        if (Player == null)
        {
            return;
        }
        Player.ButtonPrompts.AttemptAddPrompt(Name, ButtonPromptText, Name, Settings.SettingsManager.KeySettings.InteractStart, 999);
    }

    private void CreateTrophyMenu()
    {
        DisplayMainMenu = new UIMenu("Displays","Manage Placed Displays");
        MenuPool = new MenuPool();
        MenuPool.Add(DisplayMainMenu);
        DisplayMainMenu.SetBannerType(EntryPoint.LSRedColor);

        if (CabinetData == null)
        {
            return;
        }

        foreach(DisplaySlot trophySlot in CabinetData.Slots)
        {
            trophySlot.AddToMenu(MenuPool, DisplayMainMenu, ModItems, DisplayLocation, this, LocationCamera);
        }
        DisplayMainMenu.Visible = true;

        while (MenuPool.IsAnyMenuOpen() && Player.ActivityManager.CanPerformActivitiesExtended)
        {
            Player.WeaponEquipment.SetUnarmed();
            MenuPool.ProcessMenus();   
            Player.IsSetDisabledControls = true;
            GameFiber.Yield();
        }
        Player.IsSetDisabledControls = false;

    }
    public override void OnInteriorLoaded()
    {
        SpawnDisplayProps();//should be done when you get close or when you enter actually
        base.OnInteriorLoaded();
    }
    public override void OnInteriorUnloaded()
    {
        DespawnDisplayProps();
        base.OnInteriorUnloaded();
    }
    public void SpawnDisplayProps()
    {
        //spawn trophies based on what is stored
        if(DisplayLocation == null || CabinetData == null || DisplayLocation.DisplayPlacements == null)
        {
            return;
        }
        foreach(DisplayPlacement trophyPlacement in DisplayLocation.DisplayPlacements)
        {
            trophyPlacement.SpawnDisplay(CabinetData, ModItems);
        }
    }
    public void DespawnDisplayProps()
    {
        EntryPoint.WriteToConsole("DespawnTrophies RAN");
        if (DisplayLocation == null || CabinetData == null || DisplayLocation.DisplayPlacements == null)
        {
            return;
        }
        foreach (DisplayPlacement trophyPlacement in DisplayLocation.DisplayPlacements)
        {
            trophyPlacement.DespawnDisplay();
            EntryPoint.WriteToConsole($"DespawnTrophies DELETED TROPHY {trophyPlacement.SlotID} {trophyPlacement.ModItemName}");
        }
    }
}

