using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
public class DispatchableVehicleMod
{
    public DispatchableVehicleMod()
    {
    }

    public DispatchableVehicleMod(int modID)
    {
        ModID = modID;
        Percentage = 100;
    }

    public DispatchableVehicleMod(int modID, int percentage)
    {
        ModID = modID;
        Percentage = percentage;
    }

    public int ModID { get; set; }
    public int Percentage { get; set; }
    public List<DispatchableVehicleModValue> DispatchableVehicleModValues { get; set; }
    public DispatchableVehicleModValue PickValue()
    {
        if (DispatchableVehicleModValues == null || !DispatchableVehicleModValues.Any())
        {
            return null;
        }
        int Total = DispatchableVehicleModValues.Sum(x => x.Percentage);
        int RandomPick = RandomItems.MyRand.Next(0, Total);
        foreach (DispatchableVehicleModValue modValue in DispatchableVehicleModValues)
        {
            int SpawnChance = modValue.Percentage;
            if (RandomPick < SpawnChance)
            {
                return modValue;
            }
            RandomPick -= SpawnChance;
        }
        if (DispatchableVehicleModValues.Any())
        {
            return DispatchableVehicleModValues.PickRandom();
        }
        return null;

    }

}

