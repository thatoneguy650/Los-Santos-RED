using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI.Elements;
using System;
using System.Drawing;

public class UI
{
    private BigMessageThread BigMessage;
    private uint GameTimeLastDisplayedBleedingHelp;
    private IDisplayable Player;
    private ISettingsProvideable Settings;
    private bool StartedBandagingEffect = false;
    private bool StartedBustedEffect = false;
    private bool StartedDeathEffect = false;
    private IZoneJurisdictions ZoneJurisdictions;
    public UI(IDisplayable currentPlayer, ISettingsProvideable settings, IZoneJurisdictions zoneJurisdictions)
    {
        Player = currentPlayer;
        Settings = settings;
        ZoneJurisdictions = zoneJurisdictions;
        BigMessage = new BigMessageThread(true);
    }
    private enum GTAFont
    {
        FontChaletLondon = 0,
        FontHouseScript = 1,
        FontMonospace = 2,
        FontWingDings = 3,
        FontChaletComprimeCologne = 4,
        FontPricedown = 7
    };
    private enum GTAHudComponent
    {
        HUD = 0,
        HUD_WANTED_STARS = 1,
        HUD_WEAPON_ICON = 2,
        HUD_CASH = 3,
        HUD_MP_CASH = 4,
        HUD_MP_MESSAGE = 5,
        HUD_VEHICLE_NAME = 6,
        HUD_AREA_NAME = 7,
        HUD_VEHICLE_CLASS = 8,
        HUD_STREET_NAME = 9,
        HUD_HELP_TEXT = 10,
        HUD_FLOATING_HELP_TEXT_1 = 11,
        HUD_FLOATING_HELP_TEXT_2 = 12,
        HUD_CASH_CHANGE = 13,
        HUD_RETICLE = 14,
        HUD_SUBTITLE_TEXT = 15,
        HUD_RADIO_STATIONS = 16,
        HUD_SAVING_GAME = 17,
        HUD_GAME_STREAM = 18,
        HUD_WEAPON_WHEEL = 19,
        HUD_WEAPON_WHEEL_STATS = 20,
        MAX_HUD_COMPONENTS = 21,
        MAX_HUD_WEAPONS = 22,
        MAX_SCRIPTED_HUD_COMPONENTS = 141,
    }
    private enum GTATextJustification
    {
        Center = 0,
        Left = 1,
        Right = 2,
    };
    public string GetName(Zone MyZone, bool WithCounty)
    {
        if (WithCounty)
        {
            string CountyName = "San Andreas";
            if (MyZone.ZoneCounty == County.BlaineCounty)
                CountyName = "Blaine County";
            else if (MyZone.ZoneCounty == County.CityOfLosSantos)
                CountyName = "City of Los Santos";
            else if (MyZone.ZoneCounty == County.LosSantosCounty)
                CountyName = "Los Santos County";
            else if (MyZone.ZoneCounty == County.Crook)
                CountyName = "Crook County";
            else if (MyZone.ZoneCounty == County.NorthYankton)
                CountyName = "North Yankton";
            else if (MyZone.ZoneCounty == County.Vice)
                CountyName = "Vice County";

            return MyZone.DisplayName + ", " + CountyName;
        }
        else
        {
            return MyZone.DisplayName;
        }
    }
    public void Update()
    {
        if (Settings.SettingsManager.General.AlwaysShowHUD)
        {
            NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);
        }
        if (Settings.SettingsManager.General.AlwaysShowRadar)
        {
            NativeFunction.CallByName<bool>("DISPLAY_RADAR", true);
        }
        if (Settings.SettingsManager.Police.ShowPoliceRadarBlips)
        {
            NativeFunction.CallByName<bool>("SET_POLICE_RADAR_BLIPS", true);
        }
        else
        {
            NativeFunction.CallByName<bool>("SET_POLICE_RADAR_BLIPS", false);
        }
        if (Settings.SettingsManager.General.AlwaysShowCash)
        {
            NativeFunction.CallByName<bool>("DISPLAY_CASH", true);
        }
        if (Settings.SettingsManager.UI.Enabled && Player.IsAliveAndFree)
        {
            ShowUI();
        }
        ScreenEffectsUpdate();
    }
    private void DisplayTextOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification)
    {
        NativeFunction.CallByName<bool>("SET_TEXT_FONT", (int)Font);
        NativeFunction.CallByName<bool>("SET_TEXT_SCALE", Scale, Scale);
        NativeFunction.CallByName<uint>("SET_TEXT_COLOUR", (int)TextColor.R, (int)TextColor.G, (int)TextColor.B, 255);

        NativeFunction.Natives.SetTextJustification((int)Justification);
        NativeFunction.Natives.SetTextDropshadow(2, 2, 0, 0, 0);

        if (Justification == GTATextJustification.Right)
        {
            NativeFunction.CallByName<bool>("SET_TEXT_WRAP", 0f, Y);
        }
        else
        {
            NativeFunction.CallByName<bool>("SET_TEXT_WRAP", 0f, 1f);
        }
        NativeFunction.CallByHash<uint>(0x25fbb336df1804cb, "STRING");
        NativeFunction.CallByHash<uint>(0x25FBB336DF1804CB, TextToShow);
        NativeFunction.CallByHash<uint>(0x6C188BE134E074AA, TextToShow);
        NativeFunction.CallByHash<uint>(0xCD015E5BB0D96A57, Y, X);
        return;
    }
    private string GetStreetDisplay()
    {
        string StreetDisplay = "";
        if (Player.CurrentStreet != null && Player.CurrentCrossStreet != null)
        {
            StreetDisplay = string.Format(" {0} at {1} ", Player.CurrentStreet.Name, Player.CurrentCrossStreet.Name);
        }
        else if (Player.CurrentStreet != null)
        {
            StreetDisplay = string.Format(" {0} ", Player.CurrentStreet.Name);
        }
        return StreetDisplay;
    }
    private string GetVehicleStatusDisplay()
    {
        string PlayerSpeedLine = "";
        if (Player.CurrentVehicle != null && Game.LocalPlayer.Character.IsInAnyVehicle(false))//was game.localpalyer.character.isinanyvehicle(false)
        {
            float VehicleSpeedMPH = Player.CurrentVehicle.Vehicle.Speed * 2.23694f;
            if (!Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn)
            {
                PlayerSpeedLine = "ENGINE OFF";
            }
            else
            {
                string ColorPrefx = "~s~";
                if (Player.IsSpeeding)
                {
                    ColorPrefx = "~r~";
                }
                if (Player.CurrentStreet != null)
                {
                    PlayerSpeedLine = string.Format("{0}{1} ~s~MPH ({2})", ColorPrefx, Math.Round(VehicleSpeedMPH, MidpointRounding.AwayFromZero), Player.CurrentStreet.SpeedLimit);
                }
            }
            if (Player.IsViolatingAnyTrafficLaws)
            {
                PlayerSpeedLine += " !";
            }
            PlayerSpeedLine += "~n~" + Player.CurrentVehicle.FuelTank.UIText;
        }
        return PlayerSpeedLine;
    }
    private string GetZoneDisplay()
    {
        if (Player.CurrentZone == null)
        {
            return "";
        }
        string CopZoneName = "";
        string ZoneDisplay = GetName(Player.CurrentZone, true);
        if (Player.CurrentZone != null)
        {
            Agency MainZoneAgency = ZoneJurisdictions.GetMainAgency(Player.CurrentZone.InternalGameName);
            if (MainZoneAgency != null)
            {
                CopZoneName = MainZoneAgency.ColoredInitials;
            }
        }
        ZoneDisplay = ZoneDisplay + " ~s~- " + CopZoneName;
        return ZoneDisplay;
    }
    private void HideVanillaUI()
    {
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_VEHICLE_NAME);
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_AREA_NAME);
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_VEHICLE_CLASS);
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_STREET_NAME);
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_VEHICLE_CLASS);
    }
    private void ScreenEffectsUpdate()
    {
        if (Player.IsDead)
        {
            if (!StartedDeathEffect)
            {
                NativeFunction.Natives.x80C8B1846639BB19(1);
                NativeFunction.Natives.x2206BF9A37B7F724("DeathFailMPIn", 0, 0);
                NativeFunction.CallByName<bool>("PLAY_SOUND_FRONTEND", -1, "Bed", "WastedSounds", true);
                BigMessage.MessageInstance.ShowColoredShard("WASTED", "", HudColor.HUD_COLOUR_BLACK, HudColor.HUD_COLOUR_REDDARK, 2000);
                StartedDeathEffect = true;
            }
        }
        else if (Player.IsBusted)
        {
            if (!StartedBustedEffect)
            {
                NativeFunction.Natives.x80C8B1846639BB19(1);
                NativeFunction.Natives.x2206BF9A37B7F724("DeathFailMPDark", 0, 0);
                NativeFunction.CallByName<bool>("PLAY_SOUND_FRONTEND", -1, "TextHit", "WastedSounds", true);
                BigMessage.MessageInstance.ShowColoredShard("BUSTED", "", HudColor.HUD_COLOUR_BLACK, HudColor.HUD_COLOUR_BLUE, 2000);
                StartedBustedEffect = true;
            }
        }
        else
        {
            StartedBustedEffect = false;
            StartedDeathEffect = false;
            NativeFunction.Natives.xB4EDDC19532BFB85();
            NativeFunction.Natives.x80C8B1846639BB19(0);
        }
    }
    private void ShowDebugUI()
    {
        float StartingPoint = 0.1f;
        DisplayTextOnScreen($"{Player.DebugString_State}", StartingPoint + 0.01f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{Player.DebugString_Drunk}", StartingPoint + 0.02f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{Player.DebugString_SearchMode}", StartingPoint + 0.03f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{Player.DebugString_ModelInfo}", StartingPoint + 0.04f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"CrimesObs {Player.DebugString_ObservedCrimes}", StartingPoint + 0.05f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"CrimesRep {Player.DebugString_ReportedCrimes}", StartingPoint + 0.06f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{Player.DebugString_LawsViolating}", StartingPoint + 0.07f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
    }
    private void ShowUI()
    {
        ShowDebugUI();
        HideVanillaUI();
        DisplayTextOnScreen(GetVehicleStatusDisplay(), Settings.SettingsManager.UI.VehicleStatusPositionX, Settings.SettingsManager.UI.VehicleStatusPositionY, Settings.SettingsManager.UI.VehicleStatusScale, Color.White, GTAFont.FontChaletComprimeCologne, (GTATextJustification)Settings.SettingsManager.UI.VehicleStatusJustificationID);
        DisplayTextOnScreen(GetZoneDisplay(), Settings.SettingsManager.UI.ZonePositionX, Settings.SettingsManager.UI.ZonePositionY, Settings.SettingsManager.UI.ZoneScale, Color.White, GTAFont.FontHouseScript, (GTATextJustification)Settings.SettingsManager.UI.ZoneJustificationID);
        DisplayTextOnScreen(GetStreetDisplay(), Settings.SettingsManager.UI.StreetPositionX, Settings.SettingsManager.UI.StreetPositionY, Settings.SettingsManager.UI.StreetScale, Color.White, GTAFont.FontHouseScript, (GTATextJustification)Settings.SettingsManager.UI.StreetJustificationID);
    }
}