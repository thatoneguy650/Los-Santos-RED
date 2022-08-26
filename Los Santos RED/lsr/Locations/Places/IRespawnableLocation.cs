using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IRespawnableLocation
{
    string Name { get; }
    Vector3 EntrancePosition { get; }
    float EntranceHeading { get; }
    string FullStreetAddress { get; }
    bool IsEnabled { get; }
}

