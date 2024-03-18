using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using ExtensionsMethods;
using LSR.Vehicles;

public class PedVehicleInteract : PedGeneralInteract
{
    private VehicleExt VehicleExt;
    private ISettingsProvideable Settings;
    protected override bool HasTargetPositionChanged => false;// TargetPosition.DistanceTo2D(Player.Character.GetOffsetPositionFront(Offset)) >= 0.1f;
    public PedVehicleInteract(IRespawnable player, PedExt pedExt, float offset, VehicleExt vehicleExt, ISettingsProvideable settings) : base(player, pedExt, offset)
    {
        Player = player;
        PedExt = pedExt;
        Offset = offset;
        VehicleExt = vehicleExt;
        Settings = settings;
    }

    protected override void GetDesiredPosition()
    {
        if (PedExt == null || !PedExt.Pedestrian.Exists())
        {
            return;
        }
        if (VehicleExt == null || !VehicleExt.Vehicle.Exists())
        {
            return;
        }
        VehicleDoorSeatData vdsd = new VehicleDoorSeatData("unknow", "unknow", 5, -1);
        TargetPosition = vdsd.GetDoorOffset(VehicleExt.Vehicle, Settings);
        TargetHeading = vdsd.GetDoorHeading(VehicleExt.Vehicle, Settings);
    }
}

