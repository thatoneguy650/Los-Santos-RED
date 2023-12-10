using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IRestableLocation
{
    void CreateRestMenu(bool removeBanner);
    LocationCamera LocationCamera { get; }
    GameLocation GameLocation { get; }
}

