using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CombatFloatToSet
{
    public CombatFloatToSet()
    {
    }

    public CombatFloatToSet(int combatAttribute, float value)
    {
        CombatAttribute = combatAttribute;
        Value = value;
    }


    public float Value { get; set; }
    public int CombatAttribute { get; set; }
    public void ApplyToPed(Ped ped)
    {
        if (!ped.Exists())
        {
            return;
        }
        NativeFunction.Natives.SET_COMBAT_FLOAT(ped, CombatAttribute, Value);
    }
}

