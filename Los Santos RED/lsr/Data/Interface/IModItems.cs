using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IModItems
    {
        List<ModItem> Items { get; }

        ModItem Get(string text);
        ModItem GetRandomItem();
    }
}
