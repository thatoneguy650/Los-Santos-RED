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


public static class UI
{
    private static BigMessageThread bigMessage;
    private static bool StartedBandagingEffect = false;
    private static bool StartedBustedEffect = false;
    private static bool StartedDeathEffect = false;
    public static string DebugLine { get; set; }
    public enum EFont
    {
        FontChaletLondon = 0,
        FontHouseScript = 1,
        FontMonospace = 2,
        FontWingDings = 3,
        FontChaletComprimeCologne = 4,
        FontPricedown = 7
    };
    public enum TextJustification
    {
        Center = 0,
        Left = 1,
        Right = 2,
    };
    private enum HudComponent
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
    public static void Initialize()
    {
        IsRunning= true;
        bigMessage = new BigMessageThread(true);
        MainLoop();
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    private static void MainLoop()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {
                    UITick();
                    GameFiber.Yield();
                }
        }
            catch (Exception e)
        {
            General.Dispose();
            Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
        }
    });
    }
    private static void UITick()
    {
        if (General.MySettings.General.AlwaysShowHUD)
            NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);

        if (General.MySettings.General.AlwaysShowRadar)
            NativeFunction.CallByName<bool>("DISPLAY_RADAR", true);

        if (General.MySettings.Police.ShowPoliceRadarBlips)
            NativeFunction.CallByName<bool>("SET_POLICE_RADAR_BLIPS", true);
        else
            NativeFunction.CallByName<bool>("SET_POLICE_RADAR_BLIPS", false);

        if (General.MySettings.General.AlwaysShowCash)
            NativeFunction.CallByName<bool>("DISPLAY_CASH", true);


        if (General.MySettings.UI.Enabled && !General.IsBusted && !General.IsDead)
        {
            ShowUI();
        }

        ScreenEffectsTick();
    }
    private static void ShowUI()
    {
        HideVanillaUI();

        ShowPlayerStatus();
        //ShowClock();
        ShowPlayerArea();
        ShowVehicleStatus();
        //ShowDebugLine();
    }
    private static void ScreenEffectsTick()
    {
        if (General.IsDead)
        {
            if (!StartedDeathEffect)//!NativeFunction.CallByHash<bool>(0x2206BF9A37B7F724, "DeathFailMPIn"))
            {
                NativeFunction.Natives.x80C8B1846639BB19(1);
                NativeFunction.Natives.x2206BF9A37B7F724("DeathFailMPIn", 0, 0);//_START_SCREEN_EFFECT
                NativeFunction.CallByName<bool>("PLAY_SOUND_FRONTEND",-1,"Bed", "WastedSounds", true);
                bigMessage.MessageInstance.ShowColoredShard("WASTED", "", HudColor.HUD_COLOUR_BLACK, HudColor.HUD_COLOUR_REDDARK, 3000);
                StartedDeathEffect = true;
                
            }
        }
        else if (General.IsBusted)
        {
            if (!StartedBustedEffect)//!NativeFunction.CallByHash<bool>(0x2206BF9A37B7F724, "DeathFailMPDark"))
            {
                NativeFunction.Natives.x80C8B1846639BB19(1);
                NativeFunction.Natives.x2206BF9A37B7F724("DeathFailMPDark", 0, 0);//_START_SCREEN_EFFECT
                NativeFunction.CallByName<bool>("PLAY_SOUND_FRONTEND", -1, "TextHit", "WastedSounds", true);
                bigMessage.MessageInstance.ShowColoredShard("BUSTED", "", HudColor.HUD_COLOUR_BLACK, HudColor.HUD_COLOUR_BLUE, 3000);
                StartedBustedEffect = true;
            }
        }
        else if (PlayerHealth.IsBleeding)
        {
            if (!StartedBandagingEffect)//NativeFunction.CallByHash<bool>(0x2206BF9A37B7F724, "DrugsDrivingIn"))
            {
                NativeFunction.Natives.x80C8B1846639BB19(1);
                NativeFunction.Natives.x2206BF9A37B7F724("DrugsDrivingIn", 0, false);//_START_SCREEN_EFFECT  
                bigMessage.MessageInstance.ShowColoredShard("BLEEDING", "", HudColor.HUD_COLOUR_BLACK, HudColor.HUD_COLOUR_REDDARK, 1000);
                StartedBandagingEffect = true;
            }
        }
        else
        {
            StartedBustedEffect = false;
            StartedBandagingEffect = false;
            StartedDeathEffect = false;
            NativeFunction.Natives.xB4EDDC19532BFB85();
            NativeFunction.Natives.x80C8B1846639BB19(0);
        }

    }
    private static void HideVanillaUI()
    {
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)HudComponent.HUD_VEHICLE_NAME);
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)HudComponent.HUD_AREA_NAME);
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)HudComponent.HUD_VEHICLE_CLASS);
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)HudComponent.HUD_STREET_NAME);
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)HudComponent.HUD_VEHICLE_CLASS);
    }

    private static void ShowVehicleStatus()
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
                if (TrafficViolations.PlayerIsSpeeding)
                    ColorPrefx = "~r~";

                if (PlayerLocation.PlayerCurrentStreet != null)
                    PlayerSpeedLine = string.Format("{0}{1} ~s~MPH ({2})", ColorPrefx, Math.Round(VehicleSpeedMPH, MidpointRounding.AwayFromZero), PlayerLocation.PlayerCurrentStreet.SpeedLimit);
            }

            if (TrafficViolations.ViolatingTrafficLaws)
                PlayerSpeedLine += " !";

            PlayerSpeedLine += "~n~" + VehicleFuelSystem.FuelUIText;
        }
        DisplayTextOnScreen(PlayerSpeedLine, General.MySettings.UI.VehicleStatusPositionX, General.MySettings.UI.VehicleStatusPositionY, General.MySettings.UI.VehicleStatusScale, Color.White, EFont.FontChaletComprimeCologne, (TextJustification)General.MySettings.UI.VehicleStatusJustificationID);

    }
    private static void ShowPlayerArea()
    {
        DisplayTextOnScreen(GetStreetDisplay(), General.MySettings.UI.StreetPositionX, General.MySettings.UI.StreetPositionY, General.MySettings.UI.StreetScale, Color.White, EFont.FontHouseScript,(TextJustification)General.MySettings.UI.StreetJustificationID);
        DisplayTextOnScreen(GetZoneDisplay(), General.MySettings.UI.ZonePositionX, General.MySettings.UI.ZonePositionY, General.MySettings.UI.ZoneScale, Color.White, EFont.FontHouseScript, (TextJustification)General.MySettings.UI.ZoneJustificationID);
    }
    private static void ShowPlayerStatus()
    {
        string PlayerStatusLine = "";
        if (PersonOfInterest.PlayerIsPersonOfInterest)
        {
            if (General.PlayerIsWanted)
                PlayerStatusLine = "~r~Wanted~s~";
            else if (Police.PlayerHasBeenNotWantedFor <= 45000)
                PlayerStatusLine = "~o~Wanted~s~";
            else
                PlayerStatusLine = "~y~Wanted~s~";
        }
        if (General.PlayerIsWanted)
        {
            string AgenciesChasingPlayer = PedList.AgenciesChasingPlayer;
            if (AgenciesChasingPlayer != "")
                PlayerStatusLine += " (" + AgenciesChasingPlayer + "~s~)";
        }

        DisplayTextOnScreen(PlayerStatusLine, General.MySettings.UI.PlayerStatusPositionX, General.MySettings.UI.PlayerStatusPositionY, General.MySettings.UI.PlayerStatusScale, Color.White, EFont.FontChaletComprimeCologne, (TextJustification)General.MySettings.UI.PlayerStatusJustificationID);
    }
    private static string GetStreetDisplay()
    {
        string StreetDisplay = "";
        if (PlayerLocation.PlayerCurrentStreet != null && PlayerLocation.PlayerCurrentCrossStreet != null)
            StreetDisplay = string.Format(" {0} at {1} ", PlayerLocation.PlayerCurrentStreet.Name, PlayerLocation.PlayerCurrentCrossStreet.Name);
        else if (PlayerLocation.PlayerCurrentStreet != null)
            StreetDisplay = string.Format(" {0} ", PlayerLocation.PlayerCurrentStreet.Name);
        return StreetDisplay;
    }
    private static string GetZoneDisplay()
    {
        if (PlayerLocation.PlayerCurrentZone == null)
            return "";
        string ZoneDisplay = "";
        string CopZoneName = "";
        ZoneDisplay = Zones.GetFormattedZoneName(PlayerLocation.PlayerCurrentZone,true);
        if (PlayerLocation.PlayerCurrentStreet != null && PlayerLocation.PlayerCurrentStreet.IsHighway)
            CopZoneName = PlayerLocation.PlayerCurrentZone.MainZoneAgency.ColoredInitials;// +  "~s~ / " + Agencies.SAHP.ColoredInitials;
        else if (PlayerLocation.PlayerCurrentZone != null && PlayerLocation.PlayerCurrentZone.MainZoneAgency != null)
            CopZoneName = PlayerLocation.PlayerCurrentZone.MainZoneAgency.ColoredInitials;
        ZoneDisplay = ZoneDisplay + " ~s~- " + CopZoneName;
        return ZoneDisplay;
    }
    public static void DisplayTextOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, EFont Font, TextJustification Justification)
    {
        NativeFunction.CallByName<bool>("SET_TEXT_FONT",(int)Font);
        NativeFunction.CallByName<bool>("SET_TEXT_SCALE", Scale, Scale);
        NativeFunction.CallByName<uint>("SET_TEXT_COLOUR", (int)TextColor.R, (int)TextColor.G, (int)TextColor.B, 255);

        NativeFunction.Natives.SetTextJustification((int)Justification);
        NativeFunction.Natives.SetTextDropshadow(2, 2, 0, 0, 0);

        if (Justification == TextJustification.Right)
            NativeFunction.CallByName<bool>("SET_TEXT_WRAP", 0f, Y);
        else
            NativeFunction.CallByName<bool>("SET_TEXT_WRAP", 0f, 1f);

        NativeFunction.CallByHash<uint>(0x25fbb336df1804cb, "STRING");
        NativeFunction.CallByHash<uint>(0x25FBB336DF1804CB, TextToShow);
        NativeFunction.CallByHash<uint>(0x6C188BE134E074AA, TextToShow);
        NativeFunction.CallByHash<uint>(0xCD015E5BB0D96A57, Y, X);
        return;
    }
    //public static void DisplayTextOnScreen(string text, float x, float y, float scale, bool center, Color TextColor, EFont Font, TextJustification Justification)
    //{
    //    //Game.Console.Print("Invoke font");
    //    NativeFunction.Natives.SetTextFont((int)Font);
    //    //Game.Console.Print("Set scale");
    //    NativeFunction.Natives.SetTextScale(scale, scale);
    //    //Game.Console.Print("Calling color ref");
    //    NativeFunction.CallByName<uint>("SET_TEXT_COLOUR", new NativeArgument[]
    //    {
    //           (int)TextColor.R, (int)TextColor.G, (int)TextColor.B, 255
    //    });

    //    NativeFunction.Natives.SetTextJustification((int)Justification);
    //    NativeFunction.Natives.SetTextDropshadow(2, 2, 0, 0, 0);
    //    //Game.Console.Print("Set wrap");
    //    // NativeFunction.Natives.SetTextWrap((float)0.0, (float)1.0);


    //    if(Justification == TextJustification.Right)
    //        NativeFunction.CallByName<bool>("SET_TEXT_WRAP",0f, y);
    //    else
    //        NativeFunction.CallByName<bool>("SET_TEXT_WRAP", 0f, 1f);
    //    //Game.Console.Print("Set centre");
    //    //NativeFunction.Natives.SetTextCentre(center);
    //    //Game.Console.Print("Set dropshadow");

    //    //Game.Console.Print("Set edge");
    //    //NativeFunction.Natives.SetTextEdge(1, 0, 0, 0, 255);//NativeFunction.Natives.SetTextEdge(1, 0, 0, 0, 205);
    //    //Game.Console.Print("Set leading");
    //    // NativeFunction.Natives.SetTextLeading(1);



    //    //Game.Console.Print("Set entry type");
    //    NativeFunction.CallByHash<uint>(0x25fbb336df1804cb, "STRING"); // Remplacant fonction SET_TEXT_ENTRY, le nouveau nom est BEGIN_TEXT_COMMAND_DISPLAY_TEXT
    //                                                                   //Game.Console.Print("Create text component");
    //                                                                   //NativeFunction.CallByName<uint>("ADD_TEXT_COMPONENT_SUBSTRING_TEXT_LABEL", text); //Pour RPH 0.52
    //    NativeFunction.CallByHash<uint>(0x25FBB336DF1804CB, text); //BEGIN TEXT COMMAND DISPLAY TEXT
    //                                                               //Game.Console.Print("Add substring");
    //    NativeFunction.CallByHash<uint>(0x6C188BE134E074AA, text); //Hash pour le nom ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME.
    //                                                               //Game.Console.Print("Draw text !");
    //    NativeFunction.CallByHash<uint>(0xCD015E5BB0D96A57, y, x);
    //    //NativeFunction.CallByName<uint>("_DRAW_TEXT", y, x);
    //    //Game.Console.Print("Text displayed.");
    //    return;
    //}
    private static void ShowClock()
    {
        string PlayerCLockLine = string.Format("{0}", Clock.ClockTime);
        if (Clock.ClockSpeed != "1x")
            PlayerCLockLine = string.Format("{0} ({1})", Clock.ClockTime, Clock.ClockSpeed);

        if (DebugLine != "")
            DisplayTextOnScreen(DebugLine, .9f, .3f, 0.45f, Color.White, EFont.FontChaletComprimeCologne, TextJustification.Left);

        //Text(PlayerCLockLine, LosSantosRED.MySettings.UI.PositionX - 2 * LosSantosRED.MySettings.UI.Spacing, LosSantosRED.MySettings.UI.PositionY, LosSantosRED.MySettings.UI.Scale, false, Color.White, EFont.FontChaletComprimeCologne);
    }
    private static void ShowDebugLine()
    {
        if (DebugLine != "")
            DisplayTextOnScreen(DebugLine, .9f, .3f, 0.45f, Color.White, EFont.FontChaletComprimeCologne, TextJustification.Left);
    }
    private static string GetCompassHeading()
    {
        float Heading = Game.LocalPlayer.Character.Heading;
        string Abbreviation;
        if (Heading >= 354.375f || Heading <= 5.625f) { Abbreviation = "N"; }
        else if (Heading >= 5.625f && Heading <= 16.875f) { Abbreviation = "NbE"; }
        else if (Heading >= 16.875f && Heading <= 28.125f) { Abbreviation = "NNE"; }
        else if (Heading >= 28.125f && Heading <= 39.375f) { Abbreviation = "NEbN"; }
        else if (Heading >= 39.375f && Heading <= 50.625f) { Abbreviation = "NE"; }
        else if (Heading >= 50.625f && Heading <= 61.875f) { Abbreviation = "NEbE"; }
        else if (Heading >= 61.875f && Heading <= 73.125f) { Abbreviation = "ENE"; }
        else if (Heading >= 73.125f && Heading <= 84.375f) { Abbreviation = "EbN"; }
        else if (Heading >= 84.375f && Heading <= 95.625f) { Abbreviation = "E"; }
        else if (Heading >= 95.625f && Heading <= 106.875f) { Abbreviation = "EbS"; }
        else if (Heading >= 106.875f && Heading <= 118.125f) { Abbreviation = "ESE"; }
        else if (Heading >= 118.125f && Heading <= 129.375f) { Abbreviation = "SEbE"; }
        else if (Heading >= 129.375f && Heading <= 140.625f) { Abbreviation = "SE"; }
        else if (Heading >= 140.625f && Heading <= 151.875f) { Abbreviation = "SEbS"; }
        else if (Heading >= 151.875f && Heading <= 163.125f) { Abbreviation = "SSE"; }
        else if (Heading >= 163.125f && Heading <= 174.375f) { Abbreviation = "SbE"; }
        else if (Heading >= 174.375f && Heading <= 185.625f) { Abbreviation = "S"; }
        else if (Heading >= 185.625f && Heading <= 196.875f) { Abbreviation = "SbW"; }
        else if (Heading >= 196.875f && Heading <= 208.125f) { Abbreviation = "SSW"; }
        else if (Heading >= 208.125f && Heading <= 219.375f) { Abbreviation = "SWbS"; }
        else if (Heading >= 219.375f && Heading <= 230.625f) { Abbreviation = "SW"; }
        else if (Heading >= 230.625f && Heading <= 241.875f) { Abbreviation = "SWbW"; }
        else if (Heading >= 241.875f && Heading <= 253.125f) { Abbreviation = "WSW"; }
        else if (Heading >= 253.125f && Heading <= 264.375f) { Abbreviation = "WbS"; }
        else if (Heading >= 264.375f && Heading <= 275.625f) { Abbreviation = "W"; }
        else if (Heading >= 275.625f && Heading <= 286.875f) { Abbreviation = "WbN"; }
        else if (Heading >= 286.875f && Heading <= 298.125f) { Abbreviation = "WNW"; }
        else if (Heading >= 298.125f && Heading <= 309.375f) { Abbreviation = "NWbW"; }
        else if (Heading >= 309.375f && Heading <= 320.625f) { Abbreviation = "NW"; }
        else if (Heading >= 320.625f && Heading <= 331.875f) { Abbreviation = "NWbN"; }
        else if (Heading >= 331.875f && Heading <= 343.125f) { Abbreviation = "NNW"; }
        else if (Heading >= 343.125f && Heading <= 354.375f) { Abbreviation = "NbW"; }
        else if (Heading >= 354.375f || Heading <= 5.625f) { Abbreviation = "N"; }
        else { Abbreviation = ""; }

        return Abbreviation;
    }
}

