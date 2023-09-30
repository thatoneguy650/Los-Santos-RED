using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TaxiDriverBrain : PedBrain
{
    public TaxiDriverBrain(PedExt pedExt, ISettingsProvideable settings, IEntityProvideable world, IWeapons weapons) : base(pedExt, settings, world, weapons)
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
            else if (PedExt.HasCellPhone && PedExt.PedReactions.HasSeenMundaneCrime && PedExt.WillCallPolice)
            {
                SetCalmCallIn();
            }
            else if (PedExt.WasModSpawned && PedExt.CurrentTask == null)
            {
                SetIdle();
            }
        }
        else if (PedExt.WasModSpawned && PedExt.CurrentTask == null)
        {
            SetIdle();
        }
        PedExt.GameTimeLastUpdatedTask = Game.GameTime;
    }

}

