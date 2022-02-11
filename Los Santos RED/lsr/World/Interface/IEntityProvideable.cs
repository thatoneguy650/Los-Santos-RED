using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IEntityProvideable
    {
        Pedestrians Pedestrians { get; }
        Vehicles Vehicles { get; }
        Places Places { get; }
        bool IsMPMapLoaded { get; }
        string DebugString { get; }
        bool IsZombieApocalypse { get; }
        int TotalWantedLevel { get; set; }
        void LoadMPMap();
        void LoadSPMap();
        void ClearSpawned();
        void AddBlip(Blip myBlip);
    }
}
