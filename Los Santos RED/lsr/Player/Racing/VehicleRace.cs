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

    public VehicleRace(string name, VehicleRaceTrack vehicleRaceTrack, VehicleExt playerVehicle, IEntityProvideable world, int numberOfLaps, bool clearTraffic)
    {
        Name = name;
        VehicleRaceTrack = vehicleRaceTrack;
        PlayerVehicle = playerVehicle;
        World = world;
        ClearTraffic = clearTraffic;
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
                if (GameTimePlayerFinishedRace > 0 && Game.GameTime - GameTimePlayerFinishedRace >= 10000)
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
        Player.RacingManager.StopRacing();
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
            if(airacer.VehicleExt == null)
            {
                continue;
            }
            Player.VehicleOwnership.TakeOwnershipOfVehicle(airacer.VehicleExt, false);
            if (airacer.PedExt != null && airacer.PedExt.Pedestrian.Exists())
            {
                airacer.PedExt.Pedestrian.ClearLastVehicle();
                airacer.PedExt.AssignedVehicle = null;
                if (airacer.VehicleExt.Vehicle.Exists())
                {
                    NativeFunction.Natives.TASK_LEAVE_VEHICLE(airacer.PedExt.Pedestrian, airacer.VehicleExt.Vehicle, 16);
                    airacer.VehicleExt.Vehicle.Velocity = Vector3.Zero;

                    SpawnLocation spawnLocation = new SpawnLocation(airacer.VehicleExt.Vehicle.Position);
                    spawnLocation.GetClosestStreet(false);
                    spawnLocation.GetClosestSideOfRoad();
                    spawnLocation.GetClosestSidewalk();
                    if(spawnLocation.HasSideOfRoadPosition)
                    {
                        airacer.VehicleExt.Vehicle.Position = spawnLocation.StreetPosition;
                        airacer.VehicleExt.Vehicle.Heading = spawnLocation.Heading;
                    }
                    else if (spawnLocation.HasSidewalk)
                    {
                        airacer.VehicleExt.Vehicle.Position = spawnLocation.SidewalkPosition;
                        airacer.VehicleExt.Vehicle.Heading = spawnLocation.Heading;
                    }

                }
                airacer.PedExt.FullyDelete();
                //airacer.Dispose();

                //airacer.PedExt.Pedestrian.Position = airacer.PedExt.Pedestrian.GetOffsetPositionRight(-1f);
                //VehicleRaceStartingPosition vrsp =  VehicleRaceTrack.VehicleRaceStartingPositions.FirstOrDefault();
                //Vector3 walkPos = new Vector3(0f, 0f, 0f);
                ////if(vrsp != null)
                ////{
                ////    walkPos = vrsp.Position;
                ////}
                //NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(airacer.PedExt.Pedestrian, walkPos.X, walkPos.Y, walkPos.Z, 2.0f, -1, 0f, 0, 0f);
                //airacer.PedExt.Pedestrian.KeepTasks = true;

                //if(airacer.WasSpawnedForRace)
                //{
                    
                //    airacer.PedExt.FullyDelete();
                //}
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
        if(IsCountdownEnabled)
        {
            DoCountdown();
        }     
        GameTimeStartedRace = Game.GameTime;
        PlayerRacer.ShowMessage("GO!", "");
    }
    private void DoCountdown()
    {
       //EntryPoint.WriteToConsole($"COUNTDOWN START {Game.GameTime - GameTimeStartedCountdown}");
        GameTimeStartedCountdown = Game.GameTime;
        string toShow = "";
        string showing = "";
        HudColor toShowColor = HudColor.RedDark;
        while (Game.GameTime - GameTimeStartedCountdown <= 3000)
        {
            //NativeHelper.DisablePlayerMovementControl();
            if (Game.GameTime - GameTimeStartedCountdown < 1000)
            {
                toShow = "3";
                toShowColor = HudColor.RedDark;
            }
            else if (Game.GameTime - GameTimeStartedCountdown < 2000)
            {
                toShow = "2";
                toShowColor = HudColor.Red;
            }
            else if (Game.GameTime - GameTimeStartedCountdown < 3000)
            {
                toShow = "1";
                toShowColor = HudColor.OrangeDark;
            }
            if(toShow != showing)
            {
                PlayerRacer.ShowMessage(toShow, "", HudColor.Black, toShowColor, 1000);
                showing = toShow;
            }
            GameFiber.Yield();
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

                NativeFunction.Natives.CLEAR_AREA(startingPos.Position.X, startingPos.Position.Y, startingPos.Position.Z, 150f, true, false, false, false);

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
        foreach (VehicleRaceStartingPosition startingPos in VehicleRaceTrack.VehicleRaceStartingPositions)
        {
            float extendedDistance = 25f;
            NativeFunction.Natives.SET_ALL_VEHICLE_GENERATORS_ACTIVE_IN_AREA(startingPos.Position.X - extendedDistance, startingPos.Position.Y - extendedDistance, startingPos.Position.Z - extendedDistance, startingPos.Position.X + extendedDistance, startingPos.Position.Y + extendedDistance, startingPos.Position.Z + extendedDistance, true, false);
        }
        NativeFunction.Natives.REMOVE_ROAD_NODE_SPEED_ZONE(raceSpeedZoneID1);
        EntryPoint.WriteToConsole("UnclearStartingArea RAN");
    }

}

