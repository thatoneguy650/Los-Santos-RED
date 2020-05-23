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
    public static readonly Random MyRand;
    private static string ConfigFileName = "Plugins\\LosSantosRED\\Settings.xml";
    public static List<Rage.Object> CreatedObjects { get; set; }
    public static Settings MySettings { get; set; }
    public static bool IsRunning { get; set; }
    public static void Initialize()
    {
        IsRunning = true;

        CreatedObjects = new List<Rage.Object>();

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
        PlayerState.Initialize();
        ControlScript.Initialize();
        Debugging.Initialize();
        RespawnStopper.Initialize(); 
        Agencies.Initialize();
        Zones.Initialize();
        WeatherReporting.Initialize();
        Locations.Initialize();
        Police.Initialize();
        PoliceSpawning.Initialize();
        LicensePlateChanging.Initialize();
        Menus.Intitialize();
        RespawnStopper.Initialize();
        PedList.Initialize();
        DispatchAudio.Initialize();
        PoliceSpeech.Initialize();
        Vehicles.Initialize();
        VehicleEngine.Initialize();
        VehicleFuelSystem.Initialize();
        Tasking.Initialize();
        GTAWeapons.Initialize();
        WeaponDropping.Initialize();
        Streets.Initialize();
        
        PlayerLocation.Initialize();
        TrafficViolations.Initialize();
        SearchModeStopping.Initialize();
        UI.Initialize();
        PedSwapping.Initialize();
        PersonOfInterest.Initialize();
        Civilians.Initialize();
        Clock.Initialize();
        Mugging.Initialize();
        PlayerHealth.Initialize();
        PedWounds.Initialize();
    }
    public static void Dispose()
    {
        IsRunning = false;
        foreach (Blip myBlip in Police.CreatedBlips)
        {
            if (myBlip.Exists())
                myBlip.Delete();
        }
        PlayerState.Dispose();
        ControlScript.Dispose();
        LicensePlateChanging.Dispose();
        Menus.Dispose();
        RespawnStopper.Dispose(); //maye some slowness
        PedList.Dispose();
        DispatchAudio.Dispose();
        PoliceSpeech.Dispose();
        Vehicles.Dispose();
        VehicleEngine.Dispose();
        VehicleFuelSystem.Dispose();
        Smoking.Dispose();
        Tasking.Dispose();
        Agencies.Dispose();
        Locations.Dispose();
        GTAWeapons.Dispose();
        Speed.Dispose();
        WeaponDropping.Dispose();
        Streets.Dispose();
        UI.Dispose();
        Debugging.Dispose();
        PlayerLocation.Dispose();
        Police.Dispose();
        PoliceSpawning.Dispose();
        TrafficViolations.Dispose();
        SearchModeStopping.Dispose();
        WeatherReporting.Dispose();
        PedSwapping.Dispose();
        PersonOfInterest.Dispose();
        Civilians.Dispose();
        PlayerHealth.Dispose();
        Clock.Dispose();
        Mugging.Dispose();
        PedWounds.Dispose();
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
    public static void TransitionToRegularSpeed()
    {
        Game.TimeScale = 1f;//Stuff below works, could add it back, it just doesnt really do much
        //GameFiber Transition = GameFiber.StartNew(delegate
        //{
        //    int WaitTime = 100;
        //    while (Game.TimeScale < 1f)
        //    {
        //        Game.TimeScale += 0.05f;
        //        GameFiber.Wait(WaitTime);
        //        if (WaitTime >= 12)
        //            WaitTime -= 1;
        //    }

        //}, "TransitionOut");
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
    public static void RequestAnimationDictionay(String sDict)
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
    private static void LoadInteriors()
    {
        //Pillbox hill hospital?
        NativeFunction.CallByName<bool>("REMOVE_IPL", "RC12B_Destroyed");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "RC12B_HospitalInterior");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "RC12B_Default");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "RC12B_Fixed");
        NativeFunction.CallByName<bool>("REQUEST_IPL", "RC12B_Default");//state 1 normal

        //Lifeinvader
        NativeFunction.CallByName<bool>("REQUEST_IPL", "facelobby");  // lifeinvader
        NativeFunction.CallByName<bool>("REMOVE_IPL", "facelobbyfake");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -340230128, -1042.518f, -240.6915f, 38.11796f, true, 0.0f, 0.0f, -1.0f);//_DOOR_CONTROL

        //    FIB Lobby      
        NativeFunction.CallByName<bool>("REQUEST_IPL", "FIBlobby");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "FIBlobbyfake");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -1517873911, 106.3793f, -742.6982f, 46.51962f, false, 0.0f, 0.0f, 0.0f);
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -90456267, 105.7607f, -746.646f, 46.18266f, false, 0.0f, 0.0f, 0.0f);

        //Paleto Sheriff Office
        NativeFunction.CallByName<bool>("DISABLE_INTERIOR", NativeFunction.CallByName<int>("GET_INTERIOR_AT_COORDS", -444.89068603515625f, 6013.5869140625f, 30.7164f), false);
        NativeFunction.CallByName<bool>("CAP_INTERIOR", NativeFunction.CallByName<int>("GET_INTERIOR_AT_COORDS", -444.89068603515625f, 6013.5869140625f, 30.7164f), false);
        NativeFunction.CallByName<bool>("REQUEST_IPL", "v_sheriff2");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "cs1_16_sheriff_cap");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -1501157055, -444.4985f, 6017.06f, 31.86633f, false, 0.0f, 0.0f, 0.0f);
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -1501157055, -442.66f, 6015.222f, 31.86633f, false, 0.0f, 0.0f, 0.0f);

        //Sheriffs Office Sandy Shores
        NativeFunction.CallByName<bool>("DISABLE_INTERIOR", NativeFunction.CallByName<int>("GET_INTERIOR_AT_COORDS", 1854.2537841796875f, 3686.738525390625f, 33.2671012878418f), false);
        NativeFunction.CallByName<bool>("CAP_INTERIOR", NativeFunction.CallByName<bool>("GET_INTERIOR_AT_COORDS", 1854.2537841796875f, 3686.738525390625f, 33.2671012878418f), false);
        NativeFunction.CallByName<bool>("REQUEST_IPL", "v_sheriff");
        NativeFunction.CallByName<bool>("REMOVE_IPL", "sheriff_cap");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, -1765048490, 1855.685f, 3683.93f, 34.59282f, false, 0.0f, 0.0f, 0.0f);

        //    Tequila la       
        NativeFunction.CallByName<bool>("DISABLE_INTERIOR", NativeFunction.CallByName<bool>("GET_INTERIOR_AT_COORDS", -556.5089111328125f, 286.318115234375f, 81.1763f), false);
        NativeFunction.CallByName<bool>("CAP_INTERIOR", NativeFunction.CallByName<bool>("GET_INTERIOR_AT_COORDS", -556.5089111328125f, 286.318115234375f, 81.1763f), false);
        NativeFunction.CallByName<bool>("REQUEST_IPL", "v_rockclub");
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, 993120320, -565.1712f, 276.6259f, 83.28626f, false, 0.0f, 0.0f, 0.0f);// front door
        NativeFunction.CallByHash<bool>(0x9B12F9A24FABEDB0, 993120320, -561.2866f, 293.5044f, 87.77851f, false, 0.0f, 0.0f, 0.0f);// back door

    }
    private static bool IsPointInPolygon(Point p, Point[] polygon)
    {
        double minX = polygon[0].X;
        double maxX = polygon[0].X;
        double minY = polygon[0].Y;
        double maxY = polygon[0].Y;
        //
        for (int i = 1; i < polygon.Length; i++)
        {
            Point q = polygon[i];
            minX = Math.Min(q.X, minX);
            maxX = Math.Max(q.X, maxX);
            minY = Math.Min(q.Y, minY);
            maxY = Math.Max(q.Y, maxY);
        }

        if (p.X < minX || p.X > maxX || p.Y < minY || p.Y > maxY)
        {
            return false;
        }

        // http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html
        bool inside = false;
        for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
        {
            if ((polygon[i].Y > p.Y) != (polygon[j].Y > p.Y) &&
                 p.X < (polygon[j].X - polygon[i].X) * (p.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X)
            {
                inside = !inside;
            }
        }

        return inside;
    }
}