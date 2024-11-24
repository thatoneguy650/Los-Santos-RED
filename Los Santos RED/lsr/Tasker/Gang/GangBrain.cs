using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangBrain : PedBrain
{
    private GangMember GangMember;
    public GangBrain(GangMember pedExt, ISettingsProvideable settings, IEntityProvideable world, IWeapons weapons) : base(pedExt, settings, world, weapons)
    {
        GangMember = pedExt;
        PedExt = pedExt;
        Settings = settings;
        World = world;
        Weapons = weapons;
    }
    public override void Update(ITargetable player, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        PlacesOfInterest = placesOfInterest;
        if (GangMember.IsGroupMember && !GangMember.IsBusted)
        {
            if (GangMember.CurrentTask != null && GangMember.CurrentTask.ShouldUpdate)
            {
                GangMember.UpdateTask(null);
                GameFiber.Yield();
            }
            return;
        }
        else
        {
            if (!GangMember.IsBusted && !GangMember.CanBeTasked)
            {
                if (GangMember.CurrentTask != null)
                {
                    GangMember.CurrentTask = null;
                }
                return ;
            }
            if (GangMember.NeedsTaskAssignmentCheck)
            {
                UpdateCurrentTask();//has yields if it does anything
            }
            if (GangMember.CurrentTask?.Name != "GetArrested" && !GangMember.CanBeAmbientTasked)
            {
                GangMember.CurrentTask = null;
            }
            if (GangMember.CurrentTask != null && GangMember.CurrentTask.ShouldUpdate)
            {
                GangMember.UpdateTask(null);
                GameFiber.Yield();
            }
        }
    }
    protected override void UpdateCurrentTask()//this should be moved out?
    {
        GangReputation gr = Player.RelationshipManager.GangRelationships.GetReputation(GangMember.Gang);
        bool isPlayerGang = Player.RelationshipManager.GangRelationships.CurrentGang?.ID == GangMember.Gang?.ID;
        bool isHostile = gr.GangRelationship == GangRespect.Hostile;
        bool arePoliceNearby = Player.ClosestPoliceDistanceToPlayer <= 150f;// 100f;
        bool isNearHomeTerritory = false;
        string gangID = "";// GangMember.Gang?.ID;
        if(GangMember.Gang != null)
        {
            gangID = GangMember.Gang.ID;
        }

        if (Player.CurrentLocation.CurrentZone?.Gangs?.Any(x => x.ID == gangID) == true)
        {
            isNearHomeTerritory = true;
        }
        if(!isNearHomeTerritory && World.Places.ActiveLocations.Any(x=> x.DistanceToPlayer <= 200f && x.HasAssociation(GangMember.Gang)))
        {
            isNearHomeTerritory = true;
        }


        if (GangMember.IsBusted)
        {
            SetArrested(GangMember);
        }
        else if (GangMember.IsWanted && GangMember.CanBeTasked && GangMember.CanBeAmbientTasked)
        {
            if (GangMember.WillAlwaysFightPolice)
            {
                SetFight(GangMember, null);
            }
            else if (GangMember.WillFightPolice && isNearHomeTerritory)
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
            bool WillAttackPlayer = false;
            bool WillFleeFromPlayer = false;
            bool IsFedUp = false;
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

            if (isPlayerGang || GangMember.IsGroupMember)
            {
                WillAttackPlayer = false;
                WillFleeFromPlayer = false;
            }
            else
            {
                if (SeenPlayerReactiveCrime)
                {
                    if (GangMember.WillFight && !arePoliceNearby && Player.IsNotWanted && isNearHomeTerritory)
                    {
                        WillAttackPlayer = true;
                    }
                    else
                    {
                        WillFleeFromPlayer = true;
                    }
                }
                else if (GangMember.HasBeenHurtByPlayer || GangMember.HasBeenCarJackedByPlayer || gr.RecentlyAttacked || (GangMember.IsHitSquad && GangMember.CanSeePlayer))
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
                else if (isHostile && GangMember.CanRecognizePlayer && GangMember.ClosestDistanceToPlayer <= 20f && isNearHomeTerritory && !arePoliceNearby && Player.IsNotWanted)//changed from see to recognize, leave hit squads as is
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
                else if (GangMember.IsFedUpWithPlayer && !arePoliceNearby)
                {
                    IsFedUp = true;
                    if (GangMember.WillFight)
                    {
                        WillAttackPlayer = true;
                    }
                    else
                    {
                        WillFleeFromPlayer = true;
                    }
                }
            }
            if (WillAttackPlayer)
            {
                if (IsFedUp)
                {
                    SetFightPlayer(GangMember);
                }
                else
                {
                    SetFight(GangMember, null);
                }
            }
            else if (SeenOtherReactiveCrime)
            {
                if (Settings.SettingsManager.GangSettings.AllowFightingOtherCriminals && World.TotalWantedLevel == 0 && GangMember.WillFight && isNearHomeTerritory && !arePoliceNearby && Player.IsNotWanted && !Player.Investigation.IsActive)
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



                if (GangMember.IsBackupSquad && !GangMember.PlayerPerception.EverSeenTarget && GangMember.ClosestDistanceToPlayer >= 20f)
                {
                    SetLocate(GangMember);
                }
                else if (GangMember.IsHitSquad && !GangMember.PlayerPerception.EverSeenTarget && GangMember.ClosestDistanceToPlayer >= 20f)
                {
                    SetLocate(GangMember);
                }
                else if (GangMember.IsHitSquad && GangMember.PlayerPerception.EverSeenTarget && GangMember.DistanceToPlayer <= 200f)
                {
                    SetFight(GangMember, null);
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
    private void SetFightPlayer(GangMember GangMember)
    {
        if (GangMember.CurrentTask?.Name != "GangFight")
        {
            GangMember.CurrentTask = new GangFight(GangMember, Player, null) { ForceCombatPlayer = true };

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
            GangMember.CurrentTask = new GeneralIdle(GangMember, GangMember, Player, World, 
                
                
                //World.Vehicles.SimpleGangVehicles.Where(x => x.AssociatedGang != null && GangMember.Gang != null && x.AssociatedGang.ID == GangMember.Gang.ID).ToList()
                new List<VehicleExt>() { PedExt.AssignedVehicle }

                , PlacesOfInterest, Settings, false, false, false, false);//GangMember.CurrentTask = new GangIdle(GangMember, Player, PedProvider, PlacesOfInterest);
            GangMember.WeaponInventory.Reset();
            GangMember.WeaponInventory.SetDefault();
            GameFiber.Yield();//TR Added back 4    
            GangMember.CurrentTask?.Start();
        }
    }

}

