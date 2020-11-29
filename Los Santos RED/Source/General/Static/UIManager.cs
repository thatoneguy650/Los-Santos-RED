using ExtensionsMethods;
using Rage;
using Rage.Native;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class UIManager
{
    private static BigMessageThread BigMessage;
    private static bool StartedBandagingEffect = false;
    private static bool StartedBustedEffect = false;
    private static bool StartedDeathEffect = false;
    private static uint GameTimeLastDisplayedBleedingHelp;

    public static string DebugLine { get; set; }
    private enum GTAFont
    {
        FontChaletLondon = 0,
        FontHouseScript = 1,
        FontMonospace = 2,
        FontWingDings = 3,
        FontChaletComprimeCologne = 4,
        FontPricedown = 7
    };
    private enum GTATextJustification
    {
        Center = 0,
        Left = 1,
        Right = 2,
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
    public static bool IsRunning { get; set; }
    public static bool RecentlyDisplayedBleedingHelp
    {
        get
        {
            if (GameTimeLastDisplayedBleedingHelp == 0)
                return false;
            else if (Game.GameTime - GameTimeLastDisplayedBleedingHelp <= 25000)
                return true;
            else
                return false;
        }
    }

    public static void Initialize()
    {
        IsRunning= true;
        BigMessage = new BigMessageThread(true);
       // MainLoop();
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        //try
        //{
            if (SettingsManager.MySettings.General.AlwaysShowHUD)
                NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);

            if (SettingsManager.MySettings.General.AlwaysShowRadar)
                NativeFunction.CallByName<bool>("DISPLAY_RADAR", true);

            if (SettingsManager.MySettings.Police.ShowPoliceRadarBlips)
                NativeFunction.CallByName<bool>("SET_POLICE_RADAR_BLIPS", true);
            else
                NativeFunction.CallByName<bool>("SET_POLICE_RADAR_BLIPS", false);

            if (SettingsManager.MySettings.General.AlwaysShowCash)
                NativeFunction.CallByName<bool>("DISPLAY_CASH", true);


            if (SettingsManager.MySettings.UI.Enabled && !PlayerStateManager.IsBusted && !PlayerStateManager.IsDead)
            {
                ShowUI();
            }

            ScreenEffectsTick();
        //}
        //catch (Exception e)
        //{
        //    //ScriptController.Dispose();
        //    Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
        //}
    }
    private static void ShowUI()
    {
        ShowDebugUI();


        HideVanillaUI();
        DisplayTextOnScreen(GetPlayerStatusDisplay(), SettingsManager.MySettings.UI.PlayerStatusPositionX, SettingsManager.MySettings.UI.PlayerStatusPositionY, SettingsManager.MySettings.UI.PlayerStatusScale, Color.White, GTAFont.FontChaletComprimeCologne, (GTATextJustification)SettingsManager.MySettings.UI.PlayerStatusJustificationID);
        DisplayTextOnScreen(GetVehicleStatusDisplay(), SettingsManager.MySettings.UI.VehicleStatusPositionX, SettingsManager.MySettings.UI.VehicleStatusPositionY, SettingsManager.MySettings.UI.VehicleStatusScale, Color.White, GTAFont.FontChaletComprimeCologne, (GTATextJustification)SettingsManager.MySettings.UI.VehicleStatusJustificationID);
        DisplayTextOnScreen(GetZoneDisplay(), SettingsManager.MySettings.UI.ZonePositionX, SettingsManager.MySettings.UI.ZonePositionY, SettingsManager.MySettings.UI.ZoneScale, Color.White, GTAFont.FontHouseScript, (GTATextJustification)SettingsManager.MySettings.UI.ZoneJustificationID);
        DisplayTextOnScreen(GetStreetDisplay(), SettingsManager.MySettings.UI.StreetPositionX, SettingsManager.MySettings.UI.StreetPositionY, SettingsManager.MySettings.UI.StreetScale, Color.White, GTAFont.FontHouseScript, (GTATextJustification)SettingsManager.MySettings.UI.StreetJustificationID);


        DisplayHelpText();

    }
    private static void DisplayHelpText()
    {
        if (!RecentlyDisplayedBleedingHelp && PedDamageManager.IsPlayerBleeding)
        {
            Game.DisplayHelp("Hold still to bandage!", 5000);
            GameTimeLastDisplayedBleedingHelp = Game.GameTime;
        }
    }
    private static void ShowDebugUI()
    {
        //int Lines = 0;
        //foreach(string LogMessage in Debugging.LogMessages)
        //{
        //    DisplayTextOnScreen(LogMessage, Lines * 0.02f, 0.0f, 0.23f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        //    Lines++;
        //}

        // Lines++;

        DebugLine = string.Format("InvestMode {0} HaveDesc {1}, IsStationary {2}, IsSuspicious {3}", InvestigationManager.InInvestigationMode, InvestigationManager.HavePlayerDescription,PlayerStateManager.IsStationary, InvestigationManager.IsSuspicious);
        DisplayTextOnScreen(DebugLine, 0.01f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);


        string DebugLine2 = string.Format("IsInSearchMode {0} IsInActiveMode {1}, TimeInSearchMode {2}, TimeInActiveMode {3}", SearchModeManager.IsInSearchMode,SearchModeManager.IsInActiveMode,SearchModeManager.TimeInSearchMode,SearchModeManager.TimeInActiveMode);

        DisplayTextOnScreen(DebugLine2, 0.03f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        string DebugLine3 = string.Format("AnyRcntlySeen {0}, AreStarsGreyedOut {1}, SrchTm {2}, LastSeen {3}", PolicePedManager.AnyRecentlySeenPlayer, PlayerStateManager.AreStarsGreyedOut, SearchModeManager.CurrentSearchTime,PolicePedManager.PlaceLastSeenPlayer);


        


        DisplayTextOnScreen(DebugLine3, 0.04f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);


        string DebugLine4 = string.Format("NumberPlateIndexSelected {0}", Debugging.NumberPlateIndexSelected);





        DisplayTextOnScreen(DebugLine4, 0.05f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        


        float Between = 0.01f;
        float Start = 0.15f;
        foreach (string Line in PedDamageManager.AllPedDamageList)
        {
            DisplayTextOnScreen(Line, Start, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            Start = Start + Between;
        }
        
    }
    private static void ScreenEffectsTick()
    {
        if (PlayerStateManager.IsDead)
        {
            if (!StartedDeathEffect)
            {
                NativeFunction.Natives.x80C8B1846639BB19(1);
                NativeFunction.Natives.x2206BF9A37B7F724("DeathFailMPIn", 0, 0);
                NativeFunction.CallByName<bool>("PLAY_SOUND_FRONTEND",-1,"Bed", "WastedSounds", true);
                BigMessage.MessageInstance.ShowColoredShard("WASTED", "", HudColor.HUD_COLOUR_BLACK, HudColor.HUD_COLOUR_REDDARK, 2000);
                StartedDeathEffect = true;
                
            }
        }
        else if (PlayerStateManager.IsBusted)
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
        else if (PedDamageManager.IsPlayerBleeding)
        {
            if (!StartedBandagingEffect)
            {
                NativeFunction.Natives.x80C8B1846639BB19(1);
                NativeFunction.Natives.x2206BF9A37B7F724("DrugsDrivingIn", 0, false);
                BigMessage.MessageInstance.ShowColoredShard("BLEEDING", "", HudColor.HUD_COLOUR_BLACK, HudColor.HUD_COLOUR_REDDARK, 1500);
                StartedBandagingEffect = true;
            }
        }
        else
        {
            if(StartedBandagingEffect)
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
    private static void HideVanillaUI()
    {
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_VEHICLE_NAME);
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_AREA_NAME);
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_VEHICLE_CLASS);
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_STREET_NAME);
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_VEHICLE_CLASS);
    }
    private static string GetPlayerStatusDisplay()
    {
        string PlayerStatusLine = "";
        if (PersonOfInterestManager.PlayerIsPersonOfInterest)
        {
            if (PlayerStateManager.IsWanted)
                PlayerStatusLine = "~r~Wanted~s~";
            else if (WantedLevelManager.HasBeenNotWantedFor <= 45000)
                PlayerStatusLine = "~o~Wanted~s~";
            else
                PlayerStatusLine = "~y~Wanted~s~";
        }
        if (PlayerStateManager.IsWanted)
        {
            string AgenciesChasingPlayer = PedManager.AgenciesChasingPlayer;
            if (AgenciesChasingPlayer != "")
                PlayerStatusLine += " (" + AgenciesChasingPlayer + "~s~)";
        }
        return PlayerStatusLine;
    }
    private static string GetVehicleStatusDisplay()
    {
        string PlayerSpeedLine = "";
        if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
        {

            float VehicleSpeedMPH = Game.LocalPlayer.Character.CurrentVehicle.Speed * 2.23694f;
            if (!Game.LocalPlayer.Character.CurrentVehicle.IsEngineOn)
                PlayerSpeedLine = "ENGINE OFF";
            else
            {
                string ColorPrefx = "~s~";
                if (TrafficViolationsManager.PlayerIsSpeeding)
                    ColorPrefx = "~r~";

                if (PlayerLocationManager.PlayerCurrentStreet != null)
                    PlayerSpeedLine = string.Format("{0}{1} ~s~MPH ({2})", ColorPrefx, Math.Round(VehicleSpeedMPH, MidpointRounding.AwayFromZero), PlayerLocationManager.PlayerCurrentStreet.SpeedLimit);
            }

            if (TrafficViolationsManager.ViolatingTrafficLaws)
                PlayerSpeedLine += " !";

            PlayerSpeedLine += "~n~" + VehicleFuelManager.FuelUIText;
        }
        return PlayerSpeedLine;
    }
    private static string GetStreetDisplay()
    {
        string StreetDisplay = "";
        if (PlayerLocationManager.PlayerCurrentStreet != null && PlayerLocationManager.PlayerCurrentCrossStreet != null)
            StreetDisplay = string.Format(" {0} at {1} ", PlayerLocationManager.PlayerCurrentStreet.Name, PlayerLocationManager.PlayerCurrentCrossStreet.Name);
        else if (PlayerLocationManager.PlayerCurrentStreet != null)
            StreetDisplay = string.Format(" {0} ", PlayerLocationManager.PlayerCurrentStreet.Name);
        return StreetDisplay;
    }
    private static string GetZoneDisplay()
    {
        if (PlayerLocationManager.PlayerCurrentZone == null)
            return "";
        string ZoneDisplay = "";
        string CopZoneName = "";
        ZoneDisplay = ZoneManager.GetName(PlayerLocationManager.PlayerCurrentZone,true);
        if (PlayerLocationManager.PlayerCurrentZone != null)
        {
            Agency MainZoneAgency = JurisdictionManager.GetMainAgency(PlayerLocationManager.PlayerCurrentZone.InternalGameName);
            if (MainZoneAgency != null)
                CopZoneName = MainZoneAgency.ColoredInitials;
        }
        //if(PlayerLocation.PlayerCurrentStreet != null && PlayerLocation.PlayerCurrentStreet.IsHighway)
        //{
        //    Agency HighwayPatrol = Agencies.RandomHighwayAgency;
        //    if(HighwayPatrol != null)
        //    CopZoneName += "~s~ / " + HighwayPatrol.ColoredInitials;
        //}
        ZoneDisplay = ZoneDisplay + " ~s~- " + CopZoneName;
        return ZoneDisplay;
    }
    private static void DisplayTextOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification)
    {
        NativeFunction.CallByName<bool>("SET_TEXT_FONT",(int)Font);
        NativeFunction.CallByName<bool>("SET_TEXT_SCALE", Scale, Scale);
        NativeFunction.CallByName<uint>("SET_TEXT_COLOUR", (int)TextColor.R, (int)TextColor.G, (int)TextColor.B, 255);

        NativeFunction.Natives.SetTextJustification((int)Justification);
        NativeFunction.Natives.SetTextDropshadow(2, 2, 0, 0, 0);

        if (Justification == GTATextJustification.Right)
            NativeFunction.CallByName<bool>("SET_TEXT_WRAP", 0f, Y);
        else
            NativeFunction.CallByName<bool>("SET_TEXT_WRAP", 0f, 1f);

        NativeFunction.CallByHash<uint>(0x25fbb336df1804cb, "STRING");
        NativeFunction.CallByHash<uint>(0x25FBB336DF1804CB, TextToShow);
        NativeFunction.CallByHash<uint>(0x6C188BE134E074AA, TextToShow);
        NativeFunction.CallByHash<uint>(0xCD015E5BB0D96A57, Y, X);
        return;
    }
}

