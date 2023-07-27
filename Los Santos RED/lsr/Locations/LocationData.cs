using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Locations
{
    public class LocationData
    {
        private Vector3 ClosestNode;

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


        private uint GameTimeWentInside;
        private uint GameTimeWentOutside;
        public LocationData(Entity characterToLocate, IStreets streets, IZones zones, IInteriors interiors)
        {
            Streets = streets;
            Zones = zones;
            Interiors = interiors;
            EntityToLocate = characterToLocate;
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
        public Vector3 ClosestRoadNode => ClosestNode;
        public int ClosestRoadNodeID => ClosestNodeID;
        public string NodeString { get; set; }
        public uint TimeInside => IsInside && GameTimeWentInside != 0 ? Game.GameTime - GameTimeWentInside : 0;
        public uint TimeOutside => !IsInside && GameTimeWentOutside != 0 ? Game.GameTime - GameTimeWentOutside : 0;
        public bool HasBeenOffRoad => isCurrentlyOffroad && GameTimeGotOffRoad != 0 && Game.GameTime - GameTimeGotOffRoad >= 15000;
        public bool HasBeenOnHighway => CurrentStreetIsHighway && GameTimeGotOnFreeway != 0 && Game.GameTime - GameTimeGotOnFreeway >= 5000;
        public bool HasBeenOffHighway => !CurrentStreetIsHighway && GameTimeGotOffFreeway != 0 && Game.GameTime - GameTimeGotOffFreeway >= 5000 && !HasThrownGotOffFreeway;
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

        private void UpdateZone()
        {
            if (EntityToLocate.Exists())
            {
                CurrentZone = Zones.GetZone(EntityToLocate.Position);
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
            if (EntityToLocate.Exists() && (!IsInside || isInVehicle))
            {
                Vector3 position = EntityToLocate.Position;//Game.LocalPlayer.Character.Position;
                Vector3 outPos;
                bool hasNode = NativeFunction.Natives.GET_CLOSEST_VEHICLE_NODE<bool>(position.X, position.Y, position.Z, out outPos, 0, 3.0f, 0f);
                ClosestNode = outPos;
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

            CurrentStreet = Streets.GetStreet(CurrentStreetName);
            CurrentCrossStreet = Streets.GetStreet(CurrentCrossStreetName);

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
        private void OnWentInside()
        {
            GameTimeWentInside = Game.GameTime;
            GameTimeWentOutside = 0;
            //EntryPoint.WriteToConsoleTestLong("PLAYER EVENT: WENT INSIDE");
        }
        private void OnWentOutside()
        {
            GameTimeWentInside = 0;
            GameTimeWentOutside = Game.GameTime;
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
    }
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