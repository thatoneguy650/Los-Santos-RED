using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SecurityGuardTaskManager
{
    private SecurityGuard SecurityGuard;
    private ISettingsProvideable Settings;
    private IEntityProvideable World;

    public SecurityGuardTaskManager(SecurityGuard securityGuard, ISettingsProvideable settings, IEntityProvideable world)
    {
        SecurityGuard = securityGuard;
        Settings = settings;
        World = world;
    }
    public void Setup()
    {

    }
    public void Dispose()
    {

    }
    //public void Update()
    //{
    //    if(!SecurityGuard.CanBeTasked)
    //    {
    //        SecurityGuard.CurrentTask = null;
    //        return;
    //    }
    //    if(SecurityGuard.HasBeenSpawnedFor < 2000)
    //    {
    //        return;
    //    }
    //    if (SecurityGuard.NeedsTaskAssignmentCheck && SecurityGuard.CanBeAmbientTasked)
    //    {
    //        UpdateCurrentTask();
    //    }
    //    if (SecurityGuard.CurrentTask == null || !SecurityGuard.CurrentTask.ShouldUpdate)
    //    {
    //        return;
    //    }
    //    GameFiber.Yield();


    //    PedExt otherTarget = PedToAttack(SecurityGuard);


    //    SecurityGuard.CurrentTarget = otherTarget;
    //    SecurityGuard.GameTimeLastUpdatedTarget = Game.GameTime;
    //    SecurityGuard.UpdateTask(otherTarget);    
    //}
    //private void UpdateCurrentTask()//this should be moved out?
    //{
    //    SecurityGuard.GameTimeLastUpdatedTask = Game.GameTime;
    //    if (SecurityGuard.DistanceToPlayer > Player.ActiveDistance)
    //    {
    //        SetIdle();
    //        return;
    //    }
    //    PedExt MainTarget = PedToAttack(SecurityGuard);
    //    if (MainTarget != null)
    //    {
    //        SetAIApprehend(MainTarget);
    //        return;
    //    }

    //    if (Player.IsWanted && SecurityGuard.IsRespondingToWanted)
    //    {
    //        UpdateCurrentTaskWanted();
    //    }
    //    else if (Player.Investigation.IsActive && Player.Investigation.RequiresPolice && SecurityGuard.IsRespondingToInvestigation)
    //    {
    //        SetInvestigate();
    //    }
    //    else if (World.CitizenWantedLevel > 0 && SecurityGuard.IsRespondingToCitizenWanted)
    //    {
    //        SetInvestigate();
    //    }
    //    else
    //    {
    //        SetIdle();
    //    }
 
    //}
    //private void UpdateCurrentTaskWanted()
    //{
    //    if (Player.IsInSearchMode)
    //    {
    //        SetLocate();
    //        return;
    //    }
    //    if(SecurityGuard.DistanceToPlayer > 150f)
    //    {
    //        SetLocate();
    //        return;
    //    }
    //    if (Player.PoliceResponse.IsDeadlyChase && !Player.IsAttemptingToSurrender)
    //    {
    //        SetKill();
    //    }
    //    else
    //    {
    //        SetChase();
    //    }        
    //}
    //private void SetInvestigate()
    //{
    //    if(SecurityGuard.CurrentTask?.Name == "Investigate")
    //    {
    //        return;
    //    }
    //    SecurityGuard.CurrentTask = new Investigate(SecurityGuard, Player, Settings, World);
    //    SecurityGuard.WeaponInventory.Reset();
    //    GameFiber.Yield();
    //    SecurityGuard.CurrentTask.Start();      
    //}
    //private void SetIdle()
    //{
    //    if (SecurityGuard.CurrentTask?.Name == "Idle")
    //    {
    //        return;
    //    }
    //    SecurityGuard.CurrentTask = new GeneralIdle(SecurityGuard, SecurityGuard, Player, World, World.Vehicles.PoliceVehicleList, PlacesOfInterest, Settings);
    //    SecurityGuard.WeaponInventory.Reset();
    //    GameFiber.Yield();
    //    SecurityGuard.CurrentTask.Start();       
    //}
    //private void SetLocate()
    //{
    //    if (SecurityGuard.CurrentTask?.Name == "Locate")
    //    {
    //        return;
    //    }
    //    SecurityGuard.CurrentTask = new Locate(SecurityGuard, Player, Settings);
    //    SecurityGuard.WeaponInventory.Reset();
    //    GameFiber.Yield();
    //    SecurityGuard.CurrentTask.Start();
    //}
    //private void SetChase()
    //{
    //    if (SecurityGuard.CurrentTask?.Name == "Chase")
    //    {
    //        return;
    //    }
    //    SecurityGuard.CurrentTask = new Chase(SecurityGuard, Player, World, SecurityGuard, Settings);
    //    SecurityGuard.WeaponInventory.Reset();
    //    GameFiber.Yield();
    //    SecurityGuard.CurrentTask.Start();
    //}
    //private void SetKill()
    //{
    //    if (SecurityGuard.CurrentTask?.Name == "Kill")
    //    {
    //        return;
    //    }
    //    SecurityGuard.CurrentTask = new Kill(SecurityGuard, Player, Settings);
    //    SecurityGuard.WeaponInventory.Reset();
    //    GameFiber.Yield();
    //    SecurityGuard.CurrentTask.Start();
    //}
    //private void SetAIApprehend(PedExt MainTarget)
    //{
    //    if (SecurityGuard.CurrentTask?.Name == "AIApprehend")
    //    {
    //        return;
    //    }
    //    SecurityGuard.CurrentTask = new AIApprehend(SecurityGuard, Player, SecurityGuard, Settings) { OtherTarget = MainTarget };
    //    SecurityGuard.WeaponInventory.Reset();
    //    GameFiber.Yield();
    //    SecurityGuard.CurrentTask.Start();
    //}
}

