using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ISpawner
    {
        void SpawnCop(Agency agencyToSpawn, Vector3 finalSpawnPosition, float heading, VehicleInformation agencyVehicle, int wantedLevel);
        void DeleteCop(Cop toDelete);
    }
}
