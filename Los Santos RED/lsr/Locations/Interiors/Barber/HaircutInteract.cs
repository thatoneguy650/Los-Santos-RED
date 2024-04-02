using ExtensionsMethods;
using Rage;
using Rage.Native;
using RAGENativeUI;
using System;
using System.Linq;

public class HaircutInteract : InteriorInteract
{
    private Texture BannerImage;
    private PedExt Hairstylist;

    //private Rage.Object rightHandCashBundle;
    //private Rage.Object leftHandCashBundle;
    //private string drawerStealPromptText = "Steal from Drawer";
    //private string drawerStealEmptyText = "Drawer Empty";
    //public int TotalCash { get; set; }

    public Vector3 AnimEnterPosition { get; set; }
    public Vector3 AnimEnterRotation { get; set; }
    public override bool ShouldAddPrompt => (BarberShop == null || !BarberShop.SpawnedVendors.Any()) ? false : base.ShouldAddPrompt;
    public BarberShop BarberShop { get; set; }
    public HaircutInteract()
    {

    }

    public HaircutInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {

    }
    public override void OnInteriorLoaded()
    {

    }
    public override void OnInteract()
    {
        if (BarberShop == null)
        {
            return;
        }
        Interior.IsMenuInteracting = true;
        Interior?.RemoveButtonPrompts();
        RemovePrompt();
        Hairstylist = null;
        if(BarberShop == null || BarberShop.SpawnedVendors == null || !BarberShop.SpawnedVendors.Any()) 
        {
            Interior.IsMenuInteracting = false;
            Interior?.RemoveButtonPrompts();
            RemovePrompt();
            LocationCamera?.StopImmediately(true);
            return;
        }
        Hairstylist = BarberShop.SpawnedVendors.PickRandom();
        if(Hairstylist == null || !Hairstylist.Pedestrian.Exists())
        {
            Interior.IsMenuInteracting = false;
            Interior?.RemoveButtonPrompts();
            RemovePrompt();
            LocationCamera?.StopImmediately(true);
            return;
        }
        SetupCamera(false);
        PerformAnimation();
        Interior.IsMenuInteracting = false;
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        LocationCamera?.ReturnToGameplay(true);
        LocationCamera?.StopImmediately(true);

    }
    public override void AddPrompt()
    {
        if (Player == null)
        {
            return;
        }
        Player.ButtonPrompts.AttemptAddPrompt(Name, ButtonPromptText, Name, Settings.SettingsManager.KeySettings.InteractStart, 999);
    }
    public bool StopPerformingAnimation()
    {
        return true;
    }
    private bool PerformAnimation()
    {
        Player.ActivityManager.StopDynamicActivity();
        AnimationDictionary.RequestAnimationDictionay("misshair_shop@hair_dressers");
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", 0, "misshair_shop@hair_dressers", "player_enterchair", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 1000f, -1000f, -1, 5642, 0.0f, 2, 1);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }

        if (Hairstylist != null && Hairstylist.Pedestrian.Exists())
        {
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Hairstylist.Pedestrian, "misshair_shop@hair_dressers", "keeper_enterchair", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 1000f, -1000f, -1, 5642, 0.0f, 2, 1);
        }


        bool IsCancelled = false;
        while (Player.ActivityManager.CanPerformActivitiesExtended)
        {
            Player.WeaponEquipment.SetUnarmed();
            float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, "misshair_shop@hair_dressers", "player_enterchair");
            if (Player.IsMoveControlPressed || !Player.Character.IsAlive)
            {
                IsCancelled = true;
                break;
            }
            if (AnimationTime >= 1.0f)
            {
                break;
            }
            GameFiber.Yield();
        }
        if (!IsCancelled)
        {
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Game.LocalPlayer.Character, "misshair_shop@hair_dressers", "player_base", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 1000f, -1000f, -1, 5641, 0.0f, 2, 1);
            if (Hairstylist != null && Hairstylist.Pedestrian.Exists())
            {
                NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Hairstylist.Pedestrian, "misshair_shop@hair_dressers", "keeper_base", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 1000f, -1000f, -1, 5641, 0.0f, 2, 1);
            }
            ShowHaircutMenu();
        }
        NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Game.LocalPlayer.Character, "misshair_shop@hair_dressers", "player_exitchair", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 1000f, -1000f, -1, 5642, 0.0f, 2, 1);
        if (Hairstylist != null && Hairstylist.Pedestrian.Exists())
        {
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Hairstylist.Pedestrian, "misshair_shop@hair_dressers", "keeper_exitchair", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 1000f, -1000f, -1, 5642, 0.0f, 2, 1);
        }
        //LocationCamera?.ReturnToGameplay(true);
        while (Player.ActivityManager.CanPerformActivitiesExtended)
        {
            Player.WeaponEquipment.SetUnarmed();
            float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, "misshair_shop@hair_dressers", "player_exitchair");
            if (!Player.Character.IsAlive)
            {
                IsCancelled = true;
                break;
            }
            if (AnimationTime >= 0.7f)
            {
                break;
            }
            GameFiber.Yield();
        }
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        if (Hairstylist != null && Hairstylist.Pedestrian.Exists())
        {
            NativeFunction.Natives.CLEAR_PED_TASKS(Hairstylist.Pedestrian);
        }
        if (IsCancelled)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void ShowHaircutMenu()
    {
        MenuPool MenuPool = new MenuPool();
        UIMenu InteractionMenu = new UIMenu(BarberShop.Name, BarberShop.Description);
        if (BarberShop.HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BarberShop.BannerImagePath}");
            InteractionMenu.SetBannerType(BannerImage);
            Game.RawFrameRender += (s, e) => MenuPool.DrawBanners(e.Graphics);
            //RemoveBanner = false;
            EntryPoint.WriteToConsole($"SET BANNER IMAGE FOR HAIRCUT MENU!");
        }
        MenuPool.Add(InteractionMenu);
        ChangeHaircutProcess changeHaircutProcess = new ChangeHaircutProcess(LocationInteractable,BarberShop, Hairstylist, Settings);
        changeHaircutProcess.SetAnimPositions(AnimEnterPosition, AnimEnterRotation);
        changeHaircutProcess.Start(MenuPool, InteractionMenu);
        while (MenuPool.IsAnyMenuOpen() && Player.ActivityManager.CanPerformActivitiesExtended)
        {
            Player.WeaponEquipment.SetUnarmed();
            MenuPool.ProcessMenus();
            GameFiber.Yield();
        }
        changeHaircutProcess.Dispose();
    }
}

