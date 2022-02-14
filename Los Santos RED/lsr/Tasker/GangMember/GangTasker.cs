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


public class GangTasker
{
    private IEntityProvideable PedProvider;
    private ITargetable Player;
    private IWeapons Weapons;
    private ISettingsProvideable Settings;
    private Tasker Tasker;
    private IPlacesOfInterest PlacesOfInterest;
    public GangTasker(Tasker tasker, IEntityProvideable pedProvider, ITargetable player, IWeapons weapons, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest)
    {
        Tasker = tasker;
        PedProvider = pedProvider;
        Player = player;
        Weapons = weapons;
        Settings = settings;
        PlacesOfInterest = placesOfInterest;
    }

    public void Setup()
    {

    }
    public void Update()
    {
        if (Settings.SettingsManager.GangSettings.ManageTasking)
        {
            Tasker.ExpireSeatAssignments();
           //bool anyCopsNearPosition = PedProvider.Pedestrians.PoliceList.Any(x => NativeHelper.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, x.CellX, x.CellY, 4));
            foreach (GangMember gangMember in PedProvider.Pedestrians.GangMemberList.Where(x => x.Pedestrian.Exists()))
            {
                try
                {
                    if (gangMember.DistanceToPlayer >= 230f)
                    {
                        continue;
                    }
                    if (gangMember.NeedsTaskAssignmentCheck)
                    {
                        if (gangMember.DistanceToPlayer <= 200f)
                        {
                            UpdateCurrentTask(gangMember);//has yields if it does anything
                        }
                    }
                    if (gangMember.CurrentTask != null && gangMember.CurrentTask.ShouldUpdate)
                    {
                        gangMember.UpdateTask(null);
                        GameFiber.Yield();
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Setting Civilian Task");
                }
            }
        }
    }
    private void UpdateCurrentTask(GangMember GangMember)//this should be moved out?
    {
        bool isHostile = Player.GangRelationships.IsHostile(GangMember.Gang);
        if (GangMember.IsBusted)
        {
            if (GangMember.DistanceToPlayer <= 75f)
            {
                SetArrested(GangMember);
            }
        }
        else if (GangMember.DistanceToPlayer <= 175f && GangMember.CanBeTasked && GangMember.CanBeAmbientTasked)//50f
        {
            WitnessedCrime HighestPriority = GangMember.OtherCrimesWitnessed.OrderBy(x => x.Crime.Priority).ThenByDescending(x => x.GameTimeLastWitnessed).FirstOrDefault();
            bool SeenReactiveCrime = GangMember.PlayerCrimesWitnessed.Any(x => (x.ScaresCivilians || x.AngersCivilians) && x.CanBeReportedByCivilians) || GangMember.OtherCrimesWitnessed.Any(x => (x.Crime.ScaresCivilians || x.Crime.AngersCivilians) && x.Crime.CanBeReportedByCivilians);
            if (SeenReactiveCrime)
            {
                if (GangMember.WillFight && (Player.IsNotWanted || isHostile || GangMember.HasBeenHurtByPlayer || GangMember.HasBeenCarJackedByPlayer))
                {
                    SetFight(GangMember, HighestPriority);
                }
                else
                {
                    SetFlee(GangMember, HighestPriority);
                }
            }
            else
            {
                //if (Player.IsWanted && (GangMember.CurrentlyViolatingWantedLevel > 0 || GangMember.DistanceToPlayer <= 60f) && !isHostile)
                //{
                //    SetFlee(GangMember, HighestPriority);
                //}
                //else 
                
                if (GangMember.WasModSpawned && GangMember.CurrentTask == null)
                {
                    SetIdle(GangMember);
                }
            }
        }
        GangMember.GameTimeLastUpdatedTask = Game.GameTime;
    }
    private void SetFlee(GangMember GangMember, WitnessedCrime HighestPriority)
    {
        if (GangMember.CurrentTask?.Name != "GangFlee")
        {
            GangMember.CurrentTask = new GangFlee(GangMember, Player) { OtherTarget = HighestPriority?.Perpetrator };
            GameFiber.Yield();//TR Added back 7
            GangMember.CurrentTask.Start();
        }
    }
    private void SetFight(GangMember GangMember, WitnessedCrime HighestPriority)
    {
        if (GangMember.CurrentTask?.Name != "GangFight")
        {
            GangMember.CurrentTask = new GangFight(GangMember, Player, null) { OtherTarget = HighestPriority?.Perpetrator };
            GameFiber.Yield();//TR Added back 7
            GangMember.CurrentTask.Start();
        }
    }
    private void SetIdle(GangMember GangMember)
    {
        if (GangMember.CurrentTask?.Name != "GangIdle")
        {
            EntryPoint.WriteToConsole($"TASKER: gm {GangMember.Pedestrian.Handle} Task Changed from {GangMember.CurrentTask?.Name} to Idle", 3);
            GangMember.CurrentTask = new GangIdle(GangMember, Player, PedProvider, Tasker, PlacesOfInterest);
            GameFiber.Yield();//TR Added back 4
            GangMember.CurrentTask.Start();
        }
    }
    private void SetArrested(GangMember GangMember)
    {
        if (GangMember.CurrentTask?.Name != "GetArrested")
        {
            GangMember.CurrentTask = new GetArrested(GangMember, Player, PedProvider, Tasker);
            GameFiber.Yield();//TR Added back 7
            GangMember.CurrentTask.Start();
        }
    }
}


