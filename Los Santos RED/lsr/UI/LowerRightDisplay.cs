using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class LowerRightDisplay
{
    private IDisplayable DisplayablePlayer;
    private ITimeReportable Time;
    private ISettingsProvideable Settings;
    private UI UI;

    private float lowerRightHeighSpace;

    private string lastCrimesDisplay;
    private string lastPlayerDisplay;
    private string lastStreetDisplay;
    private string lastFaderStreetDisplay;
    private string lastVehicleStatusDisplay;
    private string lastZoneDisplay;

    private Fader PlayerFader;
    private Fader StreetFader;
    private Fader VehicleFader;
    private Fader WeaponFader;
    private Fader ZoneFader;

    private string overrideTimeDisplay = "";
    private bool playerIsInVehicle = false;
    public LowerRightDisplay(IDisplayable displayablePlayer, ITimeReportable time, ISettingsProvideable settings, UI ui)
    {
        DisplayablePlayer = displayablePlayer;
        Time = time;
        Settings = settings;
        UI = ui;
        StreetFader = new Fader(Settings.SettingsManager.LSRHUDSettings.StreetDisplayTimeToShow, Settings.SettingsManager.LSRHUDSettings.StreetDisplayTimeToFade, "StreetFader");
        ZoneFader = new Fader(Settings.SettingsManager.LSRHUDSettings.ZoneDisplayTimeToShow, Settings.SettingsManager.LSRHUDSettings.ZoneDisplayTimeToFade, "ZoneFader");
        VehicleFader = new Fader(Settings.SettingsManager.LSRHUDSettings.VehicleDisplayTimeToShow, Settings.SettingsManager.LSRHUDSettings.VehicleDisplayTimeToFade, "VehicleFader");
        PlayerFader = new Fader(Settings.SettingsManager.LSRHUDSettings.PlayerDisplayTimeToShow, Settings.SettingsManager.LSRHUDSettings.PlayerDisplayTimeToFade, "PlayerFader");
    }

    public void Setup()
    {

    }
    public void Dispose()
    {

    }
    public void CacheData()
    {
        if (DisplayablePlayer.IsAliveAndFree)
        {
            if (Settings.SettingsManager.LSRHUDSettings.CrimesDisplayEnabled)
            {
                lastCrimesDisplay = GetViolationsText();
            }
            if (Settings.SettingsManager.LSRHUDSettings.VehicleDisplayEnabled)
            {
                lastVehicleStatusDisplay = GetVehicleStatusDisplay();
            }
            if (Settings.SettingsManager.LSRHUDSettings.ShowPlayerDisplay)
            {
                lastPlayerDisplay = GetPlayerDisplay();
            }
            if (playerIsInVehicle != DisplayablePlayer.IsInVehicle)
            {
                OnIsInVehicleChanged();
            }
            if (Settings.SettingsManager.LSRHUDSettings.ShowStreetDisplay)
            {
                lastStreetDisplay = GetStreetDisplay(true);
                lastFaderStreetDisplay = GetStreetDisplay(false);
                if (Settings.SettingsManager.LSRHUDSettings.FadeStreetDisplay && ((!DisplayablePlayer.IsWanted && !DisplayablePlayer.Investigation.IsActive) || Settings.SettingsManager.LSRHUDSettings.FadeStreetDisplayDuringWantedAndInvestigation))
                {
                    if (Settings.SettingsManager.LSRHUDSettings.FadeZoneDisplay && ZoneFader.TextChangedLastUpdate)
                    {
                        StreetFader.UpdateTimeChanged();
                    }
                }
            }
            if (Settings.SettingsManager.LSRHUDSettings.ShowZoneDisplay)
            {
                lastZoneDisplay = GetZoneDisplay();
                if (Settings.SettingsManager.LSRHUDSettings.FadeZoneDisplay && ((!DisplayablePlayer.IsWanted && !DisplayablePlayer.Investigation.IsActive) || Settings.SettingsManager.LSRHUDSettings.FadeZoneDisplayDuringWantedAndInvestigation))
                {
                    if (Settings.SettingsManager.LSRHUDSettings.FadeStreetDisplay && StreetFader.TextChangedLastUpdate)
                    {
                        ZoneFader.UpdateTimeChanged();
                    }
                }
            }
        }
    }
    public void Display()
    {
        if (!UI.IsNotShowingFrontEndMenus)
        {
            return;
        }
        if (DisplayablePlayer.IsCustomizingPed)
        {
            return;
        }
        if (!Settings.SettingsManager.UIGeneralSettings.IsEnabled || !DisplayablePlayer.IsAliveAndFree)
        {
            return;
        }
        if (!Settings.SettingsManager.UIGeneralSettings.HideLSRUIUnlessActionWheelActive || UI.IsDrawingWheelMenu)
        {
            lowerRightHeighSpace = UI.TimeBarsShowing * Settings.SettingsManager.LSRHUDSettings.LowerDisplayTimerBarSpacing;
            if (UI.InstructionalsShowing > 0)
            {
                lowerRightHeighSpace += Settings.SettingsManager.LSRHUDSettings.LowerDisplayButtonPromptSpacing;
            }
            if (lowerRightHeighSpace == 0.0f)
            {
                lowerRightHeighSpace = Settings.SettingsManager.LSRHUDSettings.LowerDisplayNoItemSpacing;
            }
#if DEBUG
            DisplayDebug();
#endif
            DisplayCurrentCrimes();
            DisplayVehicleStatus();
            DisplayPlayerInfo();
            DisplayStreets();
            DisplayZones();
        }
    }

    private void DisplayDebug()
    {
        if (DisplayablePlayer.DebugString != "" && Settings.SettingsManager.UIGeneralSettings.ShowDebug)
        {
            NativeHelper.DisplayTextOnScreen(DisplayablePlayer.DebugString, Settings.SettingsManager.LSRHUDSettings.CrimesDisplayPositionX - lowerRightHeighSpace - 0.35f, Settings.SettingsManager.LSRHUDSettings.CrimesDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.CrimesDisplayScale, Color.White, Settings.SettingsManager.LSRHUDSettings.CrimesDisplayFont, (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.CrimesDisplayJustificationID, false);
        }
    }
    private void DisplayCurrentCrimes()
    {
        if (Settings.SettingsManager.LSRHUDSettings.CrimesDisplayEnabled)
        {
            NativeHelper.DisplayTextOnScreen(lastCrimesDisplay, Settings.SettingsManager.LSRHUDSettings.CrimesDisplayPositionX - lowerRightHeighSpace, Settings.SettingsManager.LSRHUDSettings.CrimesDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.CrimesDisplayScale, Color.White, Settings.SettingsManager.LSRHUDSettings.CrimesDisplayFont, (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.CrimesDisplayJustificationID, false);
        }
    }
    private void DisplayVehicleStatus()
    {
        if (Settings.SettingsManager.LSRHUDSettings.VehicleDisplayEnabled)
        {
            NativeHelper.DisplayTextOnScreen(lastVehicleStatusDisplay, Settings.SettingsManager.LSRHUDSettings.VehicleDisplayPositionX - lowerRightHeighSpace, Settings.SettingsManager.LSRHUDSettings.VehicleDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.VehicleDisplayScale, Color.White, Settings.SettingsManager.LSRHUDSettings.VehicleDisplayFont, (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.VehicleDisplayJustificationID, false);
        }
    }
    private void DisplayPlayerInfo()
    {
        if (Settings.SettingsManager.LSRHUDSettings.ShowPlayerDisplay)
        {
            PlayerFader.Update(lastPlayerDisplay, lastPlayerDisplay);
            if (Settings.SettingsManager.LSRHUDSettings.FadePlayerDisplay && ((!DisplayablePlayer.IsWanted && !DisplayablePlayer.Investigation.IsActive) || Settings.SettingsManager.LSRHUDSettings.FadePlayerDisplayDuringWantedAndInvestigation) && !UI.IsDrawingWheelMenu)
            {
                NativeHelper.DisplayTextOnScreen(PlayerFader.TextToShow, Settings.SettingsManager.LSRHUDSettings.PlayerStatusPositionX - lowerRightHeighSpace, Settings.SettingsManager.LSRHUDSettings.PlayerStatusPositionY, Settings.SettingsManager.LSRHUDSettings.PlayerStatusScale, Color.White, Settings.SettingsManager.LSRHUDSettings.PlayerStatusFont, (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.PlayerStatusJustificationID, false, PlayerFader.AlphaValue);
            }
            else
            {
                NativeHelper.DisplayTextOnScreen(lastPlayerDisplay, Settings.SettingsManager.LSRHUDSettings.PlayerStatusPositionX - lowerRightHeighSpace, Settings.SettingsManager.LSRHUDSettings.PlayerStatusPositionY, Settings.SettingsManager.LSRHUDSettings.PlayerStatusScale, Color.White, Settings.SettingsManager.LSRHUDSettings.PlayerStatusFont, (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.PlayerStatusJustificationID, false);
            }
        }
    }
    private void DisplayStreets()
    {
        if (Settings.SettingsManager.LSRHUDSettings.ShowStreetDisplay)
        {
            StreetFader.Update(lastFaderStreetDisplay, lastStreetDisplay);
            if (Settings.SettingsManager.LSRHUDSettings.FadeStreetDisplay && ((!DisplayablePlayer.IsWanted && !DisplayablePlayer.Investigation.IsActive) || Settings.SettingsManager.LSRHUDSettings.FadeStreetDisplayDuringWantedAndInvestigation) && !UI.IsDrawingWheelMenu)
            {
                NativeHelper.DisplayTextOnScreen(StreetFader.TextToShow, Settings.SettingsManager.LSRHUDSettings.StreetPositionX - lowerRightHeighSpace, Settings.SettingsManager.LSRHUDSettings.StreetPositionY, Settings.SettingsManager.LSRHUDSettings.StreetScale, Color.White, Settings.SettingsManager.LSRHUDSettings.StreetFont, (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.StreetJustificationID, false, StreetFader.AlphaValue);
            }
            else
            {
                NativeHelper.DisplayTextOnScreen(lastStreetDisplay, Settings.SettingsManager.LSRHUDSettings.StreetPositionX - lowerRightHeighSpace, Settings.SettingsManager.LSRHUDSettings.StreetPositionY, Settings.SettingsManager.LSRHUDSettings.StreetScale, Color.White, Settings.SettingsManager.LSRHUDSettings.StreetFont, (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.StreetJustificationID, false);
            }
        }
    }
    private void DisplayZones()
    {
        if (Settings.SettingsManager.LSRHUDSettings.ShowZoneDisplay)
        {
            ZoneFader.Update(lastZoneDisplay, lastZoneDisplay);
            if (Settings.SettingsManager.LSRHUDSettings.FadeZoneDisplay && ((!DisplayablePlayer.IsWanted && !DisplayablePlayer.Investigation.IsActive) || Settings.SettingsManager.LSRHUDSettings.FadeZoneDisplayDuringWantedAndInvestigation) && !UI.IsDrawingWheelMenu)
            {
                NativeHelper.DisplayTextOnScreen(ZoneFader.TextToShow, Settings.SettingsManager.LSRHUDSettings.ZonePositionX - lowerRightHeighSpace, Settings.SettingsManager.LSRHUDSettings.ZonePositionY, Settings.SettingsManager.LSRHUDSettings.ZoneScale, Color.White, Settings.SettingsManager.LSRHUDSettings.ZoneFont, (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.ZoneJustificationID, false, ZoneFader.AlphaValue);
            }
            else
            {
                NativeHelper.DisplayTextOnScreen(lastZoneDisplay, Settings.SettingsManager.LSRHUDSettings.ZonePositionX - lowerRightHeighSpace, Settings.SettingsManager.LSRHUDSettings.ZonePositionY, Settings.SettingsManager.LSRHUDSettings.ZoneScale, Color.White, Settings.SettingsManager.LSRHUDSettings.ZoneFont, (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.ZoneJustificationID, false);
            }
        }
    }

    private string GetFormattedTime()
    {
        if (overrideTimeDisplay != "")
        {
            return overrideTimeDisplay;
        }
        string TimeDisplay = "";
        if (Time.CurrentDay != 0)
        {
            TimeDisplay = Time.CurrentTime;
        }
        return TimeDisplay;
    }
    private string GetPlayerDisplay()
    {
        string PlayerDisplay = "";
        if (DisplayablePlayer.IsWanted)
        {
            if (!DisplayablePlayer.PoliceResponse.WantedLevelHasBeenRadioedIn)
            {
                PlayerDisplay += $"~o~ Alerted{UI.CurrentDefaultTextColor}";
            }
            else
            {
                if (DisplayablePlayer.IsInSearchMode)
                {
                    PlayerDisplay += $"~o~ Attempting To Locate";
                }
                else
                {
                    if (DisplayablePlayer.PoliceResponse.IsWeaponsFree)
                    {
                        PlayerDisplay += $"~r~ Weapons Free";
                    }
                    else if (DisplayablePlayer.PoliceResponse.IsDeadlyChase)
                    {
                        PlayerDisplay += $"~r~ Lethal Force Authorized";
                    }
                    else if (DisplayablePlayer.CurrentLocation.IsInside && DisplayablePlayer.AnyPoliceKnowInteriorLocation)
                    {
                        PlayerDisplay += $"~r~ Breaching Position";
                    }
                    else
                    {
                        PlayerDisplay += $"~r~ Pursuit Active";
                    }
                }
                if(Settings.SettingsManager.LSRHUDSettings.PlayerStatusIncludePoliceCount)
                {
                    PlayerDisplay += $" ({DisplayablePlayer.PoliceResponse.CurrentRespondingPoliceCount}) {UI.CurrentDefaultTextColor}";
                }
                else
                {
                    PlayerDisplay += $"{UI.CurrentDefaultTextColor}";
                }
                
            }
        }
        else if (DisplayablePlayer.Investigation != null && DisplayablePlayer.Investigation.IsActive)
        {
            if (DisplayablePlayer.Investigation.RequiresPolice)
            {
                if (DisplayablePlayer.Investigation.IsSuspicious)
                {
                    PlayerDisplay += $"~r~ Police Responding with Description{UI.CurrentDefaultTextColor}";
                    if (Settings.SettingsManager.LSRHUDSettings.PlayerStatusIncludePoliceCount)
                    {
                        PlayerDisplay += $" ({DisplayablePlayer.Investigation.CurrentRespondingPoliceCount}){UI.CurrentDefaultTextColor}";
                    }
                }
                else if (DisplayablePlayer.Investigation.IsNearPosition)
                {
                    PlayerDisplay += $"~o~ Police Responding{UI.CurrentDefaultTextColor}";
                    if (Settings.SettingsManager.LSRHUDSettings.PlayerStatusIncludePoliceCount)
                    {
                        PlayerDisplay += $" ({DisplayablePlayer.Investigation.CurrentRespondingPoliceCount}){UI.CurrentDefaultTextColor}";
                    }
                }

            }
            else if (DisplayablePlayer.Investigation.RequiresEMS || DisplayablePlayer.Investigation.RequiresFirefighters)
            {
                PlayerDisplay += $"~o~ Emergency Services Responding{UI.CurrentDefaultTextColor}";
            }
        }
        else if (DisplayablePlayer.CriminalHistory.HasHistory)
        {
            if (DisplayablePlayer.CriminalHistory.HasDeadlyHistory)
            {
                PlayerDisplay += $"~r~ APB Issued{UI.CurrentDefaultTextColor}";
            }
            else
            {
                PlayerDisplay += $"~o~ BOLO Issued{UI.CurrentDefaultTextColor}";
            }
        }

        if(DisplayablePlayer.IntimidationManager.IsMinimumIntimidating)
        {
            if (PlayerDisplay == "")
            {
                PlayerDisplay = $"{UI.CurrentDefaultTextColor}{DisplayablePlayer.IntimidationManager.IntimidationDisplay}";
            }
            else
            {
                PlayerDisplay += $" {UI.CurrentDefaultTextColor}{DisplayablePlayer.IntimidationManager.IntimidationDisplay}";
            }
        }

        if ((DisplayablePlayer.IsNotWanted && Settings.SettingsManager.LSRHUDSettings.PlayerStatusIncludeTime) || (Settings.SettingsManager.LSRHUDSettings.PlayerStatusShowWhenSleeping && DisplayablePlayer.IsSleeping) || Time.ForceShowClock)
        {
            if (PlayerDisplay == "")
            {
                PlayerDisplay = $"{UI.CurrentDefaultTextColor}" + GetFormattedTime();
            }
            else
            {
                PlayerDisplay += $" {UI.CurrentDefaultTextColor}" + GetFormattedTime();
            }
        }




        return PlayerDisplay;
    }
    private string GetStreetDisplay(bool includeAddress)
    {
        string StreetDisplay = "";



        if(DisplayablePlayer.InteriorManager.IsInsideTeleportInterior && DisplayablePlayer.InteriorManager.CurrentTeleportInteriorLocation != null && !DisplayablePlayer.IsInVehicle)
        {
            StreetDisplay += $"{UI.CurrentDefaultTextColor} {DisplayablePlayer.InteriorManager.CurrentTeleportInteriorLocation.Name} {UI.CurrentDefaultTextColor}";
        }
        else if (DisplayablePlayer.CurrentLocation.IsInside && !DisplayablePlayer.IsInVehicle)
        {
            if (DisplayablePlayer.CurrentLocation.CurrentInterior?.Name == "")
            {
#if DEBUG
                StreetDisplay += $"{UI.CurrentDefaultTextColor} {DisplayablePlayer.CurrentLocation.CurrentInterior?.Name} ({DisplayablePlayer.CurrentLocation.CurrentInterior?.LocalID}) {UI.CurrentDefaultTextColor}";
#endif
            }
            else
            {
                StreetDisplay += $"{UI.CurrentDefaultTextColor} {DisplayablePlayer.CurrentLocation.CurrentInterior?.Name}{UI.CurrentDefaultTextColor}";
            }
        }

        else if (DisplayablePlayer.CurrentLocation.CurrentStreet != null)
        {
            StreetDisplay = $"{UI.CurrentDefaultTextColor}";
            if (includeAddress)
            {
                StreetDisplay += $"{NativeHelper.CellToStreetNumber(DisplayablePlayer.CellX, DisplayablePlayer.CellY)}";
            }
            if (DisplayablePlayer.CurrentLocation.CurrentStreet.IsHighway)
            {
                StreetDisplay += "~y~";
            }
            StreetDisplay += $" {DisplayablePlayer.CurrentLocation.CurrentStreet.ProperStreetName}{UI.CurrentDefaultTextColor}";
            if (DisplayablePlayer.CurrentLocation.CurrentCrossStreet != null)
            {
                StreetDisplay += $" at {UI.CurrentDefaultTextColor}{DisplayablePlayer.CurrentLocation.CurrentCrossStreet.ProperStreetName} {UI.CurrentDefaultTextColor}";
            }
        }
        
        return StreetDisplay;
    }
    private string GetVehicleStatusDisplay()
    {
        string CurrentSpeedDisplay = "";
        if (DisplayablePlayer.CurrentVehicle != null && (DisplayablePlayer.IsInAutomobile || DisplayablePlayer.IsOnMotorcycle))//was game.localpalyer.character.isinanyvehicle(false)
        {
            CurrentSpeedDisplay = $" {UI.CurrentDefaultTextColor}" + "";
            if (DisplayablePlayer.CurrentVehicle.Vehicle.Exists() && DisplayablePlayer.CurrentVehicle.IsCar && !DisplayablePlayer.CurrentVehicle.Engine.IsRunning)
            {
                CurrentSpeedDisplay = $" {UI.CurrentDefaultTextColor}" + "ENGINE OFF";
            }
            else
            {
                string ColorPrefx = UI.CurrentDefaultTextColor;
                if (DisplayablePlayer.Violations.TrafficViolations.IsFelonySpeeding)
                {
                    ColorPrefx = "~r~";
                }

                if (DisplayablePlayer.CurrentLocation.CurrentStreet != null)
                {
                    if (Settings.SettingsManager.LSRHUDSettings.SpeedDisplayUnits == "MPH")
                    {
                        if (Settings.SettingsManager.LSRHUDSettings.VehicleDisplayIncludeTextSpeedLimit)
                        {
                            if (Settings.SettingsManager.LSRHUDSettings.VehicleDisplayIncludeCurrentSpeed)
                            {
                                CurrentSpeedDisplay = $"{ColorPrefx}{Math.Round(DisplayablePlayer.VehicleSpeedMPH, MidpointRounding.AwayFromZero)}{UI.CurrentDefaultTextColor} ({Math.Round(DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitMPH, MidpointRounding.AwayFromZero)} {UI.CurrentDefaultTextColor}MPH)";
                            }
                            else
                            {
                                CurrentSpeedDisplay = $"({Math.Round(DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitMPH, MidpointRounding.AwayFromZero)} {UI.CurrentDefaultTextColor}MPH)";
                            }
                        }
                        else
                        {
                            if (Settings.SettingsManager.LSRHUDSettings.VehicleDisplayIncludeCurrentSpeed)
                            {
                                CurrentSpeedDisplay = $"{ColorPrefx}{Math.Round(DisplayablePlayer.VehicleSpeedMPH, MidpointRounding.AwayFromZero)} {UI.CurrentDefaultTextColor}MPH";
                            }
                            else
                            {
                                CurrentSpeedDisplay = "";
                            }
                        }
                    }
                    else if (Settings.SettingsManager.LSRHUDSettings.SpeedDisplayUnits == "KM/H")
                    {
                        if (Settings.SettingsManager.LSRHUDSettings.VehicleDisplayIncludeTextSpeedLimit)
                        {
                            if (Settings.SettingsManager.LSRHUDSettings.VehicleDisplayIncludeCurrentSpeed)
                            {
                                CurrentSpeedDisplay = $"{ColorPrefx}{Math.Round(DisplayablePlayer.VehicleSpeedKMH, MidpointRounding.AwayFromZero)}{UI.CurrentDefaultTextColor} ({Math.Round(DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitKMH, MidpointRounding.AwayFromZero)} {UI.CurrentDefaultTextColor}KM/H)";
                            }
                            else
                            {
                                CurrentSpeedDisplay = $"({Math.Round(DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitKMH, MidpointRounding.AwayFromZero)} {UI.CurrentDefaultTextColor}KM/H)";
                            }
                        }
                        else
                        {
                            if (Settings.SettingsManager.LSRHUDSettings.VehicleDisplayIncludeCurrentSpeed)
                            {
                                CurrentSpeedDisplay = $"{ColorPrefx}{Math.Round(DisplayablePlayer.VehicleSpeedKMH, MidpointRounding.AwayFromZero)} {UI.CurrentDefaultTextColor}KM/H";
                            }
                            else
                            {
                                CurrentSpeedDisplay = "";
                            }
                        }
                    }
                }
            }
            if (DisplayablePlayer.Violations.TrafficViolations.IsViolatingAnyTrafficLaws)
            {
                CurrentSpeedDisplay += $" ~r~!{UI.CurrentDefaultTextColor}";
            }
            if (DisplayablePlayer.CurrentVehicle.Indicators.HazardsOn)
            {
                CurrentSpeedDisplay += $" ~o~(HAZ){UI.CurrentDefaultTextColor}";
            }
            else if (DisplayablePlayer.CurrentVehicle.Indicators.RightBlinkerOn)
            {
                CurrentSpeedDisplay += $" ~y~(RI){UI.CurrentDefaultTextColor}";
            }
            else if (DisplayablePlayer.CurrentVehicle.Indicators.LeftBlinkerOn)
            {
                CurrentSpeedDisplay += $" ~y~(LI){UI.CurrentDefaultTextColor}";
            }
            if (Settings.SettingsManager.VehicleSettings.UseCustomFuelSystem && DisplayablePlayer.CurrentVehicle.RequiresFuel)
            {
                CurrentSpeedDisplay += $"{UI.CurrentDefaultTextColor} " + DisplayablePlayer.CurrentVehicle.FuelTank.UIText;
            }
            if (Settings.SettingsManager.LSRHUDSettings.VehicleDisplayIncludeCompass)
            {
                string Heading = NativeHelper.GetSimpleCompassHeading(DisplayablePlayer.Character.Heading);
                CurrentSpeedDisplay += $" {Heading}";
            }
        }
        return CurrentSpeedDisplay;
    }
    private string GetZoneDisplay()
    {
         if (DisplayablePlayer.InteriorManager.IsInsideTeleportInterior)
        {
            return "";
        }
        return DisplayablePlayer.CurrentLocation.CurrentZone?.GetFullLocationName(DisplayablePlayer,Settings, UI.CurrentDefaultTextColor);
    }
    private string GetViolationsText()
    {
        string CrimeDisplay = "";
        if (DisplayablePlayer.Violations.LawsViolatingDisplay != "")
        {
            CrimeDisplay += "Violating: " + DisplayablePlayer.Violations.LawsViolatingDisplay;
        }
        return CrimeDisplay;
    }

    //private void DisplayTextOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification, bool outline)
    //{
    //    DisplayTextOnScreen(TextToShow, X, Y, Scale, TextColor, Font, Justification, outline, 255);
    //}
    //private void DisplayTextOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification, bool outline, int alpha)
    //{
    //    try
    //    {
    //        if (TextToShow == "" || alpha == 0 || TextToShow is null)
    //        {
    //            return;
    //        }
    //        NativeFunction.Natives.SET_TEXT_FONT((int)Font);
    //        NativeFunction.Natives.SET_TEXT_SCALE(Scale, Scale);
    //        NativeFunction.Natives.SET_TEXT_COLOUR((int)TextColor.R, (int)TextColor.G, (int)TextColor.B, alpha);

    //        NativeFunction.Natives.SetTextJustification((int)Justification);

    //        NativeFunction.Natives.SET_TEXT_DROP_SHADOW();

    //        if (outline)
    //        {
    //            NativeFunction.Natives.SET_TEXT_OUTLINE();


    //            //NativeFunction.Natives.SET_TEXT_EDGE(1, 0, 0, 0, 255);
    //        }
    //        NativeFunction.Natives.SET_TEXT_DROP_SHADOW();
    //        //NativeFunction.Natives.SetTextDropshadow(20, 255, 255, 255, 255);//NativeFunction.Natives.SetTextDropshadow(2, 2, 0, 0, 0);
    //        //NativeFunction.Natives.SetTextJustification((int)GTATextJustification.Center);
    //        if (Justification == GTATextJustification.Right)
    //        {
    //            NativeFunction.Natives.SET_TEXT_WRAP(0f, Y);
    //        }
    //        else
    //        {
    //            NativeFunction.Natives.SET_TEXT_WRAP(0f, 1f);
    //        }
    //        NativeFunction.Natives.x25fbb336df1804cb("STRING"); //NativeFunction.Natives.x25fbb336df1804cb("STRING");
    //        //NativeFunction.Natives.x25FBB336DF1804CB(TextToShow);
    //        NativeFunction.Natives.x6C188BE134E074AA(TextToShow);
    //        NativeFunction.Natives.xCD015E5BB0D96A57(Y, X);
    //    }
    //    catch (Exception ex)
    //    {
    //        EntryPoint.WriteToConsole($"UI ERROR {ex.Message} {ex.StackTrace}", 0);
    //    }
    //    //return;
    //}

    private void OnIsInVehicleChanged()
    {
        if (Settings.SettingsManager.LSRHUDSettings.FadeZoneDisplay)
        {
            ZoneFader.UpdateTimeChanged();
        }
        if (Settings.SettingsManager.LSRHUDSettings.FadeStreetDisplay)
        {
            StreetFader.UpdateTimeChanged();
        }
        playerIsInVehicle = DisplayablePlayer.IsInVehicle;
    }
}

