using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Tasker
{
    private IEntityProvideable PedProvider;
    private ITargetable Player;
    private IWeapons Weapons;
    private ISettingsProvideable Settings;
    private List<PedExt> OtherTargets;
    public Tasker(IEntityProvideable pedProvider, ITargetable player, IWeapons weapons, ISettingsProvideable settings)
    {
        PedProvider = pedProvider;
        Player = player;
        Weapons = weapons;
        Settings = settings;
    }
    public void RunPoliceTasks()
    {
        UpdateOtherTargets();
        foreach (Cop Cop in PedProvider.PoliceList.Where(x => x.CurrentTask != null && x.CurrentTask.ShouldUpdate).OrderBy(x => x.DistanceToPlayer))
        {
            Cop.UpdateTask(OtherTargets);
            GameFiber.Yield();
        }
    }
    public void RunCiviliansTasks()
    {
        foreach (PedExt Ped in PedProvider.CivilianList.Where(x => x.CurrentTask != null && x.CurrentTask.ShouldUpdate).OrderBy(x => x.DistanceToPlayer))//.OrderBy(x => x.CurrentTask.GameTimeLastRan))
        {
            Ped.UpdateTask(null);
            GameFiber.Yield();
        }
    }
    public void UpdatePoliceTasks()
    {
        if (Settings.SettingsManager.PoliceSettings.ManageTasking)
        {
            UpdateOtherTargets();
            foreach (Cop Cop in PedProvider.PoliceList.Where(x => x.Pedestrian.Exists() && x.HasBeenSpawnedFor >= 2000 && x.NeedsTaskAssignmentCheck).OrderBy(x => x.DistanceToPlayer))
            {
                UpdateCurrentTask(Cop);
                GameFiber.Yield();
            }
        }
    }
    public void UpdateCivilianTasks()
    {
        if (Settings.SettingsManager.CivilianSettings.ManageCivilianTasking)
        {
            foreach (PedExt Civilian in PedProvider.CivilianList.Where(x => x.Pedestrian.Exists() && x.DistanceToPlayer <= 75f && x.NeedsTaskAssignmentCheck).OrderBy(x => x.DistanceToPlayer))//.OrderBy(x => x.GameTimeLastUpdatedTask).Take(10))//2//10)//2
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
            foreach (PedExt Civilian in PedProvider.CivilianList.Where(x => x.Pedestrian.Exists() && x.DistanceToPlayer > 100f))
            {
                Civilian.CurrentTask = null;
            }
            GameFiber.Yield();
        }
    }
    private void UpdateOtherTargets()
    {
        OtherTargets = PedProvider.CivilianList.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive && (x.WantedLevel > Player.WantedLevel || Player.IsBusted) && x.DistanceToPlayer <= 60f).ToList();
        //if (OtherTargets.Any())//will reset tasks, if you set it on update they will constantly reset tasks, might need to be set once? or at the beginning and then only turned off when needed?, cant be set very well
        //{
        //    Game.LocalPlayer.IsIgnoredByPolice = true;
        //}
        //else
        //{
        //    Game.LocalPlayer.IsIgnoredByPolice = false;
        //}
    }
    private void UpdateCurrentTask(Cop Cop)//this should be moved out?
    {
        if (Cop.DistanceToPlayer <= Player.ActiveDistance)// && !Cop.IsInHelicopter)//heli, dogs, boats come next?
        {
            if (OtherTargets.Any() && Cop.DistanceToPlayer <= 60f)
            {
                if (Cop.CurrentTask?.Name != "ApprehendOther")
                {
                    EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to ApprehendOther", 3);
                    Cop.CurrentTask = new ApprehendOther(Cop, Player) { OtherTargets = OtherTargets };
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
                        Cop.CurrentTask = new Idle(Cop, Player);
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
                Cop.CurrentTask = new Idle(Cop, Player);
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
        if (Civilian.DistanceToPlayer <= 75f && Civilian.CanBeTasked && Civilian.CanBeAmbientTasked)//50f
        {
            //bool SeenAnyReportableCrime = Civilian.CrimesWitnessed.Any(x => x.CanBeReportedByCivilians);
            bool SeenScaryCrime = Civilian.CrimesWitnessed.Any(x => x.ScaresCivilians && x.CanBeReportedByCivilians);
            bool SeenAngryCrime = Civilian.CrimesWitnessed.Any(x => x.AngersCivilians && x.CanBeReportedByCivilians);
            if (SeenScaryCrime || SeenAngryCrime)
            {
                if (Civilian.WillCallPolice)
                {
                    if (Civilian.CurrentTask?.Name != "CallIn")
                    {
                        Civilian.CurrentTask = new CallIn(Civilian, Player);
                        Civilian.CurrentTask.Start();
                    }
                }
                else if (Civilian.WillFight)
                {
                    if (SeenAngryCrime)
                    {
                        if (Civilian.CurrentTask?.Name != "Fight")
                        {
                            WeaponInformation ToIssue = Civilian.IsGangMember ? Weapons.GetRandomRegularWeapon(WeaponCategory.Pistol) : Weapons.GetRandomRegularWeapon(WeaponCategory.Melee);
                            Civilian.CurrentTask = new Fight(Civilian, Player, ToIssue); ;
                            Civilian.CurrentTask.Start();
                        }
                    }
                    else
                    {
                        if (Civilian.CurrentTask?.Name != "Flee")
                        {
                            Civilian.CurrentTask = new Flee(Civilian, Player);
                            Civilian.CurrentTask.Start();
                        }
                    }
                }
                else
                {
                    if (Civilian.CurrentTask?.Name != "Flee")
                    {
                        Civilian.CurrentTask = new Flee(Civilian, Player);
                        Civilian.CurrentTask.Start();
                    }
                }
            }
            else if (Civilian.IsFedUpWithPlayer && Civilian.WillFight)
            {
                if (Civilian.CurrentTask?.Name != "Fight")
                {
                    WeaponInformation ToIssue = Civilian.IsGangMember ? Weapons.GetRandomRegularWeapon(WeaponCategory.Pistol) : Weapons.GetRandomRegularWeapon(WeaponCategory.Melee);
                    Civilian.CurrentTask = new Fight(Civilian, Player, ToIssue); ;
                    Civilian.CurrentTask.Start();
                }
            }     
        }
        Civilian.GameTimeLastUpdatedTask = Game.GameTime;




        //if (Civilian.DistanceToPlayer <= 75f && Civilian.CanBeTasked)//50f
        //{
        //    if (Civilian.CurrentTask?.Name != "Fight" && Civilian.HasSeenPlayerCommitCrime)
        //    {
        //        if (Civilian.WillCallPolice && Player.IsNotWanted && Civilian.CrimesWitnessed.Any(x => (x.ScaresCivilians || x.AngersCivilians) && x.CanBeReportedByCivilians))
        //        {
        //            if (Civilian.CurrentTask?.Name != "CallIn")
        //            {
        //                Civilian.CurrentTask = new CallIn(Civilian, Player);
        //                Civilian.CurrentTask.Start();
        //            }
        //        }
        //        else if (Civilian.CurrentTask?.Name != "Fight" && Civilian.WillFight && Player.IsNotWanted && Civilian.CrimesWitnessed.Any(x => x.AngersCivilians))
        //        {
        //            WeaponInformation ToIssue = Civilian.IsGangMember ? Weapons.GetRandomRegularWeapon(WeaponCategory.Pistol) : Weapons.GetRandomRegularWeapon(WeaponCategory.Melee);
        //            Civilian.CurrentTask = new Fight(Civilian, Player, ToIssue); ;
        //            Civilian.CurrentTask.Start();
        //        }
        //        else
        //        {
        //            if (Civilian.CurrentTask?.Name != "Flee" && Civilian.CurrentTask?.Name != "Fight" && Civilian.CrimesWitnessed.Any(x => x.ScaresCivilians))
        //            {
        //                Civilian.CurrentTask = new Flee(Civilian, Player);
        //                Civilian.CurrentTask.Start();
        //            }
        //        }
        //    }
        //    else if (Civilian.CurrentTask?.Name != "Fight" && Civilian.IsFedUpWithPlayer)
        //    {
        //        WeaponInformation ToIssue = Civilian.IsGangMember ? Weapons.GetRandomRegularWeapon(WeaponCategory.Pistol) : Weapons.GetRandomRegularWeapon(WeaponCategory.Melee);
        //        Civilian.CurrentTask = new Fight(Civilian, Player, ToIssue); ;
        //        Civilian.CurrentTask.Start();
        //    }
        //}
        //Civilian.GameTimeLastUpdatedTask = Game.GameTime;
    }
}

