using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class UI
{
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
    public static bool IsRunning { get; set; }
    public static void Initialize()
    {
        IsRunning= true;
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
            LosSantosRED.Dispose();
            Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
        }
    });
    }

    private static void UITick()
    {
        if (Settings.AlwaysShowHUD)
            NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);

        if (Settings.AlwaysShowRadar)
            NativeFunction.CallByName<bool>("DISPLAY_RADAR", true);

        if (Settings.ShowPoliceRadarBlips)
            NativeFunction.CallByName<bool>("SET_POLICE_RADAR_BLIPS", true);
        else
            NativeFunction.CallByName<bool>("SET_POLICE_RADAR_BLIPS", false);

        if (Settings.AlwaysShowCash)
            NativeFunction.CallByName<bool>("DISPLAY_CASH", true);

        if (Settings.TrafficInfoUI && !LosSantosRED.IsBusted && !LosSantosRED.IsDead)
        {
            ShowTrafficUI();
        }
    }
    private static void ShowTrafficUI()
    {
        string PlayerStatusLine;
        if (PersonOfInterest.PlayerIsPersonOfInterest)
        {
            if(LosSantosRED.PlayerIsWanted)
                PlayerStatusLine = "~r~Arrest on Sight~s~";
            else if (Police.PlayerHasBeenNotWantedFor <= 45000)
                PlayerStatusLine = "~o~Arrest on Sight~s~";
            else
                PlayerStatusLine = "~y~Arrest on Sight~s~";
        }
        else
            PlayerStatusLine = "";

        if (LosSantosRED.PlayerWantedLevel > 0)
        {
            string AgenciesChasingPlayer = PoliceScanning.AgenciesChasingPlayer;
            if (AgenciesChasingPlayer != "")
                PlayerStatusLine += " (" + AgenciesChasingPlayer + "~s~)";
        }

        string StreetLine = GetCompassHeading() + " | " + GetStreetDisplay();  

        string PlayerZoneLine = "";
        if (PlayerLocation.PlayerCurrentZone != null)
            PlayerZoneLine = GetZoneDisplay();

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
                if (PlayerLocation.PlayerCurrentStreet != null && PlayerLocation.PlayerCurrentCrossStreet != null)
                    PlayerSpeedLine = string.Format("{0}{1} ~s~MPH ({2}) / ({3})", ColorPrefx, Math.Round(VehicleSpeedMPH, MidpointRounding.AwayFromZero), PlayerLocation.PlayerCurrentStreet.SpeedLimit, PlayerLocation.PlayerCurrentCrossStreet.SpeedLimit);
                else if (PlayerLocation.PlayerCurrentStreet != null)
                    PlayerSpeedLine = string.Format("{0}{1} ~s~MPH ({2})", ColorPrefx, Math.Round(VehicleSpeedMPH, MidpointRounding.AwayFromZero), PlayerLocation.PlayerCurrentStreet.SpeedLimit);
            }

            if(TrafficViolations.ViolatingTrafficLaws)
                PlayerSpeedLine += " !";

            if(TrafficViolations.PlayerIsRunningRedLight)
                PlayerSpeedLine += " Running Red";

            PlayerSpeedLine += VehicleEngine.VehicleIndicatorStatus;
        }

        string PlayerCLockLine = string.Format("{0}", ClockSystem.ClockTime);
        if (ClockSystem.ClockSpeed != "1x")
            PlayerCLockLine = string.Format("{0} ({1})", ClockSystem.ClockTime, ClockSystem.ClockSpeed);

        if (DebugLine != "")
            Text(DebugLine, Settings.TrafficInfoUIPositionX - 3 * Settings.TrafficInfoUISpacing, Settings.TrafficInfoUIPositionY, Settings.TrafficInfoUIScale, false, Color.White, EFont.FontChaletComprimeCologne);

        Text(PlayerCLockLine, Settings.TrafficInfoUIPositionX - 2 * Settings.TrafficInfoUISpacing, Settings.TrafficInfoUIPositionY, Settings.TrafficInfoUIScale, false, Color.White, EFont.FontChaletComprimeCologne);
        Text(PlayerStatusLine, Settings.TrafficInfoUIPositionX - Settings.TrafficInfoUISpacing, Settings.TrafficInfoUIPositionY, Settings.TrafficInfoUIScale, false, Color.White, EFont.FontChaletComprimeCologne);
        Text(StreetLine, Settings.TrafficInfoUIPositionX, Settings.TrafficInfoUIPositionY, Settings.TrafficInfoUIScale, false, Color.White, EFont.FontChaletComprimeCologne);
        Text(PlayerZoneLine, Settings.TrafficInfoUIPositionX + Settings.TrafficInfoUISpacing, Settings.TrafficInfoUIPositionY, Settings.TrafficInfoUIScale, false, Color.White, EFont.FontChaletComprimeCologne);
        Text(PlayerSpeedLine, Settings.TrafficInfoUIPositionX + 2 * Settings.TrafficInfoUISpacing, Settings.TrafficInfoUIPositionY, Settings.TrafficInfoUIScale, false, Color.White, EFont.FontChaletComprimeCologne);
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

    private static string GetStreetDisplay()
    {
        string StreetDisplay = "";
        if (PlayerLocation.PlayerCurrentStreet != null && PlayerLocation.PlayerCurrentCrossStreet != null)
            StreetDisplay = string.Format(" {0} / {1} ", PlayerLocation.PlayerCurrentStreet.Name, PlayerLocation.PlayerCurrentCrossStreet.Name);
        else if (PlayerLocation.PlayerCurrentStreet != null)
            StreetDisplay = string.Format(" {0} ", PlayerLocation.PlayerCurrentStreet.Name);
        return StreetDisplay;
    }
    private static string GetZoneDisplay()
    {
        string ZoneDisplay = "";
        string CopZoneName = "";
        ZoneDisplay = Zones.GetFormattedZoneName(PlayerLocation.PlayerCurrentZone);
        if (PlayerLocation.PlayerCurrentStreet != null && PlayerLocation.PlayerCurrentStreet.isFreeway)
            CopZoneName = PlayerLocation.PlayerCurrentZone.MainZoneAgency.ColoredInitials +  "~s~ / " + Agencies.SAHP.ColoredInitials;
        else
            CopZoneName = PlayerLocation.PlayerCurrentZone.MainZoneAgency.ColoredInitials;
        ZoneDisplay = ZoneDisplay + " ~s~- " + CopZoneName;
        return ZoneDisplay;
    }
    public static void Text(string text, float x, float y, float scale, bool center, Color TextColor, EFont Font)
    {
        //Game.Console.Print("Invoke font");
        NativeFunction.Natives.SetTextFont((int)Font);
        //Game.Console.Print("Set scale");
        NativeFunction.Natives.SetTextScale(scale, scale);
        //Game.Console.Print("Calling color ref");
        NativeFunction.CallByName<uint>("SET_TEXT_COLOUR", new NativeArgument[]
        {
               (int)TextColor.R, (int)TextColor.G, (int)TextColor.B, 255
        });
        //Game.Console.Print("Set wrap");
        NativeFunction.Natives.SetTextWrap((float)0.0, (float)1.0);
        //Game.Console.Print("Set centre");
        NativeFunction.Natives.SetTextCentre(center);
        //Game.Console.Print("Set dropshadow");
        NativeFunction.Natives.SetTextDropshadow(2, 2, 0, 0, 0);
        //Game.Console.Print("Set edge");
        NativeFunction.Natives.SetTextEdge(1, 0, 0, 0, 350);//NativeFunction.Natives.SetTextEdge(1, 0, 0, 0, 205);
        //Game.Console.Print("Set leading");
        NativeFunction.Natives.SetTextLeading(1);
        //Game.Console.Print("Set entry type");
        NativeFunction.CallByHash<uint>(0x25fbb336df1804cb, "STRING"); // Remplacant fonction SET_TEXT_ENTRY, le nouveau nom est BEGIN_TEXT_COMMAND_DISPLAY_TEXT
                                                                       //Game.Console.Print("Create text component");
                                                                       //NativeFunction.CallByName<uint>("ADD_TEXT_COMPONENT_SUBSTRING_TEXT_LABEL", text); //Pour RPH 0.52
        NativeFunction.CallByHash<uint>(0x25FBB336DF1804CB, text); //BEGIN TEXT COMMAND DISPLAY TEXT
                                                                   //Game.Console.Print("Add substring");
        NativeFunction.CallByHash<uint>(0x6C188BE134E074AA, text); //Hash pour le nom ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME.
                                                                   //Game.Console.Print("Draw text !");
        NativeFunction.CallByHash<uint>(0xCD015E5BB0D96A57, y, x);
        //NativeFunction.CallByName<uint>("_DRAW_TEXT", y, x);
        //Game.Console.Print("Text displayed.");
        return;
    }
}

