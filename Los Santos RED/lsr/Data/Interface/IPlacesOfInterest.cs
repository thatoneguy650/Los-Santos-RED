using LosSantosRED.lsr.Locations;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IPlacesOfInterest
    {
        PossibleLocations PossibleLocations { get; }
        List<GameLocation> InteractableLocations();
        List<GameLocation> AllLocations();
        List<ILocationRespawnable> BustedRespawnLocations();
        List<ILocationImpoundable> VehicleImpoundLocations();
        List<ILocationRespawnable> HospitalRespawnLocations();
        List<ILocationSetupable> LocationsToSetup();
        DeadDrop GetUsableDeadDrop(bool IsOnMPMap, LocationData locationData);
        GangDen GetMainDen(string iD, bool isMPMapLoaded, LocationData locationData);
        List<ILocationAreaRestrictable> RestrictedAreaLocations();
        List<ILEDispatchableLocation> LEDispatchLocations();
    }
}
