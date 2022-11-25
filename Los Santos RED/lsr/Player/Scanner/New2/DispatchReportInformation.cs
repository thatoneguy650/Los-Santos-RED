using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DispatchReportInformation//has the most current information for the given scanner call
{
    public DispatchReportInformation()
    {

    }
    public DispatchReportInformation(bool seenOnFoot, bool seenByOfficers, Vector3 placeSeen)
    {
        SeenOnFoot = seenOnFoot;
        SeenByOfficers = seenByOfficers;
        PlaceSeen = placeSeen;
    }
    public DispatchReportInformation(bool seenOnFoot, bool seenByOfficers, Vector3 placeSeen, bool haveDescription)
    {
        SeenOnFoot = seenOnFoot;
        SeenByOfficers = seenByOfficers;
        PlaceSeen = placeSeen;
        HaveDescription = haveDescription;
    }







    public bool SeenOnFoot { get; set; } = true;




    public bool SeenByOfficers { get; set; } = false;




    public int InstancesObserved { get; set; } = 1;
    public Vector3 PlaceSeen { get; set; }





    public bool HaveDescription { get; set; }




    public float Speed { get; set; }
    public WeaponInformation WeaponSeen { get; set; }
    public VehicleExt VehicleSeen { get; set; }



}

