using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class EMTTasker
{
    private IEntityProvideable PedProvider;
    private ITargetable Player;
    private IWeapons Weapons;
    private ISettingsProvideable Settings;
    private Tasker Tasker;
    private IPlacesOfInterest PlacesOfInterest;
    private List<PedExt> PossibleTargets;
    public EMTTasker(Tasker tasker, IEntityProvideable pedProvider, ITargetable player, IWeapons weapons, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest)
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
        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@enter");
        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@base");
        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@exit");
        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@idle_a");
    }
    public void Update()
    {
        if (Settings.SettingsManager.EMSSettings.ManageTasking)
        {
            SetPossibleTargets();
            Tasker.ExpireSeatAssignments();

            foreach (EMT emt in PedProvider.Pedestrians.EMTs.Where(x => x.Pedestrian.Exists() && x.HasBeenSpawnedFor >= 2000 && x.CanBeTasked))
            {
                try
                {
                    if (emt.NeedsTaskAssignmentCheck)
                    {
                        ReAssignTask(emt);//has yields if it does anything
                    }
                    if (emt.CurrentTask != null && emt.CurrentTask.ShouldUpdate)
                    {
                        emt.UpdateTask(PedToGoTo(emt));
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
    private void ReAssignTask(EMT emt)
    {
        if (emt.DistanceToPlayer <= 1200f)
        {
            if(PossibleTargets.Any())//and the body has been called in?
            {
                PedExt MainTarget = PedToGoTo(emt);
                GameFiber.Yield();
                if (MainTarget != null)
                {
                    SetRespondTask(emt, MainTarget);
                }
                else
                {
                    SetIdleTask(emt);
                }
            }
            else
            {
                SetIdleTask(emt);
            }
        }
        else
        {
            SetIdleTask(emt);
        }
        emt.GameTimeLastUpdatedTask = Game.GameTime;
    }
    private void SetIdleTask(EMT emt)
    {
        if (emt.CurrentTask?.Name != "EMTIdle")// && Cop.IsIdleTaskable)
        {
            EntryPoint.WriteToConsole($"TASKER: Cop {emt.Pedestrian.Handle} Task Changed from {emt.CurrentTask?.Name} to EMTIdle", 3);
            emt.CurrentTask = new EMTIdle(emt, Player, PedProvider, Tasker, PlacesOfInterest, emt);
            GameFiber.Yield();//TR Added back 4
            emt.CurrentTask.Start();
        }
    }
    private void SetRespondTask(EMT emt, PedExt targetPed)
    {
        if (emt.CurrentTask?.Name != "EMTRespond" || (targetPed != null && emt.CurrentTask?.OtherTarget?.Handle != targetPed.Handle))// && Cop.IsIdleTaskable)
        {
            EntryPoint.WriteToConsole($"TASKER: Cop {emt.Pedestrian.Handle} Task Changed from {emt.CurrentTask?.Name} to EMTIdle", 3);
            emt.CurrentTask = new EMTRespond(emt, Player, targetPed);
            GameFiber.Yield();//TR Added back 4
            emt.CurrentTask.Start();
        }
    }
    private PedExt PedToGoTo(EMT emt)
    {
        PedExt MainTarget = null;
        if (emt.Pedestrian.Exists())
        {
            List<EMTTarget> EMTsPossibleTargets = new List<EMTTarget>();
            foreach (PedExt possibleTarget in PossibleTargets)
            {
                int TotalOtherAssignedEMTs = PedProvider.Pedestrians.EMTList.Count(x => x.Handle != emt.Handle && x.CurrentTask != null && x.CurrentTask.OtherTarget != null && x.CurrentTask.OtherTarget.Handle == possibleTarget.Handle);
                if (possibleTarget.Pedestrian.Exists())
                {
                    EMTsPossibleTargets.Add(new EMTTarget(possibleTarget, possibleTarget.Pedestrian.DistanceTo2D(emt.Pedestrian)) { TotalAssignedEMTs = TotalOtherAssignedEMTs });
                }
            }
            EMTTarget MainPossibleTarget = EMTsPossibleTargets
                .OrderBy(x => x.IsOverloaded)
                .ThenBy(x => x.DistanceToTarget).FirstOrDefault();

            if(MainPossibleTarget != null && !MainPossibleTarget.IsOverloaded)
            {
                MainTarget = MainPossibleTarget.Target;
            }
        }       
        return MainTarget;
    }
    private void SetPossibleTargets()
    {
        List<PedExt> TotalList = new List<PedExt>();
        TotalList.AddRange(PedProvider.Pedestrians.PoliceList);
        TotalList.AddRange(PedProvider.Pedestrians.CivilianList);
        TotalList.AddRange(PedProvider.Pedestrians.GangMemberList);
        TotalList.AddRange(PedProvider.Pedestrians.MerchantList);
        PossibleTargets = TotalList.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsInWrithe && !x.HasBeenTreatedByEMTs).ToList();//150f//writhe peds that are still alive
        PossibleTargets.AddRange(PedProvider.Pedestrians.DeadPeds.Where(x => x.Pedestrian.Exists() && !x.HasBeenTreatedByEMTs));//dead peds go here?
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
        public bool IsOverloaded => TotalAssignedEMTs > 2;
    }
}


