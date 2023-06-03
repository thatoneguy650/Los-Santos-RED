using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DispatchScannerFiles;


public class VehicleImpoundLot
{
    private bool IsFreeToEnter = false;
    private ILocationAreaRestrictable Location;
    public VehicleImpoundLot()
    {

    }

    public VehicleImpoundLot(string lotName, List<SpawnPlace> parkingSpots, Vector2[] boundaries)
    {
        LotName = lotName;
        ParkingSpots = parkingSpots;
        Boundaries = boundaries;
    }
    public string LotName { get; set; }
    public List<SpawnPlace> ParkingSpots { get; set; }
    public Vector2[] Boundaries { get; set; }
    public List<InteriorDoor> Gates { get; set; }
    public void Setup(ILocationAreaRestrictable location)
    {
        Location = location;
    }
    public void Acivate()
    {
        IsFreeToEnter = false;
        LockGates();
    }



    public void AddDistanceOffset(Vector3 offsetToAdd)
    {
        if (ParkingSpots != null)
        {
            foreach (SpawnPlace sp in ParkingSpots)
            {
                sp.AddDistanceOffset(offsetToAdd);
            }
        }
        if(Boundaries != null)
        {
            for (int index = 0; index < Boundaries.Length; index++)
            {
                Vector2 item = Boundaries[index];
                item.X += offsetToAdd.X;
                item.Y += offsetToAdd.Y;
            }
        }
    }
    public bool ImpoundVehicle(VehicleExt toImpound, ITimeReportable time)
    {
        if (toImpound == null || !toImpound.Vehicle.Exists() || ParkingSpots == null)
        {
            EntryPoint.WriteToConsole("IMPOUND VEHICLE FAIL SUB 1");
            return false;
        }
        SpawnPlace ParkingSpot = null;
        foreach (SpawnPlace sp in ParkingSpots)
        {
            if (!Rage.World.GetEntities(sp.Position, 7f, GetEntitiesFlags.ConsiderAllVehicles).Any())
            {
                ParkingSpot = sp;
                break;
            }
        }
        if (ParkingSpot == null)
        {
            EntryPoint.WriteToConsole("IMPOUND VEHICLE FAIL SUB 2");
            return false;
        }
        toImpound.Vehicle.Position = ParkingSpot.Position;
        toImpound.Vehicle.Heading = ParkingSpot.Heading;


        toImpound.SetImpounded(time);
        return true;

       // string ImpoundedNotification = $"Vehicle: {toImpound.FullName(true)}~n~Plate #:{toImpound?.CarPlate.ToString()}";

        //find parking spot and move vehicle
        //set stats
        //show info
    }
    public void Update(ILocationInteractable Player)
    {    
        if (Boundaries == null || !Boundaries.Any() || IsFreeToEnter)
        {
            Location.SetRestrictedArea(false);
            return;
        }
        Location.SetRestrictedArea(NativeHelper.IsPointInPolygon(new Vector2(Player.Position.X, Player.Position.Y), Boundaries));
    }

    public void RemoveRestriction()
    {
        IsFreeToEnter = true;
        UnLockGates();
    }
    private void LockGates()
    {
        if(Gates == null)
        {
            return;
        }
        foreach(InteriorDoor id in Gates)
        {
            id.LockDoor();
        }
    }
    private void UnLockGates()
    {
        if (Gates == null)
        {
            return;
        }
        foreach (InteriorDoor id in Gates)
        {
            id.UnLockDoor();
        }
    }
}
