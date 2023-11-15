using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IGasPumpable
{
    int PricePerGallon { get; }
    Vector3 EntrancePosition { get; }
    string Name { get; }
    string Description { get; }
    string BannerImagePath { get; }
}

