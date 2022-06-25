﻿using LosSantosRED.lsr.Helper;
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
        GangReputation gr = Player.GangRelationships.GetReputation(GangMember.Gang);
        bool isHostile = gr.GangRelationship == GangRespect.Hostile;
        bool arePoliceNearby = Player.ClosestPoliceDistanceToPlayer <= 350f;

        bool isNearHomeTerritory = false;
        if(Player.CurrentLocation.CurrentZone?.Gangs?.Any(x=>x.ID == GangMember.Gang?.ID) == true)
        {
            isNearHomeTerritory = true;
        }

       
        if (GangMember.IsBusted)
        {
            if (GangMember.DistanceToPlayer <= 275f)
            {
                SetArrested(GangMember);
            }
        }
        else if (GangMember.DistanceToPlayer <= 275f && GangMember.CanBeTasked && GangMember.CanBeAmbientTasked)//50f
        {
            //WitnessedCrime HighestPriority = GangMember.OtherCrimesWitnessed.OrderBy(x => x.Crime.Priority).ThenByDescending(x => x.GameTimeLastWitnessed).FirstOrDefault();
            bool SeenPlayerReactiveCrime = GangMember.PlayerCrimesWitnessed.Any(x => (x.ScaresCivilians || x.AngersCivilians) && x.CanBeReportedByCivilians);
            bool SeenOtherReactiveCrime = GangMember.OtherCrimesWitnessed.Any(x => (x.Crime.ScaresCivilians || x.Crime.AngersCivilians) && x.Crime.CanBeReportedByCivilians);



            WitnessedCrime HighestPriorityOtherCrime = GangMember.OtherCrimesWitnessed.OrderBy(x => x.Crime.Priority).ThenByDescending(x => x.GameTimeLastWitnessed).FirstOrDefault();
            //Crime HighestPriorityPlayer = Civilian.PlayerCrimesWitnessed.OrderBy(x => x.Priority).FirstOrDefault();


            int PlayerCrimePriority = 99;
            foreach (Crime playerCrime in GangMember.PlayerCrimesWitnessed.Where(x => x.CanBeReportedByCivilians))
            {
                if (playerCrime.Priority < PlayerCrimePriority)
                {
                    PlayerCrimePriority = playerCrime.Priority;
                }
            }
            if (PlayerCrimePriority < HighestPriorityOtherCrime?.Crime?.Priority)
            {
                HighestPriorityOtherCrime = null;
            }
            else if (PlayerCrimePriority == HighestPriorityOtherCrime?.Crime?.Priority && GangMember.DistanceToPlayer <= 30f)
            {
                HighestPriorityOtherCrime = null;
            }






            if (SeenPlayerReactiveCrime)
            {
                if (GangMember.WillFight && !arePoliceNearby && ((isHostile && isNearHomeTerritory) || GangMember.HasBeenHurtByPlayer || GangMember.HasBeenCarJackedByPlayer || gr.RecentlyAttacked)) //if (GangMember.WillFight && (!arePoliceNearby || (isHostile && (GangMember.HasBeenHurtByPlayer || GangMember.HasBeenCarJackedByPlayer || gr.RecentlyAttacked))))
                {
                    SetFight(GangMember, HighestPriorityOtherCrime);
                }
                else
                {
                    SetFlee(GangMember, HighestPriorityOtherCrime);
                }
            }
            else if (SeenOtherReactiveCrime)
            {
                if (GangMember.WillFight && !arePoliceNearby && Player.IsNotWanted && !Player.Investigation.IsActive)
                {
                    SetFight(GangMember, HighestPriorityOtherCrime);
                }
                else
                {
                    SetFlee(GangMember, HighestPriorityOtherCrime);
                }
            }
            else
            {
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

