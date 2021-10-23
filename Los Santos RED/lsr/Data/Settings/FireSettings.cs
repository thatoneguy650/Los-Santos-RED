using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FireSettings
{
    public bool ManageDispatching { get; set; } = true;
    public bool ManageTasking { get; set; } = true;
    public bool ShowSpawnedBlips { get; set; } = true;
    public FireSettings()
    {

    }
}