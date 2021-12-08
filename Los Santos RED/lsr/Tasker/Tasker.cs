using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Tasker : ITaskerable, ITaskerReportable
{
    private IEntityProvideable PedProvider;
    private ITargetable Player;
    private IWeapons Weapons;
    private ISettingsProvideable Settings;
    private uint GameTimeLastGeneratedCrime;
    private uint RandomCrimeRandomTime;
    private List<PedExt> PossibleTargets;
    private Cop ClosestCopToPlayer;
    private IPlacesOfInterest PlacesOfInterest;
    private List<AssignedSeat> SeatAssignments = new List<AssignedSeat>();
    private RelationshipGroup CriminalsRG;
    private double AverageTimeBetweenCopUpdates = 0;
    private double AverageTimeBetweenCivUpdates = 0;
    private uint MaxTimeBetweenCopUpdates = 0;
    private uint MaxTimeBetweenCivUpdates = 0;
    private bool IsTimeToCreateCrime => Game.GameTime - GameTimeLastGeneratedCrime >= (Settings.SettingsManager.CivilianSettings.MinimumTimeBetweenRandomCrimes + RandomCrimeRandomTime);
    public string TaskerDebug => $"Cop Max: {MaxTimeBetweenCopUpdates} Avg: {AverageTimeBetweenCopUpdates} Civ Max: {MaxTimeBetweenCivUpdates} Avg: {AverageTimeBetweenCivUpdates}";
    public Tasker(IEntityProvideable pedProvider, ITargetable player, IWeapons weapons, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest)
    {
        PedProvider = pedProvider;
        Player = player;
        Weapons = weapons;
        Settings = settings;
        GameTimeLastGeneratedCrime = Game.GameTime;
        RandomCrimeRandomTime = RandomItems.GetRandomNumber(0, 240000);//between 0 and 4 minutes randomly added
        PlacesOfInterest = placesOfInterest;
    }
    public void Setup()
    {
        CriminalsRG = new RelationshipGroup("CRIMINALS");
        RelationshipGroup.Cop.SetRelationshipWith(CriminalsRG, Relationship.Hate);
        CriminalsRG.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Hate);
        NativeFunction.Natives.REQUEST_ANIM_SET<bool>("move_m@drunk@verydrunk");
    }
    public void RunPoliceTasks()
    {
        SetPossibleTargets();
        ExpireSeatAssignments();
        foreach (Cop Cop in PedProvider.PoliceList.Where(x => x.CurrentTask != null && x.CurrentTask.ShouldUpdate && x.CanBeTasked).OrderBy(x => x.DistanceToPlayer))
        {
            try
            {
                Cop.UpdateTask(PedToAttack(Cop));
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Updating Cop Task");
            }
            GameFiber.Yield();
        }
    }
    public void RunCiviliansTasks()
    {
        ExpireSeatAssignments();
        foreach (PedExt Ped in PedProvider.CivilianList.Where(x => x.CurrentTask != null && x.CurrentTask.ShouldUpdate).OrderBy(x => x.DistanceToPlayer))//.OrderBy(x => x.CurrentTask.GameTimeLastRan))
        {
            try
            { 
                Ped.UpdateTask(null);
            }
            catch (Exception e)
            {
                EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Updating Civilian Task");
            }
            GameFiber.Yield();
        }
    }
    public void SetPoliceTasks()
    {
        if (Settings.SettingsManager.PoliceSettings.ManageTasking)
        {
            SetPossibleTargets();
            ExpireSeatAssignments();
            foreach (Cop Cop in PedProvider.PoliceList.Where(x => x.Pedestrian.Exists() && x.HasBeenSpawnedFor >= 2000 && x.NeedsTaskAssignmentCheck && x.CanBeTasked).OrderBy(x => x.DistanceToPlayer))
            {
                try
                {
                    UpdateCurrentTask(Cop);
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Setting Cop Task");
                }
                GameFiber.Yield();
            }
            List<Cop> PossibleCops = PedProvider.PoliceList.Where(x => x.GameTimeLastUpdatedTask != 0).ToList();
            List<PedExt> PossibleCivs = PedProvider.CivilianList.Where(x => x.GameTimeLastUpdatedTask != 0).ToList();

            if (PossibleCops.Any())
            {
                MaxTimeBetweenCopUpdates = PossibleCops.Max(x => Game.GameTime - x.GameTimeLastUpdatedTask);
                AverageTimeBetweenCopUpdates = PossibleCops.Average(x => Game.GameTime - x.GameTimeLastUpdatedTask);
            }
            else
            {
                MaxTimeBetweenCopUpdates = 0;
                AverageTimeBetweenCopUpdates = 0;
            }
            if (PossibleCivs.Any())
            {
                MaxTimeBetweenCivUpdates = PossibleCivs.Max(x => Game.GameTime - x.GameTimeLastUpdatedTask);
                AverageTimeBetweenCivUpdates = PossibleCivs.Average(x => Game.GameTime - x.GameTimeLastUpdatedTask);
            }
            else
            {
                MaxTimeBetweenCivUpdates = 0;
                AverageTimeBetweenCivUpdates = 0;
            }
        }

    }
    public void SetCivilianTasks()
    {
        if (Settings.SettingsManager.CivilianSettings.AllowRandomCrimes && IsTimeToCreateCrime)
        {
             CreateCrime();
        }
        if (Settings.SettingsManager.CivilianSettings.ManageCivilianTasking)
        {
            ExpireSeatAssignments();
            foreach (PedExt Civilian in PedProvider.CivilianList.Where(x => x.Pedestrian.Exists() && x.DistanceToPlayer <= 200f && x.NeedsTaskAssignmentCheck).OrderBy(x => x.DistanceToPlayer))//75f//.OrderBy(x => x.GameTimeLastUpdatedTask).Take(10))//2//10)//2
            {
                try
                { 
                    if (Civilian.DistanceToPlayer <= 200f)
                    {
                        UpdateCurrentTask(Civilian);
                    }
                    else if (Civilian.CurrentTask != null)
                    {
                        Civilian.CurrentTask = null;
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Setting Civilian Task");
                }
            }
            foreach (PedExt Civilian in PedProvider.CivilianList.Where(x => x.Pedestrian.Exists() && x.DistanceToPlayer > 230f))
            {
                Civilian.CurrentTask = null;
            }
            GameFiber.Yield();
        }
    }
    public void CreateCrime()
    {
        PedExt Criminal = PedProvider.CivilianList.Where(x => x.Pedestrian.Exists() && x.DistanceToPlayer <= 200f && x.CanBeAmbientTasked && !x.IsInVehicle).OrderByDescending(x=> x.IsGangMember).FirstOrDefault();//85f//150f
        if (Criminal != null && Criminal.Pedestrian.Exists())
        {
            if (Settings.SettingsManager.CivilianSettings.ShowRandomCriminalBlips && Criminal.Pedestrian.Exists())
            {
                Blip myBlip = Criminal.Pedestrian.AttachBlip();
                myBlip.Color = Color.Red;
                myBlip.Scale = 0.6f;
                NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
                NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME("Criminal");
                NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(myBlip);
                PedProvider.AddEntity(myBlip);
            }  
            Criminal.CanBeAmbientTasked = false;
            Criminal.WasSetCriminal = true;
            Criminal.WillCallPolice = false;
            Criminal.Pedestrian.RelationshipGroup = CriminalsRG;
            Criminal.CurrentTask = new CommitCrime(Criminal, Player,Weapons, PedProvider);
            Criminal.CurrentTask.Start();           
            GameTimeLastGeneratedCrime = Game.GameTime;
            RandomCrimeRandomTime = RandomItems.GetRandomNumber(0, 240000);//between 0 and 4 minutes randomly added
            //EntryPoint.WriteToConsole("TASKER: GENERATED CRIME", 5);
        }
    }
    public bool IsSeatAssigned(IComplexTaskable pedToCheck, VehicleExt vehicleToCheck, int seatToCheck) => SeatAssignments.Any(x => x.Vehicle != null && vehicleToCheck != null && x.Vehicle.Handle == vehicleToCheck.Handle && x.Seat == seatToCheck && x.Ped != null && pedToCheck != null && x.Ped.Handle != pedToCheck.Handle);
    public bool AddSeatAssignment(IComplexTaskable ped, VehicleExt vehicle, int seat)
    {
        if(ped == null || !ped.Pedestrian.Exists() || vehicle == null || !vehicle.Vehicle.Exists())
        {
            return false;
        }
        if(SeatAssignments.Any(x=> x.Vehicle != null && x.Vehicle.Handle == vehicle.Handle && x.Seat == seat))
        {
            return false;
        }
        SeatAssignments.Add(new AssignedSeat(ped,vehicle,seat));
        return true;
    }
    public void RemoveSeatAssignment(IComplexTaskable ped)
    {
        if (ped != null)
        {
            SeatAssignments.RemoveAll(x => x.Ped != null && x.Ped.Handle == ped.Handle);
        }
    }
    private void ExpireSeatAssignments()
    {
        SeatAssignments.RemoveAll(x => x.Vehicle == null || x.Ped == null || !x.Vehicle.Vehicle.Exists() || !x.Ped.Pedestrian.Exists() || x.Ped.Pedestrian.IsDead);
    }
    private PedExt PedToAttack(Cop Cop)
    {
        if(!PossibleTargets.Any(x => x.IsWanted))
        {
            return null;
        }
        PedExt MainTarget = null;
        if (Cop.Pedestrian.Exists() && Cop.DistanceToPlayer <= 200f)
        {
            if (Player.IsBusted)
            {
                if (PossibleTargets.Any(x => x.IsDeadlyChase))
                {
                    MainTarget = PossibleTargets.Where(x => x.Pedestrian.Exists() && x.IsWanted && !x.IsArrested).OrderByDescending(x => x.IsDeadlyChase).ThenByDescending(x => x.Pedestrian.DistanceTo2D(Cop.Pedestrian) <= 20f && !x.IsBusted).ThenByDescending(x => x.ArrestingPedHandle == Cop.Handle).ThenBy(x=>x.IsBusted).ThenBy(x => x.Pedestrian.DistanceTo2D(Cop.Pedestrian)).FirstOrDefault();
                }
                else
                {
                    if (ClosestCopToPlayer == null || Cop.Handle != ClosestCopToPlayer.Handle)
                    {
                        MainTarget = PossibleTargets.Where(x => x.Pedestrian.Exists() && x.IsWanted && !x.IsArrested).OrderByDescending(x => x.IsDeadlyChase).ThenByDescending(x => x.Pedestrian.DistanceTo2D(Cop.Pedestrian) <= 20f && !x.IsBusted).ThenByDescending(x => x.ArrestingPedHandle == Cop.Handle).ThenBy(x => x.IsBusted).ThenBy(x => x.Pedestrian.DistanceTo2D(Cop.Pedestrian)).FirstOrDefault();
                    }
                }

            }
            else if (Player.PoliceResponse.IsDeadlyChase)
            {
                MainTarget = PossibleTargets.Where(x => x.Pedestrian.Exists() && x.IsDeadlyChase && !x.IsArrested).OrderByDescending(x => x.IsDeadlyChase).ThenByDescending(x => x.Pedestrian.DistanceTo2D(Cop.Pedestrian) <= 20f && !x.IsBusted).ThenByDescending(x => x.ArrestingPedHandle == Cop.Handle).ThenBy(x => x.IsBusted).ThenBy(x => x.Pedestrian.DistanceTo2D(Cop.Pedestrian)).FirstOrDefault(); //MainTarget = PossibleTargets.Where(x => x.Pedestrian.Exists() && x.IsDeadlyChase && x.WantedLevel > Player.WantedLevel).OrderByDescending(x => x.IsDeadlyChase).ThenByDescending(x => x.ArrestingPedHandle == Cop.Handle).ThenBy(x => x.IsBusted).ThenBy(x => x.Pedestrian.DistanceTo2D(Cop.Pedestrian)).FirstOrDefault();
                if(MainTarget != null && MainTarget.Pedestrian.DistanceTo2D(Cop.Pedestrian) <= Cop.DistanceToPlayer + 20f)
                {
                    EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Player is Closer Than Closest Target (Deadly)", 3);
                    MainTarget = null;
                }
            }
            else
            {
                MainTarget = PossibleTargets.Where(x => x.Pedestrian.Exists() && x.IsWanted && !x.IsArrested).OrderByDescending(x => x.IsDeadlyChase).ThenByDescending(x=> x.Pedestrian.DistanceTo2D(Cop.Pedestrian) <= 20f && !x.IsBusted).ThenByDescending(x => x.ArrestingPedHandle == Cop.Handle).ThenBy(x => x.IsBusted).ThenBy(x => x.Pedestrian.DistanceTo2D(Cop.Pedestrian)).FirstOrDefault();
                if (Player.IsWanted && MainTarget != null && !MainTarget.IsDeadlyChase && MainTarget.Pedestrian.DistanceTo2D(Cop.Pedestrian) <= Cop.DistanceToPlayer + 20f)
                {
                    EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Player is Closer Than Closest Target (Non Deadly)", 3);
                    MainTarget = null;
                }
            }
            if (MainTarget != null && MainTarget.IsBusted && MainTarget.Handle != Cop.CurrentTask?.OtherTarget?.Handle && PedProvider.PoliceList.Any(x => x.Handle != Cop.Handle && x.CurrentTask?.OtherTarget?.Handle == MainTarget.Handle))
            {
                EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Too many police already on busted person, sending away", 3);
                MainTarget = null;
            }
        }
        if(MainTarget != null && MainTarget.Pedestrian.Exists() && MainTarget.Pedestrian.Handle == Game.LocalPlayer.Character.Handle)//for ped swappiung, they get confused!
        {
            MainTarget = null;
        }
        return MainTarget;
    }
    private void SetPossibleTargets()
    {
        PossibleTargets = PedProvider.CivilianList.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive && (x.IsWanted || (x.IsBusted && !x.IsArrested)) && x.DistanceToPlayer <= 200f).ToList();//150f
        ClosestCopToPlayer = PedProvider.PoliceList.Where(x => x.Pedestrian.Exists() && !x.IsInVehicle && x.DistanceToPlayer <= 30f && x.Pedestrian.IsAlive).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
    }
    private void UpdateCurrentTask(Cop Cop)//this should be moved out?
    {
        if (Cop.DistanceToPlayer <= Player.ActiveDistance)// && !Cop.IsInHelicopter)//heli, dogs, boats come next?
        {
            PedExt MainTarget = PedToAttack(Cop);
            if (MainTarget != null)
            {
                if (Cop.CurrentTask?.Name != "AIApprehend")
                {
                    EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to AIApprehend", 3);
                    Cop.CurrentTask = new AIApprehend(Cop, Player) { OtherTarget = MainTarget };
                    GameFiber.Yield();
                    Cop.CurrentTask.Start();
                }
            }
            else
            {
                if (Player.IsWanted)
                {
                    if (Player.IsInSearchMode)
                    {
                        if (Cop.CurrentTask?.Name != "Locate")
                        {
                            EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Locate", 3);
                            Cop.CurrentTask = new Locate(Cop, Player);
                            GameFiber.Yield();
                            Cop.CurrentTask.Start();
                        }
                    }
                    else
                    {
                        if (Cop.DistanceToPlayer <= 150f)//200f
                        {
                            if (Player.PoliceResponse.IsDeadlyChase && !Player.IsAttemptingToSurrender)
                            {
                                if (Cop.CurrentTask?.Name != "Kill")
                                {
                                    EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Kill", 3);
                                    Cop.CurrentTask = new Kill(Cop, Player);
                                    GameFiber.Yield();
                                    Cop.CurrentTask.Start();
                                }
                            }
                            else
                            {
                                if (Cop.CurrentTask?.Name != "Chase")
                                {
                                    EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Chase", 3);
                                    Cop.CurrentTask = new Chase(Cop, Player, PedProvider);
                                    GameFiber.Yield();
                                    Cop.CurrentTask.Start();
                                }
                            }
                        }
                        else// if (Cop.DistanceToPlayer <= Player.ActiveDistance)//1000f
                        {
                            if (Cop.CurrentTask?.Name != "Locate")
                            {
                                EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Locate", 3);
                                Cop.CurrentTask = new Locate(Cop, Player);
                                GameFiber.Yield();
                                Cop.CurrentTask.Start();
                            }
                        }
                    }
                }
                else if (Player.Investigation.IsActive && Cop.IsIdleTaskable)
                {
                    if (Cop.CurrentTask?.Name != "Investigate")
                    {
                       EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Investigate", 3);
                        Cop.CurrentTask = new Investigate(Cop, Player);
                        GameFiber.Yield();
                        Cop.CurrentTask.Start();
                    }
                }
                else
                {
                    if (Cop.CurrentTask?.Name != "Idle" && Cop.IsIdleTaskable)// && Cop.WasModSpawned)
                    {
                        EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Idle", 3);
                        Cop.CurrentTask = new Idle(Cop, Player, PedProvider, this, PlacesOfInterest);
                        GameFiber.Yield();
                        Cop.CurrentTask.Start();
                    }
                }
            }
        }
        else
        {
            if (Cop.CurrentTask?.Name != "Idle" && Cop.IsIdleTaskable)
            {
                EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Idle", 3);
                Cop.CurrentTask = new Idle(Cop, Player, PedProvider, this, PlacesOfInterest);
                GameFiber.Yield();
                Cop.CurrentTask.Start();
            }
        }
        Cop.GameTimeLastUpdatedTask = Game.GameTime;
    }
    private void UpdateCurrentTask(PedExt Civilian)//this should be moved out?
    {
        if(Civilian.IsBusted)
        {
            if (Civilian.DistanceToPlayer <= 75f)
            {
                if (Civilian.CurrentTask?.Name != "GetArrested")
                {
                    Civilian.CurrentTask = new GetArrested(Civilian, Player, PedProvider, this);
                    Civilian.CurrentTask.Start();
                }
            }
        }
        else if (Civilian.DistanceToPlayer <= 75f && Civilian.CanBeTasked && Civilian.CanBeAmbientTasked)//50f
        {
            WitnessedCrime HighestPriority = Civilian.OtherCrimesWitnessed.OrderBy(x => x.Crime.Priority).ThenByDescending(x => x.GameTimeLastWitnessed).FirstOrDefault();
            bool SeenScaryCrime = Civilian.PlayerCrimesWitnessed.Any(x => x.ScaresCivilians && x.CanBeReportedByCivilians) || Civilian.OtherCrimesWitnessed.Any(x => x.Crime.ScaresCivilians && x.Crime.CanBeReportedByCivilians);
            bool SeenAngryCrime = Civilian.PlayerCrimesWitnessed.Any(x => x.AngersCivilians && x.CanBeReportedByCivilians) || Civilian.OtherCrimesWitnessed.Any(x => x.Crime.AngersCivilians && x.Crime.CanBeReportedByCivilians);
            bool SeenMundaneCrime = Civilian.PlayerCrimesWitnessed.Any(x => !x.AngersCivilians && !x.ScaresCivilians && x.CanBeReportedByCivilians) || Civilian.OtherCrimesWitnessed.Any(x => !x.Crime.AngersCivilians && !x.Crime.ScaresCivilians && x.Crime.CanBeReportedByCivilians);

            if (SeenScaryCrime || SeenAngryCrime)
            {
                if (Civilian.WillCallPolice)
                {
                    if (Civilian.CurrentTask?.Name != "ScaredCallIn")
                    {
                        Civilian.CurrentTask = new ScaredCallIn(Civilian, Player) { OtherTarget = HighestPriority?.Perpetrator };
                        Civilian.CurrentTask.Start();
                    }
                }
                else if (Civilian.WillFight)
                {
                    if (SeenAngryCrime)
                    {
                        if (Civilian.CurrentTask?.Name != "Fight")
                        {
                            Civilian.CurrentTask = new Fight(Civilian, Player, GetWeaponToIssue(Civilian.IsGangMember)) { OtherTarget = HighestPriority?.Perpetrator };
                            Civilian.CurrentTask.Start();
                        }
                    }
                    else
                    {
                        if (Civilian.CurrentTask?.Name != "Flee")
                        {
                            Civilian.CurrentTask = new Flee(Civilian, Player) { OtherTarget = HighestPriority?.Perpetrator };
                            Civilian.CurrentTask.Start();
                        }
                    }
                }
                else
                {
                    if (Civilian.CurrentTask?.Name != "Flee")
                    {
                        Civilian.CurrentTask = new Flee(Civilian, Player) { OtherTarget = HighestPriority?.Perpetrator };
                        Civilian.CurrentTask.Start();
                    }
                }
            }
            else if (Civilian.IsFedUpWithPlayer && Civilian.WillFight)
            {
                if (Civilian.CurrentTask?.Name != "Fight")
                {
                    Civilian.CurrentTask = new Fight(Civilian, Player, GetWeaponToIssue(Civilian.IsGangMember)) { OtherTarget = HighestPriority?.Perpetrator };
                    Civilian.CurrentTask.Start();
                }
            }
            else if (SeenMundaneCrime)
            {
                if (Civilian.WillCallPolice)
                {
                    if (Civilian.CurrentTask?.Name != "CalmCallIn")
                    {
                        Civilian.CurrentTask = new CalmCallIn(Civilian, Player) { OtherTarget = HighestPriority?.Perpetrator };
                        Civilian.CurrentTask.Start();
                    }
                }
            }
        }
        Civilian.GameTimeLastUpdatedTask = Game.GameTime;
    }
    private WeaponInformation GetWeaponToIssue(bool IsGangMember)
    {
        WeaponInformation ToIssue;
        if (IsGangMember)
        {
            if (RandomItems.RandomPercent(70))
            {
                ToIssue = Weapons.GetRandomRegularWeapon(WeaponCategory.Pistol);
            }
            else
            {
                ToIssue = Weapons.GetRandomRegularWeapon(WeaponCategory.Melee);
            }
        }
        else if (RandomItems.RandomPercent(40))
        {
            ToIssue = Weapons.GetRandomRegularWeapon(WeaponCategory.Pistol);
        }
        else
        {
            if (RandomItems.RandomPercent(65))
            {
                ToIssue = Weapons.GetRandomRegularWeapon(WeaponCategory.Melee);
            }
            else
            {
                ToIssue = null;
            }
        }
        return ToIssue;
    }
}

