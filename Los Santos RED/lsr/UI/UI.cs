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

public class UI
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
    private IDisplayable Player;
    private Mod.Player PlayerIntellisense;
    private ISettingsProvideable Settings;
    private bool StartedBandagingEffect = false;
    private bool StartedBustedEffect = false;
    private bool StartedDeathEffect = false;
    private IZoneJurisdictions ZoneJurisdictions;
    private uint GameTimeLastBusted;
    private uint GameTimeLastDied;
    public UI(IDisplayable currentPlayer, ISettingsProvideable settings, IZoneJurisdictions zoneJurisdictions, IPedswappable pedSwap, IPlacesOfInterest placesOfInterest, IRespawning respawning, IActionable player, IWeapons weapons, RadioStations radioStations)
    {
        Player = currentPlayer;
        Settings = settings;
        ZoneJurisdictions = zoneJurisdictions;
        BigMessage = new BigMessageThread(true);
        menuPool = new MenuPool();
        DeathMenu = new DeathMenu(menuPool, pedSwap, respawning, placesOfInterest);
        BustedMenu = new BustedMenu(menuPool, pedSwap, respawning, placesOfInterest);
        MainMenu = new MainMenu(menuPool, pedSwap, player);
        DebugMenu = new DebugMenu(menuPool, player, weapons, radioStations);
        MenuList = new List<Menu>() { DeathMenu, BustedMenu, MainMenu, DebugMenu };
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
    public void Update()
    {
        ForceVanillaUI();
        DrawUI();
        ScreenEffectsUpdate();
        DisplayButtonPrompts();
        MenuUpdate();
    }
    private void DisplayButtonPrompts()
    {
        //if (displayedButtonPrompts != Player.ButtonPrompts)
        ////if (displayedButtonPrompts.Count() != Player.ButtonPrompts.Count())
        //{
        instructional.Buttons.Clear();
        if (Player.ButtonPrompts.Any())
        {
            foreach (ButtonPrompt buttonPrompt in Player.ButtonPrompts.OrderByDescending(x => x.Order))
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
        //    Game.Console.Print($"Button Prompts Changed {Player.ButtonPrompts.Count}");
        //    displayedButtonPrompts.Clear();
        //    displayedButtonPrompts.AddRange(Player.ButtonPrompts);
        //}
        if (Player.ButtonPrompts.Any())
        {
            instructional.Draw();
        }
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
    private void DrawUI()
    {
        if (Settings.SettingsManager.UI.Enabled && Player.IsAliveAndFree)
        {
            ShowDebugUI();
            HideVanillaUI();
            DisplayTextOnScreen(Player.CurrentSpeedDisplay, Settings.SettingsManager.UI.VehicleStatusPositionX, Settings.SettingsManager.UI.VehicleStatusPositionY, Settings.SettingsManager.UI.VehicleStatusScale, Color.White, GTAFont.FontChaletComprimeCologne, (GTATextJustification)Settings.SettingsManager.UI.VehicleStatusJustificationID);
            DisplayTextOnScreen(GetZoneDisplay(), Settings.SettingsManager.UI.ZonePositionX, Settings.SettingsManager.UI.ZonePositionY, Settings.SettingsManager.UI.ZoneScale, Color.White, GTAFont.FontHouseScript, (GTATextJustification)Settings.SettingsManager.UI.ZoneJustificationID);
            DisplayTextOnScreen(GetStreetDisplay(), Settings.SettingsManager.UI.StreetPositionX, Settings.SettingsManager.UI.StreetPositionY, Settings.SettingsManager.UI.StreetScale, Color.White, GTAFont.FontHouseScript, (GTATextJustification)Settings.SettingsManager.UI.StreetJustificationID);
        }
    }
    private void ForceVanillaUI()
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
    }
    private string GetStreetDisplay()
    {
        string StreetDisplay = "";
        if (Player.CurrentLocation.CurrentStreet != null)
        {
            if (Player.CurrentLocation.CurrentStreet.IsHighway)
            {
                StreetDisplay += "~y~";
            }
            StreetDisplay += $" {Player.CurrentLocation.CurrentStreet.Name}~s~";
        }
        if (Player.CurrentLocation.CurrentCrossStreet != null)
        {
            StreetDisplay += $" at {Player.CurrentLocation.CurrentCrossStreet.Name} ~s~";
        }
        return StreetDisplay;
    }
    private string GetZoneDisplay()
    {
        if (Player.CurrentLocation.CurrentZone == null)
        {
            return "";
        }
        return Player.CurrentLocation.CurrentZone.FullDisplayName + " ~s~- " + Player.CurrentLocation.CurrentZone.AssignedAgencyInitials;
    }
    private void HideVanillaUI()
    {
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_VEHICLE_NAME);
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_AREA_NAME);
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_VEHICLE_CLASS);
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_STREET_NAME);
        NativeFunction.CallByName<bool>("HIDE_HUD_COMPONENT_THIS_FRAME", (int)GTAHudComponent.HUD_VEHICLE_CLASS);
    }
    private void MenuUpdate()
    {
        if (Game.IsKeyDown(Keys.F10))
        {
            if (Player.IsDead)
            {
                Toggle(DeathMenu);
            }
            else if (Player.IsBusted)
            {
                Toggle(BustedMenu);
            }
            else
            {
                Toggle(MainMenu);
            }
        }
        else if (Game.IsKeyDown(Keys.F11))
        {
            Toggle(DebugMenu);
        }
        menuPool.ProcessMenus();
    }
    private void ScreenEffectsUpdate()
    {
        if (Player.IsDead)
        {
            if (!StartedDeathEffect)
            {
                GameTimeLastDied = Game.GameTime;
                NativeFunction.Natives.x80C8B1846639BB19(1);
                NativeFunction.Natives.x2206BF9A37B7F724("DeathFailMPIn", 0, 0);
                NativeFunction.CallByName<bool>("PLAY_SOUND_FRONTEND", -1, "Bed", "WastedSounds", true);
                BigMessage.MessageInstance.ShowColoredShard("WASTED", "", HudColor.Black, HudColor.RedDark, 2000);
                StartedDeathEffect = true;
            }
            if (GameTimeLastDied != 0 && Game.GameTime - GameTimeLastDied >= 1000)
            {
                GameTimeLastDied = 0;
                Show(DeathMenu);
            }
        }
        else if (Player.IsBusted)
        {
            if (!StartedBustedEffect)
            {
                GameTimeLastBusted = Game.GameTime;
                NativeFunction.Natives.x80C8B1846639BB19(1);
                NativeFunction.Natives.x2206BF9A37B7F724("DeathFailMPDark", 0, 0);
                NativeFunction.CallByName<bool>("PLAY_SOUND_FRONTEND", -1, "TextHit", "WastedSounds", true);
                BigMessage.MessageInstance.ShowColoredShard("BUSTED", "", HudColor.Black, HudColor.Blue, 2000);
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
            NativeFunction.Natives.xB4EDDC19532BFB85();
            NativeFunction.Natives.x80C8B1846639BB19(0);
        }
    }
    private void ShowDebugUI()
    {
        float StartingPoint = 0.1f;
        DisplayTextOnScreen($"{Player.DebugLine1}", StartingPoint + 0.01f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{Player.DebugLine2}", StartingPoint + 0.02f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{Player.DebugLine3}", StartingPoint + 0.03f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{Player.DebugLine4}", StartingPoint + 0.04f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{Player.DebugLine5}", StartingPoint + 0.05f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{Player.DebugLine6}", StartingPoint + 0.06f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{Player.DebugLine7}", StartingPoint + 0.07f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{Player.DebugLine8}", StartingPoint + 0.08f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{Player.DebugLine9}", StartingPoint + 0.09f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
        DisplayTextOnScreen($"{Player.DebugLine10}", StartingPoint + 0.10f, 0f, 0.2f, Color.White, GTAFont.FontChaletComprimeCologne, GTATextJustification.Left);
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