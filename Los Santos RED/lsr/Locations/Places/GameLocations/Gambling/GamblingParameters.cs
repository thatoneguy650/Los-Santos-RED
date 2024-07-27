using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GamblingParameters
{
    public string DealerName { get; set; } = "Dealer";
    public BlackjackGamblingParameters BlackjackGamblingParameters { get; set; } = new BlackjackGamblingParameters();
    public RouletteGamblingParameters RouletteGamblingParameters { get; set; } = new RouletteGamblingParameters();
    public GamblingParameters()
    {

    }
}

