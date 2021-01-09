using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Kill : ComplexTask
{
    public Kill(IComplexTaskable cop, ITargetable player) : base(player, cop, 0)
    {
        Name = "Kill";
        SubTaskName = "";
    }
    public override void Start()
    {
        ClearTasks();
        Cop.Pedestrian.Tasks.FightAgainstClosestHatedTarget(70f);
    }
    public override void Update()
    {

    }
    public void ClearTasks()//temp public
    {
        if (Cop.Pedestrian.Exists())
        {
            int seatIndex = 0;
            Vehicle CurrentVehicle = null;
            bool WasInVehicle = false;
            if (Cop.Pedestrian.IsInAnyVehicle(false))
            {
                WasInVehicle = true;
                CurrentVehicle = Cop.Pedestrian.CurrentVehicle;
                seatIndex = Cop.Pedestrian.SeatIndex;
            }
            Cop.Pedestrian.Tasks.Clear();

            Cop.Pedestrian.BlockPermanentEvents = false;
            Cop.Pedestrian.KeepTasks = false;
            Cop.Pedestrian.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Player, Relationship.Neutral);
            if (WasInVehicle && !Cop.Pedestrian.IsInAnyVehicle(false) && CurrentVehicle != null)
            {
                Cop.Pedestrian.WarpIntoVehicle(CurrentVehicle, seatIndex);
            }
            if (Cop.IsDriver && Cop.Pedestrian.CurrentVehicle != null && Cop.Pedestrian.CurrentVehicle.HasSiren)
            {
                Cop.Pedestrian.CurrentVehicle.IsSirenOn = false;
                Cop.Pedestrian.CurrentVehicle.IsSirenSilent = false;
            }
            if (Player.IsWanted)
            {
                NativeFunction.CallByName<bool>("SET_PED_ALERTNESS", Cop.Pedestrian, 3);
                if (Player.PoliceResponse.IsDeadlyChase)
                {
                    Cop.Pedestrian.Tasks.FightAgainst(Game.LocalPlayer.Character, -1);
                }
            }
            Game.Console.Print(string.Format("     ClearedTasks: {0}", Cop.Pedestrian.Handle));
        }
    }
    public override void Stop()
    {

    }
}

