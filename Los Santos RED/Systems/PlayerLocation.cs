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
    public static Zone PlayerCurrentZone { get; set; } = Zones.UNK_LSCOUNTY;
    public static bool PlayerIsOffroad { get; set; } = false;

    public static void Initialize()
    {
        IsRunning = true;
        PlayerCurrentStreet = null;
        PlayerCurrentCrossStreet = null;
        PlayerCurrentZone = Zones.UNK_LSCOUNTY;
        PlayerIsOffroad  = false;
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void UpdateLocation()
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

