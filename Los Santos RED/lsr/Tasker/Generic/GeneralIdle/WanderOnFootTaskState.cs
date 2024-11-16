using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class WanderOnFootTaskState : TaskState
{
    private PedExt PedGeneral;
    private IEntityProvideable World;
    private SeatAssigner SeatAssigner;
    private ISettingsProvideable Settings;
    private bool BlockPermanentEvents = false;

    private Vector3 taskedPosition;
    private uint GameTimeBetweenScenarios;
    private uint GameTimeLastStartedScenario;
    private uint GameTimeLastChangedWanderStuff;
    private uint GameTimeBetweenWanderDecision = 60000;
    private uint GameTimeLastExitedVehicle;

    private bool hasBeenVehiclePatrolTasked;
    private uint GameTimeLastStartedFootPatrol;
    private uint GameTimeBetweenFootPatrols;
    private uint MinGameTimeBetweenFootPatrols = 60000;
    private uint MaxGameTimeBetweenFootPatrols = 120000;
    private uint MinGameTimeBetweenGuarding = 60000;
    private uint MaxGameTimeBetweenGuarding = 120000;

    private float PercentageToTransitionToPatrol = 15f;
    private float PercentageToTransitionToGuard = 25f;

    private bool canGuard = false;
    private bool canPatrol = false;
    private bool IsGuarding = false;
    private bool IsPatrolling = false;
    private bool IsTaskedGuarding = false;
    private bool IsTaskedPatrolling = false;
    private bool HasSpawnRequirements = false;
    private bool ForceStandardScenarios = false;


    public WanderOnFootTaskState(PedExt pedGeneral, IEntityProvideable world, SeatAssigner seatAssigner, ISettingsProvideable settings, bool blockPermanentEvents, bool forceStandardScenarios)
    {
        PedGeneral = pedGeneral;
        World = world;
        SeatAssigner = seatAssigner;
        Settings = settings;
        BlockPermanentEvents = blockPermanentEvents;
        ForceStandardScenarios = forceStandardScenarios;
    }

    public bool IsValid => PedGeneral != null && PedGeneral.Pedestrian.Exists() && !PedGeneral.IsInVehicle;
    public string DebugName { get; } = "WanderOnFootTaskState";
    public void Dispose()
    {

    }
    public void Start()
    {
        if (!PedGeneral.Pedestrian.Exists())
        {
            return;
        }
        if (PedGeneral.LocationTaskRequirements.TaskRequirements.Equals(TaskRequirements.None))
        {
            if (PedGeneral.IsLocationSpawned)
            {
                canGuard = true;
            }
            else
            {
                canPatrol = true;
            }
        }
        else
        {
            HasSpawnRequirements = true;
            if (PedGeneral.LocationTaskRequirements.TaskRequirements.HasFlag(TaskRequirements.Guard))
            {
                canGuard = true;
            }
            if (PedGeneral.LocationTaskRequirements.TaskRequirements.HasFlag(TaskRequirements.Patrol))
            {
                canPatrol = true;
            }
        }
        if (canGuard)
        {
            IsGuarding = true;
        }
        else if (canPatrol)
        {
            IsPatrolling = true;
        }
        EntryPoint.WriteToConsole($"PED {PedGeneral.Pedestrian.Handle} IsGuarding {IsGuarding} IsPatrolling {IsPatrolling} canGuard {canGuard} canPatrol {canPatrol} HasSpawnRequirements {HasSpawnRequirements} {PedGeneral.LocationTaskRequirements.TaskRequirements}");   
        SetTasking();
    }
    public void Stop()
    {
        PedGeneral.ClearTasks(true);
    }
    public void Update()
    {
        if(IsGuarding)
        {
            UpdateGuardArea();
        }
        else
        {
            UpdateFootPatrol();
        }
    }
    private void SetTasking()
    {
        PedGeneral.ClearTasks(true);
        if (IsGuarding)
        {
            GuardAreaTask();
        }
        else if (IsPatrolling)
        {
            FootPatrolTask();
        }
    }
    private void UpdateGuardArea()
    {
        if (!PedGeneral.Pedestrian.Exists())
        {
            return;
        }
        if (GameTimeLastStartedScenario > 0 && Game.GameTime - GameTimeLastStartedScenario >= GameTimeBetweenScenarios)
        {
            if (canPatrol && RandomItems.RandomPercent(PercentageToTransitionToPatrol))//10 percent let tham transition to foot patrol people
            {
                IsGuarding = false;
                IsPatrolling = true;
                SetTasking();
                //EntryPoint.WriteToConsole($"PED {PedGeneral.Handle} TRANSITIONED FROM GUARDING TO PATROLLING");
            }
            GameTimeLastStartedScenario = Game.GameTime;
        }
    }
    private void GuardAreaTask()
    {
        if (!PedGeneral.Pedestrian.Exists())
        {
            return;
        }
        EntryPoint.WriteToConsole($"{PedGeneral.Handle} GuardAreaTask RAN HasSpawnRequirements: {HasSpawnRequirements} TaskRequirements: {PedGeneral.LocationTaskRequirements.TaskRequirements} ForceStandardScenarios {ForceStandardScenarios}");

        PedGeneral.ClearTasks(true);
        bool useLocal = false;
        List<string> DealerScenarios = new List<string>() { "WORLD_HUMAN_DRUG_DEALER", "WORLD_HUMAN_DRUG_DEALER_HARD" };
        List<string> NonDealerScenarios = new List<string>() { "WORLD_HUMAN_SMOKING", "WORLD_HUMAN_AA_SMOKE", "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_HANG_OUT_STREET", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_DRINKING" };
        List<string> AllScenarios = new List<string>() { "WORLD_HUMAN_DRUG_DEALER", "WORLD_HUMAN_DRUG_DEALER_HARD", "WORLD_HUMAN_SMOKING", "WORLD_HUMAN_AA_SMOKE", "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_HANG_OUT_STREET", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_DRINKING", "WORLD_HUMAN_GUARD_STAND" };
        List<string> NormalScenarios = new List<string>() { "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_HANG_OUT_STREET", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND" };
        List<string> BasicScenarios = new List<string>() { "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_GUARD_STAND" };



        List<string> DogScenarios = new List<string>() { "WORLD_DOG_BARKING_ROTTWEILER","WORLD_DOG_BARKING_RETRIEVER","WORLD_DOG_BARKING_SHEPHERD","WORLD_DOG_SITTING_ROTTWEILER","WORLD_DOG_SITTING_RETRIEVER","WORLD_DOG_SITTING_SHEPHERD","WORLD_DOG_BARKING_SMALL","WORLD_DOG_SITTING_SMALL" };

        string ScenarioChosen = "WORLD_HUMAN_STAND_IMPATIENT";

        if (BlockPermanentEvents)
        {
            PedGeneral.Pedestrian.BlockPermanentEvents = true;
            PedGeneral.Pedestrian.KeepTasks = true;
        }


        if (HasSpawnRequirements)
        {
            if (PedGeneral.LocationTaskRequirements.ForcedScenarios != null && PedGeneral.LocationTaskRequirements.ForcedScenarios.Any())
            {
                ScenarioChosen = PedGeneral.LocationTaskRequirements.ForcedScenarios.PickRandom();
            }
            else
            {
                if (PedGeneral.LocationTaskRequirements.TaskRequirements.HasFlag(TaskRequirements.AnyScenario))
                {
                    ScenarioChosen = AllScenarios.PickRandom();
                }
                else if (PedGeneral.LocationTaskRequirements.TaskRequirements.HasFlag(TaskRequirements.StandardScenario))
                {
                    ScenarioChosen = NormalScenarios.PickRandom();
                }
                else if (PedGeneral.LocationTaskRequirements.TaskRequirements.HasFlag(TaskRequirements.LocalScenario))
                {
                    useLocal = true;
                }
                if (PedGeneral.LocationTaskRequirements.TaskRequirements.HasFlag(TaskRequirements.BasicScenario))
                {
                    ScenarioChosen = BasicScenarios.PickRandom();
                }
            }
        }
        else if (ForceStandardScenarios)
        {
            ScenarioChosen = NormalScenarios.PickRandom();
        }
        else
        {
            if(PedGeneral.IsAnimal)
            {
                ScenarioChosen = DogScenarios.PickRandom();
            }
            else if (PedGeneral.HasMenu)
            {
                ScenarioChosen = DealerScenarios.PickRandom();
            }
            else
            {
                ScenarioChosen = NonDealerScenarios.PickRandom();
            }
        }

        EntryPoint.WriteToConsole($"ScenarioChosen {ScenarioChosen.ToString()}");

        if(PedGeneral.LocationTaskRequirements.TaskRequirements.HasFlag(TaskRequirements.EquipLongGunWhenIdle) || PedGeneral.LocationTaskRequirements.TaskRequirements.HasFlag(TaskRequirements.EquipSidearmWhenIdle) || PedGeneral.LocationTaskRequirements.TaskRequirements.HasFlag(TaskRequirements.EquipMeleeWhenIdle))
        {
            NativeFunction.Natives.TASK_GUARD_CURRENT_POSITION(PedGeneral.Pedestrian, PedGeneral.LocationTaskRequirements.TaskRequirements.HasFlag(TaskRequirements.CanMoveWhenGuarding) ? 15f : 0.0f, 5f, true);
           // EntryPoint.WriteToConsole($"WANDER ON FOOT useLocal {useLocal} 111111");
        }
        else if (useLocal && NativeFunction.Natives.DOES_SCENARIO_EXIST_IN_AREA<bool>(PedGeneral.Pedestrian.Position.X, PedGeneral.Pedestrian.Position.Y, PedGeneral.Pedestrian.Position.Z, 3f, true))
        {
            NativeFunction.CallByName<bool>("TASK_USE_NEAREST_SCENARIO_TO_COORD_WARP", PedGeneral.Pedestrian, PedGeneral.Pedestrian.Position.X, PedGeneral.Pedestrian.Position.Y, PedGeneral.Pedestrian.Position.Z, 3f, 0);
            //EntryPoint.WriteToConsole($"WANDER ON FOOT useLocal {useLocal} 222222");
        }
        else
        {
            NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", PedGeneral.Pedestrian, ScenarioChosen, 0, true);
            //EntryPoint.WriteToConsole($"WANDER ON FOOT useLocal {useLocal} 33333");
        }
        GameTimeBetweenScenarios = RandomItems.GetRandomNumber(MinGameTimeBetweenGuarding, MaxGameTimeBetweenGuarding);
        GameTimeLastStartedScenario = Game.GameTime;
        
    }
    private void UpdateFootPatrol()
    {
        if (!PedGeneral.Pedestrian.Exists())
        {
            return;
        }
        if (GameTimeLastStartedFootPatrol > 0 && Game.GameTime - GameTimeLastStartedFootPatrol >= GameTimeBetweenFootPatrols)
        {
            if (canGuard && RandomItems.RandomPercent(PercentageToTransitionToGuard))
            {
                IsGuarding = true;
                IsPatrolling = false;
                SetTasking();
               // EntryPoint.WriteToConsole($"PED {PedGeneral.Handle} TRANSITIONED FROM PATROLLING TO GUARDING");
            }
            GameTimeLastStartedFootPatrol = Game.GameTime;
        }    
    }
    private void FootPatrolTask()
    {
        if (!PedGeneral.Pedestrian.Exists())
        {
            return;
        }
        if (BlockPermanentEvents)
        {
            PedGeneral.Pedestrian.BlockPermanentEvents = true;
            PedGeneral.Pedestrian.KeepTasks = true;
        }
        NativeFunction.Natives.TASK_WANDER_STANDARD(PedGeneral.Pedestrian, 0, 0);
        //NativeFunction.Natives.TASK_WANDER_IN_AREA(Ped.Pedestrian, Ped.Pedestrian.Position.X, Ped.Pedestrian.Position.Y, Ped.Pedestrian.Position.Z, 100f, 0f, 0f);
        GameTimeBetweenFootPatrols = RandomItems.GetRandomNumber(MinGameTimeBetweenFootPatrols, MaxGameTimeBetweenFootPatrols);
        GameTimeLastStartedFootPatrol = Game.GameTime;     
    }
}

