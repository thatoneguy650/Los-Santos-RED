using LosSantosRED.lsr.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ILocationInteractable : IActivityPerformable,IActionable
    {
        new void GiveMoney(int Money);
        new Inventory Inventory { get; }
        new bool CanPerformActivities { get; }
    }
}
