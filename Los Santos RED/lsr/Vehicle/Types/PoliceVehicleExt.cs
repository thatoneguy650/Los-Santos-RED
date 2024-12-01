using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSR.Vehicles
{
    public class PoliceVehicleExt : VehicleExt
    {
        public override bool CanRandomlyHaveIllegalItems { get; set; } = true;


        public override float PercentageToGetRandomWeapons => Settings.SettingsManager.PlayerOtherSettings.PercentageOfPoliceVehiclesToGetRandomWeapons;
        public override bool CanUpdatePlate => false;
        public bool IsOffDuty { get; set; } = false;
        public override bool HasSonarBlip => IsOffDuty ? false : true;
        public override bool CanNeverUpdatePlate => true;
        public PoliceVehicleExt(Vehicle vehicle, ISettingsProvideable settings) : base(vehicle, settings)
        {
        }
        public override void AddVehicleToList(IEntityProvideable world)
        {
            world.Vehicles.AddPolice(this);
        }
    }
}
