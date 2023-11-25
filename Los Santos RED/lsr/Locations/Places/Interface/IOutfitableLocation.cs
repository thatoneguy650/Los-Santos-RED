using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IOutfitableLocation
{
    void CreateOutfitMenu();
    LocationCamera LocationCamera { get; }
    GameLocation GameLocation { get; }
}
