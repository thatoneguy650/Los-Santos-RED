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
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

public class DebugMenu : Menu
{

    private UIMenu Debug;
    private UIMenuListItem AutoSetRadioStation;
    private UIMenuItem GiveMoney;
    private UIMenuItem FillHealthAndArmor;
    private UIMenuItem KillPlayer;
    private UIMenuItem LogCameraPositionMenu;
    private UIMenuItem LogInteriorMenu;
    private UIMenuItem LogLocationMenu;
    private UIMenuItem LogLocationSimpleMenu;
    private UIMenuListItem GetRandomWeapon;
    private UIMenuListItem TeleportToPOI;
    private UIMenuItem SetDateToToday;
    private IActionable Player;
    private RadioStations RadioStations;
    private int RandomWeaponCategory;
    private IWeapons Weapons;
    private IPlacesOfInterest PlacesOfInterest;
    private int PlaceOfInterestSelected;
    private ISettingsProvideable Settings;
    private ITimeControllable Time;
    private Camera FreeCam;
    private float FreeCamScale = 1.0f;
    private UIMenuItem FreeCamMenu;

    public DebugMenu(MenuPool menuPool, IActionable player, IWeapons weapons, RadioStations radioStations, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, ITimeControllable time)
    {    
        Player = player;
        Weapons = weapons;
        RadioStations = radioStations;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        Time = time;
        Debug = new UIMenu("Debug", "Debug Settings");
        Debug.SetBannerType(EntryPoint.LSRedColor);
        menuPool.Add(Debug);

        
        Debug.OnItemSelect += DebugMenuSelect;
        Debug.OnListChange += OnListChange;
        CreateDebugMenu();
    }
    public override void Hide()
    {
        Debug.Visible = false;
    }
    public override void Show()
    {
        if (!Debug.Visible)
        {
            Debug.Visible = true;
        }
    }
    public override void Toggle()
    {
        if (!Debug.Visible)
        {
            Debug.Visible = true;
        }
        else
        {
            Debug.Visible = false;
        }
    }
    private void CreateDebugMenu()
    {

        KillPlayer = new UIMenuItem("Kill Player", "Immediatly die and ragdoll");
        GetRandomWeapon = new UIMenuListItem("Get Random Weapon", "Gives the Player a random weapon and ammo.", Enum.GetNames(typeof(WeaponCategory)).ToList());
        GiveMoney = new UIMenuItem("Get Money", "Give you some cash");
        FillHealthAndArmor = new UIMenuItem("Health and Armor", "Get loaded for bear");
        AutoSetRadioStation = new UIMenuListItem("Auto-Set Station", "Will auto set the station any time the radio is on", RadioStations.RadioStationList);
        LogLocationMenu = new UIMenuItem("Log Game Location", "Location Type, Then Name");
        LogLocationSimpleMenu = new UIMenuItem("Log Game Location (Simple)", "Location Type, Then Name");
        LogInteriorMenu = new UIMenuItem("Log Game Interior", "Interior Name");
        LogCameraPositionMenu = new UIMenuItem("Log Camera Position", "Logs current rendering cam post direction and rotation");
        FreeCamMenu = new UIMenuItem("Free Cam", "Start Free Camera Mode");

        TeleportToPOI = new UIMenuListItem("Teleport To POI", "Teleports to A POI on the Map", PlacesOfInterest.GetAllPlaces());

        SetDateToToday = new UIMenuItem("Set Game Date Current", "Sets the game date the same as system date");



        Debug.AddItem(KillPlayer);
        Debug.AddItem(GetRandomWeapon);
        Debug.AddItem(GiveMoney);
        Debug.AddItem(FillHealthAndArmor);
        Debug.AddItem(AutoSetRadioStation);
        Debug.AddItem(TeleportToPOI);
        Debug.AddItem(FreeCamMenu);
        Debug.AddItem(LogLocationMenu);
        Debug.AddItem(LogLocationSimpleMenu);
        Debug.AddItem(LogInteriorMenu);
        Debug.AddItem(LogCameraPositionMenu);
        Debug.AddItem(SetDateToToday);
    }
    private void DebugMenuSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == KillPlayer)
        {
            Game.LocalPlayer.Character.Kill();
        }
        else if (selectedItem == GetRandomWeapon)
        {
            WeaponInformation myGun = Weapons.GetRandomRegularWeapon((WeaponCategory)RandomWeaponCategory);
            if (myGun != null)
            {
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(myGun.ModelName, myGun.AmmoAmount, true);
            }
        }
        else if (selectedItem == TeleportToPOI)
        {
            GameLocation ToTeleportTo = PlacesOfInterest.GetAllPlaces()[PlaceOfInterestSelected];
            if(ToTeleportTo != null)
            {
                Game.LocalPlayer.Character.Position = ToTeleportTo.EntrancePosition;
                Game.LocalPlayer.Character.Heading = ToTeleportTo.EntranceHeading;
            }
        }
        else if (selectedItem == GiveMoney)
        {
            Player.GiveMoney(50000);
        }
        else if (selectedItem == FillHealthAndArmor)
        {
            Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
            Game.LocalPlayer.Character.Armor = 100;
        }

        else if (selectedItem == LogLocationMenu)
        {
            LogGameLocation();
        }
        else if (selectedItem == LogLocationSimpleMenu)
        {
            LogGameLocationSimple();
        }
        else if (selectedItem == LogCameraPositionMenu)
        {
            LogCameraPosition();
        }
        else if (selectedItem == LogInteriorMenu)
        {
            LogGameInterior();
        }
        else if (selectedItem == SetDateToToday)
        {
            Time.SetDateToToday();
        }
        else if (selectedItem == FreeCamMenu)
        {
            Frecam();
        }
        Debug.Visible = false;
    }
    private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {
        if (list == GetRandomWeapon)
        {
            RandomWeaponCategory = index;
        }
        if (list == AutoSetRadioStation)
        {
            Settings.SettingsManager.PlayerSettings.AutoTuneRadioStation = RadioStations.RadioStationList[index].InternalName;
        }
        if (list == TeleportToPOI)
        {
            PlaceOfInterestSelected = index;
        }
    }
    private void Frecam()
    {
        GameFiber.StartNew(delegate
        {
            FreeCam = new Camera(false);
            FreeCam.FOV = NativeFunction.Natives.GET_GAMEPLAY_CAM_FOV<float>();
            FreeCam.Position = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
            Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
            FreeCam.Rotation = new Rotator(r.X, r.Y, r.Z);
            FreeCam.Active = true;
            Game.LocalPlayer.HasControl = false;
            //This is all adapted from https://github.com/CamxxCore/ScriptCamTool/blob/master/GTAV_ScriptCamTool/PositionSelector.cs#L59
            while (!Game.IsKeyDownRightNow(Keys.P))
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
                    if (FreeCamScale == 1.0f)
                    {
                        FreeCamScale = 0.25f;
                    }
                    else
                    {
                        FreeCamScale = 1.0f;
                    }
                }
                string FreeCamString = FreeCamScale == 1.0f ? "Regular Scale" : "Slow Scale";
                Game.DisplayHelp($"Press P to Exit~n~Press O To Change Scale Current: {FreeCamString}");
                GameFiber.Yield();
            }
            FreeCam.Active = false;
            Game.LocalPlayer.HasControl = true;
            NativeFunction.Natives.CLEAR_FOCUS();
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
        WriteToLogLocations($"new GameLocation(new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), {Heading}f, LocationType.{text1}, \"{text2}\", \"\"),");
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
        string toWrite = $"new Interior({Player.CurrentLocation?.CurrentInterior?.ID}, \"{text1}\"),";
        WriteToLogInteriors(toWrite);
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