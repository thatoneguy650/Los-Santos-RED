using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class Morgue : GameLocation
{
    public Morgue() : base()
    {

    }
    public override string TypeName { get; set; } = "Morgue";
    public override int MapIcon { get; set; } = (int)BlipSprite.Hospital;
    public override string ButtonPromptText { get; set; }
    public Morgue(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Enter {Name}";
        return true;
    }
    public override void OnInteract(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;

        if (IsLocationClosed())
        {
            return;
        }

        if (CanInteract)
        {
            
            Player.ActivityManager.IsInteractingWithLocation = true;
            CanInteract = false;

            LocationTeleporter locationTeleporter = null;
            if (Interior != null && Interior.IsTeleportEntry)
            {
                locationTeleporter = new LocationTeleporter(Player, this,settings);
                locationTeleporter.Teleport();
            }

            //Player.IsTransacting = true;

            GameFiber.StartNew(delegate
            {
                try
                {



                    while (locationTeleporter?.IsInside == true)
                    {
                        locationTeleporter.Update();
                        GameFiber.Yield();
                    }


                    Player.ActivityManager.IsInteractingWithLocation = false;
                    CanInteract = true;
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }

            }, "Interact");
        }
    }


}

