using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SecurityGuardBrain : PedBrain
{
    private SecurityGuard SecurityGuard;
    private uint GameTimeLastSeenCrime;
    private bool printDebug = true;
    private int debugLevel => printDebug ? 5 : 6;

    public SecurityGuardBrain(SecurityGuard securityGuard, ISettingsProvideable settings, IEntityProvideable world, IWeapons weapons) : base(securityGuard,settings,world, weapons)
    {
        SecurityGuard = securityGuard;
        Settings = settings;
        World = world;
    }
    public override void Setup()
    {

    }
    public override void Dispose()
    {

    }


    public override void Update(ITargetable player, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        PlacesOfInterest = placesOfInterest;
        if (PedExt.CanBeTasked && PedExt.CanBeAmbientTasked)
        {
            if (PedExt.NeedsTaskAssignmentCheck)
            {
                UpdateCurrentTask();
            }
            if (PedExt.CurrentTask != null && PedExt.CurrentTask.ShouldUpdate)
            {
                PedExt.UpdateTask(null);
                GameFiber.Yield();
            }
        }
        else if (!PedExt.IsBusted && !PedExt.CanBeTasked)
        {
            if (PedExt.CurrentTask != null)
            {
                PedExt.CurrentTask = null;
            }
        }
    }



    //public override void Update(ITargetable player, IPlacesOfInterest placesOfInterest)
    //{
    //    Player = player;
    //    PlacesOfInterest = placesOfInterest;

    //    if (PedExt.CanBeTasked && PedExt.CanBeAmbientTasked)
    //    {
    //        UpdateTask();
    //    }
    //    else if (!PedExt.CanBeTasked)
    //    {
    //        RemoveTask();
    //    }
    //}
    //private void UpdateTask()
    //{
    //    if (PedExt.DistanceToPlayer >= 230f)
    //    {
    //        PedExt.CurrentTask = null;
    //        return;
    //    }
    //    if (PedExt.NeedsTaskAssignmentCheck)
    //    {
    //        if (PedExt.DistanceToPlayer <= 200f)
    //        {
    //            UpdateCurrentTask();//has yields if it does anything
    //        }
    //        else if (PedExt.CurrentTask != null)
    //        {
    //            PedExt.CurrentTask = null;
    //        }
    //    }
    //    if (PedExt.CurrentTask != null && PedExt.CurrentTask.ShouldUpdate)
    //    {
    //        PedExt.UpdateTask(null);
    //        GameFiber.Yield();
    //    }
    //}
    private void RemoveTask()
    {
        if (PedExt.CurrentTask != null)
        {
            PedExt.CurrentTask = null;
        }
    }
    private void UpdateCurrentTask()
    {
        if (PedExt.DistanceToPlayer <= 150f)//50f
        {
            PedExt.PedReactions.Update(Player);
            if(PedExt.PedReactions.ReactionTier == ReactionTier.Intense)
            {
                if (SecurityGuard.WeaponInventory.IsArmed)
                {
                    if (SecurityGuard.PlayerPerception.HasSeenTargetWithin(30000))
                    {
                        SetFight();
                    }
                    else
                    {
                        SetLocate();
                    }
                }
                else
                {
                    SetFlee();
                }
            }
            else if (PedExt.PedReactions.ReactionTier == ReactionTier.Alerted)
            {
                if (SecurityGuard.PlayerPerception.HasSeenTargetWithin(30000))
                {
                    SetApprehend();
                }
                else
                {
                    SetLocate();
                }
            }
            else if (PedExt.CanAttackPlayer)
            {
                if (SecurityGuard.PlayerPerception.HasSeenTargetWithin(30000))
                {
                    SetChase();// SetFight();
                }
                else
                {
                    SetLocate();
                }
            }
            else if (SecurityGuard.PedAlerts.IsAlerted)// Cop.BodiesSeen.Any() || )
            {
                SetInvestigate();
            }
            else if (PedExt.PedReactions.ReactionTier == ReactionTier.Mundane)
            {
                SetCalmCallIn();
            }
            else if (PedExt.WasModSpawned && PedExt.PedReactions.ReactionTier == ReactionTier.None)
            {
                SetIdle();
            }
            HandleCrimeReports();
        }
        else if (PedExt.WasModSpawned && PedExt.PedReactions.ReactionTier == ReactionTier.None)
        {
            SetIdle();
        }
        PedExt.GameTimeLastUpdatedTask = Game.GameTime;
    }

    private void SetApprehend()
    {
        if(PedExt.PedReactions.PrimaryPedReaction?.IsReactingToPlayer == false)//  PedExt.PedReactions.HighestPriorityCrime?.Perpetrator != null)
        {
            SetAIApprehend();
        }
        else
        {
            SetChase();
        }
    }
    private void SetChase()
    {
        SecurityGuard.WeaponInventory.SetLessLethal();
        if (PedExt.CurrentTask?.Name == "Chase")
        {
            return;
        }
        SecurityGuard.CurrentTask = new Chase(SecurityGuard, Player, World, SecurityGuard, Settings) { UseWantedLevel = false };
        SecurityGuard.WeaponInventory.Reset();
        GameFiber.Yield();//TR Added back 4
        SecurityGuard.CurrentTask.Start();
       // EntryPoint.WriteToConsole($"SECURITY SET Chase {PedExt.Handle}", debugLevel);
    }
    private void SetInvestigate()
    {
        if (SecurityGuard.CurrentTask?.Name != "Investigate")
        {
            // EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Investigate", 3);
            SecurityGuard.CurrentTask = new PoliceGeneralInvestigate(SecurityGuard, SecurityGuard, Player, World, null, PlacesOfInterest, Settings, Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringInvestigate, SecurityGuard);//Cop.CurrentTask = new Investigate(Cop, Player, Settings, World);
            SecurityGuard.WeaponInventory.Reset();
            GameFiber.Yield();//TR Added back 4
            SecurityGuard.CurrentTask.Start();
        }
    }
    private void SetAIApprehend()
    {
        SecurityGuard.WeaponInventory.SetLessLethal();
        if (PedExt.CurrentTask?.Name == "AIApprehend")
        {
            return;
        }
        SecurityGuard.CurrentTask = new AIApprehend(SecurityGuard, Player, SecurityGuard, Settings) { OtherTarget = PedExt.PedReactions.PrimaryPedReaction?.ReactingToPed, UseWantedLevel = false };
        SecurityGuard.WeaponInventory.Reset();
        GameFiber.Yield();//TR Added back 4
        SecurityGuard.CurrentTask.Start();
        //EntryPoint.WriteToConsole($"SECURITY SET AIApprehend {PedExt.Handle}", debugLevel);
    }
    private void HandleCrimeReports()
    {
        if (GameTimeLastSeenCrime == 0 && (PedExt.PlayerCrimesWitnessed.Any() || PedExt.OtherCrimesWitnessed.Any() || PedExt.PedAlerts.HasSeenUnconsciousPed))
        {
            GameTimeLastSeenCrime = Game.GameTime;
            //EntryPoint.WriteToConsole("SECURITY SEEN FIRST CRIME", debugLevel);
        }
        if (GameTimeLastSeenCrime != 0 && Game.GameTime - GameTimeLastSeenCrime >= 10000 && PedExt.Pedestrian.Exists() && !PedExt.IsDead && !PedExt.IsUnconscious && !PedExt.Pedestrian.IsRagdoll && PedExt.DistanceToPlayer <= 40f)
        {
            GameTimeLastSeenCrime = 0;
            PedExt.ReportCrime(Player);
           // EntryPoint.WriteToConsole("SECURITY REPORTED CRIME", debugLevel);
        }
    }
    //private void SetFlee()
    //{
    //    if (PedExt.CurrentTask?.Name == "Flee")
    //    {
    //        return;
    //    }
    //    PedExt.CurrentTask = new Flee(PedExt, Player) { OtherTarget = PedExt.PedReactions.HighestPriorityCrime?.Perpetrator };
    //    GameFiber.Yield();//TR Added back 7
    //    PedExt.CurrentTask?.Start();
    //    //EntryPoint.WriteToConsole($"SECURITY SET FLEE {PedExt.Handle}", debugLevel);
    //}

    private void SetLocate()
    {
        if (PedExt.CurrentTask?.Name == "GeneralLocate")
        {
            return;
        }
        PedExt.CurrentTask = new GeneralLocate(PedExt, PedExt, Player, World,null,PlacesOfInterest,Settings,true,null,true) { OtherTarget = PedExt.PedReactions.HighestPriorityCrime?.Perpetrator };
        GameFiber.Yield();//TR Added back 7
        PedExt.CurrentTask?.Start();
        //EntryPoint.WriteToConsole($"SECURITY SET GeneralLocate {PedExt.Handle}", debugLevel);
    }
    //private void SetFight()
    //{
    //    SecurityGuard.WeaponInventory.SetDeadly(false);
    //    if (PedExt.CurrentTask?.Name == "Fight")
    //    {
    //        return;
    //    }
    //    PedExt.CurrentTask?.Stop();
    //    PedExt.CurrentTask = new Fight(PedExt, Player, null) { OtherTarget = PedExt.PedReactions.HighestPriorityCrime?.Perpetrator };//gang memebrs already have guns
    //    GameFiber.Yield();//TR Added back 7
    //    PedExt.CurrentTask?.Start();
    //    //EntryPoint.WriteToConsole($"SECURITY SET FIGHT {PedExt.Handle}", debugLevel);
    //}
    //private void SetCalmCallIn()
    //{
    //    if (PedExt.CurrentTask?.Name == "CalmCallIn")
    //    {
    //        return;
    //    }
    //    PedExt.CurrentTask = new CalmCallIn(PedExt, Player, Settings);
    //    GameFiber.Yield();//TR Added back 4
    //    PedExt.CurrentTask.Start();
    //    EntryPoint.WriteToConsole($"SECURITY SET CALM CALL IN {PedExt.Handle}", debugLevel);
    //}
    //private void SetIdle()
    //{
    //    if (PedExt.CurrentTask?.Name == "Idle")
    //    {
    //        return;
    //    }
    //    PedExt.CurrentTask = new GeneralIdle(PedExt, PedExt, Player, World, new List<VehicleExt>() { PedExt.AssignedVehicle },  PlacesOfInterest, Settings,false,false,false, true);
    //    SecurityGuard.WeaponInventory.Reset();
    //    SecurityGuard.WeaponInventory.SetDefault();
    //    GameFiber.Yield();//TR Added back 4
    //    PedExt.CurrentTask.Start();
    //}
}