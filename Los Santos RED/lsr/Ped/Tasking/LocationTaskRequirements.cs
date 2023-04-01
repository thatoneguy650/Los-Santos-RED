using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class LocationTaskRequirements
{
    public LocationTaskRequirements()
    {

    }

    public TaskRequirements TaskRequirements { get; set; } = TaskRequirements.None;
    public List<string> ForcedScenarios { get; set; } = new List<string>();
}
