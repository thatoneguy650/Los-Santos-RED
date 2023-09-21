using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using static Debug;

public class DebugLocationSubMenu : DebugSubMenu
{
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private IPlacesOfInterest PlacesOfInterest;
    private Camera FreeCam;
    private IStreets Streets;
    private string FreeCamString = "Regular";
    private float FreeCamScale = 1.0f;
    private bool isHidingHelp;
    private Street CurrentStreet;
    private Street CurrentCrossStreet;
    private uint GameTimeLastUpdatedNodes;

    public DebugLocationSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, IEntityProvideable world, ISettingsProvideable settings, IStreets streets, IPlacesOfInterest placesOfInterest) : base(debug, menuPool, player)
    {
        World = world;
        Settings = settings;
        Streets = streets;
        PlacesOfInterest = placesOfInterest;
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


        UIMenuItem DrawStreetTextMenu = new UIMenuItem("Draw Street Text", "Draw Street Text");
        DrawStreetTextMenu.Activated += (menu, item) =>
        {
            menu.Visible = false;
            DrawStreetText();
        };
        LocationItemsMenu.AddItem(DrawStreetTextMenu);


        UIMenuItem OpenCloseDoors = new UIMenuItem("Open Doors", "Force open all nearby doors");
        OpenCloseDoors.Activated += (menu, item) =>
        {
            menu.Visible = false;
            foreach(GameLocation gameLocation in World.Places.ActiveLocations.ToList())
            {
                if (gameLocation.DistanceToPlayer <= 100f)
                {
                    gameLocation.Interior?.OpenDoors();
                }
            }
        };
        LocationItemsMenu.AddItem(OpenCloseDoors);


        UIMenuItem EnableScenarios = new UIMenuItem("Enable Scenarios", "Enable some scenarios groups");
        EnableScenarios.Activated += (menu, item) =>
        {
            menu.Visible = false;
            NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("City_Banks", true);
            NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Countryside_Banks", true);
            NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("AMMUNATION", true);
            NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("YellowJackInn", true);
            NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("VANGELICO", true);
        };
        LocationItemsMenu.AddItem(EnableScenarios);



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
                    if (Game.IsKeyDownRightNow(Keys.S))
                    {
                        FreeCam.Position -= NativeHelper.GetCameraDirection(FreeCam, FreeCamScale);
                    }
                    if (Game.IsKeyDownRightNow(Keys.A))
                    {
                        FreeCam.Position = NativeHelper.GetOffsetPosition(FreeCam.Position, FreeCam.Rotation.Yaw, -1.0f * FreeCamScale);
                    }
                    if (Game.IsKeyDownRightNow(Keys.D))
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



    private void DrawStreetText()
    {

        StreetNamePopUp streetNamePopUp = new StreetNamePopUp(Settings, Streets);

        GameFiber.StartNew(delegate
        {
            try
            {
                while (!Game.IsKeyDownRightNow(Keys.Z))
                {
                    Game.DisplayHelp("PRESS Z TO STOP");
                    if(Game.GameTime - GameTimeLastUpdatedNodes >= Settings.SettingsManager.DebugSettings.StreetDisplayTimeBetweenUpdate)
                    {
                        streetNamePopUp.GetNodes();
                        GameTimeLastUpdatedNodes = Game.GameTime;
                    }
                    GameFiber.Yield();
                }
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "Run Debug Logic");


        GameFiber.StartNew(delegate
        {
            try
            {
                int globalScaleformID = NativeFunction.Natives.REQUEST_SCALEFORM_MOVIE<int>("ORGANISATION_NAME");
                while (!NativeFunction.Natives.HAS_SCALEFORM_MOVIE_LOADED<bool>(globalScaleformID))
                {
                    GameFiber.Yield();
                }
                while (!Game.IsKeyDownRightNow(Keys.Z))
                {
                    streetNamePopUp.DisplayNodes(globalScaleformID);
                    GameFiber.Yield();
                }
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "Run Debug Logic");





    }

    //private void DrawStreetText()
    //{
    //    GameFiber.StartNew(delegate
    //    {
    //        try
    //        {

    //            int globalScaleformID = NativeFunction.Natives.REQUEST_SCALEFORM_MOVIE<int>("ORGANISATION_NAME");
    //            while (!NativeFunction.Natives.HAS_SCALEFORM_MOVIE_LOADED<bool>(globalScaleformID))
    //            {
    //                GameFiber.Yield();
    //            }



    //            while (!Game.IsKeyDownRightNow(Keys.Z))
    //            {
    //                Game.DisplayHelp("PRESS Z TO STOP");
    //                StreetTextLoop(globalScaleformID);

    //                GameFiber.Yield();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
    //            EntryPoint.ModController.CrashUnload();
    //        }
    //    }, "Run Debug Logic");


    //}
    private void StreetTextLoop(int globalScaleformID)
    {
        if (Player.CurrentLocation.CurrentStreet == null)
        {
            return;
        }
        Vector3 position = Game.LocalPlayer.Character.GetOffsetPositionFront(Settings.SettingsManager.DebugSettings.StreetDisplayNodeOffsetFront);
        Vector3 outPos;
        float outHeading;
        //bool hasNode = NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(position.X, position.Y, position.Z, out outPos, out outHeading, 0, 3.0f, 0f);


        bool hasNode = NativeFunction.Natives.GET_NTH_CLOSEST_VEHICLE_NODE_FAVOUR_DIRECTION<bool>(position.X, position.Y, position.Z, Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z
    , 0, out outPos, out outHeading, 0, 0x40400000, 0);



        if (!hasNode || outPos == null || outPos == Vector3.Zero)
        {
            return;
        }
        Vector3 drawPos = outPos;
        drawPos += new Vector3(Settings.SettingsManager.DebugSettings.StreetDisplayOffsetX, Settings.SettingsManager.DebugSettings.StreetDisplayOffsetY, Settings.SettingsManager.DebugSettings.StreetDisplayOffsetZ);

        int density = 0;
        int flags = 0;
        NativeFunction.Natives.GET_VEHICLE_NODE_PROPERTIES<bool>(drawPos.X, drawPos.Y, drawPos.Z, out density, out flags);
        eVehicleNodeProperties nodeProperties = (eVehicleNodeProperties)flags;


        if(!nodeProperties.HasFlag(eVehicleNodeProperties.VNP_ON_PLAYERS_ROAD))
        {
            return;
        }

        GetStreetsAtPos(drawPos);

        string textToDisplay = "";
        if (CurrentStreet != null)
        {
            textToDisplay += CurrentStreet.ProperStreetName;
        }
        if (CurrentCrossStreet != null)
        {
            textToDisplay += " - " + CurrentCrossStreet.ProperStreetName;
        }

        if (flags != 0)
        {
            textToDisplay += " - " + nodeProperties;
        }




        NativeFunction.Natives.BEGIN_SCALEFORM_MOVIE_METHOD(globalScaleformID, "SET_ORGANISATION_NAME");

        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SCALEFORM_STRING("STRING");
        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(textToDisplay);
        NativeFunction.Natives.END_TEXT_COMMAND_SCALEFORM_STRING();

        NativeFunction.Natives.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT(Settings.SettingsManager.DebugSettings.StreetDisplayStyleIndex);
        NativeFunction.Natives.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT(Settings.SettingsManager.DebugSettings.StreetDisplayColorIndex);
        NativeFunction.Natives.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT(Settings.SettingsManager.DebugSettings.StreetDisplayFontIndex);


        NativeFunction.Natives.END_SCALEFORM_MOVIE_METHOD();

        Vector3 rotationVector = new Vector3(Settings.SettingsManager.DebugSettings.StreetDisplayRotationX, Settings.SettingsManager.DebugSettings.StreetDisplayRotationY, Settings.SettingsManager.DebugSettings.StreetDisplayRotationZ);
        float realYaw = outHeading;
        //if (realYaw >= 180f)
        //{
            realYaw = 360 - realYaw;
            realYaw = realYaw - 180;
        //}


        //if(realYaw < 0)
        //{
        //    realYaw *= -1.0f;
        //}

        //realYaw = realYaw;
        //if (realYaw >= 180f)
        //{
        //    realYaw = realYaw - 180f;
        //}
        //else
        //{
        //    realYaw = realYaw + 180f;
        //}




        if (Settings.SettingsManager.DebugSettings.StreetDisplayUseCalc)
        {



            rotationVector = new Vector3(0f, 0f, realYaw);
        }

        Render3D(globalScaleformID,
            drawPos,
             rotationVector,
             Settings.SettingsManager.DebugSettings.StreetDisplayScaleX * new Vector3(1.0f,1.0f,1.0f));


        string toDisplay = textToDisplay;
        toDisplay += $" Node Heading:{outHeading}";
        toDisplay += $" PlayerRot:{Game.LocalPlayer.Character.Rotation.Yaw} realYaw:{realYaw}";

        Game.DisplaySubtitle(toDisplay);
    }



    public void Render3D(int handle, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        NativeFunction.Natives.DRAW_SCALEFORM_MOVIE_3D_SOLID(handle, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 2.0f, 2.0f, 1.0f, scale.X, scale.Y, scale.Z, 2);
    }
    private void GetStreetsAtPos(Vector3 ClosestNode)
    {
        int StreetHash = 0;
        int CrossingHash = 0;
        string CurrentStreetName;
        string CurrentCrossStreetName;
        unsafe
        {
            NativeFunction.CallByName<uint>("GET_STREET_NAME_AT_COORD", ClosestNode.X, ClosestNode.Y, ClosestNode.Z, &StreetHash, &CrossingHash);
        }
        string StreetName = string.Empty;
        if (StreetHash != 0)
        {
            unsafe
            {
                IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", StreetHash);
                StreetName = Marshal.PtrToStringAnsi(ptr);
            }
            CurrentStreetName = StreetName;
            //GameFiber.Yield();
        }
        else
        {
            CurrentStreetName = "";
        }

        string CrossStreetName = string.Empty;
        if (CrossingHash != 0)
        {
            unsafe
            {
                IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", CrossingHash);
                CrossStreetName = Marshal.PtrToStringAnsi(ptr);
            }
            CurrentCrossStreetName = CrossStreetName;
            //GameFiber.Yield();
        }
        else
        {
            CurrentCrossStreetName = "";
        }

        CurrentStreet = Streets.GetStreet(CurrentStreetName);
        CurrentCrossStreet = Streets.GetStreet(CurrentCrossStreetName);
    }

    private void StreetTextLoop_OLD()
    {
        if (Player.CurrentLocation.CurrentStreet == null)
        {
            return;
        }
        Vector3 drawPos = Player.CurrentLocation.ClosestRoadNode;
        if (drawPos == null || drawPos == Vector3.Zero)
        {
            return;
        }
        drawPos += new Vector3(0f, 0f, 5f);
        string textToDisplay = Player.CurrentLocation.CurrentStreet.ProperStreetName;
        NativeFunction.Natives.SET_DRAW_ORIGIN(drawPos.X, drawPos.Y, drawPos.Z, 0);
        NativeHelper.DisplayTextOnScreen(textToDisplay, 0f, 0f, Settings.SettingsManager.LSRHUDSettings.StreetScale, System.Drawing.Color.White, Settings.SettingsManager.LSRHUDSettings.StreetFont,
            (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.StreetJustificationID, false, 255);
        NativeFunction.Natives.CLEAR_DRAW_ORIGIN();

        Game.DisplaySubtitle(textToDisplay);

        Rage.Debug.DrawArrowDebug(drawPos, Vector3.Zero, Rotator.Zero, 1f, System.Drawing.Color.Black);


    }



    private void DisplayTextOnScreen2(string TextToShow, float X, float Y, float Scale, System.Drawing.Color TextColor, GTAFont Font, GTATextJustification Justification, bool outline, int alpha)
    {
        try
        {
            if (TextToShow == "" || alpha == 0 || TextToShow is null)
            {
                return;
            }
            NativeFunction.Natives.SET_TEXT_FONT((int)Font);

            NativeFunction.Natives.SET_TEXT_PROPORTIONAL(0);
            NativeFunction.Natives.SET_TEXT_SCALE(0f, Scale);
            NativeFunction.Natives.SET_TEXT_COLOUR((int)TextColor.R, (int)TextColor.G, (int)TextColor.B, alpha);
            NativeFunction.Natives.SET_TEXT_CENTRE(true);
            NativeFunction.Natives.SET_TEXT_OUTLINE();
            NativeFunction.Natives.x25fbb336df1804cb("STRING");//BEGIN_TEXT_COMMAND_DISPLAY_TEXT //NativeFunction.Natives.x25fbb336df1804cb("STRING");//NativeFunction.Natives.x25FBB336DF1804CB(TextToShow);
            NativeFunction.Natives.x6C188BE134E074AA(TextToShow);//ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME
            NativeFunction.Natives.xCD015E5BB0D96A57(Y, X);//END_TEXT_COMMAND_DISPLAY_TEXT
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"UI ERROR {ex.Message} {ex.StackTrace}", 0);
        }
        //return;
    }



}

