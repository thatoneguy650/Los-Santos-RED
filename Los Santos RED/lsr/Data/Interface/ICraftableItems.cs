using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface ICraftableItems
    {
        List<CraftableItem> Items { get; }

        CraftableItem Get(string name);
        void SerializeAllSettings();
    }
}
