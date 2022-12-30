using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

public class InteractableLocation : BasicLocation
{


    protected LocationCamera StoreCamera;
    protected ILocationInteractable Player;
    protected IModItems ModItems;
    protected IEntityProvideable World;
    protected ISettingsProvideable Settings;
    protected IWeapons Weapons;
    protected ITimeControllable Time;
    protected Transaction Transaction;



    //private IEntityProvideable World;
    private uint NotificationHandle;
    private readonly List<string> FallBackVendorModels = new List<string>() { "s_m_m_strvend_01", "s_m_m_linecook" };

    public string MenuID { get; set; }
    public Vector3 VendorPosition { get; set; } = Vector3.Zero;
    public float VendorHeading { get; set; } = 0f;
    public List<string> VendorModels { get; set; }
    public Vector3 CameraPosition { get; set; } = Vector3.Zero;
    public Vector3 CameraDirection { get; set; } = Vector3.Zero;
    public Rotator CameraRotation { get; set; }
    public bool CanInteractWhenWanted { get; set; } = false;
   // public virtual bool InteractsWithVendor { get; set; } = true;

    public bool IsAnyMenuVisible => MenuPool.IsAnyMenuOpen();
    public bool HasCustomCamera => CameraPosition != Vector3.Zero;
    [XmlIgnore]
    public virtual string ButtonPromptText { get; set; }
    [XmlIgnore]
    public Merchant Merchant { get; set; }
    [XmlIgnore]
    public bool HasVendor => VendorPosition != Vector3.Zero;
    [XmlIgnore]
    public ShopMenu Menu { get; set; }
    [XmlIgnore]
    public bool CanInteract { get; set; } = true;
    [XmlIgnore]
    public UIMenu InteractionMenu { get; private set; }
    [XmlIgnore]
    public MenuPool MenuPool { get; private set; }
    [XmlIgnore]
    public bool VendorAbandoned { get; set; } = false;

    public InteractableLocation(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        
    }
    public InteractableLocation() : base()
    {

    }
    public virtual void OnInteract(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;

        if(IsLocationClosed())
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
                    StoreCamera = new LocationCamera(this, Player);
                    StoreCamera.Setup();

                    CreateInteractionMenu();
                    Transaction = new Transaction(MenuPool, InteractionMenu, Menu, this);
                    Transaction.CreateTransactionMenu(Player, modItems, world, settings, weapons, time);

                    InteractionMenu.Visible = true;
                    InteractionMenu.OnItemSelect += (selnder, selectedItem, index) =>
                    {
                        if (selectedItem.Text == "Buy")
                        {
                            Transaction?.SellMenu?.Dispose();
                            Transaction?.PurchaseMenu?.Show();
                        }
                        else if (selectedItem.Text == "Sell")
                        {
                            Transaction?.PurchaseMenu?.Dispose();
                            Transaction?.SellMenu?.Show();
                        }
                    };
                    Transaction.ProcessTransactionMenu();

                    Transaction.DisposeTransactionMenu();
                    DisposeInteractionMenu();

                    StoreCamera.Dispose();
                    Player.IsTransacting = false;
                    Player.ActivityManager.IsInteractingWithLocation = false;
                    CanInteract = true;
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "BarInteract");
        }
    }
    public virtual void OnItemSold(ModItem modItem, MenuItem menuItem, int totalItems)
    {

    }
    public virtual bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Interact with {Name}";
        return true;
    }
    public virtual void OnItemPurchased(ModItem modItem, MenuItem menuItem, int totalItems)
    {

    }
    public void CreateInteractionMenu()
    {
        MenuPool = new MenuPool();
        InteractionMenu = new UIMenu(Name, Description);
        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            InteractionMenu.SetBannerType(BannerImage);
            Game.RawFrameRender += (s, e) => MenuPool.DrawBanners(e.Graphics);
        }
        //InteractionMenu.OnItemSelect += OnItemSelect;
        MenuPool.Add(InteractionMenu);
        CanInteract = false;
    }
    public void DisposeInteractionMenu()
    {
        Game.RawFrameRender -= (s, e) => MenuPool.DrawBanners(e.Graphics);
        if (InteractionMenu != null)
        {
            InteractionMenu.Visible = false;
        }
        CanInteract = true;
    }
    public void StoreData(IShopMenus shopMenus)
    {
        Menu = shopMenus.GetMenu(MenuID);
    }
    public void ProcessInteractionMenu()
    {
        while (IsAnyMenuVisible)
        {
            MenuPool.ProcessMenus();
            GameFiber.Yield();
        }
    }
    public override void Activate(IInteriors interiors, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, ITimeReportable time, IEntityProvideable world)
    {
        World = world;
        if (HasVendor)
        {
            if (1==1)//InteractsWithVendor)
            {
                CanInteract = false;
            }
            if (IsOpen(time.CurrentHour))
            {
                SpawnVendor(settings, crimes, weapons, true);// InteractsWithVendor);
            }
            GameFiber.Yield();
        }
        World.Pedestrians.AddEntity(Merchant);
        if (!World.Places.ActiveInteractableLocations.Contains(this))
        {
            World.Places.ActiveInteractableLocations.Add(this);
        }
        base.Activate(interiors, settings, crimes, weapons, time, World);
    }
    public override void Deactivate()
    {
        if (Merchant != null && Merchant.Pedestrian.Exists())
        {
            Merchant.Pedestrian.Delete();
        }
        if (World != null && World.Places != null && World.Places.ActiveInteractableLocations != null && World.Places.ActiveInteractableLocations.Contains(this))
        {
            World.Places.ActiveInteractableLocations.Remove(this);
        }
        base.Deactivate();
    }
    private void SpawnVendor(ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, bool addMenu)
    {
        Ped ped;
        string ModelName;
        if (VendorModels != null && VendorModels.Any())
        {
            ModelName = VendorModels.PickRandom();
        }
        else
        {
            ModelName = FallBackVendorModels.PickRandom();
        }


        NativeFunction.Natives.CLEAR_AREA(VendorPosition.X, VendorPosition.Y, VendorPosition.Z, 2f, true, false, false, false);

        Model modelToCreate = new Model(Game.GetHashKey(ModelName));
        modelToCreate.LoadAndWait();
        ped = NativeFunction.Natives.CREATE_PED<Ped>(26, Game.GetHashKey(ModelName), VendorPosition.X, VendorPosition.Y, VendorPosition.Z, VendorHeading, false, false);//ped = NativeFunction.Natives.CREATE_PED<Ped>(26, Game.GetHashKey(ModelName), VendorPosition.X, VendorPosition.Y, VendorPosition.Z + 1f, VendorHeading, false, false);
        GameFiber.Yield();
        if (ped.Exists())
        {
            ped.IsPersistent = true;//THIS IS ON FOR NOW!
            ped.RandomizeVariation();

            //

            NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", ped, "WORLD_HUMAN_STAND_IMPATIENT", 0, true);
            //ped.Tasks.StandStill(-1);
            ped.KeepTasks = true;
            EntryPoint.SpawnedEntities.Add(ped);
            GameFiber.Yield();
            if (ped.Exists())
            {
                Merchant = new Merchant(ped, settings, false, true, false, "Vendor", crimes, weapons, World, false);
                if (addMenu)
                {
                    //Merchant.ShopMenu = Menu;
                    Merchant.SetupTransactionItems(Menu);
                }
                Merchant.AssociatedStore = this;
                Merchant.SpawnPosition = VendorPosition;
                EntryPoint.WriteToConsole($"MERCHANT SPAWNED? Menu: {Menu == null} HANDLE {ped.Handle}");


                //if (1 == 1)//PlacePedOnGround)
                //{
                //    float resultArg = ped.Position.Z;
                //    NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD(ped.Position.X, ped.Position.Y, ped.Position.Z, out resultArg, false);
                //    ped.Position = new Vector3(ped.Position.X, ped.Position.Y, resultArg);
                //}


            }
        }
    }
    protected bool IsLocationClosed()
    {
        if(IsTemporarilyClosed)
        {
            Game.RemoveNotification(NotificationHandle);
            NotificationHandle = Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Closed", $"We're sorry, this location is ~r~Temporarily Closed~s~.");
            return true;
        }
        if (!IsOpen(Time.CurrentHour))
        {
            Game.RemoveNotification(NotificationHandle);
            NotificationHandle = Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Closed", $"We're sorry, this location is now closed.~n~Hours: {OpenTime} to {CloseTime}");
            return true;
        }
        return false;
    }
}

