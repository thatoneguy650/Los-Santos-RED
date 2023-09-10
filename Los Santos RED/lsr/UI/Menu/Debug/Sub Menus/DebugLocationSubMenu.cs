using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class DebugLocationSubMenu : DebugSubMenu
{
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private Camera FreeCam;
    private string FreeCamString = "Regular";
    private float FreeCamScale = 1.0f;
    private bool isHidingHelp;
    public DebugLocationSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, IEntityProvideable world, ISettingsProvideable settings) : base(debug, menuPool, player)
    {
        World = world;
        Settings = settings;
    }
    public override void AddItems()
    {
        UIMenu LocationItemsMenu = MenuPool.AddSubMenu(Debug, "Location Logging Menu");
        LocationItemsMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Change various location items.";
        UIMenuItem LogLocationMenu = new UIMenuItem("Log Game Location", "Location Type, Then Name");
        LogLocationMenu.Activated += (menu, item) =>
        {
            LogGameLocation();
            menu.Visible = false;
        };
        UIMenuItem LogSpawnPositionMenu = new UIMenuItem("Log Ped or Vehicle Spawn", "Logs a point for spawning");
        LogSpawnPositionMenu.Activated += (menu, item) =>
        {
            LogSpawnPosition();
            menu.Visible = false;
        };
        UIMenuItem LogLocationSimpleMenu = new UIMenuItem("Log Game Location (Simple)", "Location Type, Then Name");
        LogLocationSimpleMenu.Activated += (menu, item) =>
        {
            LogGameLocationSimple();
            menu.Visible = false;
        };
        UIMenuItem LogInteriorMenu = new UIMenuItem("Log Game Interior", "Interior Name");
        LogInteriorMenu.Activated += (menu, item) =>
        {
            LogGameInterior();
            menu.Visible = false;
        };
        UIMenuItem LogCameraPositionMenu = new UIMenuItem("Log Camera Position", "Logs current rendering cam post direction and rotation");
        LogCameraPositionMenu.Activated += (menu, item) =>
        {
            LogCameraPosition();
            menu.Visible = false;
        };
        UIMenuItem FreeCamMenu = new UIMenuItem("Free Cam", "Start Free Camera Mode");
        FreeCamMenu.Activated += (menu, item) =>
        {
            Frecam();
            menu.Visible = false;
        };
        UIMenuItem LoadSPMap = new UIMenuItem("Load SP Map", "Loads the SP map if you have the MP map enabled");
        LoadSPMap.Activated += (menu, item) =>
        {
            World.LoadSPMap();
            menu.Visible = false;
        };
        UIMenuItem LoadMPMap = new UIMenuItem("Load MP Map", "Load the MP map if you have the SP map enabled");
        LoadMPMap.Activated += (menu, item) =>
        {
            World.LoadMPMap();
            menu.Visible = false;
        };
        UIMenuItem AddAllBlips = new UIMenuItem("Add All Blips", "Add all blips to the map");
        AddAllBlips.Activated += (menu, item) =>
        {
            World.Places.StaticPlaces.AddAllBlips();
            menu.Visible = false;
        };
        UIMenuItem RemoveAllBlips = new UIMenuItem("Remove All Blips", "Remove all blips from the map");
        RemoveAllBlips.Activated += (menu, item) =>
        {
            World.RemoveBlips();
            menu.Visible = false;
        };

        UIMenuItem CheckMapLoaded = new UIMenuItem("Loaded Map", "Display the current loaded map (MP/SP)");
        CheckMapLoaded.Activated += (menu, item) =>
        {
            string iplName = "bkr_bi_hw1_13_int";
            NativeFunction.Natives.REQUEST_IPL(iplName);
            GameFiber.Sleep(500);
            Game.DisplaySubtitle(NativeFunction.Natives.IS_IPL_ACTIVE<bool>(iplName) ? "MP Map" : "SP Map");
            menu.Visible = false;
        };


        UIMenuItem DisableLS = new UIMenuItem("Disable LS", "Disable ALL LS IPLs. Useful for travelling to some far away places. CANNOT UNDO WITHOUT GAME RESTART!");
        DisableLS.Activated += (menu, item) =>
        {
            menu.Visible = false;
            Game.DisplaySubtitle("Disabling LS");
            LSMapDisabler lSMapDisabler = new LSMapDisabler();
            lSMapDisabler.DisableLS();
            GameFiber.Sleep(500);
            Game.DisplaySubtitle("LS Disabled");

        };



        UIMenuItem SetLCSettingAndItemsMenu = new UIMenuItem("Set LC Active", "Disable LS ymaps, scenarios, set the settings, and set a mission flag for LC.");
        SetLCSettingAndItemsMenu.Activated += (menu, item) =>
        {
            menu.Visible = false;
            Game.DisplaySubtitle("Disabling LS");
            LSMapDisabler lSMapDisabler = new LSMapDisabler();
            lSMapDisabler.DisableLS();
            GameFiber.Sleep(500);
            Game.DisplaySubtitle("LS Disabled");
            GameFiber.Sleep(500);
            Settings.SetLC();
            Game.DisplaySubtitle("Set LC Settings");
            NativeFunction.Natives.SET_MISSION_FLAG(true);

        };


        UIMenuItem TurnOffInterior = new UIMenuItem("Turn Interior", "Turn Off Interior by ID");
        TurnOffInterior.Activated += (menu, item) =>
        {
            menu.Visible = false;
            TunOffInterior();

        };

        UIMenuItem TurnOffCayo = new UIMenuItem("Turn Off Cayo", "Turn Off Cayo Perico");
        TurnOffCayo.Activated += (menu, item) =>
        {
            menu.Visible = false;
            NativeFunction.Natives.SET_ISLAND_ENABLED("HeistIsland", false);
            NativeFunction.Natives.SET_USE_ISLAND_MAP(false);
            NativeFunction.Natives.SET_ALLOW_STREAM_HEIST_ISLAND_NODES(false);
            NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Heist_Island_Peds", false);
            NativeFunction.Natives.SET_AMBIENT_ZONE_STATE_PERSISTENT("AZL_DLC_Hei4_Island_Zones", false, false);
            NativeFunction.Natives.SET_AMBIENT_ZONE_STATE_PERSISTENT("AZL_DLC_Hei4_Island_Disabled_Zones", true, false);
            NativeFunction.Natives.SET_ALLOW_STREAM_HEIST_ISLAND_NODES(false);

        };




        LocationItemsMenu.AddItem(LogSpawnPositionMenu);
        LocationItemsMenu.AddItem(LogLocationMenu);
        LocationItemsMenu.AddItem(LogLocationSimpleMenu);
        LocationItemsMenu.AddItem(LogInteriorMenu);
        LocationItemsMenu.AddItem(LogCameraPositionMenu);
        LocationItemsMenu.AddItem(FreeCamMenu);
        LocationItemsMenu.AddItem(CheckMapLoaded);
        LocationItemsMenu.AddItem(LoadSPMap);
        LocationItemsMenu.AddItem(LoadMPMap);
        LocationItemsMenu.AddItem(AddAllBlips);
        LocationItemsMenu.AddItem(RemoveAllBlips);
        LocationItemsMenu.AddItem(DisableLS);
        LocationItemsMenu.AddItem(SetLCSettingAndItemsMenu);
        LocationItemsMenu.AddItem(TurnOffInterior);
        LocationItemsMenu.AddItem(TurnOffCayo);
    }

    private void Frecam()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                FreeCam = new Camera(false);
                FreeCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
                FreeCam.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
                Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
                FreeCam.Rotation = new Rotator(r.X, r.Y, r.Z);
                FreeCam.Active = true;
                Game.LocalPlayer.HasControl = false;
                //This is all adapted from https://github.com/CamxxCore/ScriptCamTool/blob/master/GTAV_ScriptCamTool/PositionSelector.cs#L59
                while (!Game.IsKeyDownRightNow(Keys.Z))
                {
                    if (Game.IsKeyDownRightNow(Keys.W))
                    {
                        FreeCam.Position += NativeHelper.GetCameraDirection(FreeCam, FreeCamScale);
                    }
                    else if (Game.IsKeyDownRightNow(Keys.S))
                    {
                        FreeCam.Position -= NativeHelper.GetCameraDirection(FreeCam, FreeCamScale);
                    }
                    if (Game.IsKeyDownRightNow(Keys.A))
                    {
                        FreeCam.Position = NativeHelper.GetOffsetPosition(FreeCam.Position, FreeCam.Rotation.Yaw, -1.0f * FreeCamScale);
                    }
                    else if (Game.IsKeyDownRightNow(Keys.D))
                    {
                        FreeCam.Position = NativeHelper.GetOffsetPosition(FreeCam.Position, FreeCam.Rotation.Yaw, 1.0f * FreeCamScale);
                    }
                    FreeCam.Rotation += new Rotator(NativeFunction.Natives.GET_CONTROL_NORMAL<float>(2, 221) * -4f, 0, NativeFunction.Natives.GET_CONTROL_NORMAL<float>(2, 220) * -5f) * FreeCamScale;

                    NativeFunction.Natives.SET_FOCUS_POS_AND_VEL(FreeCam.Position.X, FreeCam.Position.Y, FreeCam.Position.Z, 0f, 0f, 0f);

                    if (Game.IsKeyDownRightNow(Keys.O))
                    {
                        FreeCamScale += 0.5f;
                        GameFiber.Sleep(100);
                    }
                    if (Game.IsKeyDownRightNow(Keys.L))
                    {
                        FreeCamScale -= 0.5f;
                        GameFiber.Sleep(100);
                    }

                    if (Game.IsKeyDownRightNow(Keys.J))
                    {
                        Game.LocalPlayer.Character.Position = FreeCam.Position;
                        Game.LocalPlayer.Character.Heading = FreeCam.Heading;
                        GameFiber.Sleep(200);
                    }

                    if (Game.IsKeyDownRightNow(Keys.K))
                    {
                        isHidingHelp = !isHidingHelp;
                        GameFiber.Sleep(200);
                    }

                    //string FreeCamString = FreeCamScale == 1.0f ? "Regular Scale" : "Slow Scale";
                    if (!isHidingHelp)
                    {
                        Game.DisplayHelp($"Press Z to Exit~n~Press O To Increase Scale~n~Press L To Decrease Scale~n~Current Scale: {FreeCamScale}~n~Press J To Move Player to Position~n~Press K to Toggle Controls");
                    }
                    GameFiber.Yield();
                }
                FreeCam.Active = false;
                Game.LocalPlayer.HasControl = true;
                NativeFunction.Natives.CLEAR_FOCUS();
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "Run Debug Logic");
    }
    private void LogGameLocation()
    {
        Vector3 pos = Game.LocalPlayer.Character.Position;
        float Heading = Game.LocalPlayer.Character.Heading;
        string text1 = NativeHelper.GetKeyboardInput("LocationType");
        string text2 = NativeHelper.GetKeyboardInput("Name");
        WriteToLogLocations($"new GameLocation(new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), {Heading}f,new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), {Heading}f, LocationType.{text1}, \"{text2}\", \"{text2}\"),");
    }
    private void LogGameLocationSimple()
    {
        Vector3 pos = Game.LocalPlayer.Character.Position;
        float Heading = Game.LocalPlayer.Character.Heading;
        string text1 = NativeHelper.GetKeyboardInput("LocationType");
        string text2 = NativeHelper.GetKeyboardInput("Name");
        string text3 = NativeHelper.GetKeyboardInput("Description");
        WriteToLogLocations($"new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), {Heading}f, \"{text2}\", \"{text3}\"),  //{text1}");
    }
    private void LogSpawnPosition()
    {
        Vector3 pos = Game.LocalPlayer.Character.Position;
        float Heading = Game.LocalPlayer.Character.Heading;
        WriteToLogLocations($"new ConditionalLocation(new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), {Heading}f, 75f),");
    }
    private void LogCameraPosition()
    {

        if (FreeCam.Active)
        {
            Vector3 pos = FreeCam.Position;
            Rotator r = FreeCam.Rotation;
            Vector3 direction = NativeHelper.GetCameraDirection(FreeCam);
            WriteToLogCameraPosition($", CameraPosition = new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), CameraDirection = new Vector3({direction.X}f, {direction.Y}f, {direction.Z}f), CameraRotation = new Rotator({r.Pitch}f, {r.Roll}f, {r.Yaw}f);");
        }
        else
        {
            uint CameraHAndle = NativeFunction.Natives.GET_RENDERING_CAM<uint>();
            Vector3 pos = NativeFunction.Natives.GET_CAM_COORD<Vector3>(CameraHAndle);
            Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
            Vector3 direction = NativeHelper.GetGameplayCameraDirection();
            WriteToLogCameraPosition($", CameraPosition = new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), CameraDirection = new Vector3({direction.X}f, {direction.Y}f, {direction.Z}f), CameraRotation = new Rotator({r.X}f, {r.Y}f, {r.Z}f);");
        }
    }
    private void LogGameInterior()
    {
        string text1 = NativeHelper.GetKeyboardInput("Name");
        string toWrite = $"new Interior({Player.CurrentLocation?.CurrentInterior?.LocalID}, \"{text1}\"),";
        WriteToLogInteriors(toWrite);
    }
    private void TunOffInterior()
    {
        if (!int.TryParse(NativeHelper.GetKeyboardInput(""), out int interiorID))
        {
            return;
        }
        NativeFunction.Natives.UNPIN_INTERIOR(interiorID);
        NativeFunction.Natives.SET_INTERIOR_ACTIVE(interiorID, false);
        if (NativeFunction.Natives.IS_INTERIOR_CAPPED<bool>(interiorID))
        {
            NativeFunction.Natives.CAP_INTERIOR(interiorID, true);
        }
        NativeFunction.Natives.DISABLE_INTERIOR(interiorID, true);
        NativeFunction.Natives.REFRESH_INTERIOR(interiorID);
        Game.DisplaySubtitle($"Disabled Interior {interiorID}");
    }
    private void WriteToLogCameraPosition(String TextToLog)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(TextToLog + System.Environment.NewLine);
        File.AppendAllText("Plugins\\LosSantosRED\\" + "CameraPositions.txt", sb.ToString());
        sb.Clear();
    }
    private void WriteToLogLocations(String TextToLog)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(TextToLog + System.Environment.NewLine);
        File.AppendAllText("Plugins\\LosSantosRED\\" + "StoredLocations.txt", sb.ToString());
        sb.Clear();
    }
    private void WriteToLogInteriors(String TextToLog)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(TextToLog + System.Environment.NewLine);
        File.AppendAllText("Plugins\\LosSantosRED\\" + "StoredInteriors.txt", sb.ToString());
        sb.Clear();
    }

}

