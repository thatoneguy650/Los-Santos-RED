using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FireExtinguish : ComplexTask
{
    protected uint GameTimeStarted;
    protected IComplexTaskable complexTaskablePed;
    protected PedExt PedGeneral;
    protected IEntityProvideable World;
    protected IPlacesOfInterest PlacesOfInterest;
    protected SeatAssigner SeatAssigner;
    protected TaskState CurrentTaskState;
    protected ISettingsProvideable Settings;
    protected IWeaponIssuable WeaponIssuable;
    protected bool BlockPermanentEvents = false;

    public FireExtinguish(PedExt pedGeneral, IComplexTaskable ped, ITargetable player, IEntityProvideable world, List<VehicleExt> possibleVehicles, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, bool blockPermanentEvents,
        IWeaponIssuable weaponIssuable) : base(player, ped, 1500)//1500
    {
        PedGeneral = pedGeneral;
        complexTaskablePed = ped;
        Name = "FireExtinguish";
        SubTaskName = "";
        World = world;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        BlockPermanentEvents = blockPermanentEvents;
        WeaponIssuable = weaponIssuable;
    }
    public override void ReTask()
    {
    }
    public override void Start()
    {
        GameTimeStarted = Game.GameTime;
        CurrentTaskState?.Stop();
        GetNewTaskState();
        CurrentTaskState?.Start();
        // EntryPoint.WriteToConsole($"{PedGeneral.Handle} STARTED Task{CurrentTaskState?.DebugName}");
    }
    public override void Stop()
    {
        CurrentTaskState?.Stop();
    }
    public override void Update()
    {

        StandardUpdate();
    }
    private void StandardUpdate()
    {
        if (CurrentTaskState == null || !CurrentTaskState.IsValid)
        {
            if (Game.GameTime - GameTimeStarted >= 1000)
            {
                Start();
                //EntryPoint.WriteToConsole($"{PedGeneral.Handle} UPDATE START Task{CurrentTaskState?.DebugName} INVALID {CurrentTaskState?.IsValid}");
            }
        }
        else
        {
            SubTaskName = CurrentTaskState.DebugName;
            CurrentTaskState.Update();
        }
    }
    private void GetNewTaskState()
    {
      
        if (PedGeneral.Pedestrian.Exists())
        {
            NativeFunction.Natives.SET_PED_SHOULD_PLAY_IMMEDIATE_SCENARIO_EXIT(PedGeneral.Pedestrian);
        }
        CurrentTaskState = new GoToAndExtinguishTaskState(PedGeneral, Player, World, SeatAssigner, Settings, BlockPermanentEvents, WeaponIssuable);
        SubTaskName = "GoToAndExtinguishTaskState";
       
    }
}

