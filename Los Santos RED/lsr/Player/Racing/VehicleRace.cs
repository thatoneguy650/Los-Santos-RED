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
using System.Windows.Forms;
using System.Xml.Serialization;


public class VehicleRace
{
    private uint GameTimeStartedCountdown;
    private List<Blip> SpawnedBlips = new List<Blip>();
    private uint GameTimeStartedRace;
    private uint GameTimeFinishedRace;
    private uint GameTimePlayerFinishedRace;
    private bool HasWinner;
    private IEntityProvideable World;
   // private VehicleExt PlayerVehicle;

    private IRaceable Player;
    private AIVehicleRacer AIWinner;
    private List<VehicleRacer> Finishers = new List<VehicleRacer>();
    private dynamic raceSpeedZoneID1;

    public VehicleRace(string name, VehicleRaceTrack vehicleRaceTrack, VehicleExt playerVehicle, IEntityProvideable world, int numberOfLaps, bool clearTraffic, bool slipstreamingEnabled)
    {
        Name = name;
        VehicleRaceTrack = vehicleRaceTrack;
        PlayerVehicle = playerVehicle;
        World = world;
        ClearTraffic = clearTraffic;
        SlipstreamingEnabled = slipstreamingEnabled;
        NumberOfLaps = numberOfLaps;
    }
    public VehicleRace()
    {
    }
    public string Name { get; set; }
    public bool IsCountdownEnabled { get; set; } = true;
    public bool AreRacerBlipsEnabled { get; set; } = true;
    public VehicleRaceTrack VehicleRaceTrack { get; set; }
    public bool HasRaceStarted => GameTimeStartedRace > 0;
    public int BetAmount { get; private set; }
    public bool IsPlayerWinner { get; private set; }
    public bool IsPinkSlipRace { get ; private set; }
    public bool ClearTraffic { get; private set;}
    public int NumberOfLaps { get; private set; }
    public bool SlipstreamingEnabled { get; set;}
    public VehicleExt PlayerVehicle { get; private set; }
    [XmlIgnore]
    public bool IsActive { get; set; }
    [XmlIgnore]
    public List<VehicleRacer> VehicleRacers
    {
        get
        {
            List<VehicleRacer> toReturn = new List<VehicleRacer>();
            toReturn.AddRange(AIVehicleRacers);
            toReturn.Add(PlayerRacer);
            return toReturn;
        }
    }
    [XmlIgnore]
    public List<AIVehicleRacer> AIVehicleRacers { get; set; }
    [XmlIgnore]
    public PlayerVehicleRacer PlayerRacer { get; set; }
    public void Setup(List<AIVehicleRacer> aivehicleRacers, PlayerVehicleRacer playerRacer, int betAmount, bool isForPinks)
    {
        VehicleRaceCheckpoint finishCheckpoint = VehicleRaceTrack.RaceCheckpoints.OrderByDescending(x => x.Order).FirstOrDefault();
        if(finishCheckpoint != null)
        {
            finishCheckpoint.IsFinish = true;
        }
        AIVehicleRacers = aivehicleRacers;
        PlayerRacer = playerRacer;
        BetAmount = betAmount;
        IsPinkSlipRace = isForPinks;
        EntryPoint.WriteToConsole($"Starting Race IsPinkSlipRace{IsPinkSlipRace} BetAmount {BetAmount}");
    }
    public void Start(ITargetable targetable, IEntityProvideable World, ISettingsProvideable Settings, IRaceable player)
    {
        Player = player;
        GameFiber raceGameFiber = GameFiber.StartNew(delegate
        {
            ResetItems();
            IsActive = true;
            foreach (VehicleRacer vre in VehicleRacers)
            {
                vre.SetupRace(this);
            }
            RacePrelimiary(World);
            foreach (AIVehicleRacer vre in AIVehicleRacers)
            {
                vre.AssignTask(this, targetable, World, Settings);
            }
            foreach (VehicleRacer vre in VehicleRacers)
            {
                vre.SetRaceStart(this);
            }
            while (IsActive)
            {
                foreach (VehicleRacer vre in VehicleRacers)
                {
                    vre.Update(this);
                }
                if (!EntryPoint.ModController.IsRunning)
                {
                    IsActive = false;
                }
                //EntryPoint.WriteToConsole($"GameTimePlayerFinishedRace {GameTimePlayerFinishedRace} IsActive {IsActive}");
                if (GameTimePlayerFinishedRace > 0 && Game.GameTime - GameTimePlayerFinishedRace >= 5000)
                {
                    IsActive = false;
                }
                GameFiber.Yield();
            }
            Dispose();
        }, "RaceGameFiber");
    }
    private void ResetItems()
    {
        GameTimeStartedRace = 0;
        GameTimeFinishedRace = 0;
        HasWinner = false;
        GameTimeStartedCountdown = 0;
        IsPlayerWinner = false;
        GameTimePlayerFinishedRace = 0;
        AIWinner = null;
    }
    public void Dispose()
    {
        PlayerRacer.Dispose();
        foreach (AIVehicleRacer vre in AIVehicleRacers)
        {
            vre.Dispose();     
        }
        UnclearStartingArea();
        World.SetTrafficEnabled();
        SetSlipstreaming(false);
    }
    public void AddBlip(Blip checkpointBlip)
    {
        SpawnedBlips.Add(checkpointBlip);
    }
    public void Cancel(IRaceable player)
    {
        ReturnBet(player);
        IsActive = false;
        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Cancelled", "Race Status", "You have cancelled the race, any bets have been returned.");
    }
    public void Forfeit()
    {
        IsActive = false;
        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~r~Forfeit", "Race Status", "You have forfeited the race");
    }
    public void OnRacerFinishedRace(VehicleRacer vehicleRacer)
    {
        if (GameTimeFinishedRace == 0)
        {
            GameTimeFinishedRace = Game.GameTime;
        }
        Finishers.Add(vehicleRacer);
        int finalPosition = Finishers.Count();
        if (vehicleRacer.IsPlayer && finalPosition == 1)
        {
            IsPlayerWinner = true;
        }
        vehicleRacer.OnFinishedRace(finalPosition, this);
        EntryPoint.WriteToConsole($" OnRacerFinishedRace {vehicleRacer.RacerName} {finalPosition} IsPlayerWinner:{IsPlayerWinner}");
        if (!PlayerRacer.HasFinishedRace && finalPosition == 1)
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "Winner", $"{vehicleRacer.RacerName}", $"The race has been won by {vehicleRacer.RacerName}~n~Total Time: {vehicleRacer.GetTotalTimeAsString()}");
        }
    }
    public void OnPlayerFinishedRace()
    {
        Player.VehicleRaceManager.StopRacing();
        EntryPoint.WriteToConsole($"OnPlayerFinishedRace IsPlayerWinner: {IsPlayerWinner} GameTimePlayerFinishedRace{GameTimePlayerFinishedRace}");
        GameTimePlayerFinishedRace = Game.GameTime;
        if (IsPinkSlipRace)
        {
            EntryPoint.WriteToConsole("START HANDLE PINK SLIPS");
            HandlePinkSlips();
        }
        if (BetAmount > 0)
        {
            EntryPoint.WriteToConsole("START HANDLE BETTING");
            HandleBetting();
        }
    }
    public void HandlePinkSlips()
    {
        if (IsPlayerWinner)
        {
            GivePlayerOpponentVehicles();      
        }
        else
        {
            ForfeitPlayerVehicle();
        }
    }
    private void ForfeitPlayerVehicle()
    {
        if(PlayerVehicle == null || !PlayerVehicle.Vehicle.Exists())
        {
            return;
        }
        Game.FadeScreenOut(2000, true);
        Player.VehicleOwnership.RemoveOwnershipOfVehicle(PlayerVehicle);
        PlayerVehicle.FullyDelete();
        GameFiber.Sleep(2000);
        Game.FadeScreenIn(2000, false);

        //Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~r~Pink Slip", "Race Outcome", "You have lost the pinkslip to your vehicle.");
        //GameFiber raceGameFiber = GameFiber.StartNew(delegate
        //{
        //    uint gameTimeStarted = Game.GameTime;
        //    if(!PlayerVehicle.Vehicle.Exists())
        //    {
        //        return;
        //    }
        //    NativeFunction.Natives.TASK_VEHICLE_TEMP_ACTION(Player.Character, PlayerVehicle.Vehicle, 27, 10000);
        //    while (Player.VehicleSpeedMPH >= 1.0f || Game.GameTime - gameTimeStarted >= 20000)
        //    {        
        //        GameFiber.Yield();
        //    }
        //    if (!PlayerVehicle.Vehicle.Exists())
        //    {
        //        return;
        //    }
        //    gameTimeStarted = Game.GameTime;
        //    NativeFunction.Natives.TASK_LEAVE_VEHICLE(Player.Character, PlayerVehicle.Vehicle, 0);
        //    while (Player.IsInVehicle || Game.GameTime - gameTimeStarted >= 20000)
        //    {
        //        GameFiber.Yield();
        //    }
        //    if (!PlayerVehicle.Vehicle.Exists())
        //    {
        //        return;
        //    }
        //    Player.VehicleOwnership.RemoveOwnershipOfVehicle(PlayerVehicle);
        //    PlayerVehicle.Vehicle.SetLockedForPlayer(Game.LocalPlayer, true);
        //}, "RaceGameFiber");
    }
    private void GivePlayerOpponentVehicles()
    {
        EntryPoint.WriteToConsole("PINK SLIP WINNER");
        Game.FadeScreenOut(2000, true);

        foreach (AIVehicleRacer airacer in AIVehicleRacers)
        {
            if (airacer == null || airacer.VehicleExt == null) continue;

            Vehicle losingVehicle = airacer.VehicleExt.Vehicle;
            if (losingVehicle == null || !losingVehicle.Exists()) continue;

            NativeFunction.Natives.SET_VEHICLE_HANDLING_OVERRIDE(losingVehicle, 0);
            NativeFunction.Natives.MODIFY_VEHICLE_TOP_SPEED(losingVehicle, -1.0f);
            NativeFunction.Natives.SET_VEHICLE_CHEAT_POWER_INCREASE(losingVehicle, 1.0f);
            NativeFunction.Natives.SET_VEHICLE_IS_RACING(losingVehicle, false);
            NativeFunction.Natives.SET_VEHICLE_STRONG(losingVehicle, false);
            NativeFunction.Natives.SET_ENTITY_ONLY_DAMAGED_BY_PLAYER(losingVehicle, false);

            Player.VehicleOwnership.TakeOwnershipOfVehicle(airacer.VehicleExt, false);

            if (airacer.PedExt != null && airacer.PedExt.Pedestrian.Exists())
            {
                airacer.IsManualDispose = true;

                if (airacer.PedExt.Pedestrian.IsAlive)
                {
                    losingVehicle.Velocity = Vector3.Zero;

                    if (airacer.PedExt.Pedestrian.IsInAnyVehicle(false))
                    {
                        NativeFunction.Natives.TASK_LEAVE_VEHICLE(airacer.PedExt.Pedestrian, losingVehicle, 16);
                    }

                    SpawnLocation spawnLocation = new SpawnLocation(losingVehicle.Position);
                    spawnLocation.GetClosestStreet(false);
                    spawnLocation.GetClosestSideOfRoad();
                    spawnLocation.GetClosestSidewalk();

                    if (spawnLocation.HasSideOfRoadPosition)
                    {
                        losingVehicle.Position = spawnLocation.StreetPosition;
                        losingVehicle.Heading = spawnLocation.Heading;
                    }
                    else if (spawnLocation.HasSidewalk)
                    {
                        losingVehicle.Position = spawnLocation.SidewalkPosition;
                        losingVehicle.Heading = spawnLocation.Heading;
                    }

                    airacer.PedExt.Pedestrian.Position = losingVehicle.GetOffsetPositionRight(-2f);
                    NativeFunction.Natives.SET_VEHICLE_DOORS_SHUT(losingVehicle, true);

                    airacer.PedExt.Pedestrian.ClearLastVehicle();
                    airacer.PedExt.AssignedVehicle = null;

                    airacer.PedExt.ClearTasks(true);
                    airacer.PedExt.CanBeAmbientTasked = true;
                    airacer.PedExt.CanBeTasked = true;
                    airacer.PedExt.CanBeIdleTasked = true;

                    try
                    {
                        airacer.PedExt.PedBrain?.AssignIdleTask();
                    }
                    catch (Exception ex)
                    {
                        EntryPoint.WriteToConsole($"Non-Fatal: PedBrain failed for {airacer.RacerName}: {ex.Message}");
                    }
                }

                airacer.PedExt.SetNonPersistent();
                airacer.PedExt.DeleteBlip();

                if (airacer.WasSpawnedForRace)
                {
                    airacer.PedExt.Pedestrian.IsPersistent = false;
                }
            }
        }

        GameFiber.Sleep(2000);
        Game.FadeScreenIn(2000, false);
        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~g~Race Wins", "Race Outcome", $"You have acquired the losing vehicles.");
    }
    private void RacePrelimiary(IEntityProvideable world)
    {
        EntryPoint.WriteToConsole($"RACE PRELIMINARY RAN ClearTraffic {ClearTraffic}");
        if(ClearTraffic)
        {
            world.SetTrafficDisabled();
        }
        if (SlipstreamingEnabled)
        {
            SetSlipstreaming(true);
        }
        if (IsCountdownEnabled)
        {
            DoCountdown();
        }     
        GameTimeStartedRace = Game.GameTime;
        PlayerRacer.ShowMessage("GO!", "");
    }
    private void DoCountdown()
    {
        GameTimeStartedCountdown = Game.GameTime;
        string showing = "";

        while (Game.GameTime - GameTimeStartedCountdown <= 3000)
        {
            string toShow = "";
            HudColor toShowColor = HudColor.RedDark;

            long elapsed = Game.GameTime - GameTimeStartedCountdown;

            if (elapsed < 1000) { toShow = "3"; toShowColor = HudColor.RedDark; }
            else if (elapsed < 2000) { toShow = "2"; toShowColor = HudColor.Red; }
            else { toShow = "1"; toShowColor = HudColor.OrangeDark; }

            if (toShow != showing)
            {
                PlayerRacer.ShowMessage(toShow, "", HudColor.Black, toShowColor, 1000);
                showing = toShow;
            }

            GameFiber.Yield(); // CRITICAL: Keeps the game from freezing
        }
    }
    private void HandleBetting()
    {
        int winAmount = VehicleRacers.Count() * BetAmount;
        if (IsPlayerWinner)
        {   
            Player.BankAccounts.GiveMoney(winAmount, false);
            EntryPoint.WriteToConsole($"Player is winner giving {winAmount}");
        }
    }
    private void ReturnBet(IRaceable player)
    {
        if(BetAmount > 0)
        {
            player.BankAccounts.GiveMoney(BetAmount, false);
        }
    }
    public void ClearStartingArea()
    {
        if (VehicleRaceTrack == null)
        {
            return;
        }
        EntryPoint.WriteToConsole("ClearStartingArea RAN");
        bool checkedFirst = false;
        foreach (VehicleRaceStartingPosition startingPos in VehicleRaceTrack.VehicleRaceStartingPositions)
        {
            float extendedDistance = 25f;

            if(!checkedFirst)
            {
                raceSpeedZoneID1 = NativeFunction.Natives.ADD_ROAD_NODE_SPEED_ZONE<int>(startingPos.Position.X, startingPos.Position.Y, startingPos.Position.Z, 50f, 0f, false);
                checkedFirst = true;

                NativeFunction.Natives.CLEAR_AREA(startingPos.Position.X, startingPos.Position.Y, startingPos.Position.Z, 500f, true, false, false, false);

            }

            NativeFunction.Natives.SET_ALL_VEHICLE_GENERATORS_ACTIVE_IN_AREA(startingPos.Position.X - extendedDistance, startingPos.Position.Y - extendedDistance, startingPos.Position.Z - extendedDistance, startingPos.Position.X + extendedDistance, startingPos.Position.Y + extendedDistance, startingPos.Position.Z + extendedDistance, false, false);
            NativeFunction.Natives.REMOVE_VEHICLES_FROM_GENERATORS_IN_AREA(startingPos.Position.X - extendedDistance, startingPos.Position.Y - extendedDistance, startingPos.Position.Z - extendedDistance, startingPos.Position.X + extendedDistance, startingPos.Position.Y + extendedDistance, startingPos.Position.Z + extendedDistance, false);
            NativeFunction.Natives.CLEAR_AREA(startingPos.Position.X, startingPos.Position.Y, startingPos.Position.Z, 15f, true, false, false, false);
            EntryPoint.WriteToConsole("ClearStartingArea RAN FOR POS");
        }

    }
    private void UnclearStartingArea()
    {
        if(VehicleRaceTrack == null)
        {
            return;
        }
        if(VehicleRaceTrack.VehicleRaceStartingPositions == null)
        {
            return;
        }
        foreach (VehicleRaceStartingPosition startingPos in VehicleRaceTrack.VehicleRaceStartingPositions)
        {
            float extendedDistance = 25f;
            NativeFunction.Natives.SET_ALL_VEHICLE_GENERATORS_ACTIVE_IN_AREA(startingPos.Position.X - extendedDistance, startingPos.Position.Y - extendedDistance, startingPos.Position.Z - extendedDistance, startingPos.Position.X + extendedDistance, startingPos.Position.Y + extendedDistance, startingPos.Position.Z + extendedDistance, true, false);
        }
        if (raceSpeedZoneID1 != 0)
        {
            NativeFunction.Natives.REMOVE_ROAD_NODE_SPEED_ZONE(raceSpeedZoneID1);
        }
        EntryPoint.WriteToConsole("UnclearStartingArea RAN");
    }
    public void SetSlipstreaming(bool toggle)
    {
        // Since this is a global native, we only need to call it once per race
        NativeFunction.Natives.SET_ENABLE_VEHICLE_SLIPSTREAMING(toggle);
        EntryPoint.WriteToConsole($"Slipstreaming Globally Set To: {toggle}");
    }
}

