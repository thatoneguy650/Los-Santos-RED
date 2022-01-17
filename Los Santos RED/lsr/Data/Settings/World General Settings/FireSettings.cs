using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FireSettings : ISettingsDefaultable
{
    public bool ManageDispatching { get; set; }
    public bool ManageTasking { get; set; }
    public bool ShowSpawnedBlips { get; set; }
    public FireSettings()
    {
        SetDefault();
        #if DEBUG
           // ShowSpawnedBlips = true;
            ManageDispatching = false;
            ManageTasking = false;
        #else
          ShowSpawnedBlips = false;
        #endif
    }
    public void SetDefault()
    {
        ManageDispatching = false;
        ManageTasking = false;
        ShowSpawnedBlips = false;
    }
}