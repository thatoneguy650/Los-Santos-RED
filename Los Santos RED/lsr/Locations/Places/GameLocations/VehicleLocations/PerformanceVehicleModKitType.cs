using LosSantosRED.lsr.Interface;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PerformanceVehicleModKitType : VehicleModKitType
{
    public PerformanceVehicleModKitType(string name, int id, ModShopMenu modShopMenu, UIMenu interactionMenu, MenuPool menuPool, ILocationInteractable player) : base(name, id, modShopMenu, interactionMenu, menuPool, player)
    {

    }
    protected override string GetModItemName(int modKitValueID)
    {
        if(modKitValueID == -1)
        {
            return "Stock";
        }
        return $"Level {modKitValueID + 1}";
    }
}

