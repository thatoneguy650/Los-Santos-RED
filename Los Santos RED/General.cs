using ExtensionsMethods;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Extensions = ExtensionsMethods.Extensions;

public static class General
{
    public static readonly Random MyRand = new Random();
    private static string ConfigFileName { get; set; } = "Plugins\\LosSantosRED\\Settings.xml";
    public static List<Rage.Object> CreatedObjects { get; set; }
    public static List<Blip> CreatedBlips { get; set; }
    public static Settings MySettings { get; set; }
    public static bool IsRunning { get; set; }
    public static void Initialize()
    {
        IsRunning = true;
        CreatedObjects = new List<Rage.Object>();
        CreatedBlips = new List<Blip>();

        Game.LocalPlayer.Character.CanBePulledOutOfVehicles = true;

        while (Game.IsLoading)
            GameFiber.Yield();

        if (File.Exists(ConfigFileName))
        {
            MySettings = DeserializeParams<Settings>(ConfigFileName).FirstOrDefault();
        }
        else
        {
            MySettings = new Settings();
            List<Settings> ToSerialize = new List<Settings>();
            ToSerialize.Add(MySettings);
            SerializeParams(ToSerialize, ConfigFileName);
        }
    }
    public static void Dispose()
    {
        IsRunning = false;
        foreach (Blip myBlip in General.CreatedBlips)
        {
            if (myBlip.Exists())
                myBlip.Delete();
        }
    }
    public static void ReadAllConfigs()
    {
        ReadConfig();
        Agencies.ReadConfig();
        Zones.ReadConfig();
        GTAWeapons.ReadConfig();
        Locations.ReadConfig();
        Vehicles.ReadConfig();
        Streets.ReadConfig();
    }
    private static void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            MySettings = DeserializeParams<Settings>(ConfigFileName).FirstOrDefault();
        }
        else
        {
            MySettings = new Settings();
            List<Settings> ToSerialize = new List<Settings>();
            ToSerialize.Add(MySettings);
            SerializeParams(ToSerialize, ConfigFileName);
        }
    }
    public static void TransitionToSlowMo()
    {
        Game.TimeScale = 0.4f;//Stuff below works, could add it back, it just doesnt really do much
        //GameFiber Transition = GameFiber.StartNew(delegate
        //{
        //    int WaitTime = 100;
        //    while (Game.TimeScale > 0.4f)
        //    {
        //        Game.TimeScale -= 0.05f;
        //        GameFiber.Wait(WaitTime);
        //        if (WaitTime <= 200)
        //            WaitTime += 1;
        //    }

        //}, "TransitionIn");
        //Debugging.GameFibers.Add(Transition);
    }
    public static bool AttemptLockStatus(Vehicle ToLock, VehicleLockStatus DesiredLockStatus)
    {
        Debugging.WriteToLog("LockCarDoor", string.Format("Start, Lock Status {0}", ToLock.LockStatus));
        if (ToLock.LockStatus != (VehicleLockStatus)1 && ToLock.LockStatus != (VehicleLockStatus)7)//if (ToLock.LockStatus != (VehicleLockStatus)1) //unlocked
            return false;
        if (ToLock.HasDriver)//If they have a driver 
            return false;
        foreach (VehicleDoor myDoor in ToLock.GetDoors())
        {
            if (!myDoor.IsValid() || myDoor.IsOpen)
                return false;//invalid doors make the car not locked
        }
        if (!NativeFunction.CallByName<bool>("ARE_ALL_VEHICLE_WINDOWS_INTACT", ToLock))
            return false;//broken windows == not locked
        if (PlayerState.TrackedVehicles.Any(x => x.VehicleEnt.Handle == ToLock.Handle))
            return false; //previously entered vehicle arent locked
        if (ToLock.IsConvertible && ToLock.ConvertibleRoofState == VehicleConvertibleRoofState.Lowered)
            return false;
        if (ToLock.IsBike || ToLock.IsPlane || ToLock.IsHelicopter)
            return false;

        Debugging.WriteToLog("LockCarDoor", "Locked");
        ToLock.LockStatus = DesiredLockStatus;//Locked for player
        return true;
    }
    public static GTAWeapon GetCurrentWeapon(Ped Pedestrian)
    {
        if (Pedestrian.Inventory.EquippedWeapon == null)
            return null;
        ulong myHash = (ulong)Pedestrian.Inventory.EquippedWeapon.Hash;
        GTAWeapon CurrentGun = GTAWeapons.GetWeaponFromHash(myHash);
        if (CurrentGun != null)
            return CurrentGun;
        else
            return null;
    }
    public static void SetPedUnarmed(Ped Pedestrian, bool SetCantChange)
    {
        if (!(Pedestrian.Inventory.EquippedWeapon == null))
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Pedestrian, (uint)2725352035, true); //Unequip weapon so you don't get shot
            if (SetCantChange)
                NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Pedestrian, false);
        }
    }
    public static GTAWeapon.WeaponVariation GetWeaponVariation(Ped WeaponOwner, uint WeaponHash)
    {
        int Tint = NativeFunction.CallByName<int>("GET_PED_WEAPON_TINT_INDEX", WeaponOwner, WeaponHash);
        GTAWeapon MyGun = GTAWeapons.GetWeaponFromHash(WeaponHash);
        if (MyGun == null)
            return new GTAWeapon.WeaponVariation("Variation1", Tint);

        // List<GTAWeapon.WeaponComponent> Components = new List<GTAWeapon.WeaponComponent>();

        //if (!Components.Any())
        //    return new GTAWeapon.WeaponVariation("Variation1",Tint);
        List<string> ComponentsOnGun = new List<string>();

        foreach (GTAWeapon.WeaponComponent PossibleComponent in MyGun.PossibleComponents)
        {
            if (NativeFunction.CallByName<bool>("HAS_PED_GOT_WEAPON_COMPONENT", WeaponOwner, WeaponHash, PossibleComponent.Hash))
            {
                ComponentsOnGun.Add(PossibleComponent.Name);
            }

        }
        return new GTAWeapon.WeaponVariation("Variation1", Tint, ComponentsOnGun);

    }
    public static void ApplyWeaponVariation(Ped WeaponOwner, uint WeaponHash, GTAWeapon.WeaponVariation _WeaponVariation)
    {
        if (_WeaponVariation == null)
            return;
        NativeFunction.CallByName<bool>("SET_PED_WEAPON_TINT_INDEX", WeaponOwner, WeaponHash, _WeaponVariation.Tint);
        GTAWeapon LookupGun = GTAWeapons.GetWeaponFromHash(WeaponHash);//Weapons.Where(x => x.Hash == WeaponHash).FirstOrDefault();
        if (LookupGun == null)
            return;
        foreach (GTAWeapon.WeaponComponent ToRemove in LookupGun.PossibleComponents)
        {
            NativeFunction.CallByName<bool>("REMOVE_WEAPON_COMPONENT_FROM_PED", WeaponOwner, WeaponHash, ToRemove.Hash);
        }
        foreach (string ToAdd in _WeaponVariation.Components)
        {
            GTAWeapon.WeaponComponent MyComponent = LookupGun.PossibleComponents.Where(x => x.Name == ToAdd).FirstOrDefault();
            if (MyComponent != null)
                NativeFunction.CallByName<bool>("GIVE_WEAPON_COMPONENT_TO_PED", WeaponOwner, WeaponHash, MyComponent.Hash);
        }
    }
    public static void GetStreetPositionandHeading(Vector3 PositionNear, out Vector3 SpawnPosition, out float Heading,bool MainRoadsOnly)
    {
        Vector3 pos = PositionNear;
        SpawnPosition = Vector3.Zero;
        Heading = 0f;

        Vector3 outPos;
        float heading;
        float val;

        if (MainRoadsOnly)
        {
            unsafe
            {
                NativeFunction.CallByName<bool>("GET_CLOSEST_VEHICLE_NODE_WITH_HEADING", pos.X, pos.Y, pos.Z, &outPos, &heading, 0, 3, 0);
            }

            SpawnPosition = outPos;
            Heading = heading;
        }
        else
        {
            for (int i = 1; i < 40; i++)
            {
                unsafe
                {
                    NativeFunction.CallByName<bool>("GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING", pos.X, pos.Y, pos.Z, i, &outPos, &heading, &val, 1, 0x40400000, 0);
                }
                if (!NativeFunction.CallByName<bool>("IS_POINT_OBSCURED_BY_A_MISSION_ENTITY", outPos.X, outPos.Y, outPos.Z, 5.0f, 5.0f, 5.0f, 0))
                {
                    SpawnPosition = outPos;
                    Heading = heading;
                    break;
                }
            }
        }
    }
    public static void RequestAnimationDictionay(string sDict)
    {
        NativeFunction.CallByName<bool>("REQUEST_ANIM_DICT", sDict);
        while (!NativeFunction.CallByName<bool>("HAS_ANIM_DICT_LOADED", sDict))
            GameFiber.Yield();
    }
    public static Rage.Object AttachScrewdriverToPed(Ped Pedestrian)
    {
        Rage.Object Screwdriver = new Rage.Object("prop_tool_screwdvr01", Pedestrian.GetOffsetPositionUp(50f));
        if (!Screwdriver.Exists())
            return null;
        CreatedObjects.Add(Screwdriver);
        int BoneIndexRightHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Pedestrian, 57005);
        Screwdriver.AttachTo(Pedestrian, BoneIndexRightHand, new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f));
        return Screwdriver;
    }
    public static Rage.Object AttachMoneyToPed(Ped Pedestrian)
    {
        Rage.Object Money = new Rage.Object("xs_prop_arena_cash_pile_m", Pedestrian.GetOffsetPositionUp(50f));
        if (!Money.Exists())
            return null;
        CreatedObjects.Add(Money);
        int BoneIndexRightHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Pedestrian, 57005);
        Money.AttachTo(Pedestrian, BoneIndexRightHand, new Vector3(0.12f, 0.03f, -0.01f), new Rotator(0f, -45f, 90f));
        return Money;
    }
    public static PedVariation GetPedVariation(Ped myPed)
    {
        try
        {
            PedVariation myPedVariation = new PedVariation
            {
                MyPedComponents = new List<PedComponent>(),
                MyPedProps = new List<PropComponent>()
            };
            for (int ComponentNumber = 0; ComponentNumber < 12; ComponentNumber++)
            {
                myPedVariation.MyPedComponents.Add(new PedComponent(ComponentNumber, NativeFunction.CallByName<int>("GET_PED_DRAWABLE_VARIATION", myPed, ComponentNumber), NativeFunction.CallByName<int>("GET_PED_TEXTURE_VARIATION", myPed, ComponentNumber), NativeFunction.CallByName<int>("GET_PED_PALETTE_VARIATION", myPed, ComponentNumber)));
            }
            for (int PropNumber = 0; PropNumber < 8; PropNumber++)
            {
                myPedVariation.MyPedProps.Add(new PropComponent(PropNumber, NativeFunction.CallByName<int>("GET_PED_PROP_INDEX", myPed, PropNumber), NativeFunction.CallByName<int>("GET_PED_PROP_TEXTURE_INDEX", myPed, PropNumber)));
            }
            return myPedVariation;
        }
        catch (Exception e)
        {
            Debugging.WriteToLog("CopyPedComponentVariation", "CopyPedComponentVariation Error; " + e.Message);
            return null;
        }
    }
    public static void ReplacePedComponentVariation(Ped myPed, PedVariation myPedVariation)
    {
        try
        {
            foreach (PedComponent Component in myPedVariation.MyPedComponents)
            {
                NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", myPed, Component.ComponentID, Component.DrawableID, Component.TextureID, Component.PaletteID);
            }
            foreach (PropComponent Prop in myPedVariation.MyPedProps)
            {
                NativeFunction.CallByName<uint>("SET_PED_PROP_INDEX", myPed, Prop.PropID, Prop.DrawableID, Prop.TextureID, false);
            }
        }
        catch (Exception e)
        {
            Debugging.WriteToLog("ReplacePedComponentVariation", "ReplacePedComponentVariation Error; " + e.Message);
        }
    }
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[MyRand.Next(s.Length)]).ToArray());
    }
    public static void SerializeParams<T>(List<T> paramList, string FileName)
    {
        XDocument doc = new XDocument();
        XmlSerializer serializer = new XmlSerializer(paramList.GetType());
        XmlWriter writer = doc.CreateWriter();
        serializer.Serialize(writer, paramList);
        writer.Close();
        File.WriteAllText(FileName, doc.ToString());
        Debugging.WriteToLog("Settings ReadConfig", string.Format("Using Default Data {0}", FileName));
    }
    public static List<T> DeserializeParams<T>(string FileName)
    {
        XDocument doc = new XDocument(XDocument.Load(FileName));
        XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
        XmlReader reader = doc.CreateReader();
        List<T> result = (List<T>)serializer.Deserialize(reader);
        reader.Close();
        Debugging.WriteToLog("Settings ReadConfig", string.Format("Using Saved Data {0}", FileName));
        return result;
    }
    public static bool RandomPercent(float Percent)
    {
        if (MyRand.Next(1, 101) <= Percent)
            return true;
        else
            return false;
    }
}