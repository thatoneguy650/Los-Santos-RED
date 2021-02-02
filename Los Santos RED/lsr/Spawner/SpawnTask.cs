using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SpawnTask
{
    private DispatchableOfficer OfficerType;
    private Agency Agency;
    private bool AddBlip;
    private Vector3 InitialPosition;
    private Vector3 StreetPosition;
    private float Heading;
    private VehicleExt Vehicle;
    private DispatchableVehicle VehicleType;
    public SpawnTask(Agency agency, Vector3 initialPosition, Vector3 streetPosition, float heading, DispatchableVehicle vehicleType, DispatchableOfficer officerType, bool addBlip)
    {
        Agency = agency;
        OfficerType = officerType;
        VehicleType = vehicleType;
        AddBlip = addBlip;
        InitialPosition = initialPosition;
        StreetPosition = streetPosition;
        Heading = heading;
    }
    public List<Cop> CreatedCops { get; private set; } = new List<Cop>();
    public List<VehicleExt> CreatedVehicles { get; private set; } = new List<VehicleExt>();
    private Vector3 Position
    {
        get
        {
            if (VehicleType.IsHelicopter)
            {
                return InitialPosition + new Vector3(0f, 0f, 250f);
            }
            else if (VehicleType.IsBoat)
            {
                return InitialPosition;
            }
            else
            {
                return StreetPosition;
            }
        }
    }
    public void AttemptSpawn()
    {
        if (Agency != null)
        {
            Vehicle = CreateVehicle();
            if (Vehicle != null && Vehicle.Vehicle.Exists())
            {
                Cop Cop = CreateCop();
                if (Cop != null && Cop.Pedestrian.Exists() && Vehicle != null && Vehicle.Vehicle.Exists())
                {
                    Cop.Pedestrian.WarpIntoVehicle(Vehicle.Vehicle, -1);
                    int OccupantsToAdd = RandomItems.MyRand.Next(VehicleType.MinOccupants, VehicleType.MaxOccupants + 1) - 1;
                    for (int OccupantIndex = 1; OccupantIndex <= OccupantsToAdd; OccupantIndex++)
                    {
                        Cop PassengerCop = CreateCop();
                        if (PassengerCop != null && PassengerCop.Pedestrian.Exists() && Vehicle != null && Vehicle.Vehicle.Exists())
                        {
                            PassengerCop.Pedestrian.WarpIntoVehicle(Vehicle.Vehicle, OccupantIndex - 1);
                        }
                    }
                }
                else
                {
                    if(Vehicle != null && Vehicle.Vehicle.Exists())
                    {
                        Vehicle.Vehicle.Delete();
                        //Game.Console.Print("Failed to complete spawn, deleting");
                    }
                }
            }
        }
    }
    private Cop CreateCop()
    {
        Ped CopPed = new Ped(OfficerType.ModelName, new Vector3(Position.X, Position.Y, Position.Z + 1f), Heading);
        GameFiber.Yield();
        if (CopPed.Exists())
        {
            NativeFunction.CallByName<bool>("SET_PED_AS_COP", CopPed, true);
            CopPed.RandomizeVariation();
            if (VehicleType.IsMotorcycle)
            {
                CopPed.GiveHelmet(false, HelmetTypes.PoliceMotorcycleHelmet, 4096);
                NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", CopPed, 4, 0, 0, 0);
            }
            else
            {
                NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", CopPed, 4, 1, 0, 0);
            }
            if (OfficerType.RequiredVariation != null)
            {
                OfficerType.RequiredVariation.ReplacePedComponentVariation(CopPed);
            }
            GameFiber.Yield();
            CopPed.IsPersistent = true;
            Cop PrimaryCop = new Cop(CopPed, CopPed.Health, Agency, true);
            PrimaryCop.IssueWeapons();
            CopPed.IsPersistent = true;
            if (AddBlip && CopPed.Exists())
            {
                Blip myBlip = CopPed.AttachBlip();
                myBlip.Color = Agency.AgencyColor;
                myBlip.Scale = 0.6f;
            }
            CreatedCops.Add(PrimaryCop);
            return PrimaryCop;
        }
        return null;
    }
    private VehicleExt CreateVehicle()
    {
        Game.Console.Print($"Attempting to spawn {VehicleType.ModelName}");
        Vehicle copcar = new Vehicle(VehicleType.ModelName, Position, Heading);
        GameFiber.Yield();
        if (copcar.Exists())
        {
            VehicleExt CopVehicle = new VehicleExt(copcar, true);
            if (copcar.Exists())
            {
                copcar.IsPersistent = true;
                CopVehicle.UpdateCopCarLivery(Agency);
                CopVehicle.UpgradeCopCarPerformance();
                CreatedVehicles.Add(CopVehicle);
                GameFiber.Yield();
                return CopVehicle;
            }
        }
        return null;
    }
}
