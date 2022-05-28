using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public class UI : IMenuProvideable
{
    private AboutMenu AboutMenu;
    private BarDisplay BarDisplay;
    private BigMessageThread BigMessage;
    private BustedMenu BustedMenu;
    private string CurrentDefaultTextColor = "~c~";
    private DeathMenu DeathMenu;
    private DebugMenu DebugMenu;
    private IDisplayable DisplayablePlayer;
    private List<ButtonPrompt> displayedButtonPrompts = new List<ButtonPrompt>();
    private bool DrawSpeedLimitTexture = true;
    private uint GameTimeLastBusted;
    private uint GameTimeLastDied;
    private uint GameTimeLastDrawnUI;
    private InstructionalButtons instructional = new InstructionalButtons();
    private bool IsShowingCustomOverlay = false;
    private IJurisdictions Jurisdictions;
    private string lastCrimesDisplay;
    private string lastPlayerDisplay;
    private string lastStreetDisplay;
    private string lastVehicleStatusDisplay;
    private string lastWeaponDisplay;
    private string lastZoneDisplay;
    private MainMenu MainMenu;
    private List<Menu> MenuList;
    private MenuPool menuPool;
    private string overrideTimeDisplay = "";
    private Fader PlayerFader;
    private bool playerIsInVehicle = false;
    private PlayerInfoMenu PlayerInfoMenu;
    private ISettingsProvideable Settings;
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
    private Texture SpeedLimitToDraw;
    private bool StartedBustedEffect = false;
    private bool StartedDeathEffect = false;
    private Fader StreetFader;
    private ITaskerable Tasker;
    private ITimeControllable Time;
    private Fader VehicleFader;
    private Fader WeaponFader;
    private IEntityProvideable World;
    private Fader ZoneFader;
    private SimplePhoneMenu SimplePhoneMenu;
    private string debugString1;
    private bool ShowRadar;


    private uint SpriteUint;
    private uint SpriteUintCounter;
    private uint PreviousGameTime;
    private bool TimeOutSprite;
    private bool IsDisposed = false;
    private uint GameTimeStartedLowHealthUI;
    private WheelMenu WheelMenu;
    private bool IsDrawingWheelMenu;

    private bool ShouldShowSpeedLimitSign => DisplayablePlayer.CurrentVehicle != null && DisplayablePlayer.CurrentLocation.CurrentStreet != null && DisplayablePlayer.IsAliveAndFree;

    public UI(IDisplayable displayablePlayer, ISettingsProvideable settings, IJurisdictions jurisdictions, IPedSwap pedSwap, IPlacesOfInterest placesOfInterest, IRespawning respawning, IActionable actionablePlayer, ISaveable saveablePlayer, IWeapons weapons, RadioStations radioStations, IGameSaves gameSaves, IEntityProvideable world, IRespawnable player, IPoliceRespondable policeRespondable, ITaskerable tasker, IInventoryable playerinventory, IModItems modItems, ITimeControllable time, IGangRelateable gangRelateable, IGangs gangs, IGangTerritories gangTerritories, IZones zones, IStreets streets, IInteriors interiors, Dispatcher dispatcher, IAgencies agencies, ILocationInteractable locationInteractableplayer, IDances dances)
    {
        DisplayablePlayer = displayablePlayer;
        Settings = settings;
        Jurisdictions = jurisdictions;
        Time = time;
        Tasker = tasker;
        World = world;
        BigMessage = new BigMessageThread(true);
        menuPool = new MenuPool();
        DeathMenu = new DeathMenu(menuPool, pedSwap, respawning, placesOfInterest, Settings, player, gameSaves);
        BustedMenu = new BustedMenu(menuPool, pedSwap, respawning, placesOfInterest, Settings, policeRespondable, time);
        MainMenu = new MainMenu(menuPool, locationInteractableplayer, saveablePlayer, gameSaves, weapons, pedSwap, world, Settings, Tasker, playerinventory, modItems, this, gangs, time,placesOfInterest, dances);
        DebugMenu = new DebugMenu(menuPool, actionablePlayer, weapons, radioStations, placesOfInterest, Settings, Time, World, Tasker, dispatcher,agencies, gangs, modItems);
        MenuList = new List<Menu>() { DeathMenu, BustedMenu, MainMenu, DebugMenu };
        StreetFader = new Fader(Settings.SettingsManager.UISettings.StreetDisplayTimeToShow, Settings.SettingsManager.UISettings.StreetDisplayTimeToFade, "StreetFader");
        ZoneFader = new Fader(Settings.SettingsManager.UISettings.ZoneDisplayTimeToShow, Settings.SettingsManager.UISettings.ZoneDisplayTimeToFade, "ZoneFader");
        VehicleFader = new Fader(Settings.SettingsManager.UISettings.VehicleStatusTimeToShow, Settings.SettingsManager.UISettings.VehicleStatusTimeToFade, "VehicleFader");
        PlayerFader = new Fader(Settings.SettingsManager.UISettings.PlayerDisplayTimeToShow, Settings.SettingsManager.UISettings.PlayerDisplayTimeToFade, "PlayerFader");
        WeaponFader = new Fader(Settings.SettingsManager.UISettings.WeaponDisplayTimeToShow, Settings.SettingsManager.UISettings.WeaponDisplayTimeToFade, "WeaponFader");
        PlayerInfoMenu = new PlayerInfoMenu(gangRelateable, Time, placesOfInterest, gangs, gangTerritories, zones, streets, interiors, World);

        SimplePhoneMenu = new SimplePhoneMenu(gangRelateable, Time, placesOfInterest, gangs, gangTerritories, zones, streets, interiors, World);


        AboutMenu = new AboutMenu(gangRelateable, Time, Settings);
        BarDisplay = new BarDisplay(DisplayablePlayer, Settings);
        WheelMenu = new WheelMenu(actionablePlayer, Settings, this);
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
    public void Dispose()
    {
        IsDisposed = true;
        //GameFiber.Sleep(50);
        //Game.RawFrameRender -= DrawSprites;
    }
    public void DrawSprites(object sender, GraphicsEventArgs args)
    {
        try
        {

            if (!TimeOutSprite && DrawSpeedLimitTexture && Game.Resolution != null && !Game.IsPaused && DisplayablePlayer.IsAliveAndFree && !menuPool.IsAnyMenuOpen() && !TabView.IsAnyPauseMenuVisible && !Game.IsScreenFadingOut && !Game.IsScreenFadedOut)
            {
                if (SpeedLimitToDraw != null && SpeedLimitToDraw.Size != null)
                {
                    float ConsistencyScale = (float)Game.Resolution.Width / 2160f;
                    float Scale = Settings.SettingsManager.UISettings.SpeedLimitScale * ConsistencyScale;
                    float posX = (Game.Resolution.Height - (SpeedLimitToDraw.Size.Height * Scale)) * Settings.SettingsManager.UISettings.SpeedLimitPositionX;
                    float posY = (Game.Resolution.Width - (SpeedLimitToDraw.Size.Width * Scale)) * Settings.SettingsManager.UISettings.SpeedLimitPositionY;
                    args.Graphics.DrawTexture(SpeedLimitToDraw, new RectangleF(posY, posX, SpeedLimitToDraw.Size.Width * Scale, SpeedLimitToDraw.Size.Height * Scale));
                }
            }


            //works but crashes the game on unload:(
            //if (PreviousGameTime == Game.GameTime)
            //{
            //    SpriteUintCounter++;
            //}
            //else if (Game.GameTime > PreviousGameTime)
            //{
            //    SpriteUintCounter = 0;
            //}





            //if (SpriteUintCounter >= 10)
            //{
            //    TimeOutSprite = true;
            //}
            //else
            //{
            //    TimeOutSprite = false;
            //}


            //PreviousGameTime = Game.GameTime;

        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"UI: Draw ERROR {ex.Message} {ex.StackTrace} ", 0);
        }
    }
    public void DrawText()
    {
        GameTimeLastDrawnUI = Game.GameTime;
        if (Settings.SettingsManager.UISettings.UIEnabled && DisplayablePlayer.IsAliveAndFree)
        {
            if (Settings.SettingsManager.UISettings.ShowCrimesDisplay)
            {
                DisplayTextOnScreen(lastCrimesDisplay, Settings.SettingsManager.UISettings.CrimesViolatingPositionX, Settings.SettingsManager.UISettings.CrimesViolatingPositionY, Settings.SettingsManager.UISettings.CrimesViolatingScale, Color.White, Settings.SettingsManager.UISettings.CrimesViolatingFont, (GTATextJustification)Settings.SettingsManager.UISettings.CrimesViolatingJustificationID);
            }
            if (Settings.SettingsManager.UISettings.ShowVehicleStatusDisplay)
            {
                DisplayTextOnScreen(lastVehicleStatusDisplay, Settings.SettingsManager.UISettings.VehicleStatusPositionX, Settings.SettingsManager.UISettings.VehicleStatusPositionY, Settings.SettingsManager.UISettings.VehicleStatusScale, Color.White, Settings.SettingsManager.UISettings.VehicleStatusFont, (GTATextJustification)Settings.SettingsManager.UISettings.VehicleStatusJustificationID);
            }
            if (Settings.SettingsManager.UISettings.ShowWeaponDisplay)
            {
                WeaponFader.Update(lastWeaponDisplay);
                if (Settings.SettingsManager.UISettings.FadeWeaponDisplay && ((!DisplayablePlayer.IsWanted && !DisplayablePlayer.Investigation.IsActive) || Settings.SettingsManager.UISettings.FadeWeaponDisplayDuringWantedAndInvestigation))
                {
                    DisplayTextOnScreen(WeaponFader.TextToShow, Settings.SettingsManager.UISettings.WeaponDisplayPositionX, Settings.SettingsManager.UISettings.WeaponDisplayPositionY, Settings.SettingsManager.UISettings.WeaponDisplayScale, Color.White, Settings.SettingsManager.UISettings.WeaponDisplayFont, (GTATextJustification)Settings.SettingsManager.UISettings.WeaponDisplayJustificationID, WeaponFader.AlphaValue);
                }
                else
                {
                    DisplayTextOnScreen(lastWeaponDisplay, Settings.SettingsManager.UISettings.WeaponDisplayPositionX, Settings.SettingsManager.UISettings.WeaponDisplayPositionY, Settings.SettingsManager.UISettings.WeaponDisplayScale, Color.White, Settings.SettingsManager.UISettings.WeaponDisplayFont, (GTATextJustification)Settings.SettingsManager.UISettings.WeaponDisplayJustificationID);
                }
            }
            if (Settings.SettingsManager.UISettings.ShowPlayerDisplay)
            {
                PlayerFader.Update(lastPlayerDisplay);
                if (Settings.SettingsManager.UISettings.FadePlayerDisplay && ((!DisplayablePlayer.IsWanted && !DisplayablePlayer.Investigation.IsActive) || Settings.SettingsManager.UISettings.FadePlayerDisplayDuringWantedAndInvestigation))
                {
                    DisplayTextOnScreen(PlayerFader.TextToShow, Settings.SettingsManager.UISettings.PlayerStatusPositionX, Settings.SettingsManager.UISettings.PlayerStatusPositionY, Settings.SettingsManager.UISettings.PlayerStatusScale, Color.White, Settings.SettingsManager.UISettings.PlayerStatusFont, (GTATextJustification)Settings.SettingsManager.UISettings.PlayerStatusJustificationID, PlayerFader.AlphaValue);
                }
                else
                {
                    DisplayTextOnScreen(lastPlayerDisplay, Settings.SettingsManager.UISettings.PlayerStatusPositionX, Settings.SettingsManager.UISettings.PlayerStatusPositionY, Settings.SettingsManager.UISettings.PlayerStatusScale, Color.White, Settings.SettingsManager.UISettings.PlayerStatusFont, (GTATextJustification)Settings.SettingsManager.UISettings.PlayerStatusJustificationID);
                }
            }
            if (Settings.SettingsManager.UISettings.ShowStreetDisplay)
            {
                StreetFader.Update(lastStreetDisplay);
                if (Settings.SettingsManager.UISettings.FadeStreetDisplay && ((!DisplayablePlayer.IsWanted && !DisplayablePlayer.Investigation.IsActive) || Settings.SettingsManager.UISettings.FadeStreetDisplayDuringWantedAndInvestigation))
                {
                    DisplayTextOnScreen(StreetFader.TextToShow, Settings.SettingsManager.UISettings.StreetPositionX, Settings.SettingsManager.UISettings.StreetPositionY, Settings.SettingsManager.UISettings.StreetScale, Color.White, Settings.SettingsManager.UISettings.StreetFont, (GTATextJustification)Settings.SettingsManager.UISettings.StreetJustificationID, StreetFader.AlphaValue);
                }
                else
                {
                    DisplayTextOnScreen(lastStreetDisplay, Settings.SettingsManager.UISettings.StreetPositionX, Settings.SettingsManager.UISettings.StreetPositionY, Settings.SettingsManager.UISettings.StreetScale, Color.White, Settings.SettingsManager.UISettings.StreetFont, (GTATextJustification)Settings.SettingsManager.UISettings.StreetJustificationID);
                }
            }
            if (Settings.SettingsManager.UISettings.ShowZoneDisplay)
            {
                ZoneFader.Update(lastZoneDisplay);
                if (Settings.SettingsManager.UISettings.FadeZoneDisplay && ((!DisplayablePlayer.IsWanted && !DisplayablePlayer.Investigation.IsActive) || Settings.SettingsManager.UISettings.FadeZoneDisplayDuringWantedAndInvestigation))
                {
                    DisplayTextOnScreen(ZoneFader.TextToShow, Settings.SettingsManager.UISettings.ZonePositionX, Settings.SettingsManager.UISettings.ZonePositionY, Settings.SettingsManager.UISettings.ZoneScale, Color.White, Settings.SettingsManager.UISettings.ZoneFont, (GTATextJustification)Settings.SettingsManager.UISettings.ZoneJustificationID, ZoneFader.AlphaValue);
                }
                else
                {
                    DisplayTextOnScreen(lastZoneDisplay, Settings.SettingsManager.UISettings.ZonePositionX, Settings.SettingsManager.UISettings.ZonePositionY, Settings.SettingsManager.UISettings.ZoneScale, Color.White, Settings.SettingsManager.UISettings.ZoneFont, (GTATextJustification)Settings.SettingsManager.UISettings.ZoneJustificationID);
                }
            }
            if (IsDrawingWheelMenu)
            {
                WheelMenu.Draw();
            }
        }
    }
    public void PrimaryTick()
    {
        if (!menuPool.IsAnyMenuOpen() && !TabView.IsAnyPauseMenuVisible)
        {
            DrawText();




            if (ShowRadar || DisplayablePlayer.Sprinting.StaminaPercentage < 1.0f || DisplayablePlayer.IntoxicatedIntensityPercent > 0.0f)
            {
                BarDisplay.Draw(DisplayablePlayer.Sprinting.StaminaPercentage, DisplayablePlayer.IntoxicatedIntensityPercent, 0.0f);
            }
            DrawDebug();
        }
        MenuUpdate();
    }
    public void SecondaryTick()
    {
        DisplayButtonPrompts();
        ForceVanillaUI();
    }
    public void Setup()
    {
        //Game.RawFrameRender += DrawSprites;
        IsDisposed = false;
        //Sign10 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\10mph.png");
        //Sign15 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\15mph.png");
        //Sign20 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\20mph.png");
        //Sign25 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\25mph.png");
        //Sign30 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\30mph.png");
        //Sign35 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\35mph.png");
        //Sign40 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\40mph.png");
        //Sign45 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\45mph.png");
        //Sign50 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\50mph.png");
        //Sign55 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\55mph.png");
        //Sign60 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\60mph.png");
        //Sign65 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\65mph.png");
        //Sign70 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\70mph.png");
        //Sign75 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\75mph.png");
        //Sign80 = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\80mph.png");
        PlayerInfoMenu.Setup();
        SimplePhoneMenu.Setup();
        AboutMenu.Setup();
    }
    public void TertiaryTick()
    {
        GameFiber.Yield();
        ScreenEffectsUpdate();
        GameFiber.Yield();
        RadarUpdate();
        GameFiber.Yield();
    }
    public void ToggleAboutMenu()
    {
        AboutMenu.Toggle();
    }
    public void ToggleDebugMenu()
    {
        Toggle(DebugMenu);
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
    public void ToggleReportingMenu()
    {
        PlayerInfoMenu.Toggle();
    }

    public void ToggleSimplePhoneMenu()
    {
        SimplePhoneMenu.Toggle();
        //if (!menuPool.IsAnyMenuOpen())
        //{
            
        //}
    }

    public void Update()
    {
        if (DisplayablePlayer.IsAliveAndFree)
        {
            if (Time.IsNight && Settings.SettingsManager.UISettings.GreyOutWhiteFontAtNight)
            {
                CurrentDefaultTextColor = "~c~";
            }
            else
            {
                CurrentDefaultTextColor = "~s~";
            }
            if (Settings.SettingsManager.UISettings.ShowCrimesDisplay)
            {
                lastCrimesDisplay = GetViolationsText();
            }
            //if (Settings.SettingsManager.UISettings.ShowSpeedLimitDisplay)
            //{
            //    GetSpeedLimitSign();
            //}
            if (Settings.SettingsManager.UISettings.ShowVehicleStatusDisplay)
            {
                lastVehicleStatusDisplay = GetVehicleStatusDisplay();
            }
            if (Settings.SettingsManager.UISettings.ShowPlayerDisplay)
            {
                lastPlayerDisplay = GetPlayerDisplay();
            }
            if (Settings.SettingsManager.UISettings.ShowWeaponDisplay)
            {
                lastWeaponDisplay = GetWeaponDisplay();
            }
            if (playerIsInVehicle != DisplayablePlayer.IsInVehicle)
            {
                if (Settings.SettingsManager.UISettings.FadeZoneDisplay)
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
                lastStreetDisplay = GetStreetDisplay();
                if (Settings.SettingsManager.UISettings.FadeStreetDisplay && ((!DisplayablePlayer.IsWanted && !DisplayablePlayer.Investigation.IsActive) || Settings.SettingsManager.UISettings.FadeStreetDisplayDuringWantedAndInvestigation))
                {
                    //StreetFader.Update(lastStreetDisplay);
                    if (Settings.SettingsManager.UISettings.FadeZoneDisplay && ZoneFader.TextChangedLastUpdate)
                    {
                        StreetFader.UpdateTimeChanged();
                    }
                }
            }
            if (Settings.SettingsManager.UISettings.ShowZoneDisplay)
            {
                lastZoneDisplay = GetZoneDisplay();
                if (Settings.SettingsManager.UISettings.FadeZoneDisplay && ((!DisplayablePlayer.IsWanted && !DisplayablePlayer.Investigation.IsActive) || Settings.SettingsManager.UISettings.FadeZoneDisplayDuringWantedAndInvestigation))
                {
                    // ZoneFader.Update(lastZoneDisplay);
                    if (Settings.SettingsManager.UISettings.FadeStreetDisplay && StreetFader.TextChangedLastUpdate)
                    {
                        ZoneFader.UpdateTimeChanged();
                    }
                }
            }


            debugString1 = $"Heading: {Math.Round(DisplayablePlayer.Character.Heading,1)}";
            DisplayablePlayer.DebugLine4 = debugString1;
        }
    }
    private void DisplayButtonPrompts()
    {
        if (Settings.SettingsManager.UISettings.DisplayButtonPrompts)
        {
            instructional.Buttons.Clear();

            if (DisplayablePlayer.ButtonPromptList.Any())
            {
                foreach (ButtonPrompt buttonPrompt in DisplayablePlayer.ButtonPromptList.OrderByDescending(x => x.Order))
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
                    else if (buttonPrompt.Modifier != Keys.None)
                    {
                        instructional.Buttons.Add(new InstructionalButtonGroup(buttonPrompt.Text, buttonPrompt.Modifier.GetInstructionalKey()));
                    }
                    //else
                    //{
                    //    if (buttonPrompt.Modifier != Keys.None)
                    //    {
                    //        instructional.Buttons.Add(new InstructionalButtonGroup(buttonPrompt.Text, buttonPrompt.Modifier.GetInstructionalKey(), InstructionalKey.SymbolPlus, InstructionalKey.MouseLeft));
                    //    }
                    //    else
                    //    {
                    //        instructional.Buttons.Add(new InstructionalButtonGroup(buttonPrompt.Text, InstructionalKey.MouseLeft));
                    //    }
                    //}
                }
            }
            instructional.Update();
            if (DisplayablePlayer.ButtonPromptList.Any())
            {
                instructional.Draw();
            }
        }
    }
    private void DisplayTextOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification, int alpha)
    {
        try
        {
            if (TextToShow == "" || alpha == 0)
            {
                return;
            }
            NativeFunction.Natives.SET_TEXT_FONT((int)Font);
            NativeFunction.Natives.SET_TEXT_SCALE(Scale, Scale);
            NativeFunction.Natives.SET_TEXT_COLOUR((int)TextColor.R, (int)TextColor.G, (int)TextColor.B, alpha);

            NativeFunction.Natives.SetTextJustification((int)Justification);
            NativeFunction.Natives.SetTextDropshadow(10, 255, 0, 255, 255);//NativeFunction.Natives.SetTextDropshadow(2, 2, 0, 0, 0);

            if (Justification == GTATextJustification.Right)
            {
                NativeFunction.Natives.SET_TEXT_WRAP(0f, Y);
            }
            else
            {
                NativeFunction.Natives.SET_TEXT_WRAP(0f, 1f);
            }
            NativeFunction.Natives.x25fbb336df1804cb("STRING"); //NativeFunction.Natives.x25fbb336df1804cb("STRING");
            NativeFunction.Natives.x25FBB336DF1804CB(TextToShow);
            NativeFunction.Natives.x6C188BE134E074AA(TextToShow);
            NativeFunction.Natives.xCD015E5BB0D96A57(Y, X);
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"UI ERROR {ex.Message} {ex.StackTrace}", 0);
        }
        //return;
    }
    private void DisplayTextOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification)
    {
        DisplayTextOnScreen(TextToShow, X, Y, Scale, TextColor, Font, Justification, 255);
    }
    private void DrawDebug()
    {
        if (Settings.SettingsManager.UISettings.ShowDebug)
        {
            //float StartingPoint = 0.1f;
            float StartingPoint = 0.5f;
            // DisplayTextOnScreen($"{DisplayablePlayer.DebugLine1}", StartingPoint + 0.01f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            //DisplayTextOnScreen($"{DisplayablePlayer.DebugLine2}", StartingPoint + 0.02f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            //DisplayTextOnScreen($"{DisplayablePlayer.DebugLine3}", StartingPoint + 0.03f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            DisplayTextOnScreen($"{DisplayablePlayer.DebugLine4}", StartingPoint + 0.04f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            DisplayTextOnScreen($"{DisplayablePlayer.DebugLine5}", StartingPoint + 0.05f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            DisplayTextOnScreen($"{DisplayablePlayer.DebugLine6}", StartingPoint + 0.06f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            //DisplayTextOnScreen($"{DisplayablePlayer.DebugLine7}", StartingPoint + 0.07f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            //DisplayTextOnScreen($"{DisplayablePlayer.DebugLine8}", StartingPoint + 0.08f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            //DisplayTextOnScreen($"{DisplayablePlayer.DebugLine9}", StartingPoint + 0.09f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            //DisplayTextOnScreen(World.DebugString, StartingPoint + 0.10f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
            //DisplayTextOnScreen(Tasker.TaskerDebug, StartingPoint + 0.12f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        }
    }
    private void ForceVanillaUI()
    {
        if (Settings.SettingsManager.UISettings.AlwaysShowHUD)
        {
            NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);
        }
        //if (Settings.SettingsManager.UISettings.AlwaysShowRadar)
        //{
        //    NativeFunction.CallByName<bool>("DISPLAY_RADAR", true);
        //}
        //else if (Settings.SettingsManager.UISettings.NeverShowRadar)
        //{
        //    NativeFunction.CallByName<bool>("DISPLAY_RADAR", false);
        //}
        //else if (DisplayablePlayer.IsInVehicle)
        //{
        //    if (Settings.SettingsManager.UISettings.ShowRadarInVehicleOnly)
        //    {
        //        if (DisplayablePlayer.IsInVehicle)
        //        {
        //            NativeFunction.CallByName<bool>("DISPLAY_RADAR", true);
        //        }
        //        else
        //        {
        //            NativeFunction.CallByName<bool>("DISPLAY_RADAR", false);
        //        }
        //    }
        //}
        //else if (!DisplayablePlayer.IsInVehicle)
        //{
        //    if (Settings.SettingsManager.UISettings.ShowRadarOnFootWhenCellPhoneActiveOnly)
        //    {
        //        if (DisplayablePlayer.CellPhone.IsActive)
        //        {
        //            NativeFunction.CallByName<bool>("DISPLAY_RADAR", true);
        //        }
        //        else
        //        {
        //            NativeFunction.CallByName<bool>("DISPLAY_RADAR", false);
        //        }
        //    }
        //    else if (Settings.SettingsManager.UISettings.ShowRadarInVehicleOnly)
        //    {
        //        NativeFunction.CallByName<bool>("DISPLAY_RADAR", false);
        //    }
        //}




        if (Settings.SettingsManager.UISettings.AlwaysShowRadar)
        {
            ShowRadar = true;
        }
        else if (Settings.SettingsManager.UISettings.NeverShowRadar)
        {
            ShowRadar = false;
        }
        else if (DisplayablePlayer.IsInVehicle)
        {
            if (Settings.SettingsManager.UISettings.ShowRadarInVehicleOnly)
            {
                if (DisplayablePlayer.IsInVehicle)
                {
                    ShowRadar = true;
                }
                else
                {
                    ShowRadar = false;
                }
            }
        }
        else if (!DisplayablePlayer.IsInVehicle)
        {
            if (Settings.SettingsManager.UISettings.ShowRadarOnFootWhenCellPhoneActiveOnly)
            {
                if (DisplayablePlayer.CellPhone.IsActive)
                {
                    ShowRadar = true;
                }
                else
                {
                    ShowRadar = false;
                }
            }
            else if (Settings.SettingsManager.UISettings.ShowRadarInVehicleOnly)
            {
                ShowRadar = false;
            }
        }

        NativeFunction.CallByName<bool>("DISPLAY_RADAR", ShowRadar);


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
        else if (DisplayablePlayer.IsTransacting || DisplayablePlayer.RecentlyChangedMoney || DisplayablePlayer.IsBusted)
        {
            NativeFunction.CallByName<bool>("DISPLAY_CASH", true);
        }
        if (!Settings.SettingsManager.UISettings.ShowVanillaAreaUI)
        {
            NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_AREA_NAME);
            NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_STREET_NAME);
            NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_VEHICLE_CLASS);
        }
        if (!Settings.SettingsManager.UISettings.ShowVanillaVehicleUI)
        {
            NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_VEHICLE_NAME);
            NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_VEHICLE_CLASS);
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
                else
                {
                    PlayerDisplay += $"~r~ Active Pursuit";
                }
            }
            PlayerDisplay += $" ({DisplayablePlayer.PoliceResponse.CurrentRespondingPoliceCount}) {CurrentDefaultTextColor}";
        }
        else if (DisplayablePlayer.Investigation != null && DisplayablePlayer.Investigation.IsActive)
        {
            if (DisplayablePlayer.Investigation.RequiresPolice)
            {
                if (DisplayablePlayer.Investigation.IsSuspicious)
                {
                    PlayerDisplay += $"~r~ Police Responding with Description";
                    PlayerDisplay += $" ({DisplayablePlayer.Investigation.CurrentRespondingPoliceCount}){CurrentDefaultTextColor}";
                }
                else if (DisplayablePlayer.Investigation.IsNearPosition)
                {
                    PlayerDisplay += $"~o~ Police Responding";
                    PlayerDisplay += $" ({DisplayablePlayer.Investigation.CurrentRespondingPoliceCount}){CurrentDefaultTextColor}";
                }
                
            }
            else if (DisplayablePlayer.Investigation.RequiresEMS || DisplayablePlayer.Investigation.RequiresFirefighters)
            {
                PlayerDisplay += $"~o~ Emergency Services Responding{CurrentDefaultTextColor}";
            }
        }
        else if (DisplayablePlayer.HasCriminalHistory)
        {
            if (DisplayablePlayer.HasDeadlyCriminalHistory)
            {
                PlayerDisplay += $"~r~ APB Issued{CurrentDefaultTextColor}";
            }
            else
            {
                PlayerDisplay += $"~o~ BOLO Issued{CurrentDefaultTextColor}";
            }
        }
        if (DisplayablePlayer.IsNotWanted && Settings.SettingsManager.UISettings.PlayerStatusIncludeTime)
        {
            if (PlayerDisplay == "")
            {
                PlayerDisplay = $"{CurrentDefaultTextColor}" + GetFormattedTime();
            }
            else
            {
                PlayerDisplay += $" {CurrentDefaultTextColor}" + GetFormattedTime();
            }
        }
        return PlayerDisplay;
    }
    private string GetSelectorText()
    {
        if (DisplayablePlayer.CurrentWeapon != null && DisplayablePlayer.CurrentWeapon.Category != WeaponCategory.Melee && DisplayablePlayer.CurrentWeapon.Category != WeaponCategory.Throwable)
        {
            if (Settings.SettingsManager.UISettings.WeaponDisplaySimpleSelector)
            {
                if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.Safe)
                {
                    return "~w~S~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.SemiAuto)
                {
                    return "~r~1~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.TwoRoundBurst)
                {
                    return "~r~2~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.ThreeRoundBurst)
                {
                    return "~r~3~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.FourRoundBurst)
                {
                    return "~r~4~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.FiveRoundBurst)
                {
                    return "~r~5~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.FullAuto)
                {
                    if (DisplayablePlayer.CurrentWeaponMagazineSize == 0)
                    {
                        return $"~r~FULL AUTO~s~";
                    }
                    else
                    {
                        return $"~r~{DisplayablePlayer.CurrentWeaponMagazineSize}~s~";
                    }
                }
                else
                {
                    return "";
                }
            }
            else
            {
                if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.Safe)
                {
                    return "~s~SAFE~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.SemiAuto)
                {
                    return "~r~Semi-Auto~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.TwoRoundBurst)
                {
                    return "~r~2 Round Burst~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.ThreeRoundBurst)
                {
                    return "~r~3 Round Burst~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.FourRoundBurst)
                {
                    return "~r~4 Round Burst~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.FiveRoundBurst)
                {
                    return "~r~5 Round Burst~s~";
                }
                else if (DisplayablePlayer.CurrentSelectorSetting == SelectorOptions.FullAuto)
                {
                    if (DisplayablePlayer.CurrentWeaponMagazineSize == 0)
                    {
                        return $"~r~FULL AUTO~s~";
                    }
                    else
                    {
                        return $"~r~FULL AUTO ~s~(~r~{DisplayablePlayer.CurrentWeaponMagazineSize}~s~)~s~";
                    }
                }
                else
                {
                    return "";
                }
            }
        }
        else
        {
            return "";
        }
    }
    private void GetSpeedLimitSign()
    {
        if (ShouldShowSpeedLimitSign)
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
                SpeedLimitToDraw = Sign10;
            }
            else if (speedLimit <= 15f)
            {
                SpeedLimitToDraw = Sign15;
            }
            else if (speedLimit <= 20f)
            {
                SpeedLimitToDraw = Sign20;
            }
            else if (speedLimit <= 25f)
            {
                SpeedLimitToDraw = Sign25;
            }
            else if (speedLimit <= 30f)
            {
                SpeedLimitToDraw = Sign30;
            }
            else if (speedLimit <= 35f)
            {
                SpeedLimitToDraw = Sign35;
            }
            else if (speedLimit <= 40f)
            {
                SpeedLimitToDraw = Sign40;
            }
            else if (speedLimit <= 45f)
            {
                SpeedLimitToDraw = Sign45;
            }
            else if (speedLimit <= 50f)
            {
                SpeedLimitToDraw = Sign50;
            }
            else if (speedLimit <= 55f)
            {
                SpeedLimitToDraw = Sign55;
            }
            else if (speedLimit <= 60f)
            {
                SpeedLimitToDraw = Sign60;
            }
            else if (speedLimit <= 65f)
            {
                SpeedLimitToDraw = Sign65;
            }
            else if (speedLimit <= 70f)
            {
                SpeedLimitToDraw = Sign70;
            }
            else if (speedLimit <= 75f)
            {
                SpeedLimitToDraw = Sign75;
            }
            else if (speedLimit <= 80f)
            {
                SpeedLimitToDraw = Sign80;
            }
            else
            {
                SpeedLimitToDraw = null;
            }
        }
        else
        {
            SpeedLimitToDraw = null;
        }
        if (SpeedLimitToDraw != null)
        {
            DrawSpeedLimitTexture = true;
        }
        else
        {
            DrawSpeedLimitTexture = false;
        }
    }
    private string GetStreetDisplay()
    {
        string StreetDisplay = "";
        if (1==0)//DisplayablePlayer.CurrentShop != null)//add back in some capacity?
        {
            //StreetDisplay += $"{CurrentDefaultTextColor}Inside ~p~{DisplayablePlayer.CurrentShop.Name}{CurrentDefaultTextColor}";
        }
        else
        {
            if (DisplayablePlayer.CurrentLocation.CurrentStreet != null)
            {

                string StreetNumber = NativeHelper.CellToStreetNumber(DisplayablePlayer.CellX, DisplayablePlayer.CellY);
                StreetDisplay = $"{CurrentDefaultTextColor}";
                StreetDisplay += $"{StreetNumber}";
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
        }
        return StreetDisplay;
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
                            if (Settings.SettingsManager.UISettings.VehicleStatusIncludeCurrentSpeed)
                            {
                                CurrentSpeedDisplay = $"{ColorPrefx}{Math.Round(DisplayablePlayer.VehicleSpeedMPH, MidpointRounding.AwayFromZero)} ({Math.Round(DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitMPH, MidpointRounding.AwayFromZero)} {CurrentDefaultTextColor}MPH)";
                            }
                            else
                            {
                                CurrentSpeedDisplay = $"({Math.Round(DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitMPH, MidpointRounding.AwayFromZero)} {CurrentDefaultTextColor}MPH)";
                            }
                        }
                        else
                        {
                            if (Settings.SettingsManager.UISettings.VehicleStatusIncludeCurrentSpeed)
                            {
                                CurrentSpeedDisplay = $"{ColorPrefx}{Math.Round(DisplayablePlayer.VehicleSpeedMPH, MidpointRounding.AwayFromZero)} {CurrentDefaultTextColor}MPH";
                            }
                            else
                            {
                                CurrentSpeedDisplay = "";
                            }
                        }
                    }
                    else if (Settings.SettingsManager.UISettings.SpeedDisplayUnits == "KM/H")
                    {
                        if (Settings.SettingsManager.UISettings.VehicleStatusIncludeTextSpeedLimit)
                        {
                            if (Settings.SettingsManager.UISettings.VehicleStatusIncludeCurrentSpeed)
                            {
                                CurrentSpeedDisplay = $"{ColorPrefx}{Math.Round(DisplayablePlayer.VehicleSpeedKMH, MidpointRounding.AwayFromZero)} ({Math.Round(DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitKMH, MidpointRounding.AwayFromZero)} {CurrentDefaultTextColor}KM/H)";
                            }
                            else
                            {
                                CurrentSpeedDisplay = $"({Math.Round(DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitKMH, MidpointRounding.AwayFromZero)} {CurrentDefaultTextColor}KM/H)";
                            }
                        }
                        else
                        {
                            if (Settings.SettingsManager.UISettings.VehicleStatusIncludeCurrentSpeed)
                            {
                                CurrentSpeedDisplay = $"{ColorPrefx}{Math.Round(DisplayablePlayer.VehicleSpeedKMH, MidpointRounding.AwayFromZero)} {CurrentDefaultTextColor}KM/H";
                            }
                            else
                            {
                                CurrentSpeedDisplay = "";
                            }
                        }
                    }
                }
            }
            if (DisplayablePlayer.Violations.IsViolatingAnyTrafficLaws)
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
            if (Settings.SettingsManager.VehicleSettings.UseCustomFuelSystem)
            {
                CurrentSpeedDisplay += $"{CurrentDefaultTextColor} " + DisplayablePlayer.CurrentVehicle.FuelTank.UIText;
            }
            if (Settings.SettingsManager.UISettings.VehicleStatusIncludeCompass)
            {
                string Heading = NativeHelper.GetSimpleCompassHeading(DisplayablePlayer.Character.Heading);
                CurrentSpeedDisplay += $" {Heading}";
            }
        }
        return CurrentSpeedDisplay;
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
    private string GetWeaponDisplay()
    {
        string WeaponDisplay = "";
        if (Settings.SettingsManager.UISettings.ShowWeaponDisplay)
        {
            if (WeaponDisplay == "")
            {
                WeaponDisplay = $"{CurrentDefaultTextColor}" + GetSelectorText();
            }
            else
            {
                WeaponDisplay += $" {CurrentDefaultTextColor}" + GetSelectorText();
            }
        }
        return WeaponDisplay;
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
            if (Settings.SettingsManager.UISettings.ZoneDisplayShowPrimaryGang && DisplayablePlayer.CurrentLocation.CurrentZone.AssignedGangInitials != string.Empty)
            {
                toDisplay += $"{CurrentDefaultTextColor} - " + DisplayablePlayer.CurrentLocation.CurrentZone.AssignedGangInitials;
            }
            else if (Settings.SettingsManager.UISettings.ZoneDisplayShowSecondaryAgency && DisplayablePlayer.CurrentLocation.CurrentZone.AssignedSecondLEAgencyInitials != string.Empty)
            {
                toDisplay += $"{CurrentDefaultTextColor} - " + DisplayablePlayer.CurrentLocation.CurrentZone.AssignedSecondLEAgencyInitials;
            }
        }
        return toDisplay;
    }
    private void MenuUpdate()
    {
        menuPool.ProcessMenus();
        PlayerInfoMenu.Update();
        SimplePhoneMenu.Update();
        AboutMenu.Update();
    }
    private void RadarUpdate()
    {
        if (Settings.SettingsManager.UISettings.SetRadarZoomDistance)
        {
            if (DisplayablePlayer.IsWanted)
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
                    IsShowingCustomOverlay = true;
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
            if (GameTimeLastDied != 0 && Game.GameTime - GameTimeLastDied >= (Settings.SettingsManager.PlayerOtherSettings.SetSlowMoOnDeath ? 1000 : 2000))
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
                    IsShowingCustomOverlay = true;
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
            if (GameTimeLastBusted != 0 && Game.GameTime - GameTimeLastBusted >= (Settings.SettingsManager.PlayerOtherSettings.SetSlowMoOnBusted ? 1000 : 2000))
            {
                GameTimeLastBusted = 0;
                Show(BustedMenu);
            }
        }
        //else if (DisplayablePlayer.Character.Health <= 130)
        //{
        //    if(Game.GameTime - GameTimeStartedLowHealthUI >= 5000)
        //    {
        //        NativeFunction.Natives.x80C8B1846639BB19(1);
        //        NativeFunction.Natives.x2206BF9A37B7F724("DeathFailMPDark", 0, 0);
        //        IsShowingCustomOverlay = true;
        //        GameTimeStartedLowHealthUI = Game.GameTime;
        //    }
        //}
        else
        {
            GameTimeLastDied = 0;
            GameTimeLastBusted = 0;
            StartedBustedEffect = false;
            StartedDeathEffect = false;
            if (Settings.SettingsManager.UISettings.AllowScreenEffectReset && IsShowingCustomOverlay)
            {
                NativeFunction.Natives.xB4EDDC19532BFB85();
                NativeFunction.Natives.x80C8B1846639BB19(0);
                IsShowingCustomOverlay = false;
            }
        }
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

    //public void DrawWheelMenu()
    //{
    //    if (WheelMenu.RecentlyRanItem)
    //    {
    //        IsDrawingWheelMenu = false;
    //    }
    //    else if(WheelMenu.HasRanItem)
    //    {
    //        IsDrawingWheelMenu = false;
    //    }
    //    else
    //    { 
    //        if (!IsDrawingWheelMenu)
    //        {
    //            Game.TimeScale = 0.2f;



    //        }

    //        IsDrawingWheelMenu = true;
    //    }
    //}

    public void DisposeWheelMenu()
    {
        if (IsDrawingWheelMenu)
        {
            Game.TimeScale = 1.0f;
            WheelMenu.Dispose();
        }
        IsDrawingWheelMenu = false;
    }

    public void UpdateWheelMenu(bool isPressingActionWheelMenu)
    {
        if(isPressingActionWheelMenu)
        {
            if(WheelMenu.HasRanItem)
            {
                IsDrawingWheelMenu = false;
            }
            else
            {
                if (!IsDrawingWheelMenu)
                {
                    Game.TimeScale = 0.2f;
                }
                IsDrawingWheelMenu = true;
            }

        }
        else
        {
            if (IsDrawingWheelMenu)
            {
                Game.TimeScale = 1.0f;
                WheelMenu.Dispose();
            }
            IsDrawingWheelMenu = false;
            WheelMenu.Reset();
        }
    }
}