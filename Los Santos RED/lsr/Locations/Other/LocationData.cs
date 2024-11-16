using LosSantosRED.lsr.Interface;
using NAudio.Gui;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Locations
{
    public class LocationData
    {
        private Vector3 ClosestNode;


        private ISettingsProvideable Settings;
        private IStreets Streets;
        private IZones Zones;
        private Zone PreviousZone;
        private uint GameTimeEnteredZone;
        private int InteriorID;
        private int PrevInteriorID = -1;
        private IInteriors Interiors;
        private uint GameTimeGotOnFreeway;
        private uint GameTimeGotOffFreeway;
        private bool CurrentStreetIsHighway = false;
        private bool HasThrownGotOffFreeway;
        private bool HasThrownGotOnFreeway;
        private bool isCurrentlyOffroad;
        private uint GameTimeGotOffRoad;
        private uint GameTimeGotOnRoad;

        private uint GameTimeStartedStationary;
        private Vector3 StationaryPosition;


        private uint GameTimeWentInside;
        private uint GameTimeWentOutside;
        private uint GameTimeLastShowed;
        private bool treatAsInTunnel;
        private uint GameTimeWentInTunnel;
        private uint GameTimeLeftTunnel;
        private bool isMostlyStationary;
        private bool isVeryStationary;
        private bool IsSetFakeZone;
        private string FakeZoneName;
        private List<int> Nodes = new List<int>();

        public LocationData(Entity characterToLocate, IStreets streets, IZones zones, IInteriors interiors, ISettingsProvideable settings)
        {
            Streets = streets;
            Zones = zones;
            Interiors = interiors;
            EntityToLocate = characterToLocate;
            Settings= settings;
        }
        public int ClosestNodeID { get; private set; }
        public Interior CurrentInterior { get; private set; }
        public Entity EntityToLocate { get; set; }
        public Street CurrentStreet { get; private set; }
        public Street CurrentCrossStreet { get; private set; }
        public Zone CurrentZone { get; private set; }
        public bool IsOffroad { get; private set; }
        public bool IsInside => InteriorID != 0 &&  CurrentInterior != null;
        public uint GameTimeInZone => GameTimeEnteredZone == 0 ? 0 : Game.GameTime - GameTimeEnteredZone;
        public bool IsOnFreeway => CurrentStreet != null && CurrentStreet.IsHighway;

        public bool PossiblyInTunnel { get; private set; } //=> IsInside && Game.LocalPlayer.Character.Position.Z <= Settings.SettingsManager.DebugSettings.TunnelZValueMax;// -10.f;
        public bool IsInTunnel { get; private set; }

        public bool TreatAsInTunnel => IsInTunnel || PossiblyInTunnel;

        public bool IsByTunnelNode { get; private set; }
        public Vector3 ClosestRoadNode => ClosestNode;
        public int ClosestRoadNodeID => ClosestNodeID;
        public float ClosestNodeHeading { get; private set; }
        public string NodeString { get; set; }
        public uint TimeInside => IsInside && GameTimeWentInside != 0 ? Game.GameTime - GameTimeWentInside : 0;
        public uint TimeOutside => !IsInside && GameTimeWentOutside != 0 ? Game.GameTime - GameTimeWentOutside : 0;

        public bool HasBeenInTunnel => TreatAsInTunnel && GameTimeWentInTunnel != 0 && Game.GameTime - GameTimeWentInTunnel >= 5000;

        public bool HasBeenOffRoad => isCurrentlyOffroad && GameTimeGotOffRoad != 0 && Game.GameTime - GameTimeGotOffRoad >= 15000;
        public bool HasBeenOnHighway => CurrentStreetIsHighway && GameTimeGotOnFreeway != 0 && Game.GameTime - GameTimeGotOnFreeway >= 5000;
        public bool HasBeenOffHighway => !CurrentStreetIsHighway && GameTimeGotOffFreeway != 0 && Game.GameTime - GameTimeGotOffFreeway >= 5000 && !HasThrownGotOffFreeway;

        public bool IsMostlyStationary => GameTimeStartedStationary != 0 && Game.GameTime - GameTimeStartedStationary >= Settings.SettingsManager.PlayerOtherSettings.StationaryTime;

        public bool IsVeryStationary => GameTimeStartedStationary != 0 && Game.GameTime - GameTimeStartedStationary >= Settings.SettingsManager.PlayerOtherSettings.VeryStationaryTime;

        public uint StationaryTime => GameTimeStartedStationary == 0 ? 0 : Game.GameTime - GameTimeStartedStationary;


        public void Update(Entity entityToLocate, bool isInVehicle)
        {
            if (entityToLocate.Exists())
            {
                EntityToLocate = entityToLocate;
            }
            if (EntityToLocate.Exists())
            {
                UpdateZone();
                GameFiber.Yield();
                UpdateInterior();
                GameFiber.Yield();
                UpdateNode(isInVehicle);
                GameFiber.Yield();
                UpdateStreets(isInVehicle);
                GameFiber.Yield();
                UpdateStationary();
            }
            else
            {
                CurrentZone = null;
                CurrentStreet = null;
                CurrentCrossStreet = null;
                CurrentInterior = null;
                InteriorID = 0;
                PrevInteriorID = -1;
                GameTimeGotOffFreeway = 0;
                GameTimeGotOnFreeway = 0;
                GameTimeGotOffRoad = 0;
                GameTimeGotOnRoad = 0;
                GameTimeWentInside = 0;
                GameTimeWentOutside = 0;
                if (isCurrentlyOffroad)
                {
                    GameTimeGotOffRoad = Game.GameTime;
                    isCurrentlyOffroad = false;
                }
                if (CurrentStreetIsHighway)
                {
                    GameTimeGotOffFreeway = Game.GameTime;
                    CurrentStreetIsHighway = false;
                }
            }
        }

        private void UpdateStationary()
        {
            if(!EntityToLocate.Exists())
            {
                return;
            }
            if(StationaryPosition == Vector3.Zero)
            {
                StationaryPosition = EntityToLocate.Position;
            }
            if(EntityToLocate.Position.DistanceTo(StationaryPosition) >= Settings.SettingsManager.PlayerOtherSettings.StationaryDistance)
            {
                StationaryPosition = EntityToLocate.Position;
                GameTimeStartedStationary = 0;
            }
            else if (GameTimeStartedStationary == 0)
            {
                GameTimeStartedStationary = Game.GameTime;
                //EntryPoint.WriteToConsole("You STARTED BECOMING StationaryISH");
            }

            if(isMostlyStationary != IsMostlyStationary)
            {
                EntryPoint.WriteToConsole($"OnIsMostlyStationaryChanged IsMostlyStationary:{IsMostlyStationary}");
                isMostlyStationary = IsMostlyStationary;
            }
            if(isVeryStationary != IsVeryStationary)
            {
                EntryPoint.WriteToConsole($"OnIsVeryStationaryChanged IsVeryStationary:{IsVeryStationary}");
                isVeryStationary = IsVeryStationary;
            }
        }

        private void UpdateZone()
        {
            if (EntityToLocate.Exists())
            {
                if(!IsSetFakeZone || string.IsNullOrEmpty(FakeZoneName))
                {
                    CurrentZone = Zones.GetZone(EntityToLocate.Position);
                }
                else
                {
                    CurrentZone = Zones.GetZone(FakeZoneName);
                }

                
                if (PreviousZone == null || CurrentZone.InternalGameName != PreviousZone.InternalGameName)
                {
                    GameTimeEnteredZone = Game.GameTime;
                    if(PreviousZone != null && CurrentZone != null && PreviousZone.StateID != CurrentZone.StateID)
                    {
                        OnChangedState();
                    }
                    PreviousZone = CurrentZone;
                }
            }
        }
        private void OnChangedState()
        {
            string prevState = "";
            string currState = "";
            if (PreviousZone != null)
            {
                prevState = PreviousZone.StateID;
            }
            if(CurrentZone != null)
            {
                currState = CurrentZone.StateID;
            }
            //EntryPoint.WriteToConsoleTestLong($"PLAYER EVENT: STATE CHANGED FROM {prevState} TO {currState}");
        }
        private void UpdateInterior()
        {
            InteriorID = NativeFunction.Natives.GET_INTERIOR_FROM_ENTITY<int>(EntityToLocate);
            if (PrevInteriorID != InteriorID)
            {
                GetInteriorFromID();
                if (PrevInteriorID == 0 && InteriorID != 0)
                {
                    OnWentInside();
                }
                else if (PrevInteriorID != 0 && InteriorID == 0)
                {
                    OnWentOutside();
                }
                PrevInteriorID = InteriorID;
            }
        }
        private void UpdateNode(bool isInVehicle)
        {
            IsByTunnelNode = false;

            if (EntityToLocate.Exists())
            {
                Vector3 playerpos = EntityToLocate.Position;
                float groundZ = 850.0f;
                bool foundGround = NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(playerpos.X, playerpos.Y, playerpos.Z + 850f, ref groundZ, false);

                if (foundGround && groundZ >= playerpos.Z + Math.Abs(Settings.SettingsManager.PlayerOtherSettings.TunnelZValueMax))
                {

                    PossiblyInTunnel = true;

                }
                else
                {
                    PossiblyInTunnel = false;
                }
            }
            else
            {
                PossiblyInTunnel = false;
            }


            if (treatAsInTunnel != TreatAsInTunnel)
            {
                if (TreatAsInTunnel)
                {
                    OnWentInTunnel();
                }
                else
                {
                    OnLeftTunnel();
                }
                treatAsInTunnel = TreatAsInTunnel;
            }




            if (EntityToLocate.Exists() && (!IsInside || isInVehicle))
            {
                Vector3 position = EntityToLocate.Position;//Game.LocalPlayer.Character.Position;
                Vector3 outPos;
                float outHeading;
               // bool hasNode = NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE<bool>(position.X, position.Y, position.Z, out outPos, 0, 3.0f, 0f);
                bool hasNode = NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING<bool>(position.X, position.Y, position.Z, out outPos, out outHeading, 0, 3.0f, 0f);
                bool hasProperties = false;
                //int busy;
                //int flags = 0;
                //if (hasNode)
                //{
                //    hasProperties = NativeFunction.Natives.GET_VEHICLE_NODE_PROPERTIES<bool>(ClosestNode.X, ClosestNode.Y, ClosestNode.Z, out busy, out flags);
                //}
                //if (hasProperties)
                //{
                //    VEHICLE_NODE_PROPERTIES coolFlag = (VEHICLE_NODE_PROPERTIES)flags;
                //    if (coolFlag.HasFlag(VEHICLE_NODE_PROPERTIES.VNP_TUNNEL_OR_INTERIOR))
                //    {
                //        IsByTunnelNode = true;
                //        EntryPoint.WriteToConsole($"NEAR A TUNNEL OR INSIDE ROAD NODE {flags}");
                //    }
                //    else
                //    {
                //        IsByTunnelNode = false;
                //        EntryPoint.WriteToConsole($"NOT NEAR A TUNNEL OR INSIDE ROAD NODE {flags}");
                //    }
                //}
                //else
                //{
                //    IsByTunnelNode = false;
                //}
                ClosestNode = outPos;
                ClosestNodeHeading = outHeading;
                if (!hasNode || ClosestNode == Vector3.Zero || ClosestNode.DistanceTo(EntityToLocate) >= 15f)//was 15f
                {
                    IsOffroad = true;
                }
                else
                {
                    IsOffroad = false;
                }
            }
            else
            {
                IsOffroad = true;
            }

            if (isCurrentlyOffroad != IsOffroad)
            {
                if (IsOffroad)
                {
                    OnWentOffRoad();
                }
                else
                {
                    OnGotOnRoad();
                }
                isCurrentlyOffroad = IsOffroad;
            }
        }



        private void UpdateStreets(bool isInVehicle)
        {
            if (IsOffroad || (IsInside && !isInVehicle))
            {
                OnLeftRoad();
                return;
            }

            int StreetHash = 0;
            int CrossingHash = 0;
            string CurrentStreetName;
            string CurrentCrossStreetName;
            unsafe
            {
                NativeFunction.CallByName<uint>("GET_STREET_NAME_AT_COORD", ClosestNode.X, ClosestNode.Y, ClosestNode.Z, &StreetHash, &CrossingHash);
            }
            string StreetName = string.Empty;
            if (StreetHash != 0)
            {
                unsafe
                {
                    IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", StreetHash);
                    StreetName = Marshal.PtrToStringAnsi(ptr);
                }
                CurrentStreetName = StreetName;
                GameFiber.Yield();
            }
            else
            {
                CurrentStreetName = "";
            }

            string CrossStreetName = string.Empty;
            if (CrossingHash != 0)
            {
                unsafe
                {
                    IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", CrossingHash);
                    CrossStreetName = Marshal.PtrToStringAnsi(ptr);
                }
                CurrentCrossStreetName = CrossStreetName;
                GameFiber.Yield();
            }
            else
            {
                CurrentCrossStreetName = "";
            }

           // int NodeID = 0;
            if (StreetHash == 0 && CrossingHash == 0 && ClosestNode != Vector3.Zero)
            {
                //NodeID = NativeFunction.Natives.GET_NTH_CLOSEST_VEHICLE_NODE_ID<int>(ClosestNode.X, ClosestNode.Y, ClosestNode.Z, 0, 0, 3.0f, 0f);
                

                //if(!Nodes.Contains(NodeID))
                //{
                //    EntryPoint.WriteToConsole($"No Street Detected, NodeID {NodeID}");

                //    Nodes.Add(NodeID);

                //    WriteToNodesLog(NodeID.ToString());



                //}
                //if (NodeID != 0)
                //{
                //    CurrentStreet = Streets.GetStreet(NodeID);
                //}
            }
            else
            {
                CurrentStreet = Streets.GetStreet(CurrentStreetName);
                CurrentCrossStreet = Streets.GetStreet(CurrentCrossStreetName);
            }

            GameFiber.Yield();

            if (CurrentStreet == null)
            {
                CurrentStreet = new Street(CurrentStreetName, 60f, "MPH");
            }

            if (CurrentStreetIsHighway != CurrentStreet.IsHighway)
            {
                if (CurrentStreet.IsHighway)
                {
                    OnGotOnFreeway();
                }
                else
                {
                    OnGotOffFreeway();
                }
                CurrentStreetIsHighway = CurrentStreet.IsHighway;
            }
        }

        private void WriteToNodesLog(String TextToLog)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(TextToLog + System.Environment.NewLine);
            File.AppendAllText("Plugins\\LosSantosRED\\" + "Nodes.txt", sb.ToString());
            sb.Clear();
        }

        public string GetStreetAndZoneString()
        {
            string streetName = "";
            string zoneName = "";

            if (CurrentStreet != null)
            {
                streetName = $"~HUD_COLOUR_YELLOWLIGHT~{CurrentStreet.ProperStreetName}~s~";
                if (CurrentCrossStreet != null)
                {
                    streetName += " at ~HUD_COLOUR_YELLOWLIGHT~" + CurrentCrossStreet.ProperStreetName + "~s~ ";
                }
                else
                {
                    streetName += " ";
                }
            }
            if (CurrentZone != null)
            {
                zoneName = CurrentZone.IsSpecificLocation ? "near ~p~" : "in ~p~" + CurrentZone.DisplayName + "~s~";
            }
            return streetName + zoneName;
        }
        private void OnGotOnRoad()
        {
            GameTimeGotOffRoad = 0;
            GameTimeGotOnRoad = Game.GameTime;
        }
        private void OnWentOffRoad()
        {
            GameTimeGotOffRoad = Game.GameTime;
            GameTimeGotOnRoad = 0;
        }
        private void OnGotOnFreeway()
        {
            GameTimeGotOnFreeway = Game.GameTime;
            GameTimeGotOffFreeway = 0;
            //EntryPoint.WriteToConsole("LocationData: Got ON Freeway", 5);
        }
        private void OnGotOffFreeway()
        {
            GameTimeGotOffFreeway = Game.GameTime;
            GameTimeGotOnFreeway = 0;
            //EntryPoint.WriteToConsole("LocationData: Got OFF Freeway", 5);
        }
        private void OnLeftRoad()
        {
            CurrentStreet = null;
            CurrentCrossStreet = null;
            if (CurrentStreetIsHighway)
            {
                OnGotOffFreeway();
                CurrentStreetIsHighway = false;
            }
        }

        private void OnLeftTunnel()
        {
            GameTimeWentInTunnel = 0;
            GameTimeLeftTunnel = Game.GameTime;
        }

        private void OnWentInTunnel()
        {
            GameTimeWentInTunnel = Game.GameTime;
            GameTimeLeftTunnel = 0;
        }

        private void OnWentInside()
        {
            GameTimeWentInside = Game.GameTime;
            GameTimeWentOutside = 0;
            if(CurrentInterior != null && CurrentInterior.IsTunnel)
            {
                IsInTunnel = true;
            }
            //EntryPoint.WriteToConsoleTestLong("PLAYER EVENT: WENT INSIDE");
        }
        private void OnWentOutside()
        {
            GameTimeWentInside = 0;
            GameTimeWentOutside = Game.GameTime;
            IsInTunnel = false;
            //EntryPoint.WriteToConsoleTestLong("PLAYER EVENT: WENT OUTSIDE");
        }
        private void GetInteriorFromID()
        {
            if (InteriorID == 0)
            {
                CurrentInterior = new Interior(0, "");
            }
            else
            {
                CurrentInterior = Interiors?.GetInteriorByInternalID(InteriorID);
                if (CurrentInterior == null)
                {
                    CurrentInterior = new Interior(InteriorID, "");
                }
            }
        }

        public void UpdatePlayer()
        {
            if (EntityToLocate.Exists() && EntityToLocate.Handle != Game.LocalPlayer.Character.Handle)
            {
                EntityToLocate = Game.LocalPlayer.Character;
            }
        }

        public void SetFakeZone(string v)
        {
            IsSetFakeZone = true;
            FakeZoneName = v;
        }

        public void ClearFakeZone()
        {
            IsSetFakeZone = false;
            FakeZoneName = "";
        }
    }

}
public enum VEHICLE_NODE_PROPERTIES
{
    VNP_OFF_ROAD = 1,                   // node has been flagged as 'off road', suitable only for 4x4 vehicles, etc
    VNP_ON_PLAYERS_ROAD = 2,                    // node has been dynamically marked as somewhere ahead, possibly on (or near to) the player's current road
    VNP_NO_BIG_VEHICLES = 4,                    // node has been marked as not suitable for big vehicles
    VNP_SWITCHED_OFF = 8,                   // node is switched off for ambient population
    VNP_TUNNEL_OR_INTERIOR = 16,                    // node is in a tunnel or an interior
    VNP_LEADS_TO_DEAD_END = 32,                 // node is, or leads to, a dead end
    VNP_HIGHWAY = 64,                   // node is marked as highway
    VNP_JUNCTION = 128,                 // node qualifies as junction
    VNP_TRAFFIC_LIGHT = 256,                    // node's special function is traffic-light
    VNP_GIVE_WAY = 512,                 // node's special function is give-way	
    VNP_WATER = 1024                    // node is water/boat
}


/*        private void UpdateNode()
        {
            if(!EntityToLocate.Exists())
            {
                return;
            }
            if (!IsInside)
            {
                Vector3 position = EntityToLocate.Position;//Game.LocalPlayer.Character.Position;
                Vector3 outPos;
                bool hasNode = false;
                hasNode = NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE<bool>(position.X, position.Y, position.Z, out outPos, 0, 3.0f, 0f);
                //hasNode = NativeFunction.Natives.GET_NTH_CLOSEST_VEHICLE_NODE<bool>(position.X, position.Y, position.Z, 1, out outPos, 1, 0x40400000, 0);//can still get the freeway offramp when you are driving near it, not sure what to do about it!
                ClosestNode = outPos;
                //ClosestNodeID = NativeFunction.Natives.GET_NTH_CLOSEST_VEHICLE_NODE_ID<int>(position.X, position.Y, position.Z, 1 , 1, 30f, 30f);
                //int busy = -1;
                //int flags = -1;
                //bool hasProperties = false;
                //if(hasNode)
                //{
                //    hasProperties = NativeFunction.Natives.GET_VEHICLE_NODE_PROPERTIES<bool>(ClosestNode.X, ClosestNode.Y, ClosestNode.Z, out busy, out flags);
                //}
                //if(hasProperties)
                //{
                //    NodeString = $"busy {busy} flags {flags}";
                //}
                //else
                //{
                //    NodeString = "";
                //}
                //ClosestNode = Rage.World.GetNextPositionOnStreet(CharacterToLocate.Position);//seems to not get the z coordinate and puts me way down on whatever is lowest
                if (!hasNode || ClosestNode == Vector3.Zero || ClosestNode.DistanceTo(EntityToLocate) >= 15f)//was 15f
                {
                    IsOffroad = true;
                }
                else
                {
                    IsOffroad = false;
                }
            }
            else
            {
                IsOffroad = true;
            }

            if (isCurrentlyOffroad != IsOffroad)
            {
                if (IsOffroad)
                {
                    OnWentOffRoad();
                }
                else
                {
                    OnGotOnRoad();
                }
                isCurrentlyOffroad = IsOffroad;
            }
        }*/