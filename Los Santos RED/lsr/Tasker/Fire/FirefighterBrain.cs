using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System.Collections.Generic;

public class FirefighterBrain : PedBrain
{
    private Firefighter Firefighter;
    private bool HasFireInArea;
    private bool prevHasFireInArea;

    public FirefighterBrain(Firefighter pedExt, ISettingsProvideable settings, IEntityProvideable world, IWeapons weapons) : base(pedExt, settings, world, weapons)
    {
        Firefighter = pedExt;
        PedExt = pedExt;
        Settings = settings;
        World = world;
        Weapons = weapons;
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
    protected override void UpdateCurrentTask()
    {
        if (PedExt.DistanceToPlayer <= 150f)//50f
        {
            PedExt.PedReactions.Update(Player);
            if (PedExt.PedReactions.HasSeenScaryCrime || PedExt.PedReactions.HasSeenAngryCrime)
            {
                if (PedExt.WillFight && PedExt.PedReactions.HasSeenAngryCrime && Player.IsNotWanted)
                {
                    SetFight();
                }
                else
                {
                    SetFlee();
                }
            }
            else if (PedExt.CanAttackPlayer && PedExt.WillFight)
            {
                SetFight();
            }
            else
            {
                if (PedExt.PedAlerts.IsAlerted)// Cop.BodiesSeen.Any() || )
                {
                    SetInvestigate();
                }
                else
                {
                    CheckFires();
                    if (HasFireInArea)
                    {
                        SetExtinguishTask();
                    }
                    else if (PedExt.HasCellPhone && PedExt.PedReactions.HasSeenMundaneCrime && PedExt.WillCallPolice)
                    {
                        SetCalmCallIn();
                    }
                    else if (Player.Investigation.IsActive && Player.Investigation.RequiresFirefighters && Firefighter.IsRespondingToInvestigation)
                    {
                        SetRespondTask();
                    }
                    else if (PedExt.WasModSpawned)
                    {
                        SetIdle();
                    }
                }
            }
        }
        else if (PedExt.DistanceToPlayer <= 1200f && Player.Investigation.IsActive && Player.Investigation.RequiresFirefighters && Firefighter.IsRespondingToInvestigation)
        {
            SetRespondTask();
        }
        else if (PedExt.WasModSpawned)
        {
            SetIdle();
        }
        PedExt.GameTimeLastUpdatedTask = Game.GameTime;
    }

    private void SetExtinguishTask()
    {
        if (PedExt.CurrentTask?.Name != "FireExtinguish")// && Cop.IsIdleTaskable)
        {
            PedExt.CurrentTask = new FireExtinguish(PedExt, PedExt, Player, World, null, PlacesOfInterest, Settings, false, Firefighter);
            GameFiber.Yield();//TR Added back 4
            PedExt.CurrentTask?.Start();
        }
    }
    private void SetInvestigate()
    {
        if (PedExt.CurrentTask?.Name != "Investigate")
        {
            // EntryPoint.WriteToConsole($"TASKER: Cop {Cop.Pedestrian.Handle} Task Changed from {Cop.CurrentTask?.Name} to Investigate", 3);
            PedExt.CurrentTask = new GeneralInvestigate(PedExt, PedExt, Player, World, null, PlacesOfInterest, Settings, false, null, true);//Cop.CurrentTask = new Investigate(Cop, Player, Settings, World);
            GameFiber.Yield();//TR Added back 4
            PedExt.CurrentTask.Start();
        }
    }
    protected override void SetIdle()
    {
        if (PedExt.CurrentTask?.Name == "Idle")
        {
            return;
        }
        PedExt.CurrentTask = new GeneralIdle(PedExt, PedExt, Player, World, new List<VehicleExt>() { PedExt.AssignedVehicle }, PlacesOfInterest, Settings, false, false, true, true);
        GameFiber.Yield();//TR Added back 4
        PedExt.CurrentTask.Start();
    }
    private void CheckFires()
    {
        int numFires = NativeFunction.Natives.GET_NUMBER_OF_FIRES_IN_RANGE<int>(PedExt.Position, Settings.SettingsManager.FireSettings.FireAwareDistance);
        HasFireInArea = numFires > 0;
        if(prevHasFireInArea != HasFireInArea)
        {
            prevHasFireInArea = HasFireInArea;
            EntryPoint.WriteToConsole($"{PedExt.Handle} FIRE IN AREA CHANGED TO {HasFireInArea}");
        }
    }

    protected override WeaponInformation GetWeaponToIssue(bool IsGangMember)
    {
        return null;
    }
    private void SetRespondTask()
    {
        if (PedExt.CurrentTask?.Name != "Investigate")
        {
            PedExt.CurrentTask = new FireGeneralInvestigate(PedExt, PedExt, Player, World, null, PlacesOfInterest, Settings, false, null, false);//Cop.CurrentTask = new Investigate(Cop, Player, Settings, World);
            GameFiber.Yield();//TR Added back 4
            PedExt.CurrentTask.Start();
        }

    }

}

