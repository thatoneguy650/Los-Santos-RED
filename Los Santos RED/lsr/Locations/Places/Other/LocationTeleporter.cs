using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class LocationTeleporter
{
    private ILocationInteractable Player;
    private GameLocation InteractableLocation;
    private ISettingsProvideable Settings;
    public bool IsInside { get; private set; }
    public LocationTeleporter(ILocationInteractable player, GameLocation interactableLocation, ISettingsProvideable settings)
    {
        Player = player;
        InteractableLocation = interactableLocation;
        Settings = settings;
    }

    public void Teleport()
    {
        if(InteractableLocation.Interior != null && InteractableLocation.Interior.IsTeleportEntry)
        {
            Game.FadeScreenOut(1500, true);
            Player.Character.Position = InteractableLocation.Interior.InteriorEgressPosition;
            Player.Character.Heading = InteractableLocation.Interior.InteriorEgressHeading;
            IsInside = true;
            Game.FadeScreenIn(1500, true);
            UpdateInside();
        }
    }


    private void UpdateInside()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsInside)
                {
                    Update();
                    GameFiber.Yield();
                }
                Player.ActivityManager.IsInteractingWithLocation = false;
                InteractableLocation.CanInteract = true;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }

        }, "Interact");
    }

    public void Update()
    {
        if (IsInside)
        {
            //GameFiber CameraWatcher = GameFiber.StartNew(delegate
            //{
            //    while (IsInside)
            //    {
            if (Player.Character.DistanceTo(InteractableLocation.Interior.InteriorEgressPosition) <= 3f)
            {
                Player.ButtonPrompts.AddPrompt("ExitTeleport", "Exit", "ExitTeleport", Settings.SettingsManager.KeySettings.InteractStart, 999);
            }
            else
            {
                Player.ButtonPrompts.RemovePrompts("ExitTeleport");
            }
            if (Player.ButtonPrompts.IsPressed("ExitTeleport"))
            {
                Exit();
            }
            //        GameFiber.Yield();
            //    }
            //    Exit();
            //}, "CameraWatcher");
        }
    }
    public void Exit()
    {
        Player.ButtonPrompts.RemovePrompts("ExitTeleport");
        if (InteractableLocation != null)
        {
            Game.FadeScreenOut(1500, true);
            Player.Character.Position = InteractableLocation.EntrancePosition;
            Player.Character.Heading = InteractableLocation.EntranceHeading;
            IsInside = false;
            Game.FadeScreenIn(1500, true);
        }
    }
}

