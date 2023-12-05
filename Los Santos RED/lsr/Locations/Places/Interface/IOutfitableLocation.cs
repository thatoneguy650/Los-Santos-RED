using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IOutfitableLocation
{
    void CreateOutfitMenu(bool removeBanner, bool isInside);
    LocationCamera LocationCamera { get; }
    GameLocation GameLocation { get; }
}
