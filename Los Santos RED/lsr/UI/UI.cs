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
    private uint GameTimeLastDisplayedBleedingHelp;
    private InstructionalButtons instructional = new InstructionalButtons();
    private MainMenu MainMenu;
    private List<Menu> MenuList;
    private MenuPool menuPool;
    private IDisplayable DisplayablePlayer;
    private Mod.Player PlayerIntellisense;
    private ISettingsProvideable Settings;
    private bool StartedBandagingEffect = false;
    private bool StartedBustedEffect = false;
    private bool StartedDeathEffect = false;
    private IJurisdictions Jurisdictions;
    private uint GameTimeLastBusted;
    private uint GameTimeLastDied;
    private string currentVehicleStatusDisplay = "";
    private string currentStreetDisplay = "";
    private string currentZoneDisplay = "";
    private uint GameTimeVehicleStatusDisplayChanged = 0;
    private uint GameTimeStreetDisplayChanged = 0;
    private uint GameTimeZoneDisplayChanged = 0;
    private int zoneDisplayAlpha;
    private int streetDisplayAlpha;
    private bool DrawTexture = true;
    private Texture ToDraw;
    private Texture Sign10MPH;
    private Texture Sign15MPH;
    private Texture Sign20MPH;
    private Texture Sign25MPH;
    private Texture Sign30MPH;
    private Texture Sign35MPH;
    private Texture Sign40MPH;
    private Texture Sign45MPH;
    private Texture Sign50MPH;
    private Texture Sign55MPH;
    private Texture Sign60MPH;
    private Texture Sign65MPH;
    private Texture Sign70MPH;
    private Texture Sign75MPH;
    private Texture Sign80MPH;
    public UI(IDisplayable displayablePlayer, ISettingsProvideable settings, IJurisdictions jurisdictions, IPedSwap pedSwap, IPlacesOfInterest placesOfInterest, IRespawning respawning, IActionable actionablePlayer, ISaveable saveablePlayer, IWeapons weapons, RadioStations radioStations, IGameSaves gameSaves, IEntityProvideable world, IRespawnable player, IPoliceRespondable policeRespondable, ITaskerable tasker, IConsumableSubstances consumableSubstances, IInventoryable playerinventory)
    {
        DisplayablePlayer = displayablePlayer;
        Settings = settings;
        Jurisdictions = jurisdictions;
        BigMessage = new BigMessageThread(true);
        menuPool = new MenuPool();
        DeathMenu = new DeathMenu(menuPool, pedSwap, respawning, placesOfInterest, Settings, player, gameSaves);
        BustedMenu = new BustedMenu(menuPool, pedSwap, respawning, placesOfInterest,Settings, policeRespondable);
        MainMenu = new MainMenu(menuPool, actionablePlayer, saveablePlayer, gameSaves, weapons, pedSwap, world, Settings,tasker, consumableSubstances, playerinventory);
        DebugMenu = new DebugMenu(menuPool, actionablePlayer, weapons, radioStations);
        MenuList = new List<Menu>() { DeathMenu, BustedMenu, MainMenu, DebugMenu };

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
        Sign10MPH = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\10mph.png");
        Sign15MPH = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\15mph.png");
        Sign20MPH = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\20mph.png");
        Sign25MPH = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\25mph.png");
        Sign30MPH = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\30mph.png");
        Sign35MPH = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\35mph.png");
        Sign40MPH = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\40mph.png");
        Sign45MPH = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\45mph.png");
        Sign50MPH = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\50mph.png");
        Sign55MPH = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\55mph.png");
        Sign60MPH = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\60mph.png");
        Sign65MPH = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\65mph.png");
        Sign70MPH = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\70mph.png");
        Sign75MPH = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\75mph.png");
        Sign80MPH = Game.CreateTextureFromFile("Plugins\\LosSantosRED\\images\\80mph.png");
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
        if(DrawTexture && !Game.IsPaused)
        {
            if (ToDraw != null)
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
                if(newVehicleStatus != currentVehicleStatusDisplay)
                {
                    currentVehicleStatusDisplay = newVehicleStatus;
                    GameTimeVehicleStatusDisplayChanged = Game.GameTime;
                }
                int alpha = 255;
                if(Settings.SettingsManager.UISettings.FadeVehicleStatusDisplay)
                {
                    alpha = CalculateAlpha(GameTimeVehicleStatusDisplayChanged, Settings.SettingsManager.UISettings.VehicleStatusTimeToShow, Settings.SettingsManager.UISettings.VehicleStatusTimeToFade);          
                }
                DisplayTextOnScreen(currentVehicleStatusDisplay, Settings.SettingsManager.UISettings.VehicleStatusPositionX, Settings.SettingsManager.UISettings.VehicleStatusPositionY, Settings.SettingsManager.UISettings.VehicleStatusScale, Color.White, Settings.SettingsManager.UISettings.VehicleStatusFont, (GTATextJustification)Settings.SettingsManager.UISettings.VehicleStatusJustificationID, alpha);
            }
            if (Settings.SettingsManager.UISettings.ShowPlayerDisplay)
            {
                DisplayTextOnScreen(GetPlayerDisplay(), Settings.SettingsManager.UISettings.PlayerStatusPositionX, Settings.SettingsManager.UISettings.PlayerStatusPositionY, Settings.SettingsManager.UISettings.PlayerStatusScale, Color.White, Settings.SettingsManager.UISettings.PlayerStatusFont, (GTATextJustification)Settings.SettingsManager.UISettings.PlayerStatusJustificationID);
            }
            if (Settings.SettingsManager.UISettings.ShowStreetDisplay)
            {
                string newStreetDisplay = GetStreetDisplay();
                if (newStreetDisplay != currentStreetDisplay)
                {
                    //EntryPoint.WriteToConsole($"GameTimeStreetDisplayChanged WAS '{currentStreetDisplay}' BECAME '{newStreetDisplay}' ", 5);
                    currentStreetDisplay = newStreetDisplay;
                    GameTimeStreetDisplayChanged = Game.GameTime;
                    
                }
                streetDisplayAlpha = 255;
                if (Settings.SettingsManager.UISettings.FadeStreetDisplay)
                {
                    streetDisplayAlpha = CalculateAlpha(GameTimeStreetDisplayChanged, Settings.SettingsManager.UISettings.StreetDisplayTimeToShow, Settings.SettingsManager.UISettings.StreetDisplayTimeToFade);
                }
                DisplayTextOnScreen(currentStreetDisplay, Settings.SettingsManager.UISettings.StreetPositionX, Settings.SettingsManager.UISettings.StreetPositionY, Settings.SettingsManager.UISettings.StreetScale, Color.White, Settings.SettingsManager.UISettings.StreetFont, (GTATextJustification)Settings.SettingsManager.UISettings.StreetJustificationID, streetDisplayAlpha);
            }
            if (Settings.SettingsManager.UISettings.ShowZoneDisplay)
            {
                string newZoneDisplay = GetZoneDisplay();
                if (newZoneDisplay != currentZoneDisplay)
                {
                   // EntryPoint.WriteToConsole($"GameTimeZoneDisplayChanged WAS '{currentZoneDisplay}' BECAME '{newZoneDisplay}' ", 5);
                    currentZoneDisplay = newZoneDisplay;
                    GameTimeZoneDisplayChanged = Game.GameTime;
                }
                zoneDisplayAlpha = 255;
                if (Settings.SettingsManager.UISettings.FadeZoneDisplay)
                {
                    zoneDisplayAlpha = CalculateAlpha(GameTimeZoneDisplayChanged, Settings.SettingsManager.UISettings.ZoneDisplayTimeToShow, Settings.SettingsManager.UISettings.ZoneDisplayTimeToFade);
                    if (Settings.SettingsManager.UISettings.FadeStreetDisplay && streetDisplayAlpha != 0)
                    {
                        currentZoneDisplay = newZoneDisplay;
                        zoneDisplayAlpha = streetDisplayAlpha;
                    }  
                }
                DisplayTextOnScreen(currentZoneDisplay, Settings.SettingsManager.UISettings.ZonePositionX, Settings.SettingsManager.UISettings.ZonePositionY, Settings.SettingsManager.UISettings.ZoneScale, Color.White, Settings.SettingsManager.UISettings.ZoneFont, (GTATextJustification)Settings.SettingsManager.UISettings.ZoneJustificationID, zoneDisplayAlpha);
            }
        }
    }

    private void DisplaySpeedLimitSign()
    {
        if (DisplayablePlayer.CurrentVehicle != null && DisplayablePlayer.CurrentLocation.CurrentStreet != null)
        {
            if (Settings.SettingsManager.UISettings.SpeedDisplayUnits == "MPH")
            {
                float speedLimitMPH = DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitMPH;
                if (speedLimitMPH <= 10f)
                {
                    ToDraw = Sign10MPH;
                }
                else if (speedLimitMPH <= 15f)
                {
                    ToDraw = Sign15MPH;
                }
                else if (speedLimitMPH <= 20f)
                {
                    ToDraw = Sign20MPH;
                }
                else if (speedLimitMPH <= 25f)
                {
                    ToDraw = Sign25MPH;
                }
                else if (speedLimitMPH <= 30f)
                {
                    ToDraw = Sign30MPH;
                }
                else if (speedLimitMPH <= 35f)
                {
                    ToDraw = Sign35MPH;
                }
                else if (speedLimitMPH <= 40f)
                {
                    ToDraw = Sign40MPH;
                }
                else if (speedLimitMPH <= 45f)
                {
                    ToDraw = Sign45MPH;
                }
                else if (speedLimitMPH <= 50f)
                {
                    ToDraw = Sign50MPH;
                }
                else if (speedLimitMPH <= 55f)
                {
                    ToDraw = Sign55MPH;
                }
                else if (speedLimitMPH <= 60f)
                {
                    ToDraw = Sign60MPH;
                }
                else if (speedLimitMPH <= 65f)
                {
                    ToDraw = Sign65MPH;
                }
                else if (speedLimitMPH <= 70f)
                {
                    ToDraw = Sign70MPH;
                }
                else if (speedLimitMPH <= 75f)
                {
                    ToDraw = Sign75MPH;
                }
                else if (speedLimitMPH <= 80f)
                {
                    ToDraw = Sign80MPH;
                }
                else
                {
                    ToDraw = null;
                }
            }
            else if (Settings.SettingsManager.UISettings.SpeedDisplayUnits == "KM/H")
            {
                ToDraw = null;
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
        if(ToDraw != null)
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
            CurrentSpeedDisplay = "";
            if (DisplayablePlayer.CurrentVehicle.IsCar && !DisplayablePlayer.CurrentVehicle.Engine.IsRunning)
            {
                CurrentSpeedDisplay = "ENGINE OFF";
            }
            else
            {
                string ColorPrefx = "~s~";
                if (DisplayablePlayer.IsSpeeding)
                {
                    ColorPrefx = "~r~";
                }
                if (DisplayablePlayer.CurrentLocation.CurrentStreet != null)
                {
                    if (Settings.SettingsManager.UISettings.SpeedDisplayUnits == "MPH")
                    {
                        CurrentSpeedDisplay = $"{ColorPrefx}{Math.Round(DisplayablePlayer.VehicleSpeedMPH, MidpointRounding.AwayFromZero)} ~s~MPH";// ({Math.Round(DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitMPH, MidpointRounding.AwayFromZero)} ~s~MPH)";
                    }
                    else if(Settings.SettingsManager.UISettings.SpeedDisplayUnits == "KM/H")
                    {
                        CurrentSpeedDisplay = $"{ColorPrefx}{Math.Round(DisplayablePlayer.VehicleSpeedKMH, MidpointRounding.AwayFromZero)} ~s~KM/H";// ({Math.Round(DisplayablePlayer.CurrentLocation.CurrentStreet.SpeedLimitKMH, MidpointRounding.AwayFromZero)} ~s~KM/H)";
                    }
                }
            }
            //if (DisplayablePlayer.IsViolatingAnyTrafficLaws)
            //{
            //    CurrentSpeedDisplay += " !";
            //}
            if (Settings.SettingsManager.PlayerSettings.UseCustomFuelSystem)
            {
                CurrentSpeedDisplay += "~n~" + DisplayablePlayer.CurrentVehicle.FuelTank.UIText;
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
                PlayerDisplay += $"~o~ Attempting To Locate~s~";
            }
            else
            {
                if (DisplayablePlayer.PoliceResponse.IsWeaponsFree)
                {
                    PlayerDisplay += $"~r~ Active Pursuit (Weapons Free)~s~";
                }
                else if (DisplayablePlayer.PoliceResponse.IsDeadlyChase)
                {
                    PlayerDisplay += $"~r~ Active Pursuit (Lethal Force Authorized)~s~";
                }
                else
                {
                    PlayerDisplay += $"~r~ Active Pursuit~s~";
                }
            }
        }
        else if (DisplayablePlayer.Investigation != null && DisplayablePlayer.Investigation.IsActive)
        {   
            if(DisplayablePlayer.Investigation.IsSuspicious)
            {
                PlayerDisplay += $"~r~ Police Responding, Description Issued~s~";
            }
            else if(DisplayablePlayer.Investigation.IsNearPosition)
            {
                PlayerDisplay += $"~o~ Police Responding~s~";
            }
        }
        else if (DisplayablePlayer.HasCriminalHistory)
        {
            if(DisplayablePlayer.HasDeadlyCriminalHistory)
            {
                PlayerDisplay += $"~r~ APB Issued~s~";
            }
            else
            {
                PlayerDisplay += $"~o~ BOLO Issued~s~";
            }
            
        }
        return PlayerDisplay;
    }
    private string GetStreetDisplay()
    {
        string StreetDisplay = "";
        if (DisplayablePlayer.CurrentLocation.CurrentStreet != null)
        {
            if (DisplayablePlayer.CurrentLocation.CurrentStreet.IsHighway)
            {
                StreetDisplay += "~y~";
            }
            StreetDisplay += $" {DisplayablePlayer.CurrentLocation.CurrentStreet.Name}~s~";
        }
        if (DisplayablePlayer.CurrentLocation.CurrentCrossStreet != null)
        {
            StreetDisplay += $" at {DisplayablePlayer.CurrentLocation.CurrentCrossStreet.Name} ~s~";
        }

        if (DisplayablePlayer.CurrentLocation.IsInside)
        {
            if (DisplayablePlayer.CurrentLocation.CurrentInterior?.Name == "")
            {
#if DEBUG
                                StreetDisplay += $" {DisplayablePlayer.CurrentLocation.CurrentInterior?.Name} ({DisplayablePlayer.CurrentLocation.CurrentInterior?.ID}) ~s~";
#endif
            }
            else
            {
                StreetDisplay += $" {DisplayablePlayer.CurrentLocation.CurrentInterior?.Name}~s~";
            }
        }
        return StreetDisplay;
    }
    private string GetZoneDisplay()
    {
        if (DisplayablePlayer.CurrentLocation.CurrentZone == null)
        {
            return "";
        }
        return DisplayablePlayer.CurrentLocation.CurrentZone.FullDisplayName + " ~s~- " + DisplayablePlayer.CurrentLocation.CurrentZone.AssignedLEAgencyInitials;
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
        float StartingPoint = 0.1f;
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