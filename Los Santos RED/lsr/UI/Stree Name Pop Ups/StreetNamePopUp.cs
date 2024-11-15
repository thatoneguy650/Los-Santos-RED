using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Debug;
using LosSantosRED.lsr.Interface;

public class StreetNamePopUp
{
    private List<StreetNode> StreetNodes = new List<StreetNode>();
    private Vector3 CenterPosition;
    private int globalScaleformID;
    private ISettingsProvideable Settings;
    private IStreets Streets;
    private StreetNode streetNode;

    public StreetNamePopUp(ISettingsProvideable settings, IStreets streets)
    {
        Settings = settings;
        Streets = streets;
    }

    public void GetNodes()
    {
        //StreetNodes.Clear();
        //CenterPosition = Game.LocalPlayer.Character.GetOffsetPositionFront(Settings.SettingsManager.DebugSettings.StreetDisplayNodeOffsetFront);
        //int gotten = 0;
        //for (int i = 0; i < Settings.SettingsManager.DebugSettings.StreetDisplayNodesToGet; i++)
        //{
        //    Vector3 outPos;
        //    float outHeading;
        //    bool hasNode = NativeFunction.Natives.GET_NTH_CLOSEST_VEHICLE_NODE_FAVOUR_DIRECTION<bool>(CenterPosition.X, CenterPosition.Y, CenterPosition.Z, Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z
        //    , i, out outPos, out outHeading, 0, 0x40400000, 0);



        //    if(hasNode)
        //    {
        //        int density = 0;
        //        int flags = 0;
        //        NativeFunction.Natives.GET_VEHICLE_NODE_PROPERTIES<bool>(outPos.X, outPos.Y, outPos.Z, out density, out flags);
        //        eVehicleNodeProperties nodeProperties = (eVehicleNodeProperties)flags;

        //        StreetNode streetNode = new StreetNode(outPos, outHeading, Settings, Streets) { eVehicleNodeProperties = nodeProperties, Density = density };
        //        streetNode.GetDistance();
        //        StreetNodes.Add(streetNode);
        //    }
        //    if(gotten > 5)
        //    {
        //        gotten = 0;
        //        GameFiber.Yield();
        //    }
        //    gotten++;
        //}
        //streetNode = StreetNodes.Where(x => x.DistanceToPlayer >= Settings.SettingsManager.DebugSettings.StreetDisplayMinNodeDistance && x.DistanceToPlayer <= Settings.SettingsManager.DebugSettings.StreetDisplayMaxNodeDistance && x.eVehicleNodeProperties.HasFlag(eVehicleNodeProperties.VNP_ON_PLAYERS_ROAD) && (x.eVehicleNodeProperties.HasFlag(eVehicleNodeProperties.VNP_JUNCTION) || x.eVehicleNodeProperties.HasFlag(eVehicleNodeProperties.VNP_TRAFFIC_LIGHT))).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
        //if(streetNode == null)
        //{
        //    return;
        //}
        //streetNode.GetStreetsAtPos();

    }

    public void DisplayNodes(int globalScaleformID)
    {
        
        if(streetNode == null)
        {
            return;
        }
        streetNode.Display(globalScaleformID);
    }
}

