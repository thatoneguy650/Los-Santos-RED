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

        //GameLocation GetClosestLocation(Vector3 position, LocationType grave);
        //List<GameLocation> GetAllPlaces();
        //List<GameLocation> GetLocations(LocationType hospital);
        List<BasicLocation> GetAllBasicLocations();
        List<InteractableLocation> GetAllInteractableLocations();
        List<BasicLocation> GetAllLocations();
    }
}
