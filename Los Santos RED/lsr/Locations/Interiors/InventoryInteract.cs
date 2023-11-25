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
    protected LocationCamera LocationCamera;
    public bool CanAccessItems { get; set; } = true;
    public bool CanAccessWeapons { get; set; } = true;
    public bool CanAccessCash { get; set; } = true;

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
        SetupCamera();
        if (!MoveToPosition())
        {
            Interior.IsMenuInteracting = false;
            Game.DisplayHelp("Access Failed");
            return;
        }
        InventoryableLocation.CreateInventoryMenu(CanAccessItems, CanAccessWeapons, CanAccessCash);
        LocationCamera?.StopImmediately(true);
        Interior.IsMenuInteracting = false;
    }
    public override void AddPrompt()
    {
        if (Player == null)
        {
            return;
        }
        Player.ButtonPrompts.AddPrompt(Name, ButtonPromptText, Name, Settings.SettingsManager.KeySettings.InteractStart, 999);
    }
    private void SetupCamera()
    {
        if (CameraPosition != Vector3.Zero)
        {
            if (LocationCamera == null)
            {
                LocationCamera = new LocationCamera(InventoryableLocation.GameLocation, LocationInteractable, Settings, true);
            }
            LocationCamera.MoveToPosition(CameraPosition, CameraDirection, CameraRotation, false);
        }
    }
}

