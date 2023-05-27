using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PedBrain
{
    protected PedExt PedExt;
    protected ISettingsProvideable Settings;
    protected IEntityProvideable World;
    protected IWeapons Weapons;

    protected ITargetable Player;
    protected IPlacesOfInterest PlacesOfInterest;

    public PedBrain(PedExt pedExt, ISettingsProvideable settings, IEntityProvideable world, IWeapons weapons)
    {
        PedExt = pedExt;
        Settings = settings;
        World = world;
        Weapons = weapons;
    }
    public virtual void Setup()
    {

    }
    public virtual void Dispose()
    {

    }
    public virtual void Update(ITargetable player, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        PlacesOfInterest = placesOfInterest;

        if (PedExt.CanBeTasked && PedExt.CanBeAmbientTasked)
        {
            if (PedExt.DistanceToPlayer >= 230f)
            {
                PedExt.CurrentTask = null;
                return;
            }
            if (PedExt.NeedsTaskAssignmentCheck)
            {
                if (PedExt.DistanceToPlayer <= 200f)
                {
                    UpdateCurrentTask();//has yields if it does anything
                }
                else if (PedExt.CurrentTask != null)
                {
                    PedExt.CurrentTask = null;
                }
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
    private void UpdateCurrentTask()
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
            else if (PedExt.CanAttackPlayer && PedExt.WillFight)// && !Civilian.IsGangMember )
            {
                SetFight();
            }
            else if (PedExt.PedReactions.HasSeenMundaneCrime && PedExt.WillCallPolice)
            {
                SetCalmCallIn();
            }
        }
        else if (PedExt.WasModSpawned && PedExt.CurrentTask == null)
        {
            SetIdle();
        }
        PedExt.GameTimeLastUpdatedTask = Game.GameTime;
    }
    protected void SetArrested()
    {
        if (PedExt.CurrentTask?.Name == "GetArrested")
        {
            return;
        }
        PedExt.CurrentTask = new GetArrested(PedExt, Player, World);
        GameFiber.Yield();//TR Added back 7
        PedExt.CurrentTask?.Start();
    }
    protected void SetFlee()
    {
        if (PedExt.CurrentTask?.Name == "Flee")
        {
            return;
        }
        PedExt.CurrentTask = new Flee(PedExt, Player) { OtherTarget = PedExt.PedReactions.HighestPriorityCrime?.Perpetrator };
        GameFiber.Yield();//TR Added back 7
        PedExt.CurrentTask?.Start();
    }
    protected void SetFight()
    {
        if (PedExt.CurrentTask?.Name == "Fight")
        {
            return;
        }
        PedExt.CurrentTask = new Fight(PedExt, Player, GetWeaponToIssue(PedExt.IsGangMember)) { OtherTarget = PedExt.PedReactions.HighestPriorityCrime?.Perpetrator };//gang memebrs already have guns
        GameFiber.Yield();//TR Added back 7
        PedExt.CurrentTask?.Start();
    }
    protected void SetScaredCallIn()
    {
        if (PedExt.CurrentTask?.Name == "ScaredCallIn")
        {
            return;
        }
        PedExt.CurrentTask = new ScaredCallIn(PedExt, Player, Settings) { OtherTarget = PedExt.PedReactions.HighestPriorityCrime?.Perpetrator };
        GameFiber.Yield();//TR Added back 7
        PedExt.CurrentTask?.Start();
    }
    protected void SetCalmCallIn()
    {
        if (PedExt.CurrentTask?.Name == "CalmCallIn")
        {
            return;
        }
        PedExt.CurrentTask = new CalmCallIn(PedExt, Player, Settings);
        GameFiber.Yield();//TR Added back 4
        PedExt.CurrentTask.Start();
    }
    protected void SetIdle()
    {
        if (PedExt.CurrentTask?.Name == "Idle")
        {
            return;
        }
        PedExt.CurrentTask = new GeneralIdle(PedExt, PedExt, Player, World, new List<VehicleExt>() { PedExt.AssignedVehicle }, PlacesOfInterest, Settings, false, false, false, true);
        GameFiber.Yield();//TR Added back 4
        PedExt.CurrentTask.Start();
    }
    protected virtual WeaponInformation GetWeaponToIssue(bool IsGangMember)
    {
        WeaponInformation ToIssue;
        if (IsGangMember)
        {
            if (RandomItems.RandomPercent(70))
            {
                ToIssue = Weapons.GetRandomRegularWeapon(WeaponCategory.Pistol);
            }
            else
            {
                ToIssue = Weapons.GetRandomRegularWeapon(WeaponCategory.Melee);
            }
        }
        else if (RandomItems.RandomPercent(40))
        {
            ToIssue = Weapons.GetRandomRegularWeapon(WeaponCategory.Pistol);
        }
        else
        {
            if (RandomItems.RandomPercent(65))
            {
                ToIssue = Weapons.GetRandomRegularWeapon(WeaponCategory.Melee);
            }
            else
            {
                ToIssue = null;
            }
        }
        return ToIssue;
    }
}

