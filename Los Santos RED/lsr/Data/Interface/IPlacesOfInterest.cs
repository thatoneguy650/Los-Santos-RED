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
        List<BasicLocation> BasicLocations();
        List<InteractableLocation> InteractableLocations();
        List<BasicLocation> AllLocations();
        List<ILocationDispatchable> PoliceDispatchLocations();
        List<ILocationRespawnable> BustedRespawnLocations();
        List<ILocationAgencyAssignable> AgencyAssignableLocations();
        List<ILocationGangAssignable> GangAssignableLocations();
        //List<BasicLocation> BasicLocations { get; }
        //List<InteractableLocation> InteractableLocations { get; }
        //List<BasicLocation> AllLocations { get; }
        //List<ILocationDispatchable> PoliceDispatchLocations { get; }
        //List<ILocationRespawnable> BustedRespawnLocations { get; }
        //List<ILocationAgencyAssignable> AgencyAssignableLocations { get; }
        //List<ILocationGangAssignable> GangAssignableLocations { get; }
    }
}
