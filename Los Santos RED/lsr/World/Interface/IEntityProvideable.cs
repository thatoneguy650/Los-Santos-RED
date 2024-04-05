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
        Vector3 PoliceBackupPoint { get; set; }
        int CitizenWantedLevel { get; set; }
        bool AnyFiresNearPlayer { get; }
        List<SpawnError> SpawnErrors { get; }
        ModDataFileManager ModDataFileManager { get; }
        ILocationInteractable LocationInteractable { get; }
        bool IsFEJInstalled { get; }

        void LoadMPMap();
        void LoadSPMap();
        void ClearSpawned(bool includeCivilians);
        void AddBlip(Blip myBlip);
        void RemoveBlips();
    }
}
