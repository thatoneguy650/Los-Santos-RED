using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FirefighterBrain : PedBrain
{
    private Firefighter Firefighter;
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
    private void UpdateCurrentTask()
    {
        if (PedExt.DistanceToPlayer <= 150f)//50f
        {
            PedExt.PedReactions.Update(Player);
            if (PedExt.PedReactions.HasSeenScaryCrime || PedExt.PedReactions.HasSeenAngryCrime)
            {
                if (PedExt.WillCallPolice || (PedExt.WillCallPoliceIntense && PedExt.PedReactions.HasSeenIntenseCrime))
                {
                    SetScaredCallIn();
                }
                else if (PedExt.WillFight)
                {
                    if (PedExt.PedReactions.HasSeenAngryCrime && Player.IsNotWanted)
                    {
                        SetFight();
                    }
                    else
                    {
                        SetFlee();
                    }
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
            else if (PedExt.PedReactions.HasSeenMundaneCrime && PedExt.WillCallPolice)
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
    protected override WeaponInformation GetWeaponToIssue(bool IsGangMember)
    {
        return null;
    }
}

