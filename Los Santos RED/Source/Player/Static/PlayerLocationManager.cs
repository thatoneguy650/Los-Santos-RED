using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


public static class PlayerLocationManager
{
    private static uint GameTimePlayerGotOnFreeway;
    private static uint GameTimePlayerGotOffFreeway;
    private static Vector3 PlayerClosestNode;
    public static bool IsRunning { get; set; }
    public static Street PlayerCurrentStreet { get; private set; }
    public static Street PlayerCurrentCrossStreet { get; private set; }
    public static Zone PlayerCurrentZone { get; private set; }
    public static bool PlayerIsOffroad { get; private set; }
    public static bool PlayerIsOnFreeway { get; private set; }
    public static bool PlayerRecentlyGotOnFreeway
    {
        get
        {
            if (PlayerIsOnFreeway && Game.GameTime - GameTimePlayerGotOnFreeway <= 10000)
                return true;
            else
                return false;
        }
    }
    public static bool PlayerRecentlyGotOffFreeway
    {
        get
        {
            if (!PlayerIsOnFreeway && Game.GameTime - GameTimePlayerGotOffFreeway <= 10000)
                return true;
            else
                return false;
        }
    }
    public static void Initialize()
    {
        IsRunning = true;
        PlayerCurrentStreet = null;
        PlayerCurrentCrossStreet = null;
        PlayerCurrentZone = new Zone();
        PlayerIsOffroad  = false;
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            Update();
        }
    }
    private static void Update()
    {
        GetZone();
        GetNode();
        GetStreets();
    }
    private static void GetZone()
    {
        PlayerCurrentZone = Zones.GetZone(Game.LocalPlayer.Character.Position);
    }
    private static void GetNode()
    {
        PlayerClosestNode = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position);
        if (PlayerClosestNode.DistanceTo2D(Game.LocalPlayer.Character) >= 15f)//was 25f
        {
            PlayerIsOffroad = true;
        }
        else
        {
            PlayerIsOffroad = false;
        }
    }
    private static void GetStreets()
    {
        if (PlayerIsOffroad)
        {
            PlayerCurrentStreet = null;
            PlayerCurrentCrossStreet = null;
            PlayerIsOnFreeway = false;
            return;
        }

        int StreetHash = 0;
        int CrossingHash = 0;
        string PlayerCurrentStreetName;
        string PlayerCurrentCrossStreetName;
        unsafe
        {
            NativeFunction.CallByName<uint>("GET_STREET_NAME_AT_COORD", PlayerClosestNode.X, PlayerClosestNode.Y, PlayerClosestNode.Z, &StreetHash, &CrossingHash);
        }
        string StreetName = string.Empty;
        if (StreetHash != 0)
        {
            unsafe
            {
                IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", StreetHash);
                StreetName = Marshal.PtrToStringAnsi(ptr);
            }
            PlayerCurrentStreetName = StreetName;
        }
        else
            PlayerCurrentStreetName = "";

        string CrossStreetName = string.Empty;
        if (CrossingHash != 0)
        {
            unsafe
            {
                IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", CrossingHash);
                CrossStreetName = Marshal.PtrToStringAnsi(ptr);
            }
            PlayerCurrentCrossStreetName = CrossStreetName;
        }
        else
            PlayerCurrentCrossStreetName = "";


        PlayerCurrentStreet = StreetManager.GetStreet(PlayerCurrentStreetName);
        PlayerCurrentCrossStreet = StreetManager.GetStreet(PlayerCurrentCrossStreetName);

        if (PlayerCurrentStreet == null)
        {
            PlayerCurrentStreet = new Street(StreetManager.GetStreet(Game.LocalPlayer.Character.Position) + "?", 60f);
            if (PlayerCurrentStreet.IsHighway)
            {
                if (!PlayerIsOnFreeway)
                    GameTimePlayerGotOnFreeway = Game.GameTime;

                PlayerIsOnFreeway = true;
            }
            else
            {
                if (PlayerIsOnFreeway)
                    GameTimePlayerGotOffFreeway = Game.GameTime;

                PlayerIsOnFreeway = false;
            }
        }
    }
}

