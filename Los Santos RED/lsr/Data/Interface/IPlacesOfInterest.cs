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
        List<InteractableLocation> InteractableLocations();
        List<BasicLocation> AllLocations();
        List<ILocationDispatchable> PoliceDispatchLocations();
        List<ILocationRespawnable> BustedRespawnLocations();
        List<ILocationGangAssignable> GangAssignableLocations();
        List<ILocationSetupable> LocationsToSetup();
        List<ILocationDispatchable> EMSDispatchLocations();
        DeadDrop GetUsableDeadDrop(bool IsOnMPMap);
    }
}
