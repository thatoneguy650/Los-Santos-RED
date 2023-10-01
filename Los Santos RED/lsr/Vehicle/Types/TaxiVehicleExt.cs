using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSR.Vehicles
{
    public class TaxiVehicleExt : VehicleExt
    {
        public override bool CanRandomlyHaveIllegalItems { get; set; } = true;
        public TaxiFirm TaxiFirm { get; set; }
        public TaxiVehicleExt(Vehicle vehicle, ISettingsProvideable settings) : base(vehicle, settings)
        {
        }
        public override void AddVehicleToList(IEntityProvideable world)
        {
            world.Vehicles.AddTaxi(this);
        }
        public override string GetDebugString()
        {
            string toReturn = base.GetDebugString();
            if (TaxiFirm == null)
            {
                return toReturn;
            }
            toReturn += $" TaxiFirm: {TaxiFirm.FullName}";
            return toReturn;
        }
    }
}
