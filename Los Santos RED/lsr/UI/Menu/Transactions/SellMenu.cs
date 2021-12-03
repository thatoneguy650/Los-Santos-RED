using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

public class SellMenu : Menu
{
    private UIMenu sellMenu;
    private IModItems ModItems;
    private GameLocation Store;
    private MenuPool MenuPool;
    private IInteractionable Player;
    private int ItemsSold;
    private Vehicle SellingVehicle;
    private Rage.Object SellingProp;
    private Ped SellingPed;
    private Camera StoreCam;
    private bool ShouldPreviewItem;
    private PedExt Ped;
    private bool IsActivelyConversing;
    public bool Visible => sellMenu.Visible;
    public bool SoldItem => ItemsSold > 0;
    private bool CanContinueConversation => Ped != null && Ped.Pedestrian.Exists() && Player.Character.DistanceTo2D(Ped.Pedestrian) <= 6f && Ped.CanConverse && Player.CanConverse;
    public SellMenu(MenuPool menuPool, UIMenu parentMenu, PedExt ped, GameLocation store, IModItems modItems, IInteractionable player, Camera storeCamera, bool shouldPreviewItem)
    {
        Ped = ped;
        ModItems = modItems;
        Store = store;
        Player = player;
        StoreCam = storeCamera;
        ShouldPreviewItem = shouldPreviewItem;
        sellMenu = menuPool.AddSubMenu(parentMenu, "Sell");
        sellMenu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
        if (Store.BannerImage != "")
        {
            sellMenu.SetBannerType(Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{Store.BannerImage}"));
            Game.RawFrameRender += (s, e) => menuPool.DrawBanners(e.Graphics);
        }
        if (Store.Name == "")
        {
            sellMenu.RemoveBanner();
        }
        sellMenu.OnIndexChange += OnIndexChange;
        sellMenu.OnItemSelect += OnItemSelect;
        sellMenu.OnListChange += OnListChange;
    }
    public void Setup()
    {
        PreloadModels();
        if (Ped != null)
        {
            AnimationDictionary.RequestAnimationDictionay("mp_safehousevagos@");
            AnimationDictionary.RequestAnimationDictionay("mp_common");
        }
    }
    public void Dispose()
    {
        Hide();
        Game.RawFrameRender -= (s, e) => MenuPool.DrawBanners(e.Graphics);
    }
    public void Update()
    {
        if (sellMenu.Visible)
        {
            if (SellingProp.Exists())
            {
                SellingProp.SetRotationYaw(SellingProp.Rotation.Yaw + 1f);
            }
            if (SellingVehicle.Exists())
            {
                SellingVehicle.SetRotationYaw(SellingVehicle.Rotation.Yaw + 1f);
            }
        }
        else
        {
            ClearPreviews();
        }
    }
    public override void Hide()
    {
        ClearPreviews();
        sellMenu.Visible = false;
        Player.ButtonPrompts.Clear();
    }
    public override void Show()
    {
        CreatePurchaseMenu();
        sellMenu.Visible = true;
    }
    public override void Toggle()
    {
        if (!sellMenu.Visible)
        {
            CreatePurchaseMenu();
            sellMenu.Visible = true;
        }
        else
        {
            Hide();
            sellMenu.Visible = false;
        }
    }
    private void CreatePurchaseMenu()
    {
        sellMenu.Clear();
        foreach (MenuItem cii in Store.Menu)
        {
            if (cii != null && cii.Sellable)
            {
                ModItem myItem = ModItems.Get(cii.ModItemName);
                if (myItem != null)
                {
                    if (myItem.ModelItem?.Type == ePhysicalItemType.Vehicle)
                    {
                        sellMenu.AddItem(new UIMenuItem(cii.ModItemName, $"{cii.ModItemName} ${cii.SalesPrice}"));
                    }
                    else
                    {
                        bool enabled = Player.HasItemInInventory(cii.ModItemName);
                        sellMenu.AddItem(new UIMenuItem(cii.ModItemName, $"{cii.ModItemName} ${cii.SalesPrice}") { Enabled = enabled });
                    }
                }
            }
        }
        OnIndexChange(sellMenu, sellMenu.CurrentSelection);
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        ModItem ToAdd = ModItems.Items.Where(x => x.Name == selectedItem.Text).FirstOrDefault();
        MenuItem menuItem = Store.Menu.Where(x => x.ModItemName == selectedItem.Text).FirstOrDefault();
        bool ExitAfterPurchase = false;
        if (ToAdd != null && menuItem != null)
        {
            if (Ped != null && Ped.Pedestrian.Exists())
            {
                StartSellAnimation(ToAdd);
            }
            else
            {
                Hide();
            }
            if (ToAdd.CanConsume)
            {
                if (Player.RemoveFromInventory(ToAdd, 1))
                {
                    Player.GiveMoney(menuItem.SalesPrice);
                    ItemsSold++;
                    EntryPoint.WriteToConsole($"REMOVED {ToAdd.Name} {ToAdd.GetType()}  Amount: {1}", 5);
                }
            }
        }
        GameFiber.Sleep(500);
        while (Player.IsPerformingActivity)
        {
            GameFiber.Sleep(500);
        }
        if (ExitAfterPurchase)
        {
            Dispose();
        }
        else
        {
            Show();
        }
    }
    private void OnIndexChange(UIMenu sender, int newIndex)
    {
        if(newIndex != -1)
        {
            CreatePreview(sender.MenuItems[newIndex]);
        }
    }
    private void OnListChange(UIMenu sender, UIMenuListItem listItem, int newIndex)
    {

    }
    private void CreatePreview(UIMenuItem myItem)
    {
        ClearPreviews();
        if (myItem != null)
        {
            EntryPoint.WriteToConsole($"SIMPLE TRANSACTION OnIndexChange Text: {myItem.Text}", 5);
            ModItem itemToShow = ModItems.Items.Where(x => x.Name == myItem.Text).FirstOrDefault();
            if (itemToShow != null && ShouldPreviewItem)
            {
                if (itemToShow.PackageItem?.Type == ePhysicalItemType.Prop || itemToShow.ModelItem?.Type == ePhysicalItemType.Prop)
                {
                    PreviewProp(itemToShow);
                }
                else if (itemToShow.ModelItem?.Type == ePhysicalItemType.Vehicle)
                {
                    PreviewVehicle(itemToShow);
                }
                else if (itemToShow.ModelItem?.Type == ePhysicalItemType.Ped)
                {
                    PreviewPed(itemToShow);
                }
            }
        }
    }
    private void PreviewPed(ModItem itemToShow)
    {
        if (itemToShow != null && itemToShow.ModelItem != null)
        {
            SellingVehicle = new Vehicle(itemToShow.ModelItem.ModelName, Store.ItemPreviewPosition, Store.ItemPreviewHeading);
        }
        GameFiber.Yield();
    }
    private void PreviewVehicle(ModItem itemToShow)
    {
        if (itemToShow != null && itemToShow.ModelItem != null)
        {
            SellingVehicle = new Vehicle(itemToShow.ModelItem.ModelName, Store.ItemPreviewPosition, Store.ItemPreviewHeading);
        }
        GameFiber.Yield();
        if (SellingVehicle.Exists())
        {

            //SellingVehicle.PrimaryColor = CurrentSelectedColor;
            //CurrentDisplayColor = CurrentSelectedColor;
            NativeFunction.Natives.SET_VEHICLE_ON_GROUND_PROPERLY<bool>(SellingVehicle, 5.0f);
        }
    }
    private void PreviewProp(ModItem itemToShow)
    {
        string ModelToSpawn = "";
        bool useClose = true;
        if (itemToShow.PackageItem != null)
        {
            ModelToSpawn = itemToShow.PackageItem.ModelName;
            useClose = !itemToShow.PackageItem.IsLarge;
        }
        if (ModelToSpawn == "")
        {
            ModelToSpawn = itemToShow.ModelItem.ModelName;
            useClose = !itemToShow.ModelItem.IsLarge;
        }
        if (ModelToSpawn != "")
        {
            if (useClose)
            {
                SellingProp = new Rage.Object(ModelToSpawn, StoreCam.Position + StoreCam.Direction);
            }
            else
            {
                SellingProp = new Rage.Object(ModelToSpawn, StoreCam.Position + (StoreCam.Direction.ToNormalized() * 3f));
            }
            if (SellingProp.Exists())
            {
                SellingProp.SetRotationYaw(SellingProp.Rotation.Yaw + 45f);
                if (SellingProp != null && SellingProp.Exists())
                {
                    NativeFunction.Natives.SET_ENTITY_HAS_GRAVITY(SellingProp, false);
                }
            }
            EntryPoint.WriteToConsole("SIMPLE TRANSACTION: PREVIEW ITEM RAN", 5);
        }
        else
        {
            if (SellingProp.Exists())
            {
                SellingProp.Delete();
            }
        }
    }
    private void ClearPreviews()
    {
        if (SellingProp.Exists())
        {
            SellingProp.Delete();
        }
        if (SellingVehicle.Exists())
        {
            SellingVehicle.Delete();
        }
        if (SellingPed.Exists())
        {
            SellingPed.Delete();
        }
    }
    private void PreloadModels()
    {
        foreach (MenuItem menuItem in Store.Menu)//preload all item models so it doesnt bog the menu down
        {
            if (menuItem.Sellable)
            {
                ModItem myItem = ModItems.Items.Where(x => x.Name == menuItem.ModItemName).FirstOrDefault();
                if (myItem != null)
                {
                    if (myItem.PackageItem != null && myItem.PackageItem.ModelName != "")
                    {
                        new Model(myItem.PackageItem.ModelName).LoadAndWait();
                    }
                    else if (myItem.ModelItem != null && myItem.ModelItem.ModelName != "")
                    {
                        new Model(myItem.ModelItem.ModelName).LoadAndWait();
                    }
                }
            }
        }
    }
    private bool PurchaseVehicle(ModItem modItem)
    {
        //bool ItemInDeliveryBay = Rage.World.GetEntities(Store.ItemDeliveryPosition, 10f, GetEntitiesFlags.ConsiderAllVehicles).Any();
        //if (!ItemInDeliveryBay)
        //{
        //    Vehicle NewVehicle = new Vehicle(modItem.ModelItem.ModelName, Store.ItemDeliveryPosition, Store.ItemDeliveryHeading);
        //    if (NewVehicle.Exists())
        //    {
        //        VehicleExt MyNewCar = new VehicleExt(NewVehicle, Settings);
        //        World.AddEntity(MyNewCar, ResponseType.None);
        //        Player.TakeOwnershipOfVehicle(MyNewCar);
        //        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Store.Name, "Purchase", "Thank you for your purchase");
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //else
        //{
        //    return false;
        //}
        return false;

    }
    private void StartSellAnimation(ModItem item)
    {
        Hide();
        IsActivelyConversing = true;
        Player.ButtonPrompts.Clear();
        SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_BUY", "GENERIC_YES", "BLOCKED_GENEIRC" }, true);
        if (Ped.Pedestrian.Exists())
        {
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "mp_common", "givetake1_b", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "mp_common", "givetake1_a", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
        }
        GameFiber.Sleep(500);
        string modelName = "";
        bool HasProp = false;
        if (item.PackageItem != null && item.PackageItem.ModelName != "")
        {
            modelName = item.PackageItem.ModelName;
            HasProp = true;
        }
        else if (item.ModelItem != null && item.ModelItem.ModelName != "")
        {
            modelName = item.ModelItem.ModelName;
            HasProp = true;
        }
        if (Ped.Pedestrian.Exists() && HasProp && modelName != "")
        {
            SellingProp = new Rage.Object(modelName, Player.Character.GetOffsetPositionUp(50f));
            GameFiber.Yield();
            if (SellingProp.Exists())
            {
                SellingProp.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, item.ModelItem.AttachBoneIndex), item.ModelItem.AttachOffset, item.ModelItem.AttachRotation);
            }
        }
        GameFiber.Sleep(500);
        if (Ped.Pedestrian.Exists())
        {
            if (SellingProp.Exists())
            {
                SellingProp.AttachTo(Ped.Pedestrian, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Ped.Pedestrian, item.ModelItem.AttachBoneIndex), item.ModelItem.AttachOffset, item.ModelItem.AttachRotation);
            }
        }
        GameFiber.Sleep(1000);
        if (Ped.Pedestrian.Exists())
        {
            if (SellingProp.Exists())
            {
                SellingProp.Delete();
            }
            SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_THANKS", "GENERIC_BYE" }, true);
            SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_BYE", "GENERIC_THANKS", "PED_RANT" }, true);
        }
        IsActivelyConversing = false;
        //Show();     
    }
    private bool SayAvailableAmbient(Ped ToSpeak, List<string> Possibilities, bool WaitForComplete)
    {
        bool Spoke = false;
        if (CanContinueConversation)
        {
            foreach (string AmbientSpeech in Possibilities)
            {
                ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
                GameFiber.Sleep(100);
                if (ToSpeak.Exists() && ToSpeak.IsAnySpeechPlaying)
                {
                    Spoke = true;
                }
                //EntryPoint.WriteToConsole($"SAYAMBIENTSPEECH: {ToSpeak.Handle} Attempting {AmbientSpeech}, Result: {Spoke}");
                if (Spoke)
                {
                    break;
                }
            }
            GameFiber.Sleep(100);
            while (ToSpeak.Exists() && ToSpeak.IsAnySpeechPlaying && WaitForComplete && CanContinueConversation)
            {
                Spoke = true;
                GameFiber.Yield();
            }
            if (!Spoke)
            {
                // Game.DisplayNotification($"\"{Possibilities.FirstOrDefault()}\"");
            }
        }
        return Spoke;
    }

}