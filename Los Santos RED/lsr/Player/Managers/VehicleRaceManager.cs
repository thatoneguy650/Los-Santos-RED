using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleRaceManager
{
    private IRaceable Player;
    private ISettingsProvideable Settings;
    private IEntityProvideable World;
    private ITargetable Targetable;
    private MenuPool MenuPool;
    private ICrimes Crimes;
    private IWeapons Weapons;
    private INameProvideable Names;
    private IModItems ModItems;
    private IShopMenus ShopMenus;
    private VehicleRace CurrentRace;
    private UIMenu CurrentRaceMenu;

    public bool IsRacing { get; private set; }
    public TextTimerBar RaceTimer { get; private set; }
    public bool IsShowingRaceMenu { get; private set; }
    public VehicleRaceManager(IRaceable player, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes, IWeapons weapons, INameProvideable names, IModItems modItems, IShopMenus shopMenus, ITargetable targetable)
    {
        Player = player;
        Settings = settings;
        World = world;
        Crimes = crimes;
        Weapons = weapons;
        Names = names;
        ModItems = modItems;
        ShopMenus = shopMenus;
        Targetable = targetable;
        MenuPool = new MenuPool();
    }
    public void Setup()
    {

    }
    public void Update()
    {
        if (IsRacing && !IsShowingRaceMenu)
        {
            Player.ButtonPrompts.AttemptAddPrompt("Racing", "Racing Menu", "RacingMenu", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 999, () => { ShowCurrentRaceMenu(); });
        }
        else
        {
            Player.ButtonPrompts.RemovePrompts("Racing");
        }
    }
    public void StartRacing(VehicleRace vehicleRace)
    {
        IsRacing = true;
        CurrentRace = vehicleRace;
    }
    public void StopRacing()
    {
        IsRacing = false;
        CurrentRace = null;
    }
    public void Dispose()
    {
        Player.ButtonPrompts.RemovePrompts("Racing");
    }
    public void StartDebugRace(VehicleRace race)
    {
        if (race == null)
        {
            return;
        }
        Game.FadeScreenOut(1000, true);

        Player.IsSetDisabledControls = true;

        List<AIVehicleRacer> vehicleRacers = new List<AIVehicleRacer>() { };

        List<VehicleRaceStartingPosition> FinalStartingPositions = GetFinalStartingPosition(race.VehicleRaceTrack.VehicleRaceStartingPositions, 8);


        foreach (VehicleRaceStartingPosition rsp in FinalStartingPositions.Where(x => x.Order >= 1))
        {
            vehicleRacers.Add(SpawnRacer(rsp, race.AreRacerBlipsEnabled, null, null));
        }
        PlayerVehicleRacer playerRacer = new PlayerVehicleRacer(Player.CurrentVehicle, Player, Settings);
        MoveVehicleToPosition(Player.CurrentVehicle, race.VehicleRaceTrack.VehicleRaceStartingPositions.Where(x => x.Order == 0).FirstOrDefault());
        race.Setup(vehicleRacers, playerRacer, 0, false);
        uint GameTimeStartedWatiing = Game.GameTime;
        Game.FadeScreenIn(1000, false);
        while (Game.GameTime - GameTimeStartedWatiing <= 1000)
        {
            NativeHelper.DisablePlayerMovementControl();
            GameFiber.Yield();
        }
        race.Start(Targetable, World, Settings, Player);
    }
    public bool StartPointToPointRace(VehicleRace race, PedExt pedExt, int betAmount, bool isForPinks)
    {
        if (race == null)
        {
            return false;
        }
        if (pedExt == null)
        {
            return false;
        }
        if (!pedExt.Pedestrian.Exists())
        {
            return false;
        }
        uint vehicleHandle = 0;
        if (pedExt.Pedestrian.CurrentVehicle.Exists())
        {
            vehicleHandle = pedExt.Pedestrian.CurrentVehicle.Handle;
        }
        if (vehicleHandle == 0)
        {
            return false;
        }
        VehicleExt racerVehicle = World.Vehicles.GetVehicleExt(pedExt.Pedestrian.CurrentVehicle);
        if (racerVehicle == null)
        {
            return false;
        }
        if (Player.BankAccounts.GetMoney(false) < betAmount)
        {
            return false;
        }
        Player.BankAccounts.GiveMoney(-1 * betAmount, false);
        pedExt.SetPersistent();
        List<AIVehicleRacer> vehicleRacers = new List<AIVehicleRacer>() { };
        AIVehicleRacer challenger = new AIVehicleRacer(pedExt, racerVehicle) { };
        vehicleRacers.Add(challenger);
        pedExt.AddBlip();
        PlayerVehicleRacer playerRacer = new PlayerVehicleRacer(Player.CurrentVehicle, Player, Settings);

        Player.IsSetDisabledControlsWithCamera = true;

        race.Setup(vehicleRacers, playerRacer, betAmount, isForPinks);
        race.Start(Targetable, World, Settings, Player);
        return true;
    }
    public bool StartRegularRace(VehicleRace race, int betAmount, bool isForPinks, DispatchableVehicleGroup selectedOpponentVehicles, DispatchablePersonGroup selectedOpponentPeds, 
        int numberofOpponents)
    {
        if (race == null)
        {
            return false;
        }
        if (Player.BankAccounts.GetMoney(false) < betAmount)
        {
            return false;
        }
        Player.BankAccounts.GiveMoney(-1 * betAmount, false);
        Game.FadeScreenOut(1000, true);
        Player.IsSetDisabledControlsWithCamera = true;
        List<AIVehicleRacer> vehicleRacers = new List<AIVehicleRacer>() { };
        int spawnedRacers = 0;


        race.ClearStartingArea();

        List<VehicleRaceStartingPosition> FinalStartingPositions = GetFinalStartingPosition(race.VehicleRaceTrack.VehicleRaceStartingPositions, numberofOpponents); 


        foreach (VehicleRaceStartingPosition rsp in FinalStartingPositions.Where(x => x.Order >= 1))
        {
            AIVehicleRacer aIVehicleRacer = SpawnRacer(rsp, race.AreRacerBlipsEnabled, selectedOpponentVehicles, selectedOpponentPeds);
            if (aIVehicleRacer == null || aIVehicleRacer.PedExt == null || !aIVehicleRacer.PedExt.Pedestrian.Exists())
            {
                continue;
            }
            vehicleRacers.Add(aIVehicleRacer);
            spawnedRacers++;
            if (spawnedRacers >= numberofOpponents)
            {
                break;
            }
        }
        if (race.PlayerVehicle == null || !race.PlayerVehicle.Vehicle.Exists())
        {
            Game.FadeScreenIn(1000);
            EntryPoint.WriteToConsole("NO PLAYER VEHICLE SELECTED");
            Player.IsSetDisabledControlsWithCamera = false;
            return false;
        }
        if (!Player.IsInVehicle)
        {
            Player.Character.WarpIntoVehicle(race.PlayerVehicle.Vehicle, -1);
        }
        PlayerVehicleRacer playerRacer = new PlayerVehicleRacer(race.PlayerVehicle, Player, Settings);
        MoveVehicleToPosition(race.PlayerVehicle, race.VehicleRaceTrack.VehicleRaceStartingPositions.Where(x => x.Order == 0).FirstOrDefault());

        race.PlayerVehicle.Engine.SetState(true);

        race.Setup(vehicleRacers, playerRacer, betAmount, isForPinks);
        uint GameTimeStartedWatiing = Game.GameTime;
        Game.FadeScreenIn(1000, false);
        while (Game.GameTime - GameTimeStartedWatiing <= 1000)
        {
            //NativeHelper.DisablePlayerMovementControl();
            GameFiber.Yield();
        }
        race.Start(Targetable, World, Settings, Player);
        return true;
    }

    private List<VehicleRaceStartingPosition> GetFinalStartingPosition(List<VehicleRaceStartingPosition> vehicleRaceStartingPositions, int numberofOpponents) 
    {
        List<VehicleRaceStartingPosition> FinalStartingPositions = new List<VehicleRaceStartingPosition>();
        if (vehicleRaceStartingPositions.Count < numberofOpponents)
        {
            VehicleRaceStartingPosition LaststartingPosition = null;
            for (int i = 0; i < numberofOpponents; i++)
            {
                VehicleRaceStartingPosition existing = vehicleRaceStartingPositions.FirstOrDefault(x => x.Order == i);
                if (existing != null)
                {
                    FinalStartingPositions.Add(existing);
                    LaststartingPosition = existing;
                    EntryPoint.WriteToConsole($"GetFinalStartingPosition VehicleRaceStartingPosition existing found {i}");
                }
                else//create new starting position
                {
                    if (LaststartingPosition != null)
                    {
                        VehicleRaceStartingPosition newPostiion = new VehicleRaceStartingPosition(i, 
                            NativeHelper.GetOffsetPosition(LaststartingPosition.Position, LaststartingPosition.Heading-90f, 9f), 
                            LaststartingPosition.Heading);
                        FinalStartingPositions.Add(newPostiion);
                        LaststartingPosition = newPostiion;
                        EntryPoint.WriteToConsole($"GetFinalStartingPosition VehicleRaceStartingPosition MAKE NEW {i} {newPostiion.Position} {newPostiion.Heading}");
                    }
                }
            }
        }
        else
        {
            FinalStartingPositions = vehicleRaceStartingPositions;
        }
        return FinalStartingPositions;
    }

    public void SetRaceTimer(TextTimerBar raceTimer)
    {
        RaceTimer = raceTimer;
    }
    private void ShowCurrentRaceMenu()
    {
        if (MenuPool.IsAnyMenuOpen())
        {
            return;
        }

        CreateCurrentRaceMenu();
        CurrentRaceMenu.Visible = true;



        GameFiber raceGameFiber = GameFiber.StartNew(delegate
        {
            while (MenuPool.IsAnyMenuOpen())
            {
                MenuPool.ProcessMenus();
                GameFiber.Yield();
            }

        }, "RaceGameFiber");
    }
    private void CreateCurrentRaceMenu()
    {
        CurrentRaceMenu = new UIMenu("Race Menu", "Select Race Options");
        MenuPool.Add(CurrentRaceMenu);
        UIMenuItem ForfeitRaceMenuItem = new UIMenuItem("Forfeit Race", "Select to forfeit the race.");
        ForfeitRaceMenuItem.Activated += (menu, item) =>
        {
            CurrentRace?.Forfeit();
            menu.Visible = false;
        };
        CurrentRaceMenu.AddItem(ForfeitRaceMenuItem);
        UIMenuItem CancelRaceMenuItem = new UIMenuItem("Cancel Race", "Select to cancel the race without any penalites.");
        CancelRaceMenuItem.Activated += (menu, item) =>
        {
            CurrentRace?.Cancel(Player);
            menu.Visible = false;
        };
        CurrentRaceMenu.AddItem(CancelRaceMenuItem);
    }
    private AIVehicleRacer SpawnRacer(VehicleRaceStartingPosition vehicleRaceStartingPosition, bool addBlip, DispatchableVehicleGroup opponentVehicleGroup, DispatchablePersonGroup opponentPedGroup)
    {
        List<string> possibleVehicle = new List<string>() { "dominator3", "coquette4", "coquette6", "comet6", "banshee3", "niobe", "stingertt" };

        DispatchableVehicle aiRaceCar = new DispatchableVehicle(possibleVehicle.PickRandom(), 100, 100);
        if (opponentVehicleGroup != null)
        {
            aiRaceCar = opponentVehicleGroup.DispatchableVehicles.RandomElementByWeight(x => x.CurrentSpawnChance(0, true));
        }
        DispatchablePerson aiRacePerson = new DispatchablePerson("ig_car3guy2", 100, 100);
        if (opponentPedGroup != null)
        {
            aiRacePerson = opponentPedGroup.DispatchablePeople.RandomElementByWeight(x => x.CurrentSpawnChance(0));
        }
        SpawnLocation sl = new SpawnLocation(vehicleRaceStartingPosition.Position) { Heading = vehicleRaceStartingPosition.Heading };
        sl.StreetPosition = vehicleRaceStartingPosition.Position;
        CivilianSpawnTask cst = new CivilianSpawnTask(sl, aiRaceCar, aiRacePerson, addBlip, false, true, Settings, Crimes, Weapons, Names, World, ModItems, ShopMenus);
        cst.ClearVehicleArea = true;
        cst.AllowAnySpawn = true;
        cst.AllowBuddySpawn = false;
        cst.PlacePedOnGround = false;
        cst.DoPersistantEntityCheck = false;
        cst.AttemptSpawn();
        //EntryPoint.WriteToConsole("ATTEMPTING TO SPAWN {PEDS");
        VehicleExt createdvehicle = cst.CreatedVehicles.FirstOrDefault();
        PedExt createdPed = cst.CreatedPeople.FirstOrDefault();
        if (createdvehicle != null && createdvehicle.Vehicle.Exists())
        {
            createdvehicle.Vehicle.IsEngineOn = true;
        }
        if(createdPed != null)
        {
            createdPed.CanTakeVehicleCrashDamage = false;
            createdPed.CanBeIdleTasked = false;
            createdPed.IsManuallyDeleted = true;
        }
        return new AIVehicleRacer(cst.CreatedPeople.FirstOrDefault(), createdvehicle) { WasSpawnedForRace = true };

    }
    private void MoveVehicleToPosition(VehicleExt toMove, VehicleRaceStartingPosition vrsp)
    {
        if (toMove == null)
        {
            return;
        }
        if (!toMove.Vehicle.Exists())
        {
            return;
        }
        toMove.Vehicle.Position = vrsp.Position;
        toMove.Vehicle.Heading = vrsp.Heading;
    }
}

