using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Flags]
public enum SelectorOptions
{
    Safe = 1,
    SemiAuto = 2,
    TwoRoundBurst = 4,
    ThreeRoundBurst = 8,
    FourRoundBurst = 16,
    FiveRoundBurst = 32,
    FullAuto = 64,
}