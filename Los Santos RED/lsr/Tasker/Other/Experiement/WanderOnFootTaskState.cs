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
    private bool ForceGuard;
    private uint GameTimeBetweenScenarios;
    private uint GameTimeLastStartedScenario;
    private uint GameTimeLastStartedFootPatrol;
    private uint GameTimeBetweenFootPatrols;

    private bool isGuarding = false;
    private bool isPatrolling = false;
    private ISettingsProvideable Settings;
    public WanderOnFootTaskState(PedExt pedGeneral, IEntityProvideable world, SeatAssigner seatAssigner, ISettingsProvideable settings)
    {
        PedGeneral = pedGeneral;
        World = world;
        SeatAssigner = seatAssigner;
        Settings = settings;
    }

    public bool IsValid => PedGeneral != null && PedGeneral.Pedestrian.Exists() && !PedGeneral.IsInVehicle;
    public string DebugName { get; } = "WanderOnFootTaskState";
    public void Dispose()
    {

    }
    public void Start()
    {
        if (PedGeneral.IsAmbientSpawn && PedGeneral.HasExistedFor <= 10000)// && Cop.HasBeenSpawnedFor <= 10000)
        {
            ForceGuard = true;
        }
        else
        {
            ForceGuard = false;
        }
        Update();
    }
    public void Stop()
    {

    }
    public void Update()
    {
        if(ForceGuard)
        {
            GuardArea(!isGuarding);
        }
        else
        {
            FootPatrol(!isPatrolling);
        }
    }
    
    private void FootPatrol(bool IsFirstRun)
    {
        isGuarding = false;
        isPatrolling = true;
        if (PedGeneral.Pedestrian.Exists())
        {
            if (IsFirstRun)
            {
                NativeFunction.Natives.CLEAR_PED_TASKS(PedGeneral.Pedestrian);
                FootPatrolTask();
            }
            else
            {
                if (GameTimeLastStartedFootPatrol > 0 && Game.GameTime - GameTimeLastStartedFootPatrol >= GameTimeBetweenFootPatrols)
                {
                    if (PedGeneral.IsAmbientSpawn && RandomItems.RandomPercent(10f))//10 percent let tham transition to foot patrol people
                    {
                        ForceGuard = true;
                    }
                }
            }
        }
    }
    private void FootPatrolTask()
    {
        if (PedGeneral.Pedestrian.Exists())
        {
            if (Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringIdle)
            {
                PedGeneral.Pedestrian.BlockPermanentEvents = true;
            }
            else
            {
                PedGeneral.Pedestrian.BlockPermanentEvents = false;
            }
            PedGeneral.Pedestrian.KeepTasks = true;
            NativeFunction.Natives.TASK_WANDER_IN_AREA(PedGeneral.Pedestrian, PedGeneral.Pedestrian.Position.X, PedGeneral.Pedestrian.Position.Y, PedGeneral.Pedestrian.Position.Z, 100f, 0f, 0f);
            GameTimeBetweenFootPatrols = RandomItems.GetRandomNumber(30000, 90000);
            GameTimeLastStartedFootPatrol = Game.GameTime;
        }
    }


    private void GuardArea(bool IsFirstRun)
    {
        isGuarding = true;
        isPatrolling = false;
        if (PedGeneral.Pedestrian.Exists())
        {
            if (IsFirstRun)
            {
                NativeFunction.Natives.CLEAR_PED_TASKS(PedGeneral.Pedestrian);
                GuardAreaTask();
            }
            else
            {
                if (GameTimeLastStartedScenario > 0 && Game.GameTime - GameTimeLastStartedScenario >= GameTimeBetweenScenarios)
                {
                    if (RandomItems.RandomPercent(10f))//10 percent let tham transition to foot patrol people
                    {
                        ForceGuard = false;
                    }
                    else
                    {
                        GuardAreaTask();
                    }
                }
            }
        }
    }
    private void GuardAreaTask()
    {
        if (PedGeneral.Pedestrian.Exists())
        {
            if (Settings.SettingsManager.PoliceTaskSettings.BlockEventsDuringIdle)
            {
                PedGeneral.Pedestrian.BlockPermanentEvents = true;
            }
            else
            {
                PedGeneral.Pedestrian.BlockPermanentEvents = false;
            }
            PedGeneral.Pedestrian.KeepTasks = true;
            List<string> PossibleScenarios = new List<string>() { "WORLD_HUMAN_COP_IDLES", "WORLD_HUMAN_AA_COFFEE", "WORLD_HUMAN_AA_SMOKE", "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_STAND_MOBILE_UPRIGHT", "WORLD_HUMAN_SMOKING" };
            string ScenarioChosen = PossibleScenarios.PickRandom();
            NativeFunction.CallByName<bool>("TASK_START_SCENARIO_IN_PLACE", PedGeneral.Pedestrian, ScenarioChosen, 0, true);
            GameTimeBetweenScenarios = RandomItems.GetRandomNumber(30000, 90000);
            GameTimeLastStartedScenario = Game.GameTime;
        }
    }
}

