using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CrimeSceneDescription
{
    public CrimeSceneDescription()
    {

    }
    public CrimeSceneDescription(bool seenOnFoot, bool seenByOfficers, Vector3 placeSeen)
    {
        SeenOnFoot = seenOnFoot;
        SeenByOfficers = seenByOfficers;
        PlaceSeen = placeSeen;
    }
    public CrimeSceneDescription(bool seenOnFoot, bool seenByOfficers, Vector3 placeSeen, bool haveDescription)
    {
        SeenOnFoot = seenOnFoot;
        SeenByOfficers = seenByOfficers;
        PlaceSeen = placeSeen;
        HaveDescription = haveDescription;
    }
    public float Speed { get; set; }
    public WeaponInformation WeaponSeen { get; set; }
    public VehicleExt VehicleSeen { get; set; }
    public bool SeenOnFoot { get; set; } = true;
    public bool SeenByOfficers { get; set; } = false;
    public int InstancesObserved { get; set; } = 1;
    public Vector3 PlaceSeen { get; set; }
    public bool HaveDescription { get; set; }
    public Interior InteriorSeen { get; set; }
}
