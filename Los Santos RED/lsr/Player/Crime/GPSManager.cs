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

}
