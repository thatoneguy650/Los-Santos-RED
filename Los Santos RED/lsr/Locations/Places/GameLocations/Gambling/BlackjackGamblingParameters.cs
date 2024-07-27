using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BlackjackGamblingParameters
{
    public BlackjackGamblingParameters()
    {

    }
    public BlackjackGamblingParameters(bool isEnabled, int MinBet, int MaxBet, bool CanSurrender, bool CanSplit)
    {
        IsEnabled = isEnabled;
        this.MinBet = MinBet;
        this.MaxBet = MaxBet;
        this.CanSurrender = CanSurrender;
        this.CanSplit = CanSplit;
    }
    public bool IsEnabled { get; set; } = true;
    public int MinBet { get; set; } = 5;
    public int MaxBet { get; set; } = 250;
    public bool CanSurrender { get; set; } = true;
    public bool CanSplit { get; set; } = true;
}

