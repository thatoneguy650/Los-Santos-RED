using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;

public class AIIdentify : ComplexTask
{
    private enum IdentifyState
    {
        ExitVehicle,
        Approach,
        Ask,
        Complete
    }

    private readonly Cop Cop;
    private IdentifyState State;
    private uint StateStartedAt;
    private uint LastApproachCommandAt;
    private Vector3 LastTargetPosition;

    public AIIdentify(Cop cop, ITargetable player) : base(player, cop, 500)
    {
        Cop = cop;
        Name = "BOLOIdentify";
        SubTaskName = "Approach";
    }

    public override void Start()
    {
        if (Cop == null || !Cop.Pedestrian.Exists())
        {
            Finish();
            return;
        }

        if (Cop.IsInVehicle && Cop.Pedestrian.CurrentVehicle.Exists())
        {
            State = IdentifyState.ExitVehicle;
            SubTaskName = "ExitVehicle";
            StateStartedAt = Game.GameTime;
            NativeFunction.Natives.TASK_LEAVE_VEHICLE(Cop.Pedestrian, Cop.Pedestrian.CurrentVehicle, 0);
            return;
        }

        State = IdentifyState.Approach;
        StateStartedAt = Game.GameTime;
        Update();
    }

    public override void Update()
    {
        if (State == IdentifyState.Complete)
            return;
        if (Cop == null || !Cop.Pedestrian.Exists() || Player == null ||
            Player.Character == null || !Player.Character.Exists() || Player.IsDead ||
            Player.IsInVehicle)
        {
            Finish();
            return;
        }

        float distance = Cop.Pedestrian.DistanceTo2D(Player.Character);
        if (distance > 35f)
        {
            Finish();
            return;
        }

        if (State == IdentifyState.ExitVehicle)
        {
            if (!Cop.IsInVehicle)
            {
                State = IdentifyState.Approach;
                StateStartedAt = Game.GameTime;
                SubTaskName = "Approach";
            }
            else if (Game.GameTime - StateStartedAt >= 10000)
            {
                Finish();
                return;
            }
        }

        if (State == IdentifyState.Approach)
        {
            if (distance <= 5f)
            {
                State = IdentifyState.Ask;
                StateStartedAt = Game.GameTime;
                SubTaskName = "Identify";
                NativeFunction.Natives.TASK_STAND_STILL(Cop.Pedestrian, 5000);
                NativeFunction.Natives.TASK_TURN_PED_TO_FACE_ENTITY(Cop.Pedestrian, Player.Character, 1500);
                Game.DisplaySubtitle("An officer approaches to identify you.", 3500);
            }
            else if (Game.GameTime - LastApproachCommandAt >= 1500 ||
                LastTargetPosition.DistanceTo2D(Player.Position) >= 3f)
            {
                NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Cop.Pedestrian,
                    Player.Position.X, Player.Position.Y, Player.Position.Z,
                    1.15f, 2500, Player.Character.Heading, 0.5f);
                LastTargetPosition = Player.Position;
                LastApproachCommandAt = Game.GameTime;
            }
        }
        else if (State == IdentifyState.Ask)
        {
            if (distance > 8f || Game.GameTime - StateStartedAt >= 3500)
                Finish();
        }

        GameTimeLastRan = Game.GameTime;
    }

    public override void Stop()
    {
        // The CopBrain assigns the next normal task after this short encounter.
    }

    public override void ReTask()
    {
        if (State != IdentifyState.Complete)
            Update();
    }

    private void Finish()
    {
        State = IdentifyState.Complete;
        SubTaskName = "Complete";
        if (Cop != null && Cop.CurrentTask == this)
            Cop.CurrentTask = null;
    }
}
