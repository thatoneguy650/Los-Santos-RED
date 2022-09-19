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

public class Morgue : InteractableLocation
{
    private LocationCamera StoreCamera;
    private ILocationInteractable Player;
    private IModItems ModItems;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private ITimeControllable Time;
    private Transaction Transaction;
    public Morgue() : base()
    {

    }
    public override string TypeName { get; set; } = "Morgue";
    public override int MapIcon { get; set; } = (int)BlipSprite.Hospital;
    public override Color MapIconColor { get; set; } = Color.White;
    public override float MapIconScale { get; set; } = 1.0f;
    public override string ButtonPromptText { get; set; }
    public Morgue(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
        ButtonPromptText = $"Enter {Name}";
    }
    public override void OnInteract(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;

        if (CanInteract)
        {
            Player.IsInteractingWithLocation = true;
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




                while(locationTeleporter?.IsInside == true)
                {
                    locationTeleporter.Update();
                    GameFiber.Yield();
                }


                Player.IsInteractingWithLocation = false;
                CanInteract = true;
            }, "Interact");
        }
    }


}

