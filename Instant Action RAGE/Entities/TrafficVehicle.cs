using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TrafficVehicle
{
    static TrafficVehicle()
    {
        rnd = new Random();
    }
    public TrafficVehicle()
    {

    }
    public TrafficVehicle(Vehicle _VehicleEntity,bool _WaitingAtRedLight, float _DotProductResult)
    {
        VehicleEntity = _VehicleEntity;
        WaitingAtRedLight = _WaitingAtRedLight;
        DotProductResult = _DotProductResult;
    }

    public TrafficVehicle(Vehicle vehicle, bool v)
    {
        this.vehicle = vehicle;
        this.v = v;
    }

    private static Random rnd;
    private Vehicle vehicle;
    private bool v;

    public Vehicle VehicleEntity { get; set; }
    public bool WaitingAtRedLight { get; set; } = false;
    public float DotProductResult { get; set; }
}

