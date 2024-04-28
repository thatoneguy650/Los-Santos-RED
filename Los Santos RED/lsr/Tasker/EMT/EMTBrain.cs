using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class EMTBrain : PedBrain
{
    private List<PedExt> PossibleTargets;
    private EMT EMT;
    public EMTBrain(EMT pedExt, ISettingsProvideable settings, IEntityProvideable world, IWeapons weapons) : base(pedExt, settings, world, weapons)
    {
        EMT = pedExt;
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
                PedExt.UpdateTask(PedToGoTo());
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
                    PedExt MainTarget = PedToGoTo();
                    if (MainTarget != null)
                    {
                        SetTreatTask(MainTarget);
                    }
                    else if (PedExt.HasCellPhone && PedExt.PedReactions.HasSeenMundaneCrime && PedExt.WillCallPolice)
                    {
                        SetCalmCallIn();
                    }
                    else if (Player.Investigation.IsActive && Player.Investigation.RequiresEMS && EMT.IsRespondingToInvestigation)
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
        else if (EMT.DistanceToPlayer <= 1200f && Player.Investigation.IsActive && Player.Investigation.RequiresEMS && EMT.IsRespondingToInvestigation)
        {
            SetRespondTask();
        }
        else if (PedExt.WasModSpawned)
        {
            SetIdle();
        }
        PedExt.GameTimeLastUpdatedTask = Game.GameTime;
    }
    protected override WeaponInformation GetWeaponToIssue(bool IsGangMember)
    {
        return null;
    }
    private void SetRespondTask()
    {
        //if (EMT.CurrentTask?.Name != "EMTRespond")// && Cop.IsIdleTaskable)
        //{
        //    //EntryPoint.WriteToConsole($"TASKER: Cop {emt.Pedestrian.Handle} Task Changed from {emt.CurrentTask?.Name} to EMTRespond", 3);
        //    EMT.CurrentTask = new EMTRespond(EMT, Player, World, PlacesOfInterest, EMT);
        //    GameFiber.Yield();//TR Added back 4
        //    EMT.CurrentTask?.Start();
        //}
        if (EMT.CurrentTask?.Name != "Investigate")
        {
            EMT.CurrentTask = new EMSGeneralInvestigate(EMT, EMT, Player, World, null, PlacesOfInterest, Settings, false, null, false);//Cop.CurrentTask = new Investigate(Cop, Player, Settings, World);
            GameFiber.Yield();//TR Added back 4
            EMT.CurrentTask.Start();
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
    private void SetTreatTask(PedExt targetPed)
    {
        if (EMT.CurrentTask?.Name != "EMTTreat" || (targetPed != null && EMT.CurrentTask?.OtherTarget?.Handle != targetPed.Handle))// && Cop.IsIdleTaskable)
        {
             EntryPoint.WriteToConsole($"TASKER: Cop {EMT.Pedestrian.Handle} Task Changed from {EMT.CurrentTask?.Name} to EMTTreat", 3);
            EMT.CurrentTask = new EMTTreat(EMT, Player, targetPed, Settings);
            GameFiber.Yield();//TR Added back 4
            EMT.CurrentTask?.Start();
        }
    }
    private PedExt PedToGoTo()
    {
        PedExt MainTarget = null;
        if (EMT.Pedestrian.Exists())
        {
            List<EMTTarget> EMTsPossibleTargets = new List<EMTTarget>();
            PossibleTargets = World.Pedestrians.PedExts.Where(x => x.Pedestrian.Exists() && (x.IsUnconscious || x.IsInWrithe) && !x.IsDead && !x.HasBeenTreatedByEMTs && x.HasBeenSeenUnconscious).ToList();

            foreach (PedExt possibleTarget in PossibleTargets)
            {
                int TotalOtherAssignedEMTs = World.Pedestrians.EMTList.Count(x => x.Handle != EMT.Handle && x.CurrentTask != null && x.CurrentTask.OtherTarget != null && x.CurrentTask.OtherTarget.Handle == possibleTarget.Handle);
                if (possibleTarget.Pedestrian.Exists())
                {
                    float distanceTo = possibleTarget.Pedestrian.DistanceTo2D(EMT.Pedestrian);
                    if (distanceTo <= 60f)
                    {
                        EMTsPossibleTargets.Add(new EMTTarget(possibleTarget, distanceTo) { TotalAssignedEMTs = TotalOtherAssignedEMTs });
                    }
                }
            }

            int PossibleTargetCount = EMTsPossibleTargets.Count();
            EMTTarget MainPossibleTarget = EMTsPossibleTargets
                .OrderBy(x => x.IsOverloaded)
                .ThenBy(x => x.DistanceToTarget).FirstOrDefault();

            if (MainPossibleTarget != null && (!MainPossibleTarget.IsOverloaded || PossibleTargetCount == 1))
            {
                MainTarget = MainPossibleTarget.Target;
            }
        }
        //EntryPoint.WriteToConsole($"EMT PedToGoTo {MainTarget?.Handle}");
        return MainTarget;
    }
    private class EMTTarget
    {
        public EMTTarget(PedExt target, float distanceToTarget)
        {
            Target = target;
            DistanceToTarget = distanceToTarget;
        }
        public PedExt Target { get; set; }
        public float DistanceToTarget { get; set; } = 999f;
        public int TotalAssignedEMTs { get; set; } = 0;
        public bool IsOverloaded => TotalAssignedEMTs >= 1;
    }


}

