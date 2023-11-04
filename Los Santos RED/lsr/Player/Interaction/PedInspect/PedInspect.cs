using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

public class PedInspect : DynamicActivity
{
    protected string FailMessage;
    protected string EnterAnimationDictionary = "";
    protected string EnterAnimation = "";
    protected string ExitAnimationDictionary = "";
    protected string ExitAnimation = "";
    protected string BaseAnimationDictionary = "";
    protected string BaseAnimation = "";
    protected string IdleAnimationDictionary = "";
    protected List<string> IdleAnimationList = new List<string>();

    private uint GameTimeStartedConversing;
    private bool IsActivelyConversing;
    private bool IsTasked;
    private bool IsBlockedEvents;
    private PedExt Ped;
    private IInteractionable Player;
    private ICellphones Cellphones;
    private bool CancelledConversation;
    private ISettingsProvideable Settings;
    private ICrimes Crimes;
    private uint pedHeadshotHandle;
    private IModItems ModItems;
    private bool IsCancelled;
    private WeaponInformation LastWeapon;
    private bool WasSetUnArmed;
    private MenuPool MenuPool;
    private UIMenu PedInspectMenu;
    private UIMenuItem LootPedMenuItem;
    private UIMenuItem RevivePedMenuItem;
    private bool HasBeenCancelled = false;
    private UIMenuItem InspectPedMenuItem;
    private UIMenuItem KillPedMenuItem;

    public PedInspect(IInteractionable player, PedExt ped, ISettingsProvideable settings, ICrimes crimes, IModItems modItems, ICellphones cellphones)
    {
        Player = player;
        Ped = ped;
        Settings = settings;
        Crimes = crimes;
        ModItems = modItems;
        Cellphones = cellphones;
        FailMessage = "Cannot Inspect Ped";
    }
    public override string DebugString => $"";
    public override ModItem ModItem { get; set; }
    public override bool CanPause { get; set; } = false;
    public override bool CanCancel { get; set; } = false;
    public override bool IsUpperBodyOnly { get; set; } = false;
    public override string PausePrompt { get; set; } = "Pause Activity";
    public override string CancelPrompt { get; set; } = "Stop Activity";
    public override string ContinuePrompt { get; set; } = "Continue Activity";
    public override void Continue()
    {

    }
    public override bool CanPerform(IActionable player)
    {
        if (!player.ActivityManager.CanInspectLookedAtPed)
        {
            Game.DisplayHelp(FailMessage);
            return false;
        }
        if (player.IsOnFoot && player.ActivityManager.CanPerformActivitesBase)
        {
            return true;
        }
        Game.DisplayHelp(FailMessage);
        return false;
    }
    public override void Cancel()
    {
        if (HasBeenCancelled)
        {
            return;
        }
        HasBeenCancelled = true;
        ResetPed();
        ResetPlayer();
        ResetVariables();
    }
    public override void Pause()
    {
        Cancel();
    }
    public override bool IsPaused() => false;
    public override void Start()
    {
        if(!Ped.Pedestrian.Exists())
        {
            return;
        }
        //EntryPoint.WriteToConsole($"Looting Started Money: {Ped.Money} Dead: {Ped.IsDead} Unconsc: {Ped.IsUnconscious}");
        GameFiber.StartNew(delegate
        {
            try
            {
                SetupVariables();
                SetupPed();
                SetupPlayer();
                if (!IsCancelled)
                {
                    InspectPed();
                }
                Cancel();
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "PedInspect");   
    }
    private void InspectPed()
    {
        if(!MoveToBody())
        {
            return;
        }
        StoreAndUnarmPlayer();
        if(!PlayAnimation(EnterAnimationDictionary, EnterAnimation))
        {
            return;
        }
        CreateMenu();
        DisplayInspectMenu();
        if(!IsCancelled)
        {
            PlayAnimation(ExitAnimationDictionary, ExitAnimation);
        }   
    }

    private void DisplayInspectMenu()
    {
        UpdateMenuItems();
        PedInspectMenu.Visible = true;
        MenuUpdate();
    }
    private void MenuUpdate()
    {
        while (EntryPoint.ModController.IsRunning && MenuPool.IsAnyMenuOpen() && !IsCancelled)
        {
            MenuPool.ProcessMenus();
            GameFiber.Yield();
        }
    }

    private void CreateMenu()
    {
        MenuPool = new MenuPool();
        PedInspectMenu = new UIMenu("Inspect", "Select an Option");
        PedInspectMenu.RemoveBanner();
        MenuPool.Add(PedInspectMenu);
        AddGeneralItems();
        AddEMTItems();
    }

    private void AddGeneralItems()
    {
        InspectPedMenuItem = new UIMenuItem("Inspect Ped", "Get info about the current ped.");
        InspectPedMenuItem.Activated += (menu, item) =>
        {
            Ped.ShowInfoDisplay(pedHeadshotHandle);
        };
        PedInspectMenu.AddItem(InspectPedMenuItem);
        string LootHeader = "Loot";
        string lootDescription = "Loot the current ped. Make sure there are no prying eyes around.";
        if (Player.IsEMT)
        {
            LootHeader = "Search";
            lootDescription = "Search the current ped and store all items free from suspicion. You are just helping them out right?";
        }
        else if (Player.IsCop)
        {
            LootHeader = "Search";
            lootDescription = "Search the ped and collect all evidence free from suspicion. You will be sure to turn in all those illegal items right?";
        }
        LootPedMenuItem = new UIMenuItem(LootHeader, lootDescription);
        LootPedMenuItem.Activated += (menu, item) =>
        {
            PedInspectMenu.Visible = false;
            Player.ActivityManager.IsLootingBody = true;
            if (PlayAnimation(IdleAnimationDictionary, IdleAnimationList.PickRandom()))
            {
                LootPed();
                DisplayInspectMenu();
            }
            else
            {
                IsCancelled = true;
            }
            Player.ActivityManager.IsLootingBody = false;
        };
        PedInspectMenu.AddItem(LootPedMenuItem);
    }
    private void AddEMTItems()
    {
        RevivePedMenuItem = new UIMenuItem("Treat Ped", "Attempt to revive the current ped, not always successful. Less often when you've been drinking.");
        RevivePedMenuItem.Activated += (menu, item) =>
        {
            PedInspectMenu.Visible = false;
            if (PlayAnimation(IdleAnimationDictionary, IdleAnimationList.PickRandom()))
            {
                AttemptRevivePed();
                if (!IsCancelled)
                {
                    DisplayInspectMenu();
                }
            }
            else
            {
                IsCancelled = true;
            }
        };
        KillPedMenuItem = new UIMenuItem("Kill Ped", "Make sure you fail in your treatment of the current ped. Who would suspect the helpful EMT?");
        KillPedMenuItem.Activated += (menu, item) =>
        {
            PedInspectMenu.Visible = false;
            if (PlayAnimation(IdleAnimationDictionary, IdleAnimationList.PickRandom()))
            {
                KillPed();
                if (!IsCancelled)
                {
                    DisplayInspectMenu();
                }
            }
            else
            {
                IsCancelled = true;
            }
        };
        if (Player.IsEMT)
        {
            PedInspectMenu.AddItem(RevivePedMenuItem);
            PedInspectMenu.AddItem(KillPedMenuItem);
        }
    }

    private void UpdateMenuItems()
    {
        LootPedMenuItem.Enabled = Ped.CanBeLooted && !Ped.HasBeenLooted;
        KillPedMenuItem.Enabled = !Ped.IsDead;
        RevivePedMenuItem.Enabled = Ped.IsUnconscious && !Ped.IsDead;
    }
    private void AttemptRevivePed()
    {
        if (Ped.OnTreatedByEMT(Settings.SettingsManager.EMSSettings.RevivePercentage))//true if died, w/.e
        {
            Player.PlaySpeech("GENERIC_SHOCKED_HIGH", false);
        }
        if(!Ped.IsDead && !Ped.IsUnconscious)
        {
            IsCancelled = true;
        }
    }
    private void KillPed()
    {
        if (Ped.OnTreatedByEMT(0))//true if died, w/.e
        {
            Player.PlaySpeech("GENERIC_SHOCKED_HIGH", false);
        }
    }
    private void LootPed()
    {
        bool hasAddedItem = false;
        bool hasAddedCash = false;
        string ItemsFound = "";
        int CashAdded = 0;
        if (Ped.Pedestrian.Exists())
        {
            if (RandomItems.RandomPercent(Settings.SettingsManager.PlayerOtherSettings.PercentageToGetRandomItems))
            {
                Ped.PedInventory.AddRandomItems(ModItems, false, true);
            }
            ItemsFound = Ped.LootInventory(Player, ModItems, Cellphones);



            hasAddedItem = ItemsFound != "";
            if (Ped.Money > 0)//dead peds already drop it, truned off dropping for now
            {
                Player.BankAccounts.GiveMoney(Ped.Money, false);
                CashAdded = Ped.Money;
                Ped.Money = 0;
                if (Ped.Pedestrian.Exists())
                {
                    Ped.Pedestrian.Money = 0;
                }
                hasAddedCash = true;
            }
        }
        string Description = "";
        if (hasAddedCash)
        {
            Description += $"Cash Stolen: ~n~~g~${CashAdded}~s~";
        }
        if (hasAddedItem)
        {
            if (hasAddedCash)
            {
                Description += $"~n~Items Stolen:";
                Description += ItemsFound;
            }
            else
            {
                Description += $"Items Stolen:";
                Description += ItemsFound;
            }
        }
        if (!hasAddedCash && !hasAddedItem)
        {
            Description = "Nothing Found";
        }
        Ped.ShowCustomDisplay(pedHeadshotHandle, "~r~Ped Searched", Description);
        EntryPoint.WriteToConsole($"LOOTING:{Ped.Name} {Description}");
    }
    private bool PlayAnimation(string dictionary, string animation)
    {
        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, dictionary, animation, 8.0f, -8.0f, -1, 2, 0, false, false, false);
        uint GameTimeStartedLootAnimation = Game.GameTime;
        float AnimationTime = 0.0f;
        while (AnimationTime < 1.0f && !IsCancelled && Game.GameTime - GameTimeStartedLootAnimation <= 10000)
        {
            AnimationTime = NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Player.Character, dictionary, animation);
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            if (!Ped.Pedestrian.Exists() || !Player.IsAliveAndFree)
            {
                IsCancelled = true;
                break;
            }
            GameFiber.Yield();
        }
        if (!IsCancelled && AnimationTime >= 1.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool MoveToBody()
    {
        pedHeadshotHandle = NativeFunction.Natives.RegisterPedheadshot<uint>(Ped.Pedestrian);
        Vector3 DesiredPosition = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Ped.Pedestrian, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Ped.Pedestrian, 0));
        float DesiredHeading = Game.LocalPlayer.Character.Heading;
        NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", Player.Character, Ped.Pedestrian, -1, 1.75f, 0.75f, 1073741824, 1); //Original and works ok
        uint GameTimeStartedMovingToBody = Game.GameTime;
        float heading = Game.LocalPlayer.Character.Heading;
        bool IsFacingDirection = true;
        bool IsCloseEnough = false;
        while (Game.GameTime - GameTimeStartedMovingToBody <= 5000 && !IsCloseEnough && !IsCancelled)
        {
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            if (!Ped.Pedestrian.Exists() || !Player.IsAliveAndFree)
            {
                IsCancelled = true;
                break;
            }
            IsCloseEnough = Game.LocalPlayer.Character.DistanceTo2D(Ped.Pedestrian) <= 1.85f;
            // Rage.Debug.DrawArrowDebug(DesiredPosition, Vector3.Zero, Rotator.Zero, 1f, System.Drawing.Color.Yellow);
            GameFiber.Yield();
        }
        Vector3 PedRoot = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Ped.Pedestrian, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Ped.Pedestrian, 0));
        float calcHeading = (float)GetHeading(Player.Character.Position, PedRoot);
        float calcHeading2 = (float)CalculeAngle(PedRoot, Player.Character.Position);
        DesiredHeading = calcHeading2;
        NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", Player.Character, Ped.Pedestrian, 1000);
        GameFiber.Sleep(1000);
        if (IsCloseEnough && IsFacingDirection && !IsCancelled)
        {
            // EntryPoint.WriteToConsole($"MoveToBody IN POSITION {Game.LocalPlayer.Character.DistanceTo(DesiredPosition)} {Extensions.GetHeadingDifference(heading, DesiredHeading)} {heading} {DesiredHeading}");
            return true;
        }
        else
        {
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            //EntryPoint.WriteToConsole($"MoveToBody NOT IN POSITION EXIT {Game.LocalPlayer.Character.DistanceTo(DesiredPosition)} {Extensions.GetHeadingDifference(heading, DesiredHeading)} {heading} {DesiredHeading}");
            return false;
        }
    }
    private double GetHeading(Vector3 a, Vector3 b)
    {
        double x = b.X - a.X;
        double y = b.Y - a.Y;
        return 270 - Math.Atan2(y, x) * (180 / Math.PI);
    }
    private double CalculeAngle(Vector3 start, Vector3 arrival)
    {
        var deltaX = Math.Pow((arrival.X - start.X), 2);
        var deltaY = Math.Pow((arrival.Y - start.Y), 2);
        var radian = Math.Atan2((arrival.Y - start.Y), (arrival.X - start.X));
        var angle = (radian * (180 / Math.PI) + 360) % 360;
        return angle;
    }
    protected virtual void SetupVariables()
    {
        EnterAnimationDictionary = "amb@medic@standing@tendtodead@enter";
        EnterAnimation = "enter";
        ExitAnimationDictionary = "amb@medic@standing@tendtodead@exit";
        ExitAnimation = "exit";
        BaseAnimationDictionary = "amb@medic@standing@tendtodead@base";
        BaseAnimation = "base";
        IdleAnimationDictionary = "amb@medic@standing@tendtodead@idle_a";
        IdleAnimationList = new List<string>() { "idle_a", "idle_b", "idle_c" };

        if (!AnimationDictionary.RequestAnimationDictionayResult(EnterAnimationDictionary) ||
            !AnimationDictionary.RequestAnimationDictionayResult(ExitAnimationDictionary) || 
            !AnimationDictionary.RequestAnimationDictionayResult(BaseAnimationDictionary) ||
            !AnimationDictionary.RequestAnimationDictionayResult(IdleAnimationDictionary))
        {
            EntryPoint.WriteToConsole("ERROR PED INSPECT COULD NOT LOAD ANIMATION DICTIONARY 1");
            IsCancelled = true;
        }
    }
    protected virtual void SetupPed()
    {
        if (IsCancelled)
        {
            return;
        }
    }
    protected virtual void SetupPlayer()
    {
        if (IsCancelled)
        {
            return;
        }
        Player.ActivityManager.IsPerformingActivity = true;
    }
    protected virtual void ResetPed()
    {

    }
    protected virtual void ResetPlayer()
    {
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        Player.ActivityManager.IsPerformingActivity = false;
        RestorePlayerWeapons();
    }
    protected virtual void ResetVariables()
    {

    }
    private void StoreAndUnarmPlayer()
    {
        WasSetUnArmed = true;
        if (Player.WeaponEquipment.CurrentWeapon != null)
        {
            LastWeapon = Player.WeaponEquipment.CurrentWeapon;
        }
        else
        {
            LastWeapon = null;
        }
        Player.WeaponEquipment.SetUnarmed();
    }
    private void RestorePlayerWeapons()
    {
        if(!WasSetUnArmed || LastWeapon == null)
        {
            return;
        }
        NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeapon.Hash, true);     
    }
}