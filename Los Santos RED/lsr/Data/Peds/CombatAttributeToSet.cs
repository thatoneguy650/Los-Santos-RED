using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class CombatAttributeToSet
{
    public CombatAttributeToSet()
    {
    }

    public CombatAttributeToSet(int combatAttribute, bool enabled)
    {
        CombatAttribute = combatAttribute;
        Enabled = enabled;
    }


    public bool Enabled { get; set; }
    public int CombatAttribute { get; set; }
    public void ApplyToPed(Ped ped)
    {
        if (!ped.Exists())
        {
            return;
        }
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(ped, CombatAttribute, Enabled);
    }
}

