using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class MoneyEntitySet
{
    public MoneyEntitySet()
    {
    }

    public MoneyEntitySet(string entitySetName, int moneyMin, int moneyMax)
    {
        EntitySetName = entitySetName;
        MoneyMin = moneyMin;
        MoneyMax = moneyMax;
    }
    public string EntitySetName { get; set; }
    public int MoneyMin { get; set; }
    public int MoneyMax { get; set; }
    public void SetStatus(int finalMoneyInteriorID, int cash)
    {
        bool isActivated = false;
        if(cash <= MoneyMax && cash >= MoneyMin)
        {
            NativeFunction.Natives.ACTIVATE_INTERIOR_ENTITY_SET(finalMoneyInteriorID, EntitySetName);
            isActivated = true;
        }
        else
        {
            NativeFunction.Natives.DEACTIVATE_INTERIOR_ENTITY_SET(finalMoneyInteriorID, EntitySetName);
        }
        EntryPoint.WriteToConsole($"isActivated {isActivated} cash{cash} MoneyMin{MoneyMin} MoneyMax{MoneyMax}");
    }


}

