using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Bank : GameLocation
{
    private string drawerStealPromptGroup = "BankSteal";
    private string drawerStealPromptIdentity = "BankStealDrawer";
    private string drawerStealPromptText = "Steal from Drawer";
    private string drawerStealEmptyText = "Drawer Empty";
    private BankInteraction BankInteraction;
    private List<Teller> SpawnedTellers = new List<Teller>();
    private BankDrawer ClosestBankDrawer;
    private bool IsCancelled;
    protected readonly List<string> FallBackTellerModels = new List<string>() { "s_f_m_shop_high", "s_f_y_airhostess_01", "s_m_m_highsec_01" };
    private List<BankDrawer> BankDrawers = new List<BankDrawer>();
    private bool IsStealingFromDrawer;

    public Bank(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string shortName) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        ShortName = shortName;
    }
    public Bank() : base()
    {

    }

    public override string TypeName { get; set; } = "Bank";
    public override int MapIcon { get; set; } = (int)BlipSprite.Devin;
    public List<SpawnPlace> TellerLocations { get; set; } = new List<SpawnPlace>();
    public List<string> TellerModels { get; set; } = new List<string>();
    public bool HasTellers => TellerLocations != null && TellerLocations.Any();
    public int DrawerCashMin { get; set; } = 1000;
    public int DrawerCashMax { get; set; } = 9000;
    public int DrawerCashGainedPerAnimation { get; set; } = 500;
    public float ExtaTellerSpawnPercentage { get; set; } = 70f;
    public string ShortName { get; set; }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Bank At {Name}";
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
        if (!CanInteract)
        {
            return;
        }
        if (Interior != null && Interior.IsTeleportEntry)
        {
            DoEntranceCamera();
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
                BankInteraction = new BankInteraction(Player, this);
                BankInteraction.Start(MenuPool, InteractionMenu, true);
                while (IsAnyMenuVisible)
                {
                    MenuPool.ProcessMenus();
                    GameFiber.Yield();
                }
                BankInteraction.Dispose();
                DisposeInteractionMenu();
                DisposeCamera(isInside);
                DisposeInterior();
                //StoreCamera.Dispose();
                Player.IsTransacting = false;
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "BankInteract");
    }
    public override void UpdatePrompts()
    {
        BankDrawer newClosestDrawer = null;
        float newClosestDistance = 999f;
        if (DistanceToPlayer <= 100f)
        {
            foreach (BankDrawer sp in BankDrawers)
            {
                float distanceto = sp.Position.DistanceTo2D(Player.Position);
                if (distanceto <= 1.0f && distanceto <= newClosestDistance)
                {
                    newClosestDistance = distanceto;
                    newClosestDrawer = sp;
                    //break;
                }
            }
        }
        //EntryPoint.WriteToConsole($"BANK PROMPTS newClosestDrawer {newClosestDrawer?.Position} newClosestDistance {newClosestDistance}");
        if (newClosestDrawer == null)
        {
            RemoveDrawerPrompt();
            EntryPoint.WriteToConsole("BANK PROMPTS newClosestDrawer is null");
            return;
        }
        if (ClosestBankDrawer == null)
        {
            ClosestBankDrawer = newClosestDrawer;
            EntryPoint.WriteToConsole($"BANK PROMPTS ClosestBankDrawer is NULL. ADDING");
            AddDrawerPrompt();
            return;
        }
        if (ClosestBankDrawer != null && newClosestDrawer.Position.DistanceTo2D(ClosestBankDrawer.Position) >= 0.1f)
        {
            ClosestBankDrawer = newClosestDrawer;
            EntryPoint.WriteToConsole($"BANK PROMPTS ClosestBankDrawer is NOT NULL. CHANGING");
            AddDrawerPrompt();
            return;
        }
        if(!IsStealingFromDrawer && ClosestBankDrawer != null && !Player.ButtonPrompts.HasPrompt(drawerStealPromptIdentity))
        {
            AddDrawerPrompt();
            EntryPoint.WriteToConsole($"BANK PROMPTS ClosestBankDrawer is NOT NULL. READDING PROMPT");
        }
    }
    private void AddDrawerPrompt()
    {
        Action action = () => {
            GameFiber.StartNew(delegate
            {
                try
                {
                    RemoveDrawerPrompt();
                    if(ClosestBankDrawer == null)
                    {
                        return;
                    }
                    IsStealingFromDrawer = true;
                    if (!MoveToPosition(ClosestBankDrawer.Position, ClosestBankDrawer.Heading))
                    {
                        IsStealingFromDrawer = false;
                        return;
                    }
                    if (ClosestBankDrawer.TotalCash == 0)
                    {
                        IsStealingFromDrawer = false;
                        Game.DisplaySubtitle(drawerStealEmptyText);
                        return;
                    }
                    PlayMoneyAnimation(Player);
                    IsStealingFromDrawer = false;
                    UpdatePrompts();
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "bankPromptInteract");
        };
        Player.ButtonPrompts.AddPrompt(drawerStealPromptGroup, drawerStealPromptText, drawerStealPromptIdentity, Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 999, action);
    }
    private void RemoveDrawerPrompt()
    {
        Player.ButtonPrompts.RemovePrompts(drawerStealPromptGroup);
    }
    public bool MoveToPosition(Vector3 PropEntryPosition, float PropEntryHeading)
    {
        if (PropEntryPosition == Vector3.Zero)
        {
            return false;
        }
        IsCancelled = false;
        NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(Player.Character, PropEntryPosition.X, PropEntryPosition.Y, PropEntryPosition.Z, 3.0f, -1, 0.5f, 0, PropEntryHeading);//NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(Player.Character, PropEntryPosition.X, PropEntryPosition.Y, PropEntryPosition.Z, 3.0f, -1, 1.0f, 0, PropEntryHeading);
        uint GameTimeStartedSitting = Game.GameTime;
        float heading = Game.LocalPlayer.Character.Heading;
        bool IsFacingDirection = false;
        bool IsCloseEnough = false;
        while (Game.GameTime - GameTimeStartedSitting <= 5000 && !IsCloseEnough && !IsCancelled)
        {
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
                break;
            }
            IsCloseEnough = Game.LocalPlayer.Character.DistanceTo2D(PropEntryPosition) < 0.75f;
            GameFiber.Yield();
        }
        GameFiber.Sleep(250);
        GameTimeStartedSitting = Game.GameTime;
        while (Game.GameTime - GameTimeStartedSitting <= 5000 && !IsFacingDirection && !IsCancelled)
        {
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
                break;
            }
            heading = Game.LocalPlayer.Character.Heading;
            if (Math.Abs(ExtensionsMethods.Extensions.GetHeadingDifference(heading, PropEntryHeading)) <= 0.5f)//0.5f)
            {
                IsFacingDirection = true;
            }
            GameFiber.Yield();
        }
        GameFiber.Sleep(250);
        if (IsCloseEnough && IsFacingDirection && !IsCancelled)
        {
            return true;
        }
        else
        {
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            return false;
        }
    }
    private bool PlayMoneyAnimation(ILocationInteractable Player)
    {
        if(ClosestBankDrawer == null)
        {
            return false;
        }
        Player.ActivityManager.StopDynamicActivity();
        AnimationDictionary.RequestAnimationDictionay("oddjobs@shop_robbery@rob_till");


        //NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Player.Character, "oddjobs@shop_robbery@rob_till", "enter", 4.0f, -4.0f, -1, 0, 0, false, false, false);
        //GameFiber.Sleep(1000);
        //NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Player.Character, "oddjobs@shop_robbery@rob_till", "loop", 4.0f, -4.0f, -1, 1, 0, false, false, false);


        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", 0, "oddjobs@shop_robbery@rob_till", "enter", 4.0f, -4.0f, -1, 0, 0, false, false, false);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", 0, "oddjobs@shop_robbery@rob_till", "loop", 4.0f, -4.0f, -1, 1, 0, false, false, false);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }




        IsCancelled = false;
        Player.Violations.TheftViolations.IsRobbingBank = true;
        uint GameTimeLastGotCash = Game.GameTime;
        while (Player.ActivityManager.CanPerformActivitiesExtended)
        {
            Player.WeaponEquipment.SetUnarmed();
            //float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, "oddjobs@shop_robbery@rob_till", "loop");
            //if (AnimationTime >= 1.0f)
            //{
            //    break;
            //}
            if (Game.GameTime - GameTimeLastGotCash >= 900)
            {
                GiveCash();
                GameTimeLastGotCash = Game.GameTime;
            }
            if(ClosestBankDrawer.TotalCash <= 0)
            {
                break;
            }
            if (Player.IsMoveControlPressed || !Player.Character.IsAlive)
            {
                IsCancelled = true;
                break;
            }
            GameFiber.Yield();
        }
        EntryPoint.WriteToConsole($"BANK PlayMoneyAnimation IsCancelled: {IsCancelled}");
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);

        Player.Violations.TheftViolations.IsRobbingBank = false;

        if (IsCancelled)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void GiveCash()
    {
        if (ClosestBankDrawer.TotalCash <= DrawerCashGainedPerAnimation)
        {      
            Player.BankAccounts.GiveMoney(ClosestBankDrawer.TotalCash, false);
            PlaySuccessSound();
            ClosestBankDrawer.TotalCash = 0;
            EntryPoint.WriteToConsole($"BANK GAVE CASH 1 {ClosestBankDrawer.TotalCash}");
        }
        else
        {
            Player.BankAccounts.GiveMoney(DrawerCashGainedPerAnimation, false);
            PlaySuccessSound();
            ClosestBankDrawer.TotalCash -= DrawerCashGainedPerAnimation;
            EntryPoint.WriteToConsole($"BANK GAVE CASH 2 {ClosestBankDrawer.TotalCash}");
        }
        Game.DisplaySubtitle($"Drawer Cash: ${ClosestBankDrawer.TotalCash}");
    }

    public override void Activate(IInteriors interiors, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, ITimeReportable time, IEntityProvideable world)
    {
        int TellersSpawned = 0;
        BankDrawers.Clear();
        foreach (SpawnPlace spawnPlace in TellerLocations)
        {
            if(IsOpen(time.CurrentHour) && settings.SettingsManager.CivilianSettings.ManageDispatching && world.Pedestrians.TotalSpawnedServiceWorkers < settings.SettingsManager.CivilianSettings.TotalSpawnedServiceMembersLimit && (TellersSpawned == 0 || RandomItems.RandomPercent(ExtaTellerSpawnPercentage)))
            {
                if (SpawnTeller(settings, crimes, weapons, spawnPlace))
                {
                    TellersSpawned++;
                }
            }
            BankDrawers.Add(new BankDrawer(NativeHelper.GetOffsetPosition(spawnPlace.Position, spawnPlace.Heading, 0.3f), spawnPlace.Heading, RandomItems.GetRandomNumberInt(DrawerCashMin, DrawerCashMax))); //BankDrawers.Add(new BankDrawer(spawnPlace.Position.Around2D(0.3f), spawnPlace.Heading, RandomItems.GetRandomNumberInt(DrawerCashMin, DrawerCashMax)));
        }
        if(HasTellers)
        {
            CanInteract = false;
        }
        base.Activate(interiors, settings, crimes, weapons, time, world);
    }
    protected bool SpawnTeller(ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, SpawnPlace spawnPlace)
    {
        //Ped ped;
        string ModelName;
        if (TellerModels != null && TellerModels.Any())
        {
            ModelName = TellerModels.PickRandom();
        }
        else
        {
            ModelName = FallBackTellerModels.PickRandom();
        }
        EntryPoint.WriteToConsole($"ATTEMPTING TELLER AT {Name} {ModelName}");
        NativeFunction.Natives.CLEAR_AREA(spawnPlace.Position.X, spawnPlace.Position.Y, spawnPlace.Position.Z, 2f, true, false, false, false);
        World.Pedestrians.CleanupAmbient();
        Ped ped = new Ped(ModelName, spawnPlace.Position, spawnPlace.Heading);
        GameFiber.Yield();
        if (!ped.Exists())
        {
            return false;
        }
        EntryPoint.SpawnedEntities.Add(ped);
        Teller teller = new Teller(ped, settings, "Teller", crimes, weapons, World);
        if (!World.Pedestrians.Tellers.Any(x => x.Handle == ped.Handle))
        {
            World.Pedestrians.Tellers.Add(teller);
        }
        teller.AssociatedBank = this;
        teller.SpawnPosition = spawnPlace.Position;
        teller.WasModSpawned = true;
        teller.WillCower = true;
        ped.IsPersistent = true;//THIS IS ON FOR NOW!
        ped.RandomizeVariation();
        teller.LocationTaskRequirements = new LocationTaskRequirements() { TaskRequirements = TaskRequirements.Guard, ForcedScenarios = new List<string>() { "WORLD_HUMAN_STAND_IMPATIENT" } };
        GameFiber.Yield();
        if (!ped.Exists())
        {
            return false;
        }
        //float resultArg = ped.Position.Z;
        //if (NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(ped.Position.X, ped.Position.Y, ped.Position.Z + 1f, out resultArg, false))
        //{
        //    ped.Position = new Vector3(ped.Position.X, ped.Position.Y, resultArg);
        //}
        SpawnedTellers.Add(teller);
        EntryPoint.WriteToConsole($"SPAWNED WORKED TELLER AT {Name}");
        return true;
    }
    public override void Deactivate(bool deleteBlip)
    {
        foreach(Teller merchant in SpawnedTellers)
        {
            if (merchant != null && merchant.Pedestrian.Exists())
            {
                merchant.DeleteBlip();
                merchant.Pedestrian.Delete();
            }
        }
        RemoveDrawerPrompt();
        base.Deactivate(deleteBlip);
    }
}

