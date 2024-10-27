using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class SpawnAdjustmentAmount
{
    public SpawnAdjustmentAmount()
    {

    }

    public SpawnAdjustmentAmount(eSpawnAdjustment eSpawnAdjustment, int percentAmount)
    {
        this.eSpawnAdjustment = eSpawnAdjustment;
        PercentAmount = percentAmount;
    }

    public eSpawnAdjustment eSpawnAdjustment { get; set; }
    public int PercentAmount { get; set; }

}

