using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WitnessedCrime
{
    public WitnessedCrime(Crime crime, PedExt perpetrator, VehicleExt vehicle, WeaponInformation weapon, Vector3 location)
    {
        Crime = crime;
        Perpetrator = perpetrator;
        Vehicle = vehicle;
        Weapon = weapon;
        Location = location;
        GameTimeLastWitnessed = Game.GameTime;
    }
    public bool IsPlayerWitnessedCrime => Perpetrator == null;
    public Crime Crime { get; set; }
    public PedExt Perpetrator { get; set; }
    public VehicleExt Vehicle { get; set; }
    public WeaponInformation Weapon { get; set; }
    public uint GameTimeLastWitnessed { get; private set; }
    public Vector3 Location { get; set; }

    public bool HasBeenReactedTo { get; private set; }
    public void SetReactedTo()
    {
        HasBeenReactedTo = true;
    }
    public void UpdateWitnessed(VehicleExt vehicle, WeaponInformation weapon, Vector3 location)
    {
        if(vehicle != null)
        {
            Vehicle = vehicle;
        }
        if(Weapon != null)
        {
            Weapon = weapon;
        }
        if (location != Vector3.Zero)
        {
            Location = location;
        }
        GameTimeLastWitnessed = Game.GameTime;
        HasBeenReactedTo = false;
    }
}

