using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


public class PlayerLocationManager
{
    private uint GameTimePlayerGotOnFreeway;
    private uint GameTimePlayerGotOffFreeway;
    private Vector3 PlayerClosestNode;
    public bool IsRunning { get; set; }
    public Street PlayerCurrentStreet { get; private set; }
    public Street PlayerCurrentCrossStreet { get; private set; }
    public Zone PlayerCurrentZone { get; private set; }
    public bool PlayerIsOffroad { get; private set; }
    public bool PlayerIsOnFreeway { get; private set; }
    public bool PlayerRecentlyGotOnFreeway
    {
        get
        {
            if (PlayerIsOnFreeway && Game.GameTime - GameTimePlayerGotOnFreeway <= 10000)
                return true;
            else
                return false;
        }
    }
    public bool PlayerRecentlyGotOffFreeway
    {
        get
        {
            if (!PlayerIsOnFreeway && Game.GameTime - GameTimePlayerGotOffFreeway <= 10000)
                return true;
            else
                return false;
        }
    }
    public void Initialize()
    {
        IsRunning = true;
        PlayerCurrentStreet = null;
        PlayerCurrentCrossStreet = null;
        PlayerCurrentZone = new Zone();
        PlayerIsOffroad  = false;
    }
    public void Dispose()
    {
        IsRunning = false;
    }
    public void Tick()
    {
        if (IsRunning)
        {
            Update();
        }
    }
    private void Update()
    {
        GetZone();
        GetNode();
        GetStreets();
    }
    private void GetZone()
    {
        PlayerCurrentZone = ZoneManager.GetZone(Game.LocalPlayer.Character.Position);
    }
    private void GetNode()
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
    private void GetStreets()
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

