using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Instant_Action_RAGE.Systems
{
    internal static class CustomOptions
    {
        private static bool SpeedModeBombActive = false;
        private static Vehicle SpeedBus;
        public static bool IsRunning { get; set; } = true;
        public static bool SpeedModeEnabled { get; private set; } = true;

        public static void Initialize()
        {
            MainLoop();
        }
        public static void MainLoop()
        {
            GameFiber.StartNew(delegate
            {
                while (IsRunning)
                {
                    NativeFunction.CallByName<bool>("SET_POLICE_RADAR_BLIPS", false); // No Radar or police blips



                    if (SpeedModeEnabled)
                    {
                        if(SpeedModeBombActive)
                        {
                            if (!SpeedBus.Exists())
                            {
                                SpeedModeBombActive = false;
                            }
                            if (SpeedBus.Speed < 24.5872f)
                            {
                                SpeedBus.Explode();
                                SpeedModeBombActive = false;
                                Game.DisplaySubtitle("BOOM");
                            }
                        }
                        else if(Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Model.Name == "BUS" && Game.LocalPlayer.Character.CurrentVehicle.Speed >= 24.5872f)
                        {
                            SpeedBus = Game.LocalPlayer.Character.CurrentVehicle;
                            SpeedModeBombActive = true;
                            Game.DisplaySubtitle("Bomb Activated, Don't Drop Below 55 MPH");
                        }
                    }


                    //if (Game.LocalPlayer.WantedLevel > 0)
                    //{
                    //   // NativeFunction.CallByName<bool>("DISPLAY_RADAR", false);
                    //    NativeFunction.CallByName<bool>("SET_POLICE_RADAR_BLIPS", false); // No Radar or police blips
                    //}
                    //else
                    //{
                    //    //NativeFunction.CallByName<bool>("DISPLAY_RADAR", true);
                    //    NativeFunction.CallByName<bool>("SET_POLICE_RADAR_BLIPS", true);
                    //}


                    //NativeFunction.CallByName<bool>("DISPLAY_RADAR", true);
                    //NativeFunction.CallByName<bool>("DISPLAY_HUD", true);
                    //NativeFunction.CallByName<bool>("DISPLAY_CASH", true);

                    //if (Game.LocalPlayer.Character.IsGettingIntoVehicle)
                    //{
                    //    NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)2725352035, true);
                    //}

                    ////Function.Call(Hash.SET_POLICE_RADAR_BLIPS, false); // No Radar or police blips
                    ////Function.Call(Hash.DISPLAY_CASH, true);


                    //if (Game.LocalPlayer.LastVehicle.Exists() && Game.LocalPlayer.LastVehicle.IsEngineOn)
                    //    NativeFunction.CallByName<bool>("SET_VEHICLE_RADIO_LOUD", Game.LocalPlayer.Character.LastVehicle, true);// Boost the volume a bit
                    //else if(Game.LocalPlayer.LastVehicle.Exists())
                    //{
                    //    NativeFunction.CallByName<bool>("SET_VEHICLE_RADIO_LOUD", Game.LocalPlayer.Character.LastVehicle, false);// Boost the volume a bit
                    //}
                    //NativeFunction.Natives.xB9EFD5C25018725A("WantedMusicDisabled", true);//SET_AUDIO_FLAG
                    // NativeFunction.CallByName<bool>("SET_AUDIO_FLAG", "WantedMusicDisabled", true); // no wanted music
                    GameFiber.Yield();
                }
            });
        }
    }
}
