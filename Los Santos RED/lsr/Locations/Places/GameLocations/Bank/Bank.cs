using ExtensionsMethods;
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
    private BankInteraction BankInteraction;
    private List<Teller> SpawnedTellers = new List<Teller>();
    private BankDrawer nearbybankDrawer;
    private bool IsCancelled;
    protected readonly List<string> FallBackTellerModels = new List<string>() { "s_f_m_shop_high", "s_f_y_airhostess_01", "s_m_m_highsec_01" };
    private List<BankDrawer> BankDrawers = new List<BankDrawer>();
    public Bank(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public Bank() : base()
    {

    }
    public override string TypeName { get; set; } = "Bank";
    public override int MapIcon { get; set; } = (int)BlipSprite.Devin;
    public List<SpawnPlace> TellerLocations { get; set; } = new List<SpawnPlace>();
    public List<string> TellerModels { get; set; } = new List<string>();
    public bool HasTellers => TellerLocations != null && TellerLocations.Any();
    public int DrawerCash { get; set; } = 3000;
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Bank At {Name}";
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
        if (!CanInteract)
        {
            return;
        }
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        Player.IsTransacting = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                StoreCamera = new LocationCamera(this, Player, Settings);
                StoreCamera.Setup();
                CreateInteractionMenu();
                BankInteraction = new BankInteraction(Player, this);
                BankInteraction.Start(MenuPool, InteractionMenu);
                while (IsAnyMenuVisible)
                {
                    MenuPool.ProcessMenus();
                    GameFiber.Yield();
                }
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
    protected override void RemoveButtonPrompts()
    {
        Player.ButtonPrompts.RemovePrompts("BankSteal");
    }
    public override void UpdatePrompts()
    {
        foreach (BankDrawer sp in BankDrawers)
        {
            float distanceto = sp.Position.DistanceTo2D(Player.Position);
            if (distanceto <= 1.0f)
            {
                nearbybankDrawer = sp;
                break;
            }
        }
        if (nearbybankDrawer != null && !Player.ButtonPrompts.HasPrompt("BankStealDrawer"))
        {
            Action action = () => {
                Player.ButtonPrompts.RemovePrompts("BankSteal");
                if (nearbybankDrawer != null && MoveToPosition(nearbybankDrawer.Position, nearbybankDrawer.Heading) && PlayMoneyAnimation(Player))
                {
                    if(nearbybankDrawer.TotalCash == 0)
                    {
                        Game.DisplaySubtitle("Drawer Empty");
                    }
                    if(nearbybankDrawer.TotalCash <= 1500)
                    {
                        Player.BankAccounts.GiveMoney(nearbybankDrawer.TotalCash, false);
                        nearbybankDrawer.TotalCash = 0;
                    }
                    else
                    {
                        Player.BankAccounts.GiveMoney(1500, false);
                        nearbybankDrawer.TotalCash -= 1500;
                    }
                }
            };
            Player.ButtonPrompts.AddPrompt("BankSteal", "Steal from Drawer", "BankStealDrawer", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 999, action);
        }
        if (nearbybankDrawer == null || nearbybankDrawer.TotalCash <= 0)
        {
            Player.ButtonPrompts.RemovePrompts("BankSteal");
        }
    }
    public bool MoveToPosition(Vector3 PropEntryPosition, float PropEntryHeading)
    {
        if (PropEntryPosition == Vector3.Zero)
        {
            return false;
        }
        NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(Player.Character, PropEntryPosition.X, PropEntryPosition.Y, PropEntryPosition.Z, 1.0f, -1, 0.3f, 0, PropEntryHeading);
        uint GameTimeStartedSitting = Game.GameTime;
        float heading = Game.LocalPlayer.Character.Heading;
        bool IsFacingDirection = false;
        bool IsCloseEnough = false;
        while (Game.GameTime - GameTimeStartedSitting <= 5000 && !IsCloseEnough && !IsCancelled)
        {
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            IsCloseEnough = Game.LocalPlayer.Character.DistanceTo2D(PropEntryPosition) < 0.5f;
            GameFiber.Yield();
        }
        GameFiber.Sleep(250);
        GameTimeStartedSitting = Game.GameTime;
        while (Game.GameTime - GameTimeStartedSitting <= 5000 && !IsFacingDirection && !IsCancelled)
        {
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
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
        Player.ActivityManager.StopDynamicActivity();
        AnimationDictionary.RequestAnimationDictionay("mp_safehousevagos@");

        NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Player.Character, "mp_safehousevagos@", "package_dropoff", 4.0f, -4.0f, 2000, 0, 0, false, false, false);
        //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "mp_car_bomb", "car_bomb_mechanic", 2.0f, -2.0f, 5000, 0, 0, false, false, false);
        IsCancelled = false;
        uint GameTimeStartedAnimation = Game.GameTime;

        Player.Violations.TheftViolations.IsRobbingBank = true;

        while (Player.ActivityManager.CanPerformActivitiesExtended && Game.GameTime - GameTimeStartedAnimation <= 2000)
        {
            //Player.WeaponEquipment.SetUnarmed();
            float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, "mp_safehousevagos@", "package_dropoff");
            if (AnimationTime >= 1.0f)
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
        //EntryPoint.WriteToConsoleTestLong($"Dead Drop PlayMoneyAnimation IsCancelled: {IsCancelled}");
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
    public override void Activate(IInteriors interiors, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, ITimeReportable time, IEntityProvideable world)
    {
        foreach(SpawnPlace spawnPlace in TellerLocations)
        {
            SpawnTeller(settings,crimes,weapons,spawnPlace);
            BankDrawers.Add(new BankDrawer(spawnPlace.Position, spawnPlace.Heading, DrawerCash));
        }
        if(HasTellers)
        {
            CanInteract = false;
        }
        base.Activate(interiors, settings, crimes, weapons, time, world);
    }
    protected void SpawnTeller(ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, SpawnPlace spawnPlace)
    {
        Ped ped;
        string ModelName;
        if (TellerModels != null && TellerModels.Any())
        {
            ModelName = TellerModels.PickRandom();
        }
        else
        {
            ModelName = FallBackTellerModels.PickRandom();
        }
        NativeFunction.Natives.CLEAR_AREA(spawnPlace.Position.X, spawnPlace.Position.Y, spawnPlace.Position.Z, 2f, true, false, false, false);
        Model modelToCreate = new Model(Game.GetHashKey(ModelName));
        modelToCreate.LoadAndWait();
        ped = NativeFunction.Natives.CREATE_PED<Ped>(26, Game.GetHashKey(ModelName), spawnPlace.Position.X, spawnPlace.Position.Y, spawnPlace.Position.Z, spawnPlace.Heading, false, false);//ped = NativeFunction.Natives.CREATE_PED<Ped>(26, Game.GetHashKey(ModelName), VendorPosition.X, VendorPosition.Y, VendorPosition.Z + 1f, VendorHeading, false, false);
        GameFiber.Yield();
        if (!ped.Exists())
        {
            return;
        }
        ped.IsPersistent = true;//THIS IS ON FOR NOW!
        ped.RandomizeVariation();
        NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", ped, "WORLD_HUMAN_STAND_IMPATIENT", 0, true);
        ped.KeepTasks = true;
        EntryPoint.SpawnedEntities.Add(ped);
        GameFiber.Yield();
        if (!ped.Exists())
        {
            return;
        }
        Teller teller = new Teller(ped, settings, "Teller", crimes, weapons, World);
        teller.AssociatedBank = this;
        teller.SpawnPosition = spawnPlace.Position;
        teller.WasModSpawned = true;

        float resultArg = ped.Position.Z;
        if (NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(ped.Position.X, ped.Position.Y, ped.Position.Z + 1f, out resultArg, false))
        {
            ped.Position = new Vector3(ped.Position.X, ped.Position.Y, resultArg);
        }



        World.Pedestrians.AddEntity(teller);
        SpawnedTellers.Add(teller);
    }
    public override void Deactivate(bool deleteBlip)
    {
        foreach(Teller merchant in SpawnedTellers)
        {
            if (merchant != null && merchant.Pedestrian.Exists())
            {
                merchant.Pedestrian.Delete();
            }
        }
        base.Deactivate(deleteBlip);
    }
}

