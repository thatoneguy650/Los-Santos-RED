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

public class DriveThru : GameLocation
{
    public DriveThru() : base()
    {

    }
    public override string TypeName { get; set; } = "Drive-Thru";
    public override int MapIcon { get; set; } = 524;//523;
    public override string ButtonPromptText { get; set; }
    public DriveThru(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Shop At {Name}";
        return true;
    }
    public override void OnInteract()//ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        //Player = player;
        //ModItems = modItems;
        //World = world;
        //Settings = settings;
        //Weapons = weapons;
        //Time = time;
        if (IsLocationClosed())
        {
            return;
        }
        if (CanInteract)
        {
            Player.ActivityManager.IsInteractingWithLocation = true;
            CanInteract = false;
            Player.IsTransacting = true;
            GameFiber.StartNew(delegate
            {
                try
                {
                    NativeFunction.Natives.SET_GAMEPLAY_COORD_HINT(EntrancePosition.X, EntrancePosition.Y, EntrancePosition.Z, -1, 2000, 2000);
                    CreateInteractionMenu();
                    HandleVariableItems();
                    Transaction = new Transaction(MenuPool, InteractionMenu, Menu, this);
                    Transaction.PreviewItems = false;
                    Transaction.CreateTransactionMenu(Player, ModItems, World, Settings, Weapons, Time);
                    InteractionMenu.Visible = true;
                    Transaction.ProcessTransactionMenu();
                    Transaction.DisposeTransactionMenu();
                    DisposeInteractionMenu();
                    NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
                    Player.ActivityManager.IsInteractingWithLocation = false;
                    Player.IsTransacting = false;
                    CanInteract = true;
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "DriveThruInteract");
        }
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.DriveThrus.Add(this);
        base.AddLocation(possibleLocations);
    }
}

