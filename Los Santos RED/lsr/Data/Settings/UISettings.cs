using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UISettings
{
    public bool UIEnabled { get; set; } = true;
    public float VehicleStatusPositionX { get; set; } = 0.7f;
    public float VehicleStatusPositionY { get; set; } = 0.98f;
    public float VehicleStatusScale { get; set; } = 0.4f;
    public int VehicleStatusJustificationID { get; set; } = 2;


    public float PlayerStatusPositionX { get; set; } = 0.6f;
    public float PlayerStatusPositionY { get; set; } = 0.98f;
    public float PlayerStatusScale { get; set; } = 0.4f;
    public int PlayerStatusJustificationID { get; set; } = 2;


    public float StreetPositionX { get; set; } = 0.87f;
    public float StreetPositionY { get; set; } = 0.98f;
    public float StreetScale { get; set; } = 0.5f;
    public int StreetJustificationID { get; set; } = 2;
    public float ZonePositionX { get; set; } = 0.90f;
    public float ZonePositionY { get; set; } = 0.98f;
    public float ZoneScale { get; set; } = 0.5f;
    public int ZoneJustificationID { get; set; } = 2;
    public bool AlwaysShowCash { get; set; } = true;
    public bool AlwaysShowRadar { get; set; } = true;
    public bool AlwaysShowHUD { get; set; } = true;

    #if DEBUG
        public bool ShowDebug { get; set; } = true;
    #else
        public bool ShowDebug { get; set; } = false;
    #endif
    public bool ShowVanillaVehicleUI { get; set; } = false;
    public bool ShowVanillaAreaUI { get; set; } = false;
    public bool ShowSpeedDisplay { get; set; } = true;
    public bool ShowZoneDisplay { get; set; } = true;
    public bool ShowStreetDisplay { get; set; } = true;
    public bool ShowPlayerDisplay { get; set; } = true;
    public bool SetBustedEffect { get; set; } = true;
    public bool PlayWastedSounds { get; set; } = true;
    public bool DisplayBustedMessage { get; set; } = true;
    public bool SetDeathEffect { get; set; } = true;
    public bool DisplayWastedMessage { get; set; } = true;
    public string WastedMessageText { get; set; } = "WASTED";
    public string BustedMessageText { get; set; } = "BUSTED";
    public bool AllowScreenEffectReset { get; set; } = true;
    public string SpeedDisplayUnits { get; set; } = "MPH";
    public bool DisplayButtonPrompts { get; set; } = true;

    public UISettings()
    {

    }
}