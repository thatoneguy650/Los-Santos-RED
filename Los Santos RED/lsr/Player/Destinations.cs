using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Destinations
{
    private IDestinateable Player;
    private IEntityProvideable World;
    private uint GameTimeLastCheckedRouteBlip;

    public Blip CurrentGPSBlip { get; private set; }
    public Destinations(IDestinateable player, IEntityProvideable world)
    {
        Player = player;
        World = world;
    }
    public void Setup()
    {

    }
    public void Update()
    {
        if (CurrentGPSBlip.Exists())
        {
            if (CurrentGPSBlip.DistanceTo2D(Player.Position) <= 30f)
            {
                NativeFunction.Natives.SET_BLIP_ROUTE(CurrentGPSBlip, false);
                CurrentGPSBlip.Delete();
            }
            else
            {
                if (GameTimeLastCheckedRouteBlip == 0 || Game.GameTime - GameTimeLastCheckedRouteBlip >= 10000)
                {
                    NativeFunction.Natives.SET_BLIP_ROUTE(CurrentGPSBlip, true);
                    GameTimeLastCheckedRouteBlip = Game.GameTime;
                }
            }
        }
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
        if (CurrentGPSBlip.Exists())
        {
            NativeFunction.Natives.SET_BLIP_ROUTE(CurrentGPSBlip, false);
            CurrentGPSBlip.Delete();
        }
        if (position != Vector3.Zero)
        {
            Blip MyLocationBlip = new Blip(position)
            {
                Name = Name
            };
            if (MyLocationBlip.Exists())
            {
                MyLocationBlip.Color = Color.LightYellow;
                NativeFunction.Natives.SET_BLIP_AS_SHORT_RANGE(MyLocationBlip, false);
                NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
                NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(Name);
                NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(MyLocationBlip);
                NativeFunction.Natives.SET_BLIP_ROUTE(MyLocationBlip, true);
                CurrentGPSBlip = MyLocationBlip;
                World.AddBlip(MyLocationBlip);
                Game.DisplaySubtitle($"Adding GPS To {Name}");
                GameTimeLastCheckedRouteBlip = Game.GameTime;
            }
        }
    }
    public void RemoveGPSRoute()
    {
        if (CurrentGPSBlip.Exists())
        {
            NativeFunction.Natives.SET_BLIP_ROUTE(CurrentGPSBlip, false);
            CurrentGPSBlip.Delete();
        }
    }

}
