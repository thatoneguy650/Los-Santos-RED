using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ISaveable
    {
        string PlayerName { get; }
        string ModelName { get; }
        PedVariation CurrentModelVariation { get; }
        bool IsMale { get; }
        int Money { get; }
        Inventory Inventory { get; set; }
        //HeadBlendData CurrentHeadBlendData { get; }
        //List<HeadOverlayData> CurrentHeadOverlays { get; }
        //int CurrentPrimaryHairColor { get; }
        //int CurrentSecondaryColor { get; }
       // VehicleExt OwnedVehicle { get; }
        List<GangReputation> GangReputations { get; }
        Vector3 Position { get; }
        Ped Character { get; }
        List<VehicleExt> OwnedVehicles { get; }
    }
}
