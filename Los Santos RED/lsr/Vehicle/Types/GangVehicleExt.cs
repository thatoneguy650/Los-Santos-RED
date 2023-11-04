using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSR.Vehicles
{
    public class GangVehicleExt : VehicleExt
    {
        public override bool CanRandomlyHaveIllegalItems { get; set; } = true;
        public override float PercentageToGetRandomWeapons => Settings.SettingsManager.PlayerOtherSettings.PercentageOfGangVehiclesToGetRandomWeapons;
        public GangVehicleExt(Vehicle vehicle, ISettingsProvideable settings) : base(vehicle, settings)
        {
        }
        public override void AddVehicleToList(IEntityProvideable world)
        {
            world.Vehicles.AddGang(this);
        }
    }
}
