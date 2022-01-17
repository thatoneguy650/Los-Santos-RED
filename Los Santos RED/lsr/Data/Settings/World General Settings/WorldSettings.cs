using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WorldSettings : ISettingsDefaultable
{
    public bool AddPOIBlipsToMap { get; set; }
    public bool UpdateVehiclePlates { get; set; }
    public bool CleanupVehicles { get; set; }
    public bool ReplaceVanillaShopKeepers { get; set; }
    public WorldSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        AddPOIBlipsToMap = true;
        UpdateVehiclePlates = true;
        CleanupVehicles = true;
        ReplaceVanillaShopKeepers = true;
    }

}