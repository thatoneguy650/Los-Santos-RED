using Rage;
using Rage.Native;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TryOnInteract : InteriorInteract
{
    private Texture BannerImage;
    private OrbitCamera OrbitCamera;
    private MenuPool MenuPool;

    public Vector3 AnimEnterPosition { get; set; }
    public Vector3 AnimEnterRotation { get; set; }
    public override bool ShouldAddPrompt => (ClothingShop == null || !ClothingShop.SpawnedVendors.Any()) ? false : base.ShouldAddPrompt;
    public ClothingShop ClothingShop { get; set; }
    public TryOnInteract()
    {

    }
    public TryOnInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {

    }
    public override void OnInteriorLoaded()
    {

    }
    public override void OnInteract()
    {
        if (ClothingShop == null)
        {
            return;
        }
        if (Interior != null)
        {
            Interior.IsMenuInteracting = true;
        }
        Player.IsTransacting = true;
        Interior?.RemoveButtonPrompts();
        RemovePrompt();


        SetPlayerAtPosition();

        MenuPool = new MenuPool();
        SetupOrbitCamera();

        ShowClothingMenu();






        OrbitCamera.Dispose();


        if (Interior != null)
        {
            Interior.IsMenuInteracting = false;
        }
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        Player.IsTransacting = false;
    }

    private void SetPlayerAtPosition()
    {
        Player.Character.Position = Position; 
        Player.Character.Heading = Heading;
    }

    private void SetupOrbitCamera()
    {
        OrbitCamera = new OrbitCamera(LocationInteractable, Player.Character, null, Settings, MenuPool);
        OrbitCamera.HandleUpdates = false;
        OrbitCamera.Radius = 1.25f;
        OrbitCamera.MinRadius = 0.25f;
        OrbitCamera.MaxRadius = 1.75f;
        OrbitCamera.InitialVerticalOffset = 90f;
        OrbitCamera.InitialHorizonatlOffset = 90f;
        OrbitCamera.IsSensitive = true;
        OrbitCamera.InitialVerticalPositionOffset = 0.5f;
        OrbitCamera.Setup();
    }
    private void ShowClothingMenu()
    {   
        UIMenu InteractionMenu = new UIMenu(ClothingShop.Name, ClothingShop.Description);
        if (ClothingShop.HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{ClothingShop.BannerImagePath}");
            InteractionMenu.SetBannerType(BannerImage);
            Game.RawFrameRender += (s, e) => MenuPool.DrawBanners(e.Graphics);
            //RemoveBanner = false;
            EntryPoint.WriteToConsole($"SET BANNER IMAGE FOR HAIRCUT MENU!");
        }
        MenuPool.Add(InteractionMenu);
        ClothingPurchaseMenu clothingPurchaseMenuProcess = new ClothingPurchaseMenu(LocationInteractable, ClothingShop, this, Settings);
        clothingPurchaseMenuProcess.Start(MenuPool, InteractionMenu, OrbitCamera, ClothingShop.PedClothingShopMenu.PedClothingShopMenuItems.Where(x => x.ModelNames.Contains(Player.ModelName.ToLower())).ToList(),true, true);

        while (MenuPool.IsAnyMenuOpen() && Player.ActivityManager.CanPerformActivitiesExtended)
        {
            Player.WeaponEquipment.SetUnarmed();
            if (OrbitCamera.IsInputPressed)
            {
                MenuPool.Draw();
            }
            else
            {
                MenuPool.ProcessMenus();
            }
            Player.IsSetDisabledControls = true;
            GameFiber.Yield();
        }
        Player.IsSetDisabledControls = false;
        clothingPurchaseMenuProcess.Dispose();
    }

    public override void AddPrompt()
    {
        if (Player == null)
        {
            return;
        }
        Player.ButtonPrompts.AttemptAddPrompt(Name, ButtonPromptText, Name, Settings.SettingsManager.KeySettings.InteractStart, 999);
    }
    public override void AddDistanceOffset(Vector3 offsetToAdd)
    {
        AnimEnterPosition += offsetToAdd;
        base.AddDistanceOffset(offsetToAdd);
    }



}

