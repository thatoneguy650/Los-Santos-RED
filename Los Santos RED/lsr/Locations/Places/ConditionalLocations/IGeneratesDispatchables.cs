using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IGeneratesDispatchables
{
    DispatchablePerson GetRandomPed(int v, string requiredGroup);
    DispatchableVehicle GetRandomVehicle(int v1, bool v2, bool v3, bool v4, string requiredGroup, ISettingsProvideable settings);
}
