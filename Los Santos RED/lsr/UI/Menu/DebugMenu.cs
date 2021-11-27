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
    private IActionable Player;
    private RadioStations RadioStations;
    private int RandomWeaponCategory;
    private IWeapons Weapons;
    private IPlacesOfInterest PlacesOfInterest;
    private int PlaceOfInterestSelected;
    private ISettingsProvideable Settings;
    public DebugMenu(MenuPool menuPool, IActionable player, IWeapons weapons, RadioStations radioStations, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings)
    {    
        Player = player;
        Weapons = weapons;
        RadioStations = radioStations;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        Debug = new UIMenu("Debug", "Debug Settings");
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


        TeleportToPOI = new UIMenuListItem("Teleport To POI", "Teleports to A POI on the Map", PlacesOfInterest.GetAllPlaces());

        Debug.AddItem(KillPlayer);
        Debug.AddItem(GetRandomWeapon);
        Debug.AddItem(GiveMoney);
        Debug.AddItem(FillHealthAndArmor);
        Debug.AddItem(AutoSetRadioStation);
        Debug.AddItem(TeleportToPOI);
        Debug.AddItem(LogLocationMenu);
        Debug.AddItem(LogLocationSimpleMenu);
        Debug.AddItem(LogInteriorMenu);
        Debug.AddItem(LogCameraPositionMenu);
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
        uint CameraHAndle = NativeFunction.Natives.GET_RENDERING_CAM<uint>();
        Vector3 pos = NativeFunction.Natives.GET_CAM_COORD<Vector3>(CameraHAndle);
        Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
        Vector3 direction = NativeHelper.GetGameplayCameraDirection();
        WriteToLogCameraPosition($", CameraPosition = new Vector3({pos.X}f, {pos.Y}f, {pos.Z}f), CameraDirection = new Vector3({direction.X}f, {direction.Y}f, {direction.Z}f), CameraRotation = new Rotator({r.X}f, {r.Y}f, {r.Z}f);");
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