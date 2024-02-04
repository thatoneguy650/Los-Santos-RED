using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SpawnLocation
{
    private float WaterHeight = 0.0f;
    public SpawnLocation()
    {

    }
    public SpawnLocation(Vector3 initialPosition)
    {
        InitialPosition = initialPosition;
    }
    public bool HasStreetPosition { get; private set; }
    public bool HasSideOfRoadPosition { get; private set; }
    public bool HasSpawns => InitialPosition != Vector3.Zero && (IsWater || StreetPosition != Vector3.Zero);
    public float Heading { get; set; }
    public Vector3 InitialPosition { get; set; } = Vector3.Zero;
    public Vector3 StreetPosition { get; set; } = Vector3.Zero;
    public Vector3 SidewalkPosition { get; set; } = Vector3.Zero;
    public bool HasSidewalk => SidewalkPosition != Vector3.Zero;
    public bool IsWater { get; private set; } = false;
    public Vector3 FinalPosition => IsWater || !HasStreetPosition ? InitialPosition : StreetPosition;
    public bool HasRoadBoundaryPosition { get; set; } = false;
    public Vector3 RoadBoundaryPosition { get; set; } = Vector3.Zero;
    public Vector3 WaterPosition => new Vector3(InitialPosition.X, InitialPosition.Y, IsWater ? WaterHeight + 3.0f : InitialPosition.Z);
    public float DebugWaterHeight => WaterHeight;
    public void GetWaterHeight()
    {
        if (IsWater = NativeFunction.Natives.GET_WATER_HEIGHT_NO_WAVES<bool>(InitialPosition.X, InitialPosition.Y, InitialPosition.Z, out float height))
        {
            WaterHeight = height;
        }
        else
        {
            WaterHeight = 0.0f;
        }
    }
    public void GetClosestStreet(bool favorPlayer)
    {
        Vector3 streetPosition;
        float streetHeading;
        if (favorPlayer)
        {
            HasStreetPosition = NativeFunction.Natives.GET_NTH_CLOSEST_VEHICLE_NODE_FAVOUR_DIRECTION<bool>(InitialPosition.X, InitialPosition.Y, InitialPosition.Z, Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z
                , 0, out streetPosition, out streetHeading, 0, 0x40400000, 0);
        }
        else
        {
            HasStreetPosition =  NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(InitialPosition.X, InitialPosition.Y, InitialPosition.Z, out streetPosition, out streetHeading, 0, 3, 0);
        }
      
        StreetPosition = streetPosition;
        Heading = streetHeading;
    }
    public void GetClosestSideOfRoad()
    {
        Vector3 sideOfRoad = Vector3.Zero;
        if(NativeFunction.Natives.GET_POSITION_BY_SIDE_OF_ROAD<bool>(StreetPosition.X, StreetPosition.Y, StreetPosition.Z,-1,out sideOfRoad))
        {
            HasStreetPosition = true;
            HasSideOfRoadPosition = true;
            StreetPosition = sideOfRoad;
        }
    }
    public void GetRoadBoundaryPosition()
    {
        Vector3 roadBoundaryPosition = Vector3.Zero;
        if(NativeFunction.Natives.GET_ROAD_BOUNDARY_USING_HEADING<bool>(StreetPosition.X, StreetPosition.Y, StreetPosition.Z,0f, out roadBoundaryPosition))
        {
            HasRoadBoundaryPosition = true;
            RoadBoundaryPosition = roadBoundaryPosition;
        }
    }
    public void GetClosestSidewalk()
    {
        if(IsWater)
        {
            return;
        }
        Vector3 posToSearch = InitialPosition;
        if(StreetPosition != Vector3.Zero)
        {
            posToSearch = StreetPosition;
        }
        Vector3 newSidewalkPos;
        if(NativeFunction.Natives.GET_SAFE_COORD_FOR_PED<bool>(posToSearch.X, posToSearch.Y, posToSearch.Z, true, out newSidewalkPos, 1))
        {
            SidewalkPosition = newSidewalkPos;
        }
    }
}

