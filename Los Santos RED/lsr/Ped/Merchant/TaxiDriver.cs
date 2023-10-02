using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TaxiDriver : PedExt
{

    public TaxiFirm TaxiFirm { get; set; }
    public bool IsRideActive { get;private set; }
    public Vector3 DestinationLocation { get; private set; }
    public PedDrivingStyle TaxiDrivingStyle { get; set; }
    public TaxiDriver(Ped _Pedestrian, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, string _Name, string groupName, IEntityProvideable world, bool wasModSpawned) : base(_Pedestrian, settings, crimes, weapons, _Name, groupName, world)
    {
        WasModSpawned = wasModSpawned;
        PedBrain = new TaxiDriverBrain(this, Settings, world, weapons, this);
        TaxiDrivingStyle = new PedDrivingStyle("Normal", eCustomDrivingStyles.RegularDriving, 10f);
    }
    public void SetTaskingActive(Vector3 pickupLocation)
    {
        DestinationLocation = pickupLocation;
        IsRideActive = true;
    }
    public void ReleaseTasking()
    {
        IsRideActive = false;
        DestinationLocation = Vector3.Zero;
    }
}

