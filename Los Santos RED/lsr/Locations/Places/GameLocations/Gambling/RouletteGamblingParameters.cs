using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RouletteGamblingParameters
{
    public RouletteGamblingParameters()
    {

    }
    public RouletteGamblingParameters(bool isEnabled)
    {
        IsEnabled = isEnabled;
    }
    public bool IsEnabled { get; set; } = false;
}

