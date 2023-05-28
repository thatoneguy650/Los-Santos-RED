using LosSantosRED.lsr.Helper;
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


public class GangTasker
{
    private IEntityProvideable World;
    private ITargetable Player;
    private IWeapons Weapons;
    private ISettingsProvideable Settings;
    private Mod.Tasker Tasker;
    private IPlacesOfInterest PlacesOfInterest;
    public GangTasker(Mod.Tasker tasker, IEntityProvideable pedProvider, ITargetable player, IWeapons weapons, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest)
    {
        Tasker = tasker;
        World = pedProvider;
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
            World.Pedestrians.ExpireSeatAssignments();
            foreach (GangMember gangMember in World.Pedestrians.GangMemberList.Where(x => x.Pedestrian.Exists() && x.HasExistedFor >= 1000))
            {
                try
                {
                    if (gangMember.IsGroupMember && !gangMember.IsBusted)
                    {
                        if (gangMember.CurrentTask != null && gangMember.CurrentTask.ShouldUpdate)
                        {
                            gangMember.UpdateTask(null);
                            GameFiber.Yield();
                        }
                        continue;
                    }
                    else
                    {
                        if (!gangMember.IsBusted && !gangMember.CanBeTasked)
                        {
                            if (gangMember.CurrentTask != null)
                            {
                                gangMember.CurrentTask = null;
                            }
                            continue;
                        }

                        //if (gangMember.DistanceToPlayer >= 230f)
                        //{
                        //    continue;
                        //}

                        if (gangMember.NeedsTaskAssignmentCheck)
                        {
                            //if (gangMember.DistanceToPlayer <= 200f)
                            //{
                                UpdateCurrentTask(gangMember);//has yields if it does anything
                            //}
                        }


                        if (gangMember.CurrentTask?.Name != "GetArrested" && !gangMember.CanBeAmbientTasked)
                        {
                            gangMember.CurrentTask = null;
                        }



                        if (gangMember.CurrentTask != null && gangMember.CurrentTask.ShouldUpdate)
                        {
                            gangMember.UpdateTask(null);
                            GameFiber.Yield();
                        }
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
        GangReputation gr = Player.RelationshipManager.GangRelationships.GetReputation(GangMember.Gang);
        bool isPlayerGang = Player.RelationshipManager.GangRelationships.CurrentGang?.ID == GangMember.Gang?.ID;
        bool isHostile = gr.GangRelationship == GangRespect.Hostile;
        bool arePoliceNearby = Player.ClosestPoliceDistanceToPlayer <= 150f;// 100f;
        bool isNearHomeTerritory = false;
        if(Player.CurrentLocation.CurrentZone?.Gangs?.Any(x=>x.ID == GangMember.Gang?.ID) == true)
        {
            isNearHomeTerritory = true;
        } 
        if (GangMember.IsBusted)
        {
            //if (GangMember.DistanceToPlayer <= 275f)
            //{
                SetArrested(GangMember);
            //}
        }
        else if (GangMember.IsWanted && GangMember.CanBeTasked && GangMember.CanBeAmbientTasked)
        {
            if (GangMember.WillFightPolice)
            {
                SetFight(GangMember, null);
            }
            else
            {
                SetFlee(GangMember, null);
            }
        }
        else if (GangMember.CanBeTasked && GangMember.CanBeAmbientTasked)//50f
        {
            //GangMember.PedReactions.Update(); need a version for this?

            bool WillAttackPlayer = false;
            bool WillFleeFromPlayer = false;
            bool SeenPlayerReactiveCrime = GangMember.PlayerCrimesWitnessed.Any(x => (x.Crime.ScaresCivilians || x.Crime.AngersCivilians) && x.Crime.CanBeReactedToByCivilians);
            bool SeenOtherReactiveCrime = GangMember.OtherCrimesWitnessed.Any(x => (x.Crime.ScaresCivilians || x.Crime.AngersCivilians) && x.Crime.CanBeReactedToByCivilians);
            WitnessedCrime HighestPriorityOtherCrime = GangMember.OtherCrimesWitnessed.OrderBy(x => x.Crime.Priority).ThenByDescending(x => x.GameTimeLastWitnessed).FirstOrDefault();
            int PlayerCrimePriority = 99;
            foreach (WitnessedCrime playerCrime in GangMember.PlayerCrimesWitnessed.Where(x => x.Crime.CanBeReactedToByCivilians))
            {
                if (playerCrime.Crime.Priority < PlayerCrimePriority)
                {
                    PlayerCrimePriority = playerCrime.Crime.Priority;
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

            if(isPlayerGang)
            {
                WillAttackPlayer = false;
                WillFleeFromPlayer = false;
            }
            else
            {
                if(SeenPlayerReactiveCrime)
                {
                    if (GangMember.WillFight && !arePoliceNearby && Player.IsNotWanted)
                    {
                        WillAttackPlayer = true;
                    }
                    else
                    {
                        WillFleeFromPlayer = true;
                    }
                }
                else if (GangMember.HasBeenHurtByPlayer || GangMember.HasBeenCarJackedByPlayer || gr.RecentlyAttacked || (GangMember.IsHitSquad && GangMember.EverSeenPlayer))
                {
                    if (GangMember.WillFight)
                    {
                        WillAttackPlayer = true;
                    }
                    else
                    {
                        WillFleeFromPlayer = true;
                    }
                }
                else if (isHostile && GangMember.CanSeePlayer && isNearHomeTerritory && !arePoliceNearby && Player.IsNotWanted)
                {
                    if (GangMember.WillFight)
                    {
                        WillAttackPlayer = true;
                    }
                    else
                    {
                        WillFleeFromPlayer = true;
                    }
                }
                //if (SeenPlayerReactiveCrime)
                //{
                //    WillFleeFromPlayer = true;
                //}
            }
            if (WillAttackPlayer)
            {
                SetFight(GangMember, null);
            }
            else if (SeenOtherReactiveCrime)
            {
                if (Settings.SettingsManager.GangSettings.AllowFightingOtherCriminals && GangMember.WillFight && !arePoliceNearby && Player.IsNotWanted && !Player.Investigation.IsActive)
                {
                    SetFight(GangMember, HighestPriorityOtherCrime);
                }
                else
                {
                    SetFlee(GangMember, HighestPriorityOtherCrime);
                }
            }
            else if (WillFleeFromPlayer)
            {
                SetFlee(GangMember, null);
            }
            else
            {
                if(GangMember.IsHitSquad && GangMember.CurrentTask == null && !GangMember.PlayerPerception.EverSeenTarget && GangMember.ClosestDistanceToPlayer >= 20f)
                {
                    SetLocate(GangMember);
                }
                else if (GangMember.WasModSpawned && GangMember.CurrentTask == null)
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
            GangMember.CurrentTask = new GangFlee(GangMember, Player, Settings) { OtherTarget = HighestPriority?.Perpetrator };
            GameFiber.Yield();//TR Added back 7
            GangMember.CurrentTask?.Start();
        }
    }
    private void SetFight(GangMember GangMember, WitnessedCrime HighestPriority)
    {
        if (GangMember.CurrentTask?.Name != "GangFight")
        {
            GangMember.CurrentTask = new GangFight(GangMember, Player, null) { OtherTarget = HighestPriority?.Perpetrator };
            GameFiber.Yield();//TR Added back 7
            GangMember.CurrentTask?.Start();
        }
    }
    private void SetLocate(GangMember GangMember)
    {
        if (GangMember.CurrentTask?.Name != "Locate")
        {
            GangMember.CurrentTask = new GangGeneralLocate(GangMember, GangMember, Player, World, null, PlacesOfInterest, Settings, Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringLocate, GangMember);
            GameFiber.Yield();//TR Added back 4
            GangMember.CurrentTask.Start();
        }
    }
    private void SetArrested(GangMember GangMember)
    {
        if (GangMember.CurrentTask?.Name != "GetArrested")
        {
            GangMember.CurrentTask = new GetArrested(GangMember, Player, World);
            GameFiber.Yield();//TR Added back 7
            GangMember.CurrentTask?.Start();
        }
    }
    private void SetIdle(GangMember GangMember)
    {

        if (GangMember.CurrentTask?.Name != "Idle")
        { 
            //EntryPoint.WriteToConsole($"TASKER: gm {GangMember.Pedestrian.Handle} Task Changed from {GangMember.CurrentTask?.Name} to GeneralIdle", 3);
            GangMember.CurrentTask = new GeneralIdle(GangMember, GangMember, Player, World,World.Vehicles.CivilianVehicleList.Where(x=> x.AssociatedGang != null && GangMember.Gang != null && x.AssociatedGang.ID == GangMember.Gang.ID).ToList(),PlacesOfInterest,Settings,false,false,false, false);//GangMember.CurrentTask = new GangIdle(GangMember, Player, PedProvider, PlacesOfInterest);
            GangMember.WeaponInventory.Reset();
            GangMember.WeaponInventory.SetDefault();
            GameFiber.Yield();//TR Added back 4    
            GangMember.CurrentTask?.Start();
        }

        //if (GangMember.CurrentTask?.Name != "GenericIdle")
        //{
        //    EntryPoint.WriteToConsole($"TASKER: gm {GangMember.Pedestrian.Handle} Task Changed from {GangMember.CurrentTask?.Name} to GenericIdle", 3);
        //    GangMember.CurrentTask = new GenericIdle(GangMember, Player, World, PlacesOfInterest);//GangMember.CurrentTask = new GangIdle(GangMember, Player, PedProvider, PlacesOfInterest);
        //    GameFiber.Yield();//TR Added back 4
        //    GangMember.CurrentTask?.Start();
        //}






    }
}


