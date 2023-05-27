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
    private void UpdateCurrentTask()
    {
        if (PedExt.DistanceToPlayer <= 150f)//50f
        {
            PedExt.PedReactions.Update(Player);
            PedExt MainTarget = PedToGoTo();
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
            else if (MainTarget != null)
            {
                SetTreatTask(MainTarget);
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
        else if (EMT.DistanceToPlayer <= 1200f && Player.Investigation.IsActive && Player.Investigation.RequiresEMS)
        {
            SetRespondTask();
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
    private void SetRespondTask()
    {
        if (EMT.CurrentTask?.Name != "EMTRespond")// && Cop.IsIdleTaskable)
        {
            //EntryPoint.WriteToConsole($"TASKER: Cop {emt.Pedestrian.Handle} Task Changed from {emt.CurrentTask?.Name} to EMTRespond", 3);
            EMT.CurrentTask = new EMTRespond(EMT, Player, World, PlacesOfInterest, EMT);
            GameFiber.Yield();//TR Added back 4
            EMT.CurrentTask?.Start();
        }
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
        EntryPoint.WriteToConsole($"EMT PedToGoTo {MainTarget?.Handle}");
        return MainTarget;
    }
    //private void SetPossibleTargets()
    //{
    //    PossibleTargets = World.Pedestrians.PedExts.Where(x => x.Pedestrian.Exists() && (x.IsUnconscious || x.IsInWrithe) && !x.IsDead && !x.HasBeenTreatedByEMTs && x.HasBeenSeenUnconscious).ToList();//150f//writhe peds that are still alive
    //    //PossibleTargets.AddRange(PedProvider.Pedestrians.DeadPeds.Where(x => x.Pedestrian.Exists() && !x.HasBeenTreatedByEMTs));//dead peds go here?
    //}
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

