using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


public static class PlayerLocation
{
    public static bool IsRunning { get; set; } = true;
    public static Street PlayerCurrentStreet { get; set; }
    public static Street PlayerCurrentCrossStreet { get; set; }
    public static Zone PlayerCurrentZone { get; set; } = new Zone("UNK_LSCOUNTY", "Los Santos County", "", County.LosSantosCounty);
    public static bool PlayerIsOffroad { get; set; } = false;
    public static bool PlayerIsOnFreeway { get; set; } = false;
    public static bool PlayerRecentlyGotOnFreeway
    {
        get
        {
            if (PlayerIsOnFreeway && Game.GameTime - GameTimePlayerGotOnFreeway <= 6000)
                return true;
            else
                return false;
        }
    }
    public static bool PlayerRecentlyGotOffFreeway
    {
        get
        {
            if (!PlayerIsOnFreeway && Game.GameTime - GameTimePlayerGotOnFreeway <= 6000)
                return true;
            else
                return false;
        }
    }

    private static uint GameTimePlayerGotOnFreeway;
    

    public static void Initialize()
    {
        IsRunning = true;
        PlayerCurrentStreet = null;
        PlayerCurrentCrossStreet = null;
        PlayerCurrentZone = new Zone("UNK_LSCOUNTY", "Los Santos County", "", County.LosSantosCounty);
        PlayerIsOffroad  = false;
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        PlayerCurrentZone = Zones.GetZoneAtLocation(Game.LocalPlayer.Character.Position);
        Vector3 PlayerClosestNode = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position);
        if (PlayerClosestNode.DistanceTo2D(Game.LocalPlayer.Character) >= 15f)//was 25f
        {
            PlayerIsOffroad = true;
        }
        else
        {
            PlayerIsOffroad = false;
        }
        if (PlayerIsOffroad)
        {
            PlayerCurrentStreet = null;
            PlayerCurrentCrossStreet = null;
            PlayerIsOnFreeway = false;
            return;
        }

        Vector3 PlayerPos = PlayerClosestNode;//Game.LocalPlayer.Character.Position;
        int StreetHash = 0;
        int CrossingHash = 0;
        string PlayerCurrentStreetName;
        string PlayerCurrentCrossStreetName;
        unsafe
        {
            NativeFunction.CallByName<uint>("GET_STREET_NAME_AT_COORD", PlayerPos.X, PlayerPos.Y, PlayerPos.Z, &StreetHash, &CrossingHash);
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


        PlayerCurrentStreet = Streets.GetStreetFromName(PlayerCurrentStreetName);
        PlayerCurrentCrossStreet = Streets.GetStreetFromName(PlayerCurrentCrossStreetName);

        if(PlayerCurrentStreet == null)
        {
            PlayerCurrentStreet = new Street(GetCurrentStreet(Game.LocalPlayer.Character.Position) + "?", 60f);
            if(PlayerCurrentStreet.IsHighway)
            {
                PlayerIsOnFreeway = true;
                GameTimePlayerGotOnFreeway = Game.GameTime;
            }
            else
            {
                PlayerIsOnFreeway = false;
            }
        }

    }
    public static string GetCurrentStreet(Vector3 Position)
    {
        int StreetHash = 0;
        int CrossingHash = 0;
        unsafe
        {
            NativeFunction.CallByName<uint>("GET_STREET_NAME_AT_COORD", Position.X, Position.Y, Position.Z, &StreetHash, &CrossingHash);
        }
        string StreetName = string.Empty;
        string CrossStreetName = string.Empty;
        if (StreetHash != 0)
        {
            unsafe
            {
                IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", StreetHash);

                StreetName = Marshal.PtrToStringAnsi(ptr);
            }
        }
        return StreetName;
    }
}

