using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class InventoryInteract : InteriorInteract
{

    public bool CanAccessItems { get; set; } = true;
    public bool CanAccessWeapons { get; set; } = true;
    public bool CanAccessCash { get; set; } = true;
    public List<ItemType> AllowedItemTypes { get; set; }
    public List<ItemType> DisallowedItemTypes { get; set; }
    public bool RemoveMenuBanner { get; set; } = true;
    public string Title { get; set; }
    public string Description { get; set; }
    [XmlIgnore]
    public IInventoryableLocation InventoryableLocation { get; set; }
    public InventoryInteract()
    {
    }

    public InventoryInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {

    }
    public override void OnInteract()
    {
        Interior.IsMenuInteracting = true;
        Interior?.RemoveButtonPrompts();
        RemovePrompt();
        SetupCamera(false);
        if (!MoveToPosition())
        {
            Interior.IsMenuInteracting = false;
            Game.DisplayHelp("Access Failed");
            LocationCamera?.StopImmediately(true);
            return;
        }
        Player.InteriorManager.OnStartedInteriorInteract();
        InventoryableLocation.CreateInventoryMenu(CanAccessItems, CanAccessWeapons, CanAccessCash, AllowedItemTypes, DisallowedItemTypes, RemoveMenuBanner, Title, Description);
        LocationCamera?.ReturnToGameplay(true);
        LocationCamera?.StopImmediately(true);
        Interior.IsMenuInteracting = false;
        Player.InteriorManager.OnEndedInteriorInteract();
    }
    public override void AddPrompt()
    {
        if (Player == null)
        {
            return;
        }
        Player.ButtonPrompts.AddPrompt(Name, ButtonPromptText, Name, Settings.SettingsManager.KeySettings.InteractStart, 999);
    }

}

