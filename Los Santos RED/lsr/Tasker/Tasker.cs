using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Tasker : ITaskerable
{
    private IEntityProvideable PedProvider;
    private ITargetable Player;
    private IWeapons Weapons;
    private ISettingsProvideable Settings;
    private List<PedExt> OtherTargets;
    private Cop PlayerGuard;
    private uint GameTimeLastGeneratedCrime;
    private uint RandomCrimeRandomTime;
    private bool IsIgnoredByPolice = false;
    private bool shouldGuardPlayer;
    private List<PedExt> PossibleTargets;
    private Cop ClosestCopToPlayer;

    private bool IsTimeToCreateCrime => Game.GameTime - GameTimeLastGeneratedCrime >= (Settings.SettingsManager.CivilianSettings.MinimumTimeBetweenRandomCrimes + RandomCrimeRandomTime);
    public Tasker(IEntityProvideable pedProvider, ITargetable player, IWeapons weapons, ISettingsProvideable settings)
    {
        PedProvider = pedProvider;
        Player = player;
        Weapons = weapons;
        Settings = settings;
        GameTimeLastGeneratedCrime = Game.GameTime;
        RandomCrimeRandomTime = RandomItems.GetRandomNumber(0, 240000);//between 0 and 4 minutes randomly added
    }
    public void RunPoliceTasks()
    {
        UpdateOtherTargets();
        foreach (Cop Cop in PedProvider.PoliceList.Where(x => x.CurrentTask != null && x.CurrentTask.ShouldUpdate).OrderBy(x => x.DistanceToPlayer))
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
            UpdateOtherTargets();
            foreach (Cop Cop in PedProvider.PoliceList.Where(x => x.Pedestrian.Exists() && x.HasBeenSpawnedFor >= 2000 && x.NeedsTaskAssignmentCheck).OrderBy(x => x.DistanceToPlayer))
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
            foreach (PedExt Civilian in PedProvider.CivilianList.Where(x => x.Pedestrian.Exists() && x.DistanceToPlayer <= 75f && x.NeedsTaskAssignmentCheck).OrderBy(x => x.DistanceToPlayer))//.OrderBy(x => x.GameTimeLastUpdatedTask).Take(10))//2//10)//2
            {
                try
                { 
                    if (Civilian.DistanceToPlayer <= 75f)
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
            foreach (PedExt Civilian in PedProvider.CivilianList.Where(x => x.Pedestrian.Exists() && x.DistanceToPlayer > 100f))
            {
                Civilian.CurrentTask = null;
            }
            GameFiber.Yield();
        }
    }
    public void CreateCrime()
    {
        PedExt Criminal = PedProvider.CivilianList.Where(x => x.Pedestrian.Exists() && x.DistanceToPlayer <= 85f && x.CanBeAmbientTasked && !x.IsInVehicle).OrderByDescending(x=> x.IsGangMember).FirstOrDefault();//150f
        if (Criminal != null && Criminal.Pedestrian.Exists())
        {
            PedExt Victim = PedProvider.CivilianList.Where(x => x.Pedestrian.Exists() && x.Handle != Criminal.Handle && x.DistanceToPlayer <= 85f && x.CanBeAmbientTasked && x.Pedestrian.Speed <= 2.0f && !x.IsGangMember).OrderBy(x=> x.Pedestrian.DistanceTo2D(Criminal.Pedestrian)).FirstOrDefault();//150f
            if (Victim != null && Victim.Pedestrian.Exists())
            {
                if (Settings.SettingsManager.CivilianSettings.ShowRandomCriminalBlips && Criminal.Pedestrian.Exists())
                {
                    Blip myBlip = Criminal.Pedestrian.AttachBlip();
                    myBlip.Color = Color.Red;
                    myBlip.Scale = 0.6f;
                    PedProvider.AddEntity(myBlip);
                }
                
                Criminal.CanBeAmbientTasked = false;
                Criminal.CurrentTask = new CommitCrime(Criminal, Player, GetWeaponToIssue(Criminal.IsGangMember), Victim);
                Criminal.CurrentTask.Start();
                EntryPoint.WriteToConsole("TASKER: GENERATED CRIME", 5);
                GameTimeLastGeneratedCrime = Game.GameTime;
                RandomCrimeRandomTime = RandomItems.GetRandomNumber(0, 240000);//between 0 and 4 minutes randomly added
            }
        }
    }

    private PedExt PedToAttack(Cop Cop)
    {
        PedExt MainTarget = null;
        if (Cop.Pedestrian.Exists() && Cop.DistanceToPlayer <= 200f)
        {
            if (Player.IsBusted)
            {
                if (PossibleTargets.Any(x => x.IsDeadlyChase))
                {
                    MainTarget = PossibleTargets.Where(x => x.Pedestrian.Exists() && x.IsWanted).OrderByDescending(x => x.IsDeadlyChase).ThenByDescending(x => x.ArrestingPedHandle == Cop.Handle).ThenBy(x=>x.IsBusted).ThenBy(x => x.Pedestrian.DistanceTo2D(Cop.Pedestrian)).FirstOrDefault();
                }
                else
                {
                    if (ClosestCopToPlayer == null || Cop.Handle != ClosestCopToPlayer.Handle)
                    {
                        MainTarget = PossibleTargets.Where(x => x.Pedestrian.Exists() && x.IsWanted).OrderByDescending(x => x.IsDeadlyChase).ThenByDescending(x => x.ArrestingPedHandle == Cop.Handle).ThenBy(x => x.IsBusted).ThenBy(x => x.Pedestrian.DistanceTo2D(Cop.Pedestrian)).FirstOrDefault();
                    }
                }

            }
            else if (Player.PoliceResponse.IsDeadlyChase)
            {
                MainTarget = PossibleTargets.Where(x => x.Pedestrian.Exists() && x.IsDeadlyChase && x.WantedLevel > Player.WantedLevel).OrderByDescending(x => x.IsDeadlyChase).ThenByDescending(x => x.ArrestingPedHandle == Cop.Handle).ThenBy(x => x.IsBusted).ThenBy(x => x.Pedestrian.DistanceTo2D(Cop.Pedestrian)).FirstOrDefault();
            }
            else
            {
                MainTarget = PossibleTargets.Where(x => x.Pedestrian.Exists() && x.WantedLevel > Player.WantedLevel).OrderByDescending(x => x.IsDeadlyChase).ThenByDescending(x => x.ArrestingPedHandle == Cop.Handle).ThenBy(x => x.IsBusted).ThenBy(x => x.Pedestrian.DistanceTo2D(Cop.Pedestrian)).FirstOrDefault();
            }
        }
        return MainTarget;
    }
    private PedExt PedToAttack_Old(Cop Cop)
    {
        PedExt MainTarget = null;
        if (Cop.Pedestrian.Exists() && Cop.DistanceToPlayer <= 200f)
        {
            if (Player.IsBusted)
            {
                if (PossibleTargets.Any(x => x.IsDeadlyChase))
                {
                    MainTarget = PossibleTargets.Where(x => x.Pedestrian.Exists() && x.IsWanted).OrderBy(x => x.IsBusted).ThenByDescending(x => x.WantedLevel).ThenBy(x => x.Pedestrian.DistanceTo2D(Cop.Pedestrian)).FirstOrDefault();
                }
                else
                {
                    if (ClosestCopToPlayer == null || Cop.Handle != ClosestCopToPlayer.Handle)
                    {
                        MainTarget = PossibleTargets.Where(x => x.Pedestrian.Exists() && x.IsWanted).OrderBy(x => x.IsBusted).ThenByDescending(x => x.WantedLevel).ThenBy(x => x.Pedestrian.DistanceTo2D(Cop.Pedestrian)).FirstOrDefault();
                    }
                }

            }
            else if (Player.PoliceResponse.IsDeadlyChase)
            {
                MainTarget = PossibleTargets.Where(x => x.Pedestrian.Exists() && x.IsDeadlyChase && x.WantedLevel > Player.WantedLevel).OrderBy(x => x.IsBusted).ThenByDescending(x => x.WantedLevel).ThenBy(x => x.Pedestrian.DistanceTo2D(Cop.Pedestrian)).FirstOrDefault();
            }
            else
            {
                MainTarget = PossibleTargets.Where(x => x.Pedestrian.Exists() && x.WantedLevel > Player.WantedLevel).OrderBy(x => x.IsBusted).ThenByDescending(x => x.WantedLevel).ThenBy(x => x.Pedestrian.DistanceTo2D(Cop.Pedestrian)).FirstOrDefault();
            }
        }
        return MainTarget;
    }
    private void UpdateOtherTargets()
    {
        PossibleTargets = PedProvider.CivilianList.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive && x.IsWanted && x.DistanceToPlayer <= 200f).ToList();//150f
        ClosestCopToPlayer = PedProvider.PoliceList.Where(x => x.Pedestrian.Exists() && !x.IsInVehicle && x.DistanceToPlayer <= 30f).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
    }
    private void UpdateCurrentTask(Cop Cop)//this should be moved out?
    {
        if (Cop.DistanceToPlayer <= Player.ActiveDistance)// && !Cop.IsInHelicopter)//heli, dogs, boats come next?
        {
            PedExt MainTarget = PedToAttack(Cop);
            if (MainTarget != null)//if (OtherTargets.Any(x=> x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Cop.Pedestrian) <= 150f) && Cop.DistanceToPlayer <= 150f && (PlayerGuard == null || Cop.Handle != PlayerGuard.Handle))
            {
                //if (Cop.CurrentTask?.Name != "ApprehendOther")
                //{
                //    EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to ApprehendOther", 3);
                //    Cop.CurrentTask = new ApprehendOther(Cop, Player) { OtherTargets = OtherTargets };
                //    Cop.CurrentTask.Start();
                //}
                if (Cop.CurrentTask?.Name != "AIApprehend")
                {
                    EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to AIApprehend", 3);
                    Cop.CurrentTask = new AIApprehend(Cop, Player) { OtherTarget = MainTarget };
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
                            Cop.CurrentTask.Start();
                        }
                    }
                    else
                    {
                        if (Cop.DistanceToPlayer <= 150f)//200f
                        {
                            if (Player.PoliceResponse.IsDeadlyChase && (Player.PoliceResponse.IsWeaponsFree || !Player.IsAttemptingToSurrender))
                            {
                                if (Cop.CurrentTask?.Name != "Kill")
                                {
                                    EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Kill", 3);
                                    Cop.CurrentTask = new Kill(Cop, Player);
                                    Cop.CurrentTask.Start();
                                }
                            }
                            else
                            {
                                if (Cop.CurrentTask?.Name != "Chase")
                                {
                                    EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Chase", 3);
                                    Cop.CurrentTask = new Chase(Cop, Player);
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
                                Cop.CurrentTask.Start();
                            }
                        }
                    }
                }
                else if (Player.Investigation.IsActive)
                {
                    if (Cop.CurrentTask?.Name != "Investigate")
                    {
                        EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Investigate", 3);
                        Cop.CurrentTask = new Investigate(Cop, Player);
                        Cop.CurrentTask.Start();
                    }
                }
                else
                {
                    if (Cop.CurrentTask?.Name != "Idle")// && Cop.WasModSpawned)
                    {
                        EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Idle", 3);
                        Cop.CurrentTask = new Idle(Cop, Player, PedProvider);
                        Cop.CurrentTask.Start();
                    }

                }
            }
        }
        else
        {
            if (Cop.CurrentTask?.Name != "Idle" && Cop.WasModSpawned)
            {
                EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Idle", 3);
                Cop.CurrentTask = new Idle(Cop, Player, PedProvider);
                Cop.CurrentTask.Start();
            }
            else
            {
                //if(Cop.CurrentTask != null && Cop.Pedestrian.Exists())
                //{
                //    EntryPoint.WriteToConsole($"TASKER: Cop Task Changed from {Cop.CurrentTask?.Name} to null", 3);
                //    Cop.Pedestrian.Tasks.Clear();
                //    Cop.CurrentTask = null;
                //}
                
            }
        }
        Cop.GameTimeLastUpdatedTask = Game.GameTime;
    }
    private void UpdateCurrentTask(PedExt Civilian)//this should be moved out?
    {
        if(Civilian.DistanceToPlayer <= 75f && Civilian.IsBusted)
        {
            if (Civilian.CurrentTask?.Name != "GetArrested")
            {
                //VehicleExt ToGoTo = PedProvider.PoliceVehicleList.Where(x => x.Vehicle.Exists() && (x.Vehicle.IsSeatFree(1) || x.Vehicle.IsSeatFree(2)) && x.Vehicle.Speed == 0f).OrderBy(x => x.Vehicle.DistanceTo2D(Civilian.Pedestrian)).FirstOrDefault();
                Civilian.CurrentTask = new GetArrested(Civilian, Player, PedProvider);
                Civilian.CurrentTask.Start();
            }
        }
        else if (Civilian.DistanceToPlayer <= 75f && Civilian.CanBeTasked && Civilian.CanBeAmbientTasked)//50f
        {
            WitnessedCrime HighestPriority = Civilian.OtherCrimesWitnessed.OrderBy(x => x.Crime.Priority).ThenByDescending(x => x.GameTimeLastWitnessed).FirstOrDefault();
            bool SeenScaryCrime = Civilian.PlayerCrimesWitnessed.Any(x => x.ScaresCivilians && x.CanBeReportedByCivilians) || Civilian.OtherCrimesWitnessed.Any(x => x.Crime.ScaresCivilians && x.Crime.CanBeReportedByCivilians);
            bool SeenAngryCrime = Civilian.PlayerCrimesWitnessed.Any(x => x.AngersCivilians && x.CanBeReportedByCivilians) || Civilian.OtherCrimesWitnessed.Any(x => x.Crime.AngersCivilians && x.Crime.CanBeReportedByCivilians);
            if (SeenScaryCrime || SeenAngryCrime)
            {
                if (Civilian.WillCallPolice)
                {
                    if (Civilian.CurrentTask?.Name != "CallIn")
                    {
                        Civilian.CurrentTask = new CallIn(Civilian, Player) { OtherTarget = HighestPriority?.Perpetrator };
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




    private void CopToPerpAssigning()
    {
        foreach (PedExt criminal in PossibleTargets.Where(x => x.Pedestrian.Exists()))
        {

        }
        //List<uint> PerpsAssigned = new List<uint>();
        //foreach(Cop cop in PedProvider.PoliceList.Where(x => x.Pedestrian.Exists() && x.DistanceToPlayer <= 200f))
        //{
        //    float closestDistance = 999f;
        //    PedExt ClosestCriminal = null;


        //    float closestBustedDistance = 999f;
        //    PedExt ClosestBustedCriminal = null;

        //    float closestDeadlyDistance = 999f;
        //    PedExt ClosestDeadlyCriminal = null;

        //    float closestDeadlyBustedDistance = 999f;
        //    PedExt ClosestDeadlyBustedCriminal = null;
        //    foreach (PedExt criminal in PossibleTargets.Where(x=> x.Pedestrian.Exists()))
        //    {
        //        float distanceToCriminal = cop.Pedestrian.DistanceTo2D(criminal.Pedestrian);
        //        if (criminal.IsDeadlyChase)
        //        {
        //            if (criminal.IsBusted)
        //            {
        //                if (distanceToCriminal <= closestDeadlyBustedDistance)
        //                {
        //                    closestDeadlyBustedDistance = distanceToCriminal;
        //                    ClosestDeadlyBustedCriminal = criminal;
        //                }
        //            }
        //            else
        //            {
        //                if (distanceToCriminal <= closestDeadlyDistance)
        //                {
        //                    closestDeadlyDistance = distanceToCriminal;
        //                    ClosestDeadlyCriminal = criminal;
        //                }
        //            }
        //        }
        //        if(criminal.IsBusted)
        //        {
        //            if (distanceToCriminal <= closestBustedDistance)
        //            {
        //                closestBustedDistance = distanceToCriminal;
        //                ClosestBustedCriminal = criminal;
        //            }
        //        }
        //        else
        //        {
        //            if (distanceToCriminal <= closestDistance)
        //            {
        //                closestDistance = distanceToCriminal;
        //                ClosestCriminal = criminal;
        //            }
        //        }

        //    }

        //    if(ClosestDeadlyCriminal != null)
        //    {
        //        if(Player.PoliceResponse.IsDeadlyChase)
        //        {
        //            if(closestDeadlyDistance <= cop.DistanceToPlayer)
        //            {
        //                //go after ped
        //            }
        //            else
        //            {
        //                //go after player?
        //            }
        //        }
        //    }
        //    else if (ClosestDeadlyBustedCriminal != null)
        //    {

        //    }

        //}
    }
    private PedExt PedToAttackNew(Cop Cop)
    {
        PedExt MainTarget = null;

        if (Cop.Pedestrian.Exists() && Cop.DistanceToPlayer <= 200f)
        {
            float closestDistance = 999f;
            PedExt ClosestCrimnal = null;

            float closestDeadlyDistance = 999f;
            PedExt ClosestDeadlyCrimnal = null;

            foreach (PedExt criminal in PossibleTargets.Where(x => x.Pedestrian.Exists() && x.IsWanted))
            {
                float distanceToCriminal = Cop.Pedestrian.DistanceTo2D(criminal.Pedestrian);
                if (criminal.IsDeadlyChase)
                {
                    closestDeadlyDistance = distanceToCriminal;
                }
                if (distanceToCriminal <= closestDeadlyDistance)
                {
                    closestDeadlyDistance = distanceToCriminal;
                    ClosestDeadlyCrimnal = criminal;
                }
                if (distanceToCriminal <= closestDistance)
                {
                    closestDistance = distanceToCriminal;
                    ClosestCrimnal = criminal;
                }
            }
        }
        return MainTarget;

        //assign first deadly or not, then closeset, if busted, make sure none o\thers are assigned to him already!
    }

}

