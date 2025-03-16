using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RacingManager
{
    private IRaceable Player;
    private ISettingsProvideable Settings;
    private IEntityProvideable World;
    private ITargetable Targetable;

    private ICrimes Crimes;
    private IWeapons Weapons;
    private INameProvideable Names;
    private IModItems ModItems;
    private IShopMenus ShopMenus;


    public bool IsRacing { get; private set; }
    public TextTimerBar RaceTimer { get; private set; }
    // public string RaceTime { get; set; } = "";

    //Crimes,Weapons,Names,World,ModItems,ShopMenus
    public RacingManager(IRaceable player, ISettingsProvideable settings, IEntityProvideable world, ICrimes crimes, IWeapons weapons, INameProvideable names, IModItems modItems, IShopMenus shopMenus, ITargetable targetable)
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
    }
    public void StartRacing()
    {
        IsRacing = true;
    }
    public void StopRacing()
    {
        IsRacing = false;
    }
    public void Setup()
    {

    }
    public void Update()
    {

    }
    public void Dispose()
    {

    }
    public void StartDebugRace(VehicleRace race)
    {
        if(race== null)
        {
            return;
        }
        Game.FadeScreenOut(1000, true);

        Player.IsSetDisabledControls = true;

        List<AIVehicleRacer> vehicleRacers = new List<AIVehicleRacer>() { };
        foreach (VehicleRaceStartingPosition rsp in race.VehicleRaceStartingPositions.Where(x => x.Order >= 1))
        {
            vehicleRacers.Add(SpawnRacer(rsp, race.AreRacerBlipsEnabled));
        }
        PlayerVehicleRacer playerRacer = new PlayerVehicleRacer(Player.CurrentVehicle, Player, Settings);
        MoveVehicleToPosition(Player.CurrentVehicle, race.VehicleRaceStartingPositions.Where(x => x.Order == 0).FirstOrDefault());    
        race.Setup(vehicleRacers, playerRacer);
        uint GameTimeStartedWatiing = Game.GameTime;
        Game.FadeScreenIn(1000, false);
        while (Game.GameTime - GameTimeStartedWatiing <= 1000)
        {
            NativeHelper.DisablePlayerMovementControl();
            GameFiber.Yield();
        }  
        race.Start(Targetable,World,Settings);
    }
    public bool StartPointToPointRace(VehicleRace race, PedExt pedExt)
    {
        if (race == null)
        {
            return false;
        }
        if (pedExt == null)
        {
            return false;
        }
        if(!pedExt.Pedestrian.Exists())
        {
            return false;
        }
        uint vehicleHandle = 0;
        if(pedExt.Pedestrian.CurrentVehicle.Exists())
        {
            vehicleHandle = pedExt.Pedestrian.CurrentVehicle.Handle;
        }
        if(vehicleHandle == 0)
        {
            return false;
        }
        VehicleExt racerVehicle = World.Vehicles.GetVehicleExt(pedExt.Pedestrian.CurrentVehicle);

        if(racerVehicle == null)
        {
            return false; 
        }
        pedExt.SetPersistent();
        pedExt.WasModSpawned = true;
        List<AIVehicleRacer> vehicleRacers = new List<AIVehicleRacer>() { };
        AIVehicleRacer challenger = new AIVehicleRacer(pedExt, racerVehicle) { WasSpawnedForRace = true };

        vehicleRacers.Add(challenger);
        pedExt.AddBlip();
        PlayerVehicleRacer playerRacer = new PlayerVehicleRacer(Player.CurrentVehicle, Player, Settings);
        race.Setup(vehicleRacers, playerRacer);
        race.Start(Targetable, World, Settings);

        return true;
    }
    private AIVehicleRacer SpawnRacer(VehicleRaceStartingPosition vehicleRaceStartingPosition, bool addBlip)
    {
        List<string> possibleVehicle = new List<string>() { "dominator3", "coquette4", "coquette6", "comet6","banshee3","niobe", "stingertt" };
        DispatchableVehicle aiRaceCar = new DispatchableVehicle(possibleVehicle.PickRandom(), 100, 100);
        DispatchablePerson aiRacePerson = new DispatchablePerson("ig_car3guy2", 100, 100);

        SpawnLocation sl = new SpawnLocation(vehicleRaceStartingPosition.Position) { Heading = vehicleRaceStartingPosition.Heading };
        sl.StreetPosition = vehicleRaceStartingPosition.Position;

        CivilianSpawnTask cst = new CivilianSpawnTask(sl, aiRaceCar, aiRacePerson, addBlip, false, true, Settings, Crimes, Weapons, Names, World, ModItems, ShopMenus);
        cst.ClearVehicleArea = false;
        cst.AllowAnySpawn = true;
        cst.AllowBuddySpawn = false;
        cst.PlacePedOnGround = false;
        cst.DoPersistantEntityCheck = false;
        cst.AttemptSpawn();
        //EntryPoint.WriteToConsole("ATTEMPTING TO SPAWN {PEDS");
        VehicleExt createdvehicle = cst.CreatedVehicles.FirstOrDefault();
        if(createdvehicle != null && createdvehicle.Vehicle.Exists())
        {
            createdvehicle.Vehicle.IsEngineOn = true;
        }
        return new AIVehicleRacer(cst.CreatedPeople.FirstOrDefault(), createdvehicle) { WasSpawnedForRace = true };
       
    }
    private void MoveVehicleToPosition(VehicleExt toMove, VehicleRaceStartingPosition vrsp)
    {
        if(toMove == null)
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

    public void SetRaceTimer(TextTimerBar raceTimer)
    {
        RaceTimer = raceTimer;
    }
}

