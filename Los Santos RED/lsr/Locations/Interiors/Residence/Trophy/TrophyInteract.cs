using Rage;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


public class TrophyInteract : InteriorInteract
{
    private UIMenu TrophyMainMenu;
    private MenuPool MenuPool;

    public TrophyInteract() 
    { 

    }

    public TrophyInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {
        AutoCamera = false;
    }

    [XmlIgnore]
    public Residence TrophyableLocation { get; set; }
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
        TrophyMainMenu = new UIMenu("Trophies","Manage Placed Trophies");
        MenuPool = new MenuPool();
        MenuPool.Add(TrophyMainMenu);
        foreach(TrophySlot trophySlot in CabinetData.Slots)
        {
            trophySlot.AddToMenu(MenuPool, TrophyMainMenu, ModItems, TrophyableLocation, this, LocationCamera);
        }
        TrophyMainMenu.Visible = true;

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
        SpawnTrophies();//should be done when you get close or when you enter actually
        base.OnInteriorLoaded();
    }
    public override void OnInteriorUnloaded()
    {
        DespawnTrophies();
        base.OnInteriorUnloaded();
    }
    public void SpawnTrophies()
    {
        //spawn trophies based on what is stored
        if(TrophyableLocation == null || CabinetData == null || TrophyableLocation.TrophyPlacements == null)
        {
            return;
        }
        foreach(TrophyPlacement trophyPlacement in TrophyableLocation.TrophyPlacements)
        {
            trophyPlacement.SpawnTrophy(CabinetData);
        }
    }
    public void DespawnTrophies()
    {
        EntryPoint.WriteToConsole("DespawnTrophies RAN");
        if (TrophyableLocation == null || CabinetData == null || TrophyableLocation.TrophyPlacements == null)
        {
            return;
        }
        foreach (TrophyPlacement trophyPlacement in TrophyableLocation.TrophyPlacements)
        {
            trophyPlacement.DespawnTrophy();
            EntryPoint.WriteToConsole($"DespawnTrophies DELETED TROPHY {trophyPlacement.SlotID} {trophyPlacement.TrophyModelName}");
        }
    }
}

