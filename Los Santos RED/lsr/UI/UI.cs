using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ExtensionsMethods;

public class UI : IMenuProvideable
{

    private BigMessageThread BigMessage;
    private BustedMenu BustedMenu;
    private DeathMenu DeathMenu;
    private DebugMenu DebugMenu;
    private List<ButtonPrompt> displayedButtonPrompts = new List<ButtonPrompt>();
   // private uint GameTimeLastDisplayedBleedingHelp;
    private InstructionalButtons instructional = new InstructionalButtons();
    private MainMenu MainMenu;
    private List<Menu> MenuList;
    private MenuPool menuPool;
    private IDisplayable DisplayablePlayer;
    //private Mod.Player PlayerIntellisense;
    private ISettingsProvideable Settings;
   // private bool StartedBandagingEffect = false;
    private bool StartedBustedEffect = false;
    private bool StartedDeathEffect = false;
    private IJurisdictions Jurisdictions;
    private uint GameTimeLastBusted;
    private uint GameTimeLastDied;
    //private string currentVehicleStatusDisplay = "";

    //private string lastVehicleStatusDisplay = "";

    //private string lastStreetDisplay = "";
    //private string currentStreetDisplay = "";
    //private string currentZoneDisplay = "";
    //private uint GameTimeVehicleStatusDisplayChanged = 0;
    //private uint GameTimeStreetDisplayChanged = 0;
    //private uint GameTimeZoneDisplayChanged = 0;
    //private int zoneDisplayAlpha;
    //private int streetDisplayAlpha;
    private bool DrawTexture = true;
    private Texture ToDraw;
    private Texture Sign10;
    private Texture Sign15;
    private Texture Sign20;
    private Texture Sign25;
    private Texture Sign30;
    private Texture Sign35;
    private Texture Sign40;
    private Texture Sign45;
    private Texture Sign50;
    private Texture Sign55;
    private Texture Sign60;
    private Texture Sign65;
    private Texture Sign70;
    private Texture Sign75;
    private Texture Sign80;
    //private uint GameTimeVehicleStatusDisplayStopped;
    //private uint GameTimeStreetDisplayRemoved;
    private Fader StreetFader;
    private Fader ZoneFader;
    private Fader VehicleFader;
    private Fader PlayerFader;
    private bool ZoneDisplayingStreetAlpha = false;
    private bool playerIsInVehicle = false;
    private ITimeReportable Time;
    private string CurrentDefaultTextColor = "~c~";
    

    //private bool StreetFadeIsInverse = false;
    //private bool ZoneFadeIsInverse;

    public UI(IDisplayable displayablePlayer, ISettingsProvideable settings, IJurisdictions jurisdictions, IPedSwap pedSwap, IPlacesOfInterest placesOfInterest, IRespawning respawning, IActionable actionablePlayer, ISaveable saveablePlayer, IWeapons weapons, RadioStations radioStations, IGameSaves gameSaves, IEntityProvideable world, IRespawnable player, IPoliceRespondable policeRespondable, ITaskerable tasker, IInventoryable playerinventory, IModItems modItems, ITimeReportable time)
    {
        DisplayablePlayer = displayablePlayer;
        Settings = settings;
        Jurisdictions = jurisdictions;
        Time = time;
        BigMessage = new BigMessageThread(true);
        menuPool = new MenuPool();
        DeathMenu = new DeathMenu(menuPool, pedSwap, respawning, placesOfInterest, Settings, player, gameSaves);
        BustedMenu = new BustedMenu(menuPool, pedSwap, respawning, placesOfInterest,Settings, policeRespondable);
        MainMenu = new MainMenu(menuPool, actionablePlayer, saveablePlayer, gameSaves, weapons, pedSwap, world, Settings,tasker, playerinventory, modItems);
        DebugMenu = new DebugMenu(menuPool, actionablePlayer, weapons, radioStations, placesOfInterest, Settings);
        MenuList = new List<Menu>() { DeathMenu, BustedMenu, MainMenu, DebugMenu };
        StreetFader = new Fader(Settings.SettingsManager.UISettings.StreetDisplayTimeToShow, Settings.SettingsManager.UISettings.StreetDisplayTimeToFade, "StreetFader");
        ZoneFader = new Fader(Settings.SettingsManager.UISettings.ZoneDisplayTimeToShow, Settings.SettingsManager.UISettings.ZoneDisplayTimeToFade, "ZoneFader");
        VehicleFader = new Fader(Settings.SettingsManager.UISettings.VehicleStatusTimeToShow, Settings.SettingsManager.UISettings.VehicleStatusTimeToFade, "VehicleFader");
        PlayerFader = new Fader(Settings.SettingsManager.UISettings.PlayerDisplayTimeToShow, Settings.SettingsManager.UISettings.PlayerDisplayTimeToFade, "PlayerFader");
    }

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
    public void Setup()
    {
        Game.RawFrameRender += Draw;
        Sign10 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\10mph.png");
        Sign15 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\15mph.png");
        Sign20 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\20mph.png");
        Sign25 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\25mph.png");
        Sign30 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\30mph.png");
        Sign35 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\35mph.png");
        Sign40 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\40mph.png");
        Sign45 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\45mph.png");
        Sign50 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\50mph.png");
        Sign55 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\55mph.png");
        Sign60 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\60mph.png");
        Sign65 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\65mph.png");
        Sign70 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\70mph.png");
        Sign75 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\75mph.png");
        Sign80 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\80mph.png");
    }
    public void Dispose()
    {
        Game.RawFrameRender -= Draw;
    }
    public void Update()
    {
        ForceVanillaUI();
        DrawUI();
        ScreenEffectsUpdate();
        DisplayButtonPrompts();
        MenuUpdate();
        RadarUpdate();
    }
    public void ToggleMenu()
    {
        if (DisplayablePlayer.IsDead)
        {
            Toggle(DeathMenu);
        }
        else if (DisplayablePlayer.IsBusted)
        {
            Toggle(BustedMenu);
        }
        else
        {
            Toggle(MainMenu);
        }
    }
    public void ToggleDebugMenu()
    {
        Toggle(DebugMenu);
    }
    private void DisplayButtonPrompts()
    {
        if (Settings.SettingsManager.UISettings.DisplayButtonPrompts)
        {
            instructional.Buttons.Clear();

            if (DisplayablePlayer.ButtonPrompts.Any())
            {
                foreach (ButtonPrompt buttonPrompt in DisplayablePlayer.ButtonPrompts.OrderByDescending(x => x.Order))
                {
                    if (buttonPrompt.Key != Keys.None)
                    {
                        if (buttonPrompt.Modifier != Keys.None)
                        {
                            instructional.Buttons.Add(new InstructionalButtonGroup(buttonPrompt.Text, buttonPrompt.Modifier.GetInstructionalKey(), InstructionalKey.SymbolPlus, buttonPrompt.Key.GetInstructionalKey()));
                        }
                        else
                        {
                            instructional.Buttons.Add(new InstructionalButtonGroup(buttonPrompt.Text, buttonPrompt.Key.GetInstructionalKey()));
                        }
                    }
                    else
                    {
                        if (buttonPrompt.Modifier != Keys.None)
                        {
                            instructional.Buttons.Add(new InstructionalButtonGroup(buttonPrompt.Text, buttonPrompt.Modifier.GetInstructionalKey(), InstructionalKey.SymbolPlus, InstructionalKey.MouseLeft));
                        }
                        else
                        {
                            instructional.Buttons.Add(new InstructionalButtonGroup(buttonPrompt.Text, InstructionalKey.MouseLeft));
                        }
                    }


                }
            }
            instructional.Update();
            if (DisplayablePlayer.ButtonPrompts.Any())
            {
                instructional.Draw();
            }
        }
    }
    private void DisplayTextOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification, int alpha)
    {
        NativeFunction.CallByName<bool>("SET_TEXT_FONT", (int)Font);
        NativeFunction.CallByName<bool>("SET_TEXT_SCALE", Scale, Scale);
        NativeFunction.CallByName<uint>("SET_TEXT_COLOUR", (int)TextColor.R, (int)TextColor.G, (int)TextColor.B, alpha);

        NativeFunction.Natives.SetTextJustification((int)Justification);
        NativeFunction.Natives.SetTextDropshadow(10, 255, 0, 255, 255);//NativeFunction.Natives.SetTextDropshadow(2, 2, 0, 0, 0);

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
    private void DisplayTextOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification)
    {
        DisplayTextOnScreen(TextToShow, X, Y, Scale, TextColor, Font, Justification, 255);       
    }
    public void Draw(object sender, GraphicsEventArgs args)
    {
        if(DrawTexture && !Game.IsPaused && DisplayablePlayer.IsAliveAndFree && !menuPool.IsAnyMenuOpen())
        {
            if (ToDraw != null && ToDraw.Size != null)
            {  
                float Scale = Settings.SettingsManager.UISettings.SpeedLimitScale;
                float posX = (Game.Resolution.Height - (ToDraw.Size.Height * Scale)) * Settings.SettingsManager.UISettings.SpeedLimitPositionX;
                float posY = (Game.Resolution.Width - (ToDraw.Size.Width * Scale)) * Settings.SettingsManager.UISettings.SpeedLimitPositionY;         
                args.Graphics.DrawTexture(ToDraw, new RectangleF(posY, posX, ToDraw.Size.Width * Scale, ToDraw.Size.Height * Scale));
            }
        }
    }
    private void ForceVanillaUI()
    {
        if (Settings.SettingsManager.UISettings.AlwaysShowHUD)
        {
            NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);
        }

        if (Settings.SettingsManager.UISettings.AlwaysShowRadar)
        {
            NativeFunction.CallByName<bool>("DISPLAY_RADAR", true);
        }

        if (Settings.SettingsManager.PoliceSettings.ShowVanillaBlips)
        {
            NativeFunction.CallByName<bool>("SET_POLICE_RADAR_BLIPS", true);
        }
        else
        {
            NativeFunction.CallByName<bool>("SET_POLICE_RADAR_BLIPS", false);
        }

        if (Settings.SettingsManager.UISettings.AlwaysShowCash)
        {
            NativeFunction.CallByName<bool>("DISPLAY_CASH", true);
        }
    }
    private void DrawUI()
    {
        if (Settings.SettingsManager.UISettings.UIEnabled && DisplayablePlayer.IsAliveAndFree)
        {
            if(Time.IsNight && Settings.SettingsManager.UISettings.GreyOutWhiteFontAtNight)
            {
                CurrentDefaultTextColor = "~c~";
            }
            else
            {
                CurrentDefaultTextColor = "~s~";
            }


            if (Settings.SettingsManager.UISettings.ShowDebug)
            {
                ShowDebugUI();
            }
            if (!Settings.SettingsManager.UISettings.ShowVanillaAreaUI)
            {
                HideVanillaAreaUI();
            }
            if (!Settings.SettingsManager.UISettings.ShowVanillaVehicleUI)
            {
                HideVanillaVehicleUI();
            }

            if (Settings.SettingsManager.UISettings.ShowCrimesDisplay)
            {
                DisplayTextOnScreen(GetViolatingDisplay(), Settings.SettingsManager.UISettings.CrimesViolatingPositionX, Settings.SettingsManager.UISettings.CrimesViolatingPositionY, Settings.SettingsManager.UISettings.CrimesViolatingScale, Color.White, Settings.SettingsManager.UISettings.CrimesViolatingFont, (GTATextJustification)Settings.SettingsManager.UISettings.CrimesViolatingJustificationID);
            }
            if(Settings.SettingsManager.UISettings.ShowSpeedLimitDisplay)
            {
                DisplaySpeedLimitSign();
            }
            if (Settings.SettingsManager.UISettings.ShowVehicleStatusDisplay)
            {
                string newVehicleStatus = GetVehicleStatusDisplay();
                DisplayTextOnScreen(newVehicleStatus, Settings.SettingsManager.UISettings.VehicleStatusPositionX, Settings.SettingsManager.UISettings.VehicleStatusPositionY, Settings.SettingsManager.UISettings.VehicleStatusScale, Color.White, Settings.SettingsManager.UISettings.VehicleStatusFont, (GTATextJustification)Settings.SettingsManager.UISettings.VehicleStatusJustificationID);
            }
            if (Settings.SettingsManager.UISettings.ShowPlayerDisplay)
            {
                string newPlayerDisplay = GetPlayerDisplay();
                if (Settings.SettingsManager.UISettings.FadePlayerDisplay && ((!DisplayablePlayer.IsWanted && !DisplayablePlayer.Investigation.IsActive) || Settings.SettingsManager.UISettings.FadePlayerDisplayDuringWantedAndInvestigation))
                {
                    PlayerFader.Update(newPlayerDisplay);
                    DisplayTextOnScreen(PlayerFader.TextToShow, Settings.SettingsManager.UISettings.PlayerStatusPositionX, Settings.SettingsManager.UISettings.PlayerStatusPositionY, Settings.SettingsManager.UISettings.PlayerStatusScale, Color.White, Settings.SettingsManager.UISettings.PlayerStatusFont, (GTATextJustification)Settings.SettingsManager.UISettings.PlayerStatusJustificationID, PlayerFader.AlphaValue);
                }
                else
                {
                    DisplayTextOnScreen(newPlayerDisplay, Settings.SettingsManager.UISettings.PlayerStatusPositionX, Settings.SettingsManager.UISettings.PlayerStatusPositionY, Settings.SettingsManager.UISettings.PlayerStatusScale, Color.White, Settings.SettingsManager.UISettings.PlayerStatusFont, (GTATextJustification)Settings.SettingsManager.UISettings.PlayerStatusJustificationID);
                }
            }
            if(playerIsInVehicle != DisplayablePlayer.IsInVehicle)
            {
                if(Settings.SettingsManager.UISettings.FadeZoneDisplay)
                {
                    ZoneFader.UpdateTimeChanged();
                }
                if (Settings.SettingsManager.UISettings.FadeStreetDisplay)
                {
                    StreetFader.UpdateTimeChanged();
                }
                playerIsInVehicle = DisplayablePlayer.IsInVehicle;
            }
            if (Settings.SettingsManager.UISettings.ShowStreetDisplay)
            {
                string newStreetDisplay = GetStreetDisplay();
                if (Settings.SettingsManager.UISettings.FadeStreetDisplay && ((!DisplayablePlayer.IsWanted && !DisplayablePlayer.Investigation.IsActive) || Settings.SettingsManager.UISettings.FadeStreetDisplayDuringWantedAndInvestigation))
                {
                    StreetFader.Update(newStreetDisplay);
                    if (Settings.SettingsManager.UISettings.FadeZoneDisplay && ZoneFader.TextChangedLastUpdate)
                    {
                        StreetFader.UpdateTimeChanged();
                    }
                    DisplayTextOnScreen(StreetFader.TextToShow, Settings.SettingsManager.UISettings.StreetPositionX, Settings.SettingsManager.UISettings.StreetPositionY, Settings.SettingsManager.UISettings.StreetScale, Color.White, Settings.SettingsManager.UISettings.StreetFont, (GTATextJustification)Settings.SettingsManager.UISettings.StreetJustificationID, StreetFader.AlphaValue);
                }
                else
                {
                    DisplayTextOnScreen(newStreetDisplay, Settings.SettingsManager.UISettings.StreetPositionX, Settings.SettingsManager.UISettings.StreetPositionY, Settings.SettingsManager.UISettings.StreetScale, Color.White, Settings.SettingsManager.UISettings.StreetFont, (GTATextJustification)Settings.SettingsManager.UISettings.StreetJustificationID);
                }    
            }
            if (Settings.SettingsManager.UISettings.ShowZoneDisplay)
            {
                string newZoneDisplay = GetZoneDisplay();
                if (Settings.SettingsManager.UISettings.FadeZoneDisplay && ((!DisplayablePlayer.IsWanted && !DisplayablePlayer.Investigation.IsActive) || Settings.SettingsManager.UISettings.FadeZoneDisplayDuringWantedAndInvestigation))
                {
                    ZoneFader.Update(newZoneDisplay);
                    if (Settings.SettingsManager.UISettings.FadeStreetDisplay && StreetFader.TextChangedLastUpdate)
                    {
                        ZoneFader.UpdateTimeChanged();
                    }
                    DisplayTextOnScreen(ZoneFader.TextToShow, Settings.SettingsManager.UISettings.ZonePositionX, Settings.SettingsManager.UISettings.ZonePositionY, Settings.SettingsManager.UISettings.ZoneScale, Color.White, Settings.SettingsManager.UISettings.ZoneFont, (GTATextJustification)Settings.SettingsManager.UISettings.ZoneJustificationID, ZoneFader.AlphaValue);
                }
                else
                {
                    DisplayTextOnScreen(newZoneDisplay, Settings.SettingsManager.UISettings.ZonePositionX, Settings.SettingsManager.UISettings.ZonePositionY, Settings.SettingsManager.UISettings.ZoneScale, Color.White, Settings.SettingsManager.UISettings.ZoneFont, (GTATextJustification)Settings.SettingsManager.UISettings.ZoneJustificationID);
                }
            }
        }
    }
    private string GetTimeDisplay()
    {
        string TimeDisplay = "";
        if (Time.CurrentDay != 0)
        {
            TimeDisplay = Time.CurrentTime;
        } 
        return TimeDisplay;
    }
    private void DisplaySpeedLimitSign()
    {
        if (DisplayablePlayer.CurrentVehicle != null && DisplayablePlayer.CurrentLocation.CurrentStreet != null && DisplayablePlayer.IsAliveAndFree)
        {
            float speedLimit = 60f;
            if (Settings.SettingsManager.UISettings.SpeedDisplayUnits == "MPH")
            {
                speedLimit = DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitMPH;
            }
            else if (Settings.SettingsManager.UISettings.SpeedDisplayUnits == "KM/H")
            {
                speedLimit = DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitKMH;
            }
            if (speedLimit <= 10f)
            {
                ToDraw = Sign10;
            }
            else if (speedLimit <= 15f)
            {
                ToDraw = Sign15;
            }
            else if (speedLimit <= 20f)
            {
                ToDraw = Sign20;
            }
            else if (speedLimit <= 25f)
            {
                ToDraw = Sign25;
            }
            else if (speedLimit <= 30f)
            {
                ToDraw = Sign30;
            }
            else if (speedLimit <= 35f)
            {
                ToDraw = Sign35;
            }
            else if (speedLimit <= 40f)
            {
                ToDraw = Sign40;
            }
            else if (speedLimit <= 45f)
            {
                ToDraw = Sign45;
            }
            else if (speedLimit <= 50f)
            {
                ToDraw = Sign50;
            }
            else if (speedLimit <= 55f)
            {
                ToDraw = Sign55;
            }
            else if (speedLimit <= 60f)
            {
                ToDraw = Sign60;
            }
            else if (speedLimit <= 65f)
            {
                ToDraw = Sign65;
            }
            else if (speedLimit <= 70f)
            {
                ToDraw = Sign70;
            }
            else if (speedLimit <= 75f)
            {
                ToDraw = Sign75;
            }
            else if (speedLimit <= 80f)
            {
                ToDraw = Sign80;
            }
            else
            {
                ToDraw = null;
            }
        }
        else
        {
            ToDraw = null;
        }
        if (ToDraw != null)
        {
            DrawTexture = true;
        }
        else
        {
            DrawTexture = false;
        }
    }
    private int CalculateAlpha(uint GameTimeLastChanged, uint timeToShow, uint fadeTime)
    {
        uint TimeSinceChanged = Game.GameTime - GameTimeLastChanged;
        if (TimeSinceChanged < timeToShow)
        {
            return 255;
        }
        else if (TimeSinceChanged < timeToShow + fadeTime)
        {
            float percentVisible = 1f - (1f * (TimeSinceChanged - timeToShow)) / (1f * (fadeTime));
            float alphafloat = percentVisible * 255f;
            return ((int)Math.Floor(alphafloat)).Clamp(0, 255);
        }
        else
        {
            return 0;
        }
    }
    private string GetViolatingDisplay()
    {
        string CrimeDisplay = "";
        if (DisplayablePlayer.LawsViolating != "")
        {
            CrimeDisplay += "Violating: " + DisplayablePlayer.LawsViolating;
        }
        return CrimeDisplay;
    }
    private string GetVehicleStatusDisplay()
    {
        string CurrentSpeedDisplay = "";
        if (DisplayablePlayer.CurrentVehicle != null)//was game.localpalyer.character.isinanyvehicle(false)
        {
            CurrentSpeedDisplay = $" {CurrentDefaultTextColor}" + "";
            if (DisplayablePlayer.CurrentVehicle.Vehicle.Exists() && DisplayablePlayer.CurrentVehicle.IsCar && !DisplayablePlayer.CurrentVehicle.Engine.IsRunning)
            {
                CurrentSpeedDisplay = $" {CurrentDefaultTextColor}" + "ENGINE OFF";
            }
            else
            {
                string ColorPrefx = CurrentDefaultTextColor;
                if (DisplayablePlayer.IsSpeeding)
                {
                    ColorPrefx = "~r~";
                }
                if (DisplayablePlayer.CurrentLocation.CurrentStreet != null)
                {
                    if (Settings.SettingsManager.UISettings.SpeedDisplayUnits == "MPH")
                    {
                        if (Settings.SettingsManager.UISettings.VehicleStatusIncludeTextSpeedLimit)
                        {
                            CurrentSpeedDisplay = $"{ColorPrefx}{Math.Round(DisplayablePlayer.VehicleSpeedMPH, MidpointRounding.AwayFromZero)} ({Math.Round(DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitMPH, MidpointRounding.AwayFromZero)}) {CurrentDefaultTextColor}MPH)";
                        }
                        else
                        {
                            CurrentSpeedDisplay = $"{ColorPrefx}{Math.Round(DisplayablePlayer.VehicleSpeedMPH, MidpointRounding.AwayFromZero)} {CurrentDefaultTextColor}MPH";
                        }
                    }
                    else if(Settings.SettingsManager.UISettings.SpeedDisplayUnits == "KM/H")
                    {
                        if (Settings.SettingsManager.UISettings.VehicleStatusIncludeTextSpeedLimit)
                        {
                            CurrentSpeedDisplay = $"{ColorPrefx}{Math.Round(DisplayablePlayer.VehicleSpeedKMH, MidpointRounding.AwayFromZero)} ({Math.Round(DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitKMH, MidpointRounding.AwayFromZero)}) {CurrentDefaultTextColor}KM/H)";
                        }
                        else
                        {
                            CurrentSpeedDisplay = $"{ColorPrefx}{Math.Round(DisplayablePlayer.VehicleSpeedKMH, MidpointRounding.AwayFromZero)} {CurrentDefaultTextColor}KM/H";
                        }
                    }
                }
            }
            if (DisplayablePlayer.IsViolatingAnyTrafficLaws)
            {
                CurrentSpeedDisplay += " ~r~!";
            }
            if (DisplayablePlayer.CurrentVehicle.Indicators.HazardsOn)
            {
                CurrentSpeedDisplay += " ~o~(HAZ)";
            }
            else if (DisplayablePlayer.CurrentVehicle.Indicators.RightBlinkerOn)
            {
                CurrentSpeedDisplay += " ~y~(RI)";
            }
            else if (DisplayablePlayer.CurrentVehicle.Indicators.LeftBlinkerOn)
            {
                CurrentSpeedDisplay += " ~y~(LI)";
            }
            if (Settings.SettingsManager.PlayerSettings.UseCustomFuelSystem)
            {
                CurrentSpeedDisplay += $"{CurrentDefaultTextColor} " + DisplayablePlayer.CurrentVehicle.FuelTank.UIText;
            }
        }
        return CurrentSpeedDisplay;
    }
    private string GetPlayerDisplay()
    {
        string PlayerDisplay = "";
        if(DisplayablePlayer.IsWanted)
        {
            if(DisplayablePlayer.IsInSearchMode)
            {
                PlayerDisplay += $"~o~ Attempting To Locate{CurrentDefaultTextColor}";
            }
            else
            {
                if (DisplayablePlayer.PoliceResponse.IsWeaponsFree)
                {
                    PlayerDisplay += $"~r~ Active Pursuit (Weapons Free){CurrentDefaultTextColor}";
                }
                else if (DisplayablePlayer.PoliceResponse.IsDeadlyChase)
                {
                    PlayerDisplay += $"~r~ Active Pursuit (Lethal Force Authorized){CurrentDefaultTextColor}";
                }
                else
                {
                    PlayerDisplay += $"~r~ Active Pursuit{CurrentDefaultTextColor}";
                }
            }
        }
        else if (DisplayablePlayer.Investigation != null && DisplayablePlayer.Investigation.IsActive)
        {   
            if(DisplayablePlayer.Investigation.IsSuspicious)
            {
                PlayerDisplay += $"~r~ Police Responding, Description Issued{CurrentDefaultTextColor}";
            }
            else if(DisplayablePlayer.Investigation.IsNearPosition)
            {
                PlayerDisplay += $"~o~ Police Responding{CurrentDefaultTextColor}";
            }
        }
        else if (DisplayablePlayer.HasCriminalHistory)
        {
            if(DisplayablePlayer.HasDeadlyCriminalHistory)
            {
                PlayerDisplay += $"~r~ APB Issued{CurrentDefaultTextColor}";
            }
            else
            {
                PlayerDisplay += $"~o~ BOLO Issued{CurrentDefaultTextColor}";
            }
        }
        if(DisplayablePlayer.IsNotWanted && Settings.SettingsManager.UISettings.PlayerStatusIncludeTime)
        {
            if (PlayerDisplay == "")
            {
                PlayerDisplay = $"{CurrentDefaultTextColor}" + GetTimeDisplay();
            }
            else
            {
                PlayerDisplay += $" {CurrentDefaultTextColor}" + GetTimeDisplay();
            }
        }
        return PlayerDisplay;
    }
    private string GetStreetDisplay()
    {
        string StreetDisplay = "";
        if (DisplayablePlayer.CurrentLocation.CurrentStreet != null)
        {
            StreetDisplay += $" {CurrentDefaultTextColor}";
            if (DisplayablePlayer.CurrentLocation.CurrentStreet.IsHighway)
            {
                StreetDisplay += "~y~";
            }
            StreetDisplay += $" {DisplayablePlayer.CurrentLocation.CurrentStreet.Name}{CurrentDefaultTextColor}";

            if (DisplayablePlayer.CurrentLocation.CurrentCrossStreet != null)
            {
                StreetDisplay += $" at {CurrentDefaultTextColor}{DisplayablePlayer.CurrentLocation.CurrentCrossStreet.Name} {CurrentDefaultTextColor}";
            }
        }
        else if (DisplayablePlayer.CurrentLocation.IsInside)
        {
            if (DisplayablePlayer.CurrentLocation.CurrentInterior?.Name == "")
            {
                #if DEBUG
                    StreetDisplay += $"{CurrentDefaultTextColor} {DisplayablePlayer.CurrentLocation.CurrentInterior?.Name} ({DisplayablePlayer.CurrentLocation.CurrentInterior?.ID}) {CurrentDefaultTextColor}";
                #endif
            }
            else
            {
                StreetDisplay += $"{CurrentDefaultTextColor} {DisplayablePlayer.CurrentLocation.CurrentInterior?.Name}{CurrentDefaultTextColor}";
            }
        }
        return StreetDisplay;
    }
    private string GetZoneDisplay()
    {
        string toDisplay = "";
        if (DisplayablePlayer.CurrentLocation.CurrentZone != null)
        {
            toDisplay = $"{CurrentDefaultTextColor}" + DisplayablePlayer.CurrentLocation.CurrentZone.FullDisplayName;
            if (Settings.SettingsManager.UISettings.ZoneDisplayShowPrimaryAgency && DisplayablePlayer.CurrentLocation.CurrentZone.AssignedLEAgencyInitials != "")
            {
                toDisplay += $"{CurrentDefaultTextColor} / " + DisplayablePlayer.CurrentLocation.CurrentZone.AssignedLEAgencyInitials;
            }
            if (Settings.SettingsManager.UISettings.ZoneDisplayShowSecondaryAgency && DisplayablePlayer.CurrentLocation.CurrentZone.AssignedSecondLEAgencyInitials != string.Empty)
            {
                toDisplay += $"{CurrentDefaultTextColor} - " + DisplayablePlayer.CurrentLocation.CurrentZone.AssignedSecondLEAgencyInitials;
            }
        }
        return toDisplay;
    }
    private void HideVanillaVehicleUI()
    {
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_VEHICLE_NAME);
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_VEHICLE_CLASS);
    }
    private void HideVanillaAreaUI()
    {
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_AREA_NAME);
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_STREET_NAME);
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_VEHICLE_CLASS);
    }
    private void MenuUpdate()
    {
        menuPool.ProcessMenus();
    }
    private void RadarUpdate()
    {
        if(Settings.SettingsManager.UISettings.SetRadarZoomDistance)
        {
            if(DisplayablePlayer.IsWanted)
            {
                NativeFunction.Natives.SET_RADAR_ZOOM_TO_DISTANCE(Settings.SettingsManager.UISettings.RadarZoomDistance_Wanted);
            }
            else if (DisplayablePlayer.Investigation.IsActive)
            {
                NativeFunction.Natives.SET_RADAR_ZOOM_TO_DISTANCE(Settings.SettingsManager.UISettings.RadarZoomDistance_Investigation);
            }
            else
            {
                NativeFunction.Natives.SET_RADAR_ZOOM_TO_DISTANCE(Settings.SettingsManager.UISettings.RadarZoomDistance_Default);
            }
            
        }
    }
    private void ScreenEffectsUpdate()
    {
        if (DisplayablePlayer.IsDead)
        {
            if (!StartedDeathEffect)
            {
                GameTimeLastDied = Game.GameTime;
                
                if (Settings.SettingsManager.UISettings.SetDeathEffect)
                {
                    NativeFunction.Natives.x80C8B1846639BB19(1);
                    NativeFunction.Natives.x2206BF9A37B7F724("DeathFailMPIn", 0, 0);
                }
                if (Settings.SettingsManager.UISettings.PlayWastedSounds)
                {
                    NativeFunction.CallByName<bool>("PLAY_SOUND_FRONTEND", -1, "Bed", "WastedSounds", true);
                }
                if (Settings.SettingsManager.UISettings.DisplayWastedMessage)
                {
                    BigMessage.MessageInstance.ShowColoredShard(Settings.SettingsManager.UISettings.WastedMessageText, "", HudColor.Black, HudColor.RedDark, 2000);
                }
                StartedDeathEffect = true;
            }
            if (GameTimeLastDied != 0 && Game.GameTime - GameTimeLastDied >= 1000)
            {
                GameTimeLastDied = 0;
                Show(DeathMenu);
            }
        }
        else if (DisplayablePlayer.IsBusted)
        {
            if (!StartedBustedEffect)
            {
                GameTimeLastBusted = Game.GameTime;
                if (Settings.SettingsManager.UISettings.SetBustedEffect)
                {
                    NativeFunction.Natives.x80C8B1846639BB19(1);
                    NativeFunction.Natives.x2206BF9A37B7F724("DeathFailMPDark", 0, 0);
                }
                if (Settings.SettingsManager.UISettings.PlayWastedSounds)
                {
                    NativeFunction.CallByName<bool>("PLAY_SOUND_FRONTEND", -1, "TextHit", "WastedSounds", true);
                }
                if (Settings.SettingsManager.UISettings.DisplayBustedMessage)
                {
                    BigMessage.MessageInstance.ShowColoredShard(Settings.SettingsManager.UISettings.BustedMessageText, "", HudColor.Black, HudColor.Blue, 2000);
                }
                StartedBustedEffect = true;
            }
            if (GameTimeLastBusted != 0 && Game.GameTime - GameTimeLastBusted >= 2000)
            {
                GameTimeLastBusted = 0;
                Show(BustedMenu);
            }
        }
        else
        {
            GameTimeLastDied = 0;
            GameTimeLastBusted = 0;
            StartedBustedEffect = false;
            StartedDeathEffect = false;
            if (Settings.SettingsManager.UISettings.AllowScreenEffectReset)
            {
                NativeFunction.Natives.xB4EDDC19532BFB85();
                NativeFunction.Natives.x80C8B1846639BB19(0);
            }
        }
    }
    private void ShowDebugUI()
    {
        //float StartingPoint = 0.1f;
        float StartingPoint = 0.5f;
        DisplayTextOnScreen($"{DisplayablePlayer.DebugLine1}", StartingPoint + 0.01f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{DisplayablePlayer.DebugLine2}", StartingPoint + 0.02f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{DisplayablePlayer.DebugLine3}", StartingPoint + 0.03f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{DisplayablePlayer.DebugLine4}", StartingPoint + 0.04f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{DisplayablePlayer.DebugLine5}", StartingPoint + 0.05f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{DisplayablePlayer.DebugLine6}", StartingPoint + 0.06f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{DisplayablePlayer.DebugLine7}", StartingPoint + 0.07f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{DisplayablePlayer.DebugLine8}", StartingPoint + 0.08f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{DisplayablePlayer.DebugLine9}", StartingPoint + 0.09f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{DisplayablePlayer.DebugLine10}", StartingPoint + 0.10f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{DisplayablePlayer.DebugLine11}", StartingPoint + 0.11f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);

        //DisplayTextOnScreen($"~s~Street.Text: {StreetFader?.TextToShow} Street.Alpha:{StreetFader?.AlphaValue}", StartingPoint + 0.12f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        //DisplayTextOnScreen($"~s~Zone.Text: {ZoneFader?.TextToShow} ~s~Zone.Alpha:{ZoneFader?.AlphaValue}", StartingPoint + 0.13f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        
        
        DisplayTextOnScreen($"PPC: {EntryPoint.PersistentPedsCreated} PPD: {EntryPoint.PersistentPedsDeleted} PPNP: {EntryPoint.PersistentPedsNonPersistent} PVC: {EntryPoint.PersistentVehiclesCreated} PVD: {EntryPoint.PersistentVehiclesDeleted} PVNP: {EntryPoint.PersistentVehiclesNonPersistent}", StartingPoint + 0.12f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
    }
    private void Toggle(Menu toToggle)
    {
        foreach (Menu menu in MenuList)
        {
            if (menu != toToggle)
            {
                menu.Hide();
            }
        }
        toToggle.Toggle();
    }
    private void Show(Menu toShow)
    {
        foreach (Menu menu in MenuList)
        {
            if (menu != toShow)
            {
                menu.Hide();
            }
        }
        toShow.Show();
    }

}