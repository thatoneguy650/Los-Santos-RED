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
    private uint GameTimeLastCheckedRouteBlip;

    public Blip CurrentGPSBlip { get; private set; }
    public GPSManager(IDestinateable player, IEntityProvideable world, ISettingsProvideable settings)
    {
        Player = player;
        World = world;
        Settings = settings;
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
        TeleportToDestination(new SpawnLocation(markerPos));
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
