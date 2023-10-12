using LosSantosRED.lsr.Interface;
using Microsoft.VisualBasic.Logging;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static RAGENativeUI.Elements.UIMenuStatsPanel;


public class GPSManager
{
    private IDestinateable Player;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private ITimeControllable Time;
    private uint GameTimeLastCheckedRouteBlip;

    public Blip CurrentGPSBlip { get; private set; }
    public GPSManager(IDestinateable player, IEntityProvideable world, ISettingsProvideable settings, ITimeControllable time)
    {
        Player = player;
        World = world;
        Settings = settings;
        Time = time;
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
        if (!NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(markerPos.X, markerPos.Y, 1000f, out float GroundZ, true, false))
        {

            Vector3 NewVec = ForceGroundZ(markerPos);



            if (NewVec.Z == 0.0f)
            {
                NewVec = ForceGroundZ(markerPos);
            }

            if (NewVec.Z == 0.0f)
            {
                EntryPoint.WriteToConsole($"NEWVEC Z IS Z? {markerPos} NEW:{NewVec}");

                return Vector3.Zero ;
            }
            EntryPoint.WriteToConsole($"Current Marker NO GROUND Z FOUND RETURNING REGULAR MARKERPOS {markerPos} NEW:{NewVec}");

            return NewVec;




            //return new Vector3(markerPos.X, markerPos.Y, markerPos.Z);
        }
        //EntryPoint.WriteToConsole($"Current Marker Position2: {new Vector3(markerPos.X, markerPos.Y, GroundZ)} GroundZ{GroundZ}");
        return new Vector3(markerPos.X, markerPos.Y, GroundZ);

    }








    //Adapted from TomGrobbe
    public void TeleportToCoords(Vector3 pos, float heading, bool forceRoadNode, bool safeModeDisabled, int hoursToAdvance)
    {
        EntryPoint.WriteToConsole($"STARTING TELEPORT TO COORDS {pos} {heading}");
        if (!safeModeDisabled)
        {
            // Is player in a vehicle and the driver? Then we'll use that to teleport.
            var veh = Game.LocalPlayer.Character.CurrentVehicle;
            bool inVehicle() => veh != null && veh.Exists();// && Game.LocalPlayer.Character == veh.Driver;

            //bool vehicleRestoreVisibility = inVehicle() && veh.IsVisible;
            //bool pedRestoreVisibility = Game.LocalPlayer.Character.IsVisible;

            // Fade out the screen and wait for it to be faded out completely.
            Game.FadeScreenOut(500, true);

            while (!Game.IsScreenFadedOut)
            {
                GameFiber.Yield();
            }

            // Freeze vehicle or player location and fade out the entity to the network.
            if (inVehicle())
            {
                veh.IsPositionFrozen = true;
            }
            else
            {
                NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(Game.LocalPlayer.Character);
                Game.LocalPlayer.Character.IsPositionFrozen = true;
            }

            // This will be used to get the return value from the groundz native.
            float groundZ = 850.0f;

            // Bool used to determine if the groundz coord could be found.
            bool found = false;

            // Loop from 950 to 0 for the ground z coord, and take away 25 each time.
            if (!forceRoadNode)
            {
                for (float zz = 950.0f; zz >= 0f; zz -= 25f)
                {
                    float z = zz;
                    // The z coord is alternating between a very high number, and a very low one.
                    // This way no matter the location, the actual ground z coord will always be found the fastest.
                    // If going from top > bottom then it could take a long time to reach the bottom. And vice versa.
                    // By alternating top/bottom each iteration, we minimize the time on average for ANY location on the map.
                    if (zz % 2 != 0)
                    {
                        z = 950f - zz;
                    }

                    // Request collision at the coord. I've never actually seen this do anything useful, but everyone keeps telling me this is needed.
                    // It doesn't matter to get the ground z coord, and neither does it actually prevent entities from falling through the map, nor does
                    // it seem to load the world ANY faster than without, but whatever.
                    NativeFunction.Natives.REQUEST_COLLISION_AT_COORD(pos.X, pos.Y, z);

                    // Request a new scene. This will trigger the world to be loaded around that area.
                    NativeFunction.Natives.NEW_LOAD_SCENE_START(pos.X, pos.Y, z, pos.X, pos.Y, z, 50f, 0);

                    // Timer to make sure things don't get out of hand (player having to wait forever to get teleported if something fails).
                    uint tempTimer = Game.GameTime;

                    // Wait for the new scene to be loaded.
                    while (NativeFunction.Natives.IS_NETWORK_LOADING_SCENE<bool>())//  IsNetworkLoadingScene())
                    {
                        // If this takes longer than 1 second, just abort. It's not worth waiting that long.
                        if (Game.GameTime - tempTimer > 1000)
                        {
                            EntryPoint.WriteToConsole("Waiting for the scene to load is taking too long (more than 1s). Breaking from wait loop.");
                            break;
                        }
                        GameFiber.Yield();
                    }

                    // If the player is in a vehicle, teleport the vehicle to this new position.
                    if (inVehicle())
                    {
                        NativeFunction.Natives.SET_ENTITY_COORDS(veh, pos.X, pos.Y, z, false, false, false, true);
                        veh.Heading = heading;
                    }
                    // otherwise, teleport the player to this new position.
                    else
                    {
                        NativeFunction.Natives.SET_ENTITY_COORDS(Game.LocalPlayer.Character, pos.X, pos.Y, z, false, false, false, true);
                        Game.LocalPlayer.Character.Heading = heading;
                    }

                    // Reset the timer.
                    tempTimer = Game.GameTime;

                    // Wait for the collision to be loaded around the entity in this new location.
                    while (!NativeFunction.Natives.HAS_COLLISION_LOADED_AROUND_ENTITY<bool>(Game.LocalPlayer.Character))
                    {
                        // If this takes too long, then just abort, it's not worth waiting that long since we haven't found the real ground coord yet anyway.
                        if (Game.GameTime - tempTimer > 1000)
                        {
                            EntryPoint.WriteToConsole("Waiting for the collision is taking too long (more than 1s). Breaking from wait loop.");
                            break;
                        }
                        GameFiber.Yield();
                    }

                    // Check for a ground z coord.
                    found = NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(pos.X, pos.Y, z, ref groundZ, false);

                    // If we found a ground z coord, then teleport the player (or their vehicle) to that new location and break from the loop.
                    if (found)
                    {
                        EntryPoint.WriteToConsole($"Ground coordinate found: {groundZ}");
                        if (inVehicle())
                        {
                            NativeFunction.Natives.SET_ENTITY_COORDS(veh, pos.X, pos.Y, groundZ, false, false, false, true);

                            // We need to unfreeze the vehicle because sometimes having it frozen doesn't place the vehicle on the ground properly.
                            veh.IsPositionFrozen = false;
                            NativeFunction.Natives.SET_VEHICLE_ON_GROUND_PROPERLY(veh);
                            veh.Heading = heading;
                            // Re-freeze until screen is faded in again.
                            veh.IsPositionFrozen = true;
                        }
                        else
                        {
                            NativeFunction.Natives.SET_ENTITY_COORDS(Game.LocalPlayer.Character, pos.X, pos.Y, groundZ, false, false, false, true);
                            Game.LocalPlayer.Character.Heading = heading;
                        }
                        break;
                    }

                    // Wait 10ms before trying the next location.
                    GameFiber.Sleep(10);
                }
            }
            // If the loop ends but the ground z coord has not been found yet, then get the nearest vehicle node as a fail-safe coord.
            if (!found)
            {
                var safePos = pos;
                float safeHeading = 0.0f;
                float valNumLanes;
               // NativeFunction.Natives.GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(pos.X, pos.Y, pos.Z, 0, ref safePos, ref safeHeading, 0, 0, 0);
                NativeFunction.Natives.GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(pos.X, pos.Y, pos.Z, 1, out safePos, out safeHeading, out valNumLanes, 1, 3.0f, 0);
                // Notify the user that the ground z coord couldn't be found, so we will place them on a nearby road instead.
                EntryPoint.WriteToConsole("Could not find a safe ground coord. Placing you on the nearest road instead.");
                // Teleport vehicle, or player.
                if (inVehicle())
                {
                    NativeFunction.Natives.SET_ENTITY_COORDS(veh, safePos.X, safePos.Y, safePos.Z, false, false, false, true);
                    veh.Heading = safeHeading;
                    veh.IsPositionFrozen = false;
                    NativeFunction.Natives.SET_VEHICLE_ON_GROUND_PROPERLY(veh);
                    veh.IsPositionFrozen = true;
                }
                else
                {
                    NativeFunction.Natives.SET_ENTITY_COORDS(Game.LocalPlayer.Character, safePos.X, safePos.Y, safePos.Z, false, false, false, true);
                    Game.LocalPlayer.Character.Heading = safeHeading;
                }
            }
            // Once the teleporting is done, unfreeze vehicle or player and fade them back in.
            if (inVehicle())
            {
                veh.IsPositionFrozen = false;
            }
            else
            {
                Game.LocalPlayer.Character.IsPositionFrozen = false;
            }
            // Fade screen in and reset the camera angle.
            if (hoursToAdvance > 0)
            {
                Time.SetDateTime(Time.CurrentDateTime.AddHours(hoursToAdvance));
            }
            GameFiber.Sleep(1000);
            Game.FadeScreenIn(500, true);
            NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_PITCH(0.0f, 1.0f);
        }
        // Disable safe teleporting and go straight to the specified coords.
        else
        {
            NativeFunction.Natives.REQUEST_COLLISION_AT_COORD(pos.X, pos.Y, pos.Z);

            // Teleport directly to the coords without trying to get a safe z pos.
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Driver == Game.LocalPlayer.Character)
            {
                NativeFunction.Natives.SET_ENTITY_COORDS(Game.LocalPlayer.Character.CurrentVehicle, pos.X, pos.Y, pos.Z, false, false, false, true);
                Game.LocalPlayer.Character.CurrentVehicle.Heading = heading;
            }
            else
            {
                NativeFunction.Natives.SET_ENTITY_COORDS(Game.LocalPlayer.Character, pos.X, pos.Y, pos.Z, false, false, false, true);
                Game.LocalPlayer.Character.Heading = heading;
            }
        }
    }
    //ORIG
    //public void TeleportToCoords(Vector3 pos, bool safeModeDisabled = false)
    //{
    //    if (!safeModeDisabled)
    //    {
    //        // Is player in a vehicle and the driver? Then we'll use that to teleport.
    //        var veh = Game.LocalPlayer.Character.CurrentVehicle;
    //        bool inVehicle() => veh != null && veh.Exists() && Game.LocalPlayer.Character == veh.Driver;

    //        bool vehicleRestoreVisibility = inVehicle() && veh.IsVisible;
    //        bool pedRestoreVisibility = Game.LocalPlayer.Character.IsVisible;

    //        // Freeze vehicle or player location and fade out the entity to the network.
    //        if (inVehicle())
    //        {
    //            veh.IsPositionFrozen = true;
    //            //if (veh.IsVisible)
    //            //{
    //            //    NetworkFadeOutEntity(veh.Handle, true, false);
    //            //}
    //        }
    //        else
    //        {
    //            NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(Game.LocalPlayer.Character);
    //            Game.LocalPlayer.Character.IsPositionFrozen = true;
    //            //if (Game.LocalPlayer.Character.IsVisible)
    //            //{
    //            //    NetworkFadeOutEntity(Game.LocalPlayer.Character.Handle, true, false);
    //            //}
    //        }

    //        // Fade out the screen and wait for it to be faded out completely.
    //        Game.FadeScreenOut(500, true);

    //        while (!Game.IsScreenFadedOut)
    //        {
    //            GameFiber.Yield();
    //        }

    //        // This will be used to get the return value from the groundz native.
    //        float groundZ = 850.0f;

    //        // Bool used to determine if the groundz coord could be found.
    //        bool found = false;

    //        // Loop from 950 to 0 for the ground z coord, and take away 25 each time.
    //        for (float zz = 950.0f; zz >= 0f; zz -= 25f)
    //        {
    //            float z = zz;
    //            // The z coord is alternating between a very high number, and a very low one.
    //            // This way no matter the location, the actual ground z coord will always be found the fastest.
    //            // If going from top > bottom then it could take a long time to reach the bottom. And vice versa.
    //            // By alternating top/bottom each iteration, we minimize the time on average for ANY location on the map.
    //            if (zz % 2 != 0)
    //            {
    //                z = 950f - zz;
    //            }

    //            // Request collision at the coord. I've never actually seen this do anything useful, but everyone keeps telling me this is needed.
    //            // It doesn't matter to get the ground z coord, and neither does it actually prevent entities from falling through the map, nor does
    //            // it seem to load the world ANY faster than without, but whatever.
    //            NativeFunction.Natives.REQUEST_COLLISION_AT_COORD(pos.X, pos.Y, z);

    //            // Request a new scene. This will trigger the world to be loaded around that area.
    //            NativeFunction.Natives.NEW_LOAD_SCENE_START(pos.X, pos.Y, z, pos.X, pos.Y, z, 50f, 0);

    //            // Timer to make sure things don't get out of hand (player having to wait forever to get teleported if something fails).
    //            uint tempTimer = Game.GameTime;

    //            // Wait for the new scene to be loaded.
    //            while (NativeFunction.Natives.IS_NETWORK_LOADING_SCENE<bool>())//  IsNetworkLoadingScene())
    //            {
    //                // If this takes longer than 1 second, just abort. It's not worth waiting that long.
    //                if (Game.GameTime - tempTimer > 1000)
    //                {
    //                    EntryPoint.WriteToConsole("Waiting for the scene to load is taking too long (more than 1s). Breaking from wait loop.");
    //                    break;
    //                }
    //                GameFiber.Yield();
    //            }

    //            // If the player is in a vehicle, teleport the vehicle to this new position.
    //            if (inVehicle())
    //            {
    //                NativeFunction.Natives.SET_ENTITY_COORDS(veh, pos.X, pos.Y, z, false, false, false, true);
    //            }
    //            // otherwise, teleport the player to this new position.
    //            else
    //            {
    //                NativeFunction.Natives.SET_ENTITY_COORDS(Game.LocalPlayer.Character, pos.X, pos.Y, z, false, false, false, true);
    //            }

    //            // Reset the timer.
    //            tempTimer = Game.GameTime;

    //            // Wait for the collision to be loaded around the entity in this new location.
    //            while (!NativeFunction.Natives.HAS_COLLISION_LOADED_AROUND_ENTITY<bool>(Game.LocalPlayer.Character))
    //            {
    //                // If this takes too long, then just abort, it's not worth waiting that long since we haven't found the real ground coord yet anyway.
    //                if (Game.GameTime - tempTimer > 1000)
    //                {
    //                    EntryPoint.WriteToConsole("Waiting for the collision is taking too long (more than 1s). Breaking from wait loop.");
    //                    break;
    //                }
    //                GameFiber.Yield();
    //            }

    //            // Check for a ground z coord.
    //            found = NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(pos.X, pos.Y, z, ref groundZ, false);

    //            // If we found a ground z coord, then teleport the player (or their vehicle) to that new location and break from the loop.
    //            if (found)
    //            {
    //                EntryPoint.WriteToConsole($"Ground coordinate found: {groundZ}");
    //                if (inVehicle())
    //                {
    //                    NativeFunction.Natives.SET_ENTITY_COORDS(veh, pos.X, pos.Y, groundZ, false, false, false, true);

    //                    // We need to unfreeze the vehicle because sometimes having it frozen doesn't place the vehicle on the ground properly.
    //                    veh.IsPositionFrozen = false;
    //                    NativeFunction.Natives.SET_VEHICLE_ON_GROUND_PROPERLY(veh);

    //                    // Re-freeze until screen is faded in again.
    //                    veh.IsPositionFrozen = true;
    //                }
    //                else
    //                {
    //                    NativeFunction.Natives.SET_ENTITY_COORDS(Game.LocalPlayer.Character, pos.X, pos.Y, groundZ, false, false, false, true);
    //                }
    //                break;
    //            }

    //            // Wait 10ms before trying the next location.
    //            GameFiber.Sleep(10);
    //        }

    //        // If the loop ends but the ground z coord has not been found yet, then get the nearest vehicle node as a fail-safe coord.
    //        if (!found)
    //        {
    //            var safePos = pos;
    //            NativeFunction.Natives.GET_NTH_CLOSEST_VEHICLE_NODE<bool>(pos.X, pos.Y, pos.Z, 0, ref safePos, 0, 0, 0);

    //            // Notify the user that the ground z coord couldn't be found, so we will place them on a nearby road instead.
    //            EntryPoint.WriteToConsole("Could not find a safe ground coord. Placing you on the nearest road instead.");
    //            EntryPoint.WriteToConsole("Could not find a safe ground coord. Placing you on the nearest road instead.");

    //            // Teleport vehicle, or player.
    //            if (inVehicle())
    //            {
    //                NativeFunction.Natives.SET_ENTITY_COORDS(veh, safePos.X, safePos.Y, safePos.Z, false, false, false, true);
    //                veh.IsPositionFrozen = false;
    //                NativeFunction.Natives.SET_VEHICLE_ON_GROUND_PROPERLY(veh);
    //                veh.IsPositionFrozen = true;
    //            }
    //            else
    //            {
    //                NativeFunction.Natives.SET_ENTITY_COORDS(Game.LocalPlayer.Character, safePos.X, safePos.Y, safePos.Z, false, false, false, true);
    //            }
    //        }

    //        // Once the teleporting is done, unfreeze vehicle or player and fade them back in.
    //        if (inVehicle())
    //        {
    //            if (vehicleRestoreVisibility)
    //            {
    //                //NetworkFadeInEntity(veh.Handle, true);
    //                //if (!pedRestoreVisibility)
    //                //{
    //                //    Game.LocalPlayer.Character.IsVisible = false;
    //                //}
    //            }
    //            veh.IsPositionFrozen = false;
    //        }
    //        else
    //        {
    //            //if (pedRestoreVisibility)
    //            //{
    //            //    NetworkFadeInEntity(Game.LocalPlayer.Character.Handle, true);
    //            //}
    //            Game.LocalPlayer.Character.IsPositionFrozen = false;
    //        }

    //        // Fade screen in and reset the camera angle.
    //        Game.FadeScreenIn(500, true);
    //        NativeFunction.Natives.SET_GAMEPLAY_CAM_RELATIVE_PITCH(0.0f, 1.0f);
    //    }

    //    // Disable safe teleporting and go straight to the specified coords.
    //    else
    //    {
    //        NativeFunction.Natives.REQUEST_COLLISION_AT_COORD(pos.X, pos.Y, pos.Z);

    //        // Teleport directly to the coords without trying to get a safe z pos.
    //        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Driver == Game.LocalPlayer.Character)
    //        {
    //            NativeFunction.Natives.SET_ENTITY_COORDS(Game.LocalPlayer.Character.CurrentVehicle, pos.X, pos.Y, pos.Z, false, false, false, true);
    //        }
    //        else
    //        {
    //            NativeFunction.Natives.SET_ENTITY_COORDS(Game.LocalPlayer.Character, pos.X, pos.Y, pos.Z, false, false, false, true);
    //        }
    //    }
    //}












    public void TeleportToDestination(SpawnLocation positionToTeleportTo)
    {
        if(positionToTeleportTo == null)
        {
            return;
        }
        Vector3 finalPos = positionToTeleportTo.FinalPosition;
        if(Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
        {
            Vehicle toTransport = Player.CurrentVehicle.Vehicle;
            //if (Settings.SettingsManager.PlayerOtherSettings.RemovePlayerFromVehicleWhenTeleporting)
            //{
            //    Game.FadeScreenOut(1000, true);
            //    int existingSeat = Player.Character.SeatIndex;
            //    Player.Character.Position = Player.Character.GetOffsetPosition(new Vector3(0f, 10f, 2f));

            //    Vehicle bike = new Vehicle("asea", Player.Character.GetOffsetPosition(new Vector3(0f, 10f, 2f)));
            //    if (bike.Exists())
            //    {
            //        Player.Character.WarpIntoVehicle(bike, -1);
            //    }





            //    GameFiber.Sleep(500);
            //    if (bike.Exists())
            //    {
            //        bike.Delete();
            //    }

            //    if (toTransport.Exists())
            //    {
            //        toTransport.Position = finalPos;
            //    }
            //    Player.Character.Position = finalPos.Around2D(5f) + new Vector3(0f, 0f, 2f);
            //    GameFiber.Sleep(500);
            //    if (toTransport.Exists())
            //    {

            //        Player.Character.WarpIntoVehicle(toTransport, existingSeat);
            //    }
            //    Game.FadeScreenIn(1000, true);
            //    //Player.Character.Position = finalPos.Around2D(5f) + new Vector3(0f, 0f, 10f);

            //}
            //else
            //{
                Game.FadeScreenOut(500, true);
            toTransport.Position = finalPos;// + new Vector3(0f,0f,1000f);

            EntryPoint.WriteToConsole($"TELEPORTING TO {finalPos}");
            NativeFunction.Natives.PLACE_OBJECT_ON_GROUND_PROPERLY(toTransport);
            GameFiber.Sleep(1000);

            Game.FadeScreenIn(500, true);
            //}
        }
        else
        {
            Game.FadeScreenOut(500, true);
            Player.Character.Position = finalPos;
            GameFiber.Sleep(1000);
            Game.FadeScreenIn(500, true);
        }
    }

    public void TeleportToMarker()
    {
        Vector3 markerPos = GetGPSRoutePosition();
        if(markerPos == Vector3.Zero)
        {
            Game.DisplaySubtitle("No Marker Set");
            return;
        }
        TeleportToCoords(markerPos,0f, false, false, 0);
        //TeleportToDestination(new SpawnLocation(markerPos));
    }



    //FROM Dilapidated
    public Vector3 ForceGroundZ(Vector3 v)
    {
        float zcoord = 0.0f;
        //var outArgb = new OutputArgument();


        List<float> firstCheck = new List<float> { 0, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };

        List<float> secondCheck = new List<float> { 1000, 900, 800, 700, 600, 500,
            400, 300, 200, 100, 0, -100, -200, -300, -400, -500 };

        List<float> thirdCheck = new List<float> { -500, -400, -300, -200, -100, 0,
            100, 200, 300, 400, 500, 600, 700, 800, 900, 1000};

        List<Tuple<int,List<float>>> AllList = new List<Tuple<int, List<float>>>() { new Tuple<int, List<float>>(0,firstCheck), new Tuple<int, List<float>>(1, secondCheck), new Tuple<int, List<float>>(2, thirdCheck) };


        if(NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(v.X, v.Y, 1000f, out float GroundZ, true, false))
        {
            zcoord = GroundZ;
            EntryPoint.WriteToConsole($"ForceGroundZ1 FOUND Z: {zcoord}");
            return new Vector3(v.X, v.Y, zcoord);
        }
        foreach(Tuple<int,List<float>> tuple in AllList.OrderBy(x=> x.Item1))
        {
            foreach(float i in tuple.Item2)
            {
                NativeFunction.Natives.REQUEST_COLLISION_AT_COORD(v.X, v.Y, i);
                GameFiber.Yield();
            }
            if (NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(v.X, v.Y, 1000f, out float GroundZ2, true, false))
            {
                zcoord = GroundZ2;
                EntryPoint.WriteToConsole($"ForceGroundZ2 FOUND Z: {zcoord}");
                return new Vector3(v.X, v.Y, zcoord);
            }
        }




        //if (zcoord == 0)
        //{
        //    for (int i = 0; i < firstCheck.Length; i++)
        //    {
        //        Function.Call(Hash.REQUEST_COLLISION_AT_COORD, v.X, v.Y, firstCheck[i]);
        //        GTA.Script.Wait(0);
        //    }

        //    if (Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, v.X, v.Y, 1000f, outArgb))
        //        zcoord = outArgb.GetResult<float>();
        //}

        //if (zcoord == 0)
        //{
        //    Log.Write(true, "ZCoord secondCheck");

        //    for (int i = 0; i < secondCheck.Length; i++)
        //    {
        //        Function.Call(Hash.REQUEST_COLLISION_AT_COORD, v.X, v.Y, secondCheck[i]);
        //        GTA.Script.Wait(0);
        //    }

        //    if (Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, v.X, v.Y, 1000f, outArgb))
        //        zcoord = outArgb.GetResult<float>();
        //}

        //if (zcoord == 0)
        //{
        //    Log.Write(true, "ZCoord thirdCheck");

        //    for (int i = 0; i < thirdCheck.Length; i++)
        //    {
        //        Function.Call(Hash.REQUEST_COLLISION_AT_COORD, v.X, v.Y, thirdCheck[i]);
        //        GTA.Script.Wait(0);
        //    }

        //    if (Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, v.X, v.Y, 1000f, outArgb))
        //        zcoord = outArgb.GetResult<float>();
        //}
        EntryPoint.WriteToConsole("FORCEGORUNDz DIDNT FIND ANYTHING");
        return new Vector3(v.X, v.Y, zcoord);
    }

    ///// <summary>
    /////     Forces Water Z position even when the location doesn't have collisions loaded
    ///// </summary>
    //public Vector3 ForceWaterZ(this Vector3 v)
    //{
    //    float zcoord = -500.0f;
    //    var outArgb = new OutputArgument();


    //    float[] firstCheck = new float[] { 0, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };

    //    float[] secondCheck = new float[] { 1000, 900, 800, 700, 600, 500,
    //        400, 300, 200, 100, 0, -100, -200, -300, -400, -500 };

    //    float[] thirdCheck = new float[] { -500, -400, -300, -200, -100, 0,
    //        100, 200, 300, 400, 500, 600, 700, 800, 900, 1000};

    //    if (Function.Call<bool>(Hash.TEST_PROBE_AGAINST_ALL_WATER, v.X, v.Y, 1000f, v.X, v.Y, -500f, 1, outArgb))
    //        zcoord = outArgb.GetResult<Vector3>().Z;

    //    if (zcoord == -500)
    //    {
    //        for (int i = 0; i < firstCheck.Length; i++)
    //        {
    //            Function.Call(Hash.REQUEST_COLLISION_AT_COORD, v.X, v.Y, firstCheck[i]);
    //            GTA.Script.Wait(0);
    //        }

    //        if (Function.Call<bool>(Hash.TEST_PROBE_AGAINST_ALL_WATER, v.X, v.Y, 1000f, v.X, v.Y, -500f, 1, outArgb))
    //            zcoord = outArgb.GetResult<Vector3>().Z;
    //    }

    //    if (zcoord == -500)
    //    {
    //        Log.Write(true, "ZCoord secondCheck");

    //        for (int i = 0; i < secondCheck.Length; i++)
    //        {
    //            Function.Call(Hash.REQUEST_COLLISION_AT_COORD, v.X, v.Y, secondCheck[i]);
    //            GTA.Script.Wait(0);
    //        }

    //        if (Function.Call<bool>(Hash.TEST_PROBE_AGAINST_ALL_WATER, v.X, v.Y, 1000f, v.X, v.Y, -500f, 1, outArgb))
    //            zcoord = outArgb.GetResult<Vector3>().Z;

    //    }

    //    if (zcoord == -500)
    //    {
    //        Log.Write(true, "ZCoord thirdCheck");

    //        for (int i = 0; i < thirdCheck.Length; i++)
    //        {
    //            Function.Call(Hash.REQUEST_COLLISION_AT_COORD, v.X, v.Y, thirdCheck[i]);
    //            GTA.Script.Wait(0);
    //        }

    //        if (Function.Call<bool>(Hash.TEST_PROBE_AGAINST_ALL_WATER, v.X, v.Y, 1000f, v.X, v.Y, -500f, 1, outArgb))
    //            zcoord = outArgb.GetResult<Vector3>().Z;
    //    }

    //    return new Vector3(v.X, v.Y, zcoord);
    //}



    ///// <summary>
    /////     Forces Water Depth even when the location doesn't have collisions loaded 
    ///// </summary>
    //public float ForceWaterDepth(this Vector3 v)
    //{
    //    float result = 0.0f;
    //    var ground = v.ForceGroundZ().Z;
    //    var water = v.ForceWaterZ().Z;

    //    if (water > ground)
    //        result = water - ground;

    //    if (ground == 0)
    //        result = 500f;

    //    return result;
    //}












}
