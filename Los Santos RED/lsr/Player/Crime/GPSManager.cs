using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GPSManager
{
    private IDestinateable Player;
    private IEntityProvideable World;
    private uint GameTimeLastCheckedRouteBlip;

    public Blip CurrentGPSBlip { get; private set; }
    public GPSManager(IDestinateable player, IEntityProvideable world)
    {
        Player = player;
        World = world;
    }
    public void Setup()
    {

    }
    public void Update()
    {

    }
    public void Dispose()
    {
        RemoveGPSRoute();
    }
    public void Reset()
    {
        RemoveGPSRoute();
    }
    public void AddGPSRoute(string Name, Vector3 position)
    {
       if(NativeFunction.Natives.IS_WAYPOINT_ACTIVE<bool>())
        {
            NativeFunction.Natives.xD8E694757BCEA8E9();//_DELETE_WAYPOINT
        }
        NativeFunction.Natives.SET_NEW_WAYPOINT(position.X, position.Y);
        Game.DisplaySubtitle($"Adding Waypoint To {Name}");
    }
    public void RemoveGPSRoute()
    {
        if (NativeFunction.Natives.IS_WAYPOINT_ACTIVE<bool>())
        {
            NativeFunction.Natives.xD8E694757BCEA8E9();//_DELETE_WAYPOINT
            Game.DisplaySubtitle("Waypoint Removed");
        }
    }
    public Vector3 GetGPSRoutePosition()
    {
        if (!NativeFunction.Natives.IS_WAYPOINT_ACTIVE<bool>())
        {
            return Vector3.Zero;
        }
        Vector3 markerPos = NativeFunction.Natives.GET_BLIP_COORDS<Vector3>(NativeFunction.Natives.GET_FIRST_BLIP_INFO_ID<int>(8));
        //EntryPoint.WriteToConsole($"Current Marker Position1: {markerPos}");
        if (markerPos == Vector3.Zero)
        {
            return Vector3.Zero;
        }
        if (!NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(markerPos.X, markerPos.Y, 500f, out float GroundZ, true, false))
        {
            EntryPoint.WriteToConsole($"Current Marker NO GROUND Z FOUND RETURNING REGULAR MARKERPOS");
            return new Vector3(markerPos.X, markerPos.Y, markerPos.Z);
        }
        //EntryPoint.WriteToConsole($"Current Marker Position2: {new Vector3(markerPos.X, markerPos.Y, GroundZ)} GroundZ{GroundZ}");
        return new Vector3(markerPos.X, markerPos.Y, GroundZ);

    }

}
