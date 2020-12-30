using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using RAGENativeUI.Elements;
using System;
using System.Drawing;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;

public class UI
{
    private IPlayer CurrentPlayer;
    private IWorld World;
    private ISearchMode SearchMode;
    private ISettings Settings;
    private IZoneJurisdictions ZoneJurisdictions;
    public UI(IWorld world, IPlayer currentPlayer, ISearchMode searchMode, ISettings settings, IZoneJurisdictions zoneJurisdictions)
    {
        World = world;
        CurrentPlayer = currentPlayer;
        SearchMode = searchMode;
        Settings = settings;
        ZoneJurisdictions = zoneJurisdictions;
        BigMessage = new BigMessageThread(true);
    }
    private BigMessageThread BigMessage;
    private uint GameTimeLastDisplayedBleedingHelp;
    private bool StartedBandagingEffect = false;
    private bool StartedBustedEffect = false;
    private bool StartedDeathEffect = false;
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
        if (Settings.SettingsManager.UI.Enabled && !CurrentPlayer.IsBusted && !CurrentPlayer.IsDead)
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
            NativeFunction.CallByName<bool>("SET_TEXT_WRAP", 0f, 1f);

        NativeFunction.CallByHash<uint>(0x25fbb336df1804cb, "STRING");
        NativeFunction.CallByHash<uint>(0x25FBB336DF1804CB, TextToShow);
        NativeFunction.CallByHash<uint>(0x6C188BE134E074AA, TextToShow);
        NativeFunction.CallByHash<uint>(0xCD015E5BB0D96A57, Y, X);
        return;
    }
    //private string GetPlayerStatusDisplay()
    //{
    //    string PlayerStatusLine = "";
    //    if (CurrentPlayer.IsPersonOfInterest)
    //    {
    //        if (CurrentPlayer.IsWanted)
    //        {
    //            PlayerStatusLine = "~r~Wanted~s~";
    //        }
    //        else if (CurrentPlayer.CurrentPoliceResponse.HasBeenNotWantedFor <= 45000)
    //        {
    //            PlayerStatusLine = "~o~Wanted~s~";
    //        }
    //        else
    //            PlayerStatusLine = "~y~Wanted~s~";
    //    }
    //    if (CurrentPlayer.IsWanted)
    //    {
    //        string AgenciesChasingPlayer = Mod.World.Instance.AgenciesChasingPlayer;
    //        if (AgenciesChasingPlayer != "")
    //        {
    //            PlayerStatusLine += " (" + AgenciesChasingPlayer + "~s~)";
    //        }
    //    }
    //    return PlayerStatusLine;
    //}
    private string GetStreetDisplay()
    {
        string StreetDisplay = "";
        if (CurrentPlayer.CurrentStreet != null && CurrentPlayer.CurrentCrossStreet != null)
        {
            StreetDisplay = string.Format(" {0} at {1} ", CurrentPlayer.CurrentStreet.Name, CurrentPlayer.CurrentCrossStreet.Name);
        }
        else if (CurrentPlayer.CurrentStreet != null)
        {
            StreetDisplay = string.Format(" {0} ", CurrentPlayer.CurrentStreet.Name);
        }
        return StreetDisplay;
    }
    private string GetVehicleStatusDisplay()
    {
        string PlayerSpeedLine = "";
        if (CurrentPlayer.CurrentVehicle != null && Game.LocalPlayer.Character.IsInAnyVehicle(false))//was game.localpalyer.character.isinanyvehicle(false)
        {
            float VehicleSpeedMPH = CurrentPlayer.CurrentVehicle.Vehicle.Speed * 2.23694f;
            if (!Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn)
            {
                PlayerSpeedLine = "ENGINE OFF";
            }
            else
            {
                string ColorPrefx = "~s~";
                if (CurrentPlayer.IsSpeeding)
                {
                    ColorPrefx = "~r~";
                }

                if (CurrentPlayer.CurrentStreet != null)
                {
                    PlayerSpeedLine = string.Format("{0}{1} ~s~MPH ({2})", ColorPrefx, Math.Round(VehicleSpeedMPH, MidpointRounding.AwayFromZero), CurrentPlayer.CurrentStreet.SpeedLimit);
                }
            }

            if (CurrentPlayer.IsViolatingAnyTrafficLaws)
            {
                PlayerSpeedLine += " !";
            }

            PlayerSpeedLine += "~n~" + CurrentPlayer.CurrentVehicle.FuelTank.UIText;
        }
        return PlayerSpeedLine;
    }
    private string GetZoneDisplay()
    {
        if (CurrentPlayer.CurrentZone == null)
        {
            return "";
        }
        string ZoneDisplay = "";
        string CopZoneName = "";
        ZoneDisplay = GetName(CurrentPlayer.CurrentZone, true);
        if (CurrentPlayer.CurrentZone != null)
        {
            Agency MainZoneAgency = ZoneJurisdictions.GetMainAgency(CurrentPlayer.CurrentZone.InternalGameName);
            if (MainZoneAgency != null)
            {
                CopZoneName = MainZoneAgency.ColoredInitials;
            }
        }
        ZoneDisplay = ZoneDisplay + " ~s~- " + CopZoneName;
        return ZoneDisplay;
    }
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

            return MyZone.DisplayName + ", " + CountyName;
        }
        else
        {
            return MyZone.DisplayName;
        }

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
        if (CurrentPlayer.IsDead)
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
        else if (CurrentPlayer.IsBusted)
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
        //else if (Mod.World.Instance.Wounds.IsPlayerBleeding)
        //{
        //    if (!StartedBandagingEffect)
        //    {
        //        NativeFunction.Natives.x80C8B1846639BB19(1);
        //        NativeFunction.Natives.x2206BF9A37B7F724("DrugsDrivingIn", 0, false);
        //        BigMessage.MessageInstance.ShowColoredShard("BLEEDING", "", HudColor.HUD_COLOUR_BLACK, HudColor.HUD_COLOUR_REDDARK, 1500);
        //        StartedBandagingEffect = true;
        //    }
        //}
        else
        {
            if (StartedBandagingEffect)
            {
                NativeFunction.Natives.x2206BF9A37B7F724("DrugsDrivingOut", 0, false);
            }
            StartedBustedEffect = false;
            StartedBandagingEffect = false;
            StartedDeathEffect = false;
            NativeFunction.Natives.xB4EDDC19532BFB85();
            NativeFunction.Natives.x80C8B1846639BB19(0);
        }
    }
    private void ShowDebugUI()
    {
        //int Lines = 0;
        //foreach(string LogMessage in Debugging.LogMessages)
        //{
        //    DisplayTextOnScreen(LogMessage, Lines * 0.02f, 0.0f, 0.23f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        //    Lines++;
        //}

        // Lines++;

        string DebugLine = string.Format("InvestMode {0} HaveDesc {1}, IsStationary {2}, IsSuspicious {3}", CurrentPlayer.Investigations.IsActive, CurrentPlayer.Investigations.HaveDescription, CurrentPlayer.IsStationary, CurrentPlayer.Investigations.IsSuspicious);
        DisplayTextOnScreen(DebugLine, 0.01f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);

        string DebugLine1 = string.Format($"{CurrentPlayer.DrunkDebug}");
        DisplayTextOnScreen(DebugLine1, 0.02f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);

        string DebugLine2 = string.Format("{0}", SearchMode.SearchModeDebug);
        DisplayTextOnScreen(DebugLine2, 0.03f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);

        string DebugLine3 = string.Format("","");
        DisplayTextOnScreen(DebugLine3, 0.04f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);

        string DebugLine4 = string.Format("CrimesObs {0}", CurrentPlayer.CurrentPoliceResponse.CrimesObservedJoined);
        DisplayTextOnScreen(DebugLine4, 0.05f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);

        string DebugLine5 = string.Format("CrimesRep {0}", CurrentPlayer.CurrentPoliceResponse.CrimesReportedJoined);
        DisplayTextOnScreen(DebugLine5, 0.06f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);

        string DebugLine6 = string.Format("{0}", CurrentPlayer.LawsViolatingDisplay);
        DisplayTextOnScreen(DebugLine6, 0.07f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);

        //float Between = 0.01f;
        //float Start = 0.15f;
        //foreach (string Line in Mod.PedDamageManager.AllPedDamageList)
        //{
        //    DisplayTextOnScreen(Line, Start, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        //    Start = Start + Between;
        //}
    }
    private void ShowUI()
    {
        ShowDebugUI();
        HideVanillaUI();

        //DisplayTextOnScreen(GetPlayerStatusDisplay(), DataMart.Instance.Settings.SettingsManager.UI.PlayerStatusPositionX, DataMart.Instance.Settings.SettingsManager.UI.PlayerStatusPositionY, DataMart.Instance.Settings.SettingsManager.UI.PlayerStatusScale, Color.White, GTAFont.FontChaletComprimeCologne, (GTATextJustification)DataMart.Instance.Settings.SettingsManager.UI.PlayerStatusJustificationID);
        DisplayTextOnScreen(GetVehicleStatusDisplay(), Settings.SettingsManager.UI.VehicleStatusPositionX, Settings.SettingsManager.UI.VehicleStatusPositionY, Settings.SettingsManager.UI.VehicleStatusScale, Color.White, GTAFont.FontChaletComprimeCologne, (GTATextJustification)Settings.SettingsManager.UI.VehicleStatusJustificationID);
        DisplayTextOnScreen(GetZoneDisplay(), Settings.SettingsManager.UI.ZonePositionX, Settings.SettingsManager.UI.ZonePositionY, Settings.SettingsManager.UI.ZoneScale, Color.White, GTAFont.FontHouseScript, (GTATextJustification)Settings.SettingsManager.UI.ZoneJustificationID);
        DisplayTextOnScreen(GetStreetDisplay(), Settings.SettingsManager.UI.StreetPositionX, Settings.SettingsManager.UI.StreetPositionY, Settings.SettingsManager.UI.StreetScale, Color.White, GTAFont.FontHouseScript, (GTATextJustification)Settings.SettingsManager.UI.StreetJustificationID);
    }
}