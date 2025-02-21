using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BodyExport : GameLocation
{
    private UIMenu BodySubMenu;
    private List<StoredBody> StoredBodies;
    private List<StoredBodyLookup> StoredBodyLookups;

    public float VehiclePickupDistance { get; set; } = 25f;
    public int DeadPrice { get; set; } = 1500;
    public int AlivePrice { get; set; } = 5000;
    public override string TypeName { get; set; } = "Body Exports";
    public override int MapIcon { get; set; } = 310;

    public override bool ShowsOnDirectory => false;

    private int FinalPrice(bool isDead) => isDead ? DeadPrice : AlivePrice;
    public BodyExport() : base()
    {

    }
    public BodyExport(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Export Body At {Name}";
        return true;
    }
    public override void OnInteract()
    {
        if (IsLocationClosed())
        {
            return;
        }

        if (!CanInteract)
        {
            return;
        }
        if (Interior != null && Interior.IsTeleportEntry)
        {
            DoEntranceCamera(true);
            Interior.Teleport(Player, this, StoreCamera);
        }
        else
        {
            StandardInteract(null, false);
        }
    }
    public override void StandardInteract(LocationCamera locationCamera, bool isInside)
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        Player.IsTransacting = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                SetupLocationCamera(locationCamera, isInside, true);
                CreateInteractionMenu();
                InteractionMenu.Visible = true;
                GenerateBodyExportMenu();
                ProcessInteractionMenu();
                DisposeInteractionMenu();
                DisposeCamera(isInside);
                DisposeInterior();
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
                Player.IsTransacting = false;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "HotelInteract");
    }

    private void BodySubMenu_OnMenuClose(UIMenu sender)
    {
        StoreCamera.ReHighlightStoreWithCamera();
    }
    private void BodySubMenu_OnMenuOpen(UIMenu sender)
    {
        BodySubMenu_OnIndexChange(sender, sender.CurrentSelection);
    }
    private void BodySubMenu_OnIndexChange(UIMenu sender, int newIndex)
    {
        if(sender == null || sender.MenuItems == null || !sender.MenuItems.Any() || newIndex == -1)
        {
            return;
        }
        UIMenuItem foundMenu = sender.MenuItems[newIndex];
        if (foundMenu == null)
        {
            return;
        }
        StoredBodyLookup foundItem = StoredBodyLookups.FirstOrDefault(x => x.MenuItem == foundMenu);
        if (foundItem == null)
        {
            return;
        }
        PedExt body = foundItem.Body;
        if (body != null && body.Pedestrian.Exists())
        {
            StoreCamera.HighlightEntity(body.Pedestrian);
        }      
    }
    private void GenerateBodyExportMenu()
    {
        BodySubMenu = MenuPool.AddSubMenu(InteractionMenu, "Export Body");
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Select a body to sell on the marketplace.";
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Barber;
        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            BodySubMenu.SetBannerType(BannerImage);
        }
        StoredBodyLookups = new List<StoredBodyLookup>();
        //BodySubMenu.OnItemSelect += InteractionMenu_OnItemSelect;
        BodySubMenu.OnIndexChange += BodySubMenu_OnIndexChange;
        BodySubMenu.OnMenuOpen += BodySubMenu_OnMenuOpen;
        BodySubMenu.OnMenuClose += BodySubMenu_OnMenuClose;
        bool Added = false;
        StoredBodies = new List<StoredBody>();
        foreach (VehicleExt veh in World.Vehicles.AllVehicleList)
        {
            if (!IsValidForConsideration(veh))
            {
                continue;
            }
            foreach (StoredBody storedBody in veh.VehicleBodyManager.StoredBodies)
            {
                EntryPoint.WriteToConsole($"ADDED STORED BODY TO LIST");
                StoredBodies.Add(storedBody);
            }
        }
        foreach(StoredBody storedBody in StoredBodies)
        {
            if(storedBody.PedExt == null)
            {
                continue;
            }
            EntryPoint.WriteToConsole($"ATTEMPT TO ADD MENU ITEM");

            string description = $"Gender: {(storedBody.PedExt.Pedestrian.IsMale ? "M" : "F")}~n~Status:{(storedBody.PedExt.IsUnconscious ? "~p~Unconscious~s~" : storedBody.PedExt.IsDead ? "~r~Dead~s~" : "Unknown")}";

            UIMenuItem personMenu = new UIMenuItem(storedBody.PedExt.Name,  storedBody.PedExt.GroupName) { RightLabel = "~g~" + FinalPrice(storedBody.PedExt.IsDead).ToString("C0") + "~s~" };
            BodySubMenu.AddItem(personMenu);

            StoredBodyLookup stb = new StoredBodyLookup(storedBody.PedExt, storedBody.VehicleExt, personMenu);

            StoredBodyLookups.Add(stb);

            personMenu.Activated += (sender, e) =>
            {
                ExportBody(stb);
            };

            Added = true;
        }
        if (!Added)
        {
            InteractionMenu.Visible = false;
            PlayErrorSound();
            DisplayMessage("~r~Export Failed", "Please come back with a valid body!");
        }
    }
    private void ExportBody(StoredBodyLookup storedBodyLookup)
    {
        if (storedBodyLookup != null && storedBodyLookup.Body != null && storedBodyLookup.Body.Pedestrian.Exists())
        {
            Game.FadeScreenOut(1000, true);
            storedBodyLookup.Body.Pedestrian.Delete();
            BodySubMenu.MenuItems.Remove(storedBodyLookup.MenuItem);
            BodySubMenu.RefreshIndex();
            BodySubMenu.Close(true);
            Game.FadeScreenIn(1000, true);
            Player.BankAccounts.GiveMoney(FinalPrice(storedBodyLookup.Body.IsDead), false);
            PlaySuccessSound();
            DisplayMessage("~g~Exported", $"Thank you for exporting at ~y~{Name}~s~");
        }
        else
        {
            PlayErrorSound();
            DisplayMessage("~r~Exporting Failed", "We are unable to complete this export.");
        }
    }
    private bool IsValidForConsideration(VehicleExt toScrap)
    {
        if (toScrap.Vehicle.Exists() && 
            toScrap.Vehicle.DistanceTo2D(EntrancePosition) <= VehiclePickupDistance && 
             toScrap.VehicleBodyManager.StoredBodies.Any() && toScrap.HasBeenEnteredByPlayer)
        {
            return true;
        }
        return false;
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.BodyExports.Add(this);
        base.AddLocation(possibleLocations);
    }
    private class StoredBodyLookup
    {
        public StoredBodyLookup()
        {
        }

        public StoredBodyLookup(PedExt body, VehicleExt storedVehicle, UIMenuItem menuItem)
        {
            Body = body;
            StoredVehicle = storedVehicle;
            MenuItem = menuItem;
        }

        public PedExt Body { get; set; }
        public VehicleExt StoredVehicle { get; set; }
        public UIMenuItem MenuItem { get; set; }
    }

}
