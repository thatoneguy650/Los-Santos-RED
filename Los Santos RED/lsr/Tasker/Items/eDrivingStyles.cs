using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum eDrivingStyles
{

    StopBeforeVehicles = 1,
    StopBeforePeds = 2,
    AvoidVehicles = 4,
    AvoidEmptyVehicles = 8,
    AvoidPeds = 16,
    AvoidObject = 32,
    Unknown1 = 64,
    StopAtTrafficLights = 128,
    UseBlinkers = 256,
    AllowWrongWay = 512,
    GoInReverse = 1024,
    Unknown2 = 2048,
    Unknown3 = 4096,
    Unknown4 = 8192,
    Unknown5 = 16384,
    Unknwon6 = 32768,
    Unknown7 = 65536,
    Unknown8 = 131072,
    ShortestPath = 262144,
    AvoidOffroad = 524288,
    Unknown9 = 1048576,
    Unknown10 = 2097152,
    IgnoreRoads = 4194304,// (Uses local pathing, only works within 200~ meters around the player)
    Unknown11 = 8388608,
    IgnoreAllPathing = 16777216,//(Goes straight to destination)
    Unknown12 = 33554432,
    Unknown13 = 67108864,
    Unknwon14 = 134217728,
    Unknown15 = 268435456,
    AvoidHighways = 536870912,//(will use the highway if there is no other way to get to the destination)
    Unknown16 = 1073741824,
}

