using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System.Collections.Generic;


public class TaxiDriverBrain : PedBrain
{
    private TaxiDriver TaxiDriver;
    public TaxiDriverBrain(PedExt pedExt, ISettingsProvideable settings, IEntityProvideable world, IWeapons weapons, TaxiDriver taxiDriver) : base(pedExt, settings, world, weapons)
    {
        TaxiDriver = taxiDriver;
    }
    public override void Update(ITargetable player, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        PlacesOfInterest = placesOfInterest;

        if (PedExt.CanBeTasked && PedExt.CanBeAmbientTasked)
        {
            if (PedExt.NeedsTaskAssignmentCheck)
            {
                UpdateCurrentTask();//has yields if it does anything
            }
            if (PedExt.CurrentTask != null && PedExt.CurrentTask.ShouldUpdate)
            {
                PedExt.UpdateTask(null);
                GameFiber.Yield();
            }
        }
        else if (PedExt.IsBusted || PedExt.IsWanted)
        {
            UpdateCurrentTask();
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
        if (PedExt.IsBusted)
        {
            if (PedExt.DistanceToPlayer <= 175f)//75f
            {
                SetArrested();
            }
        }
        else if (PedExt.IsWanted)
        {
            if (PedExt.WillFightPolice)
            {
                SetFight();
            }
            else
            {
                SetFlee();
            }
        }
        else if (PedExt.DistanceToPlayer <= 75f)//50f
        {
            PedExt.PedReactions.Update(Player);
            if (PedExt.PedReactions.HasSeenScaryCrime || PedExt.PedReactions.HasSeenAngryCrime)
            {
                TaxiDriver.TaxiRide?.Cancel();
                if (PedExt.WillFight && PedExt.PedReactions.HasSeenAngryCrime && Player.IsNotWanted)
                {
                    SetFight();
                }
                else
                {
                    SetFlee();
                }
            }
            else if (PedExt.CanAttackPlayer && PedExt.WillFight)// && !Civilian.IsGangMember )
            {
                SetFight();
            }
            //else if (PedExt.HasCellPhone && PedExt.PedReactions.HasSeenMundaneCrime && PedExt.WillCallPolice)
            //{
            //    SetCalmCallIn();
            //}
            else
            {
                HandleIdle();
            }
        }
        else
        {
            HandleIdle();
        }
        PedExt.GameTimeLastUpdatedTask = Game.GameTime;
    }
    private void HandleIdle()
    {
        if(TaxiDriver.TaxiRide?.IsActive == true)
        {
            SetTaxiService();
        }
        else if (PedExt.WasModSpawned && PedExt.CanBeIdleTasked)
        {
            SetIdle();
        }
    }
    private void SetTaxiService()
    {
        if (PedExt.CurrentTask?.Name == "TaxiService")
        {
            return;
        }
        PedExt.CurrentTask = new TaxiService(PedExt, PedExt, Player, World, new List<VehicleExt>() { PedExt.AssignedVehicle }, PlacesOfInterest, Settings, true, TaxiDriver);
        GameFiber.Yield();//TR Added back 4
        PedExt.CurrentTask.Start();
        EntryPoint.WriteToConsole($"PED: {PedExt.Handle} STARTED TaxiService");
    }
    protected override void SetIdle()
    {
        if (PedExt.CurrentTask?.Name == "Idle")
        {
            return;
        }
        PedExt.CurrentTask = new TaxiGeneralIdle(PedExt, PedExt, Player, World, new List<VehicleExt>() { PedExt.AssignedVehicle }, PlacesOfInterest, Settings, false, false, true, true);
        GameFiber.Yield();//TR Added back 4
        PedExt.CurrentTask.Start();
        EntryPoint.WriteToConsole($"PED: {PedExt.Handle} STARTED TAXIGeneralIdle");


    }
}

