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
        private IInteriors Interiors;
        private uint GameTimeGotOnFreeway;
        private uint GameTimeGotOffFreeway;
        private bool CurrentStreetIsHighway = false;
        private bool HasThrownGotOffFreeway;
        private bool HasThrownGotOnFreeway;

        public LocationData(Entity characterToLocate, IStreets streets, IZones zones, IInteriors interiors)
        {
            Streets = streets;
            Zones = zones;
            Interiors = interiors;
            EntityToLocate = characterToLocate;
        }
        public Interior CurrentInterior { get; private set; }
        public Entity EntityToLocate { get; set; }
        public Street CurrentStreet { get; private set; }
        public Street CurrentCrossStreet { get; private set; }
        public Zone CurrentZone { get; private set; }
        public bool IsOffroad { get; private set; }
        public bool IsInside => CurrentInterior != null && CurrentInterior.ID != 0;
        public uint GameTimeInZone => GameTimeEnteredZone == 0 ? 0 : Game.GameTime - GameTimeEnteredZone;
        public bool IsOnFreeway => CurrentStreet != null && CurrentStreet.IsHighway;
        public Vector3 ClosestRoadNode => ClosestNode;
        public bool HasBeenOnHighway => CurrentStreetIsHighway && GameTimeGotOnFreeway != 0 && Game.GameTime - GameTimeGotOnFreeway >= 5000;
        public bool HasBeenOffHighway => !CurrentStreetIsHighway && GameTimeGotOffFreeway != 0 && Game.GameTime - GameTimeGotOffFreeway >= 5000 && !HasThrownGotOffFreeway;
        public void Update(Entity entityToLocate)
        {
            if(entityToLocate.Exists())
            {
                EntityToLocate = entityToLocate;
            }
            if (EntityToLocate.Exists())
            {
                GetZone();
                GameFiber.Yield();
                GetInterior();
                GameFiber.Yield();
                GetNode();
                GameFiber.Yield();
                GetStreets();
                GameFiber.Yield();
            }
            else
            {
                CurrentZone = null;
                CurrentStreet = null;
                CurrentCrossStreet = null;
                CurrentInterior = null;
                GameTimeGotOffFreeway = 0;
                GameTimeGotOnFreeway = 0;
                if (CurrentStreetIsHighway)
                {
                    GameTimeGotOffFreeway = Game.GameTime;
                    CurrentStreetIsHighway = false;
                }
            }
        }
        private void GetZone()
        {
            if (EntityToLocate.Exists())
            {
                CurrentZone = Zones.GetZone(EntityToLocate.Position);
                if(PreviousZone == null || CurrentZone.InternalGameName != PreviousZone.InternalGameName)
                {
                    GameTimeEnteredZone = Game.GameTime;
                    PreviousZone = CurrentZone;
                }
            }
        }
        private void GetNode()
        {
            if (EntityToLocate.Exists() && !IsInside)
            {
                Vector3 position = EntityToLocate.Position;//Game.LocalPlayer.Character.Position;
                Vector3 outPos;
                NativeFunction.Natives.GET_NTH_CLOSEST_VEHICLE_NODE<bool>(position.X, position.Y, position.Z, 1, out outPos, 1, 0x40400000, 0);//can still get the freeway offramp when you are driving near it, not sure what to do about it!
                ClosestNode = outPos;

                //ClosestNode = Rage.World.GetNextPositionOnStreet(CharacterToLocate.Position);//seems to not get the z coordinate and puts me way down on whatever is lowest
                if (ClosestNode == Vector3.Zero ||  ClosestNode.DistanceTo2D(EntityToLocate) >= 15f)//was 15f
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
        }
        private void GetStreets()
        {
            if (IsOffroad || IsInside)
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
                if(CurrentStreet.IsHighway)
                {
                    OnGotOnFreeway();
                }
                else
                {
                    OnGotOffFreeway();
                }
                CurrentStreetIsHighway = CurrentStreet.IsHighway;
            }
            //if(CurrentStreetIsHighway && GameTimeGotOnFreeway != 0 && Game.GameTime - GameTimeGotOnFreeway >= 5000 && !HasThrownGotOnFreeway)
            //{
            //    if (EntityToLocate.Handle == Game.LocalPlayer.Character.Handle)
            //    {
            //        Player.OnGotOnFreeway();
            //    }
            //    HasThrownGotOnFreeway = true;
            //    HasThrownGotOffFreeway = false;
            //}
            //else if (!CurrentStreetIsHighway && GameTimeGotOffFreeway != 0 && Game.GameTime - GameTimeGotOffFreeway >= 5000 && !HasThrownGotOffFreeway)
            //{
            //    if (EntityToLocate.Handle == Game.LocalPlayer.Character.Handle)
            //    {
            //        Player.OnGotOffFreeway();
            //    }
            //    HasThrownGotOnFreeway = false;
            //    HasThrownGotOffFreeway = true;
            //}

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
        private void GetInterior()
        {
            InteriorID = NativeFunction.Natives.GET_INTERIOR_FROM_ENTITY<int>(EntityToLocate);
            if(InteriorID == 0)
            {
                CurrentInterior = new Interior(0,"");
            }
            else
            {
                CurrentInterior = Interiors?.GetInterior(InteriorID);
                if(CurrentInterior == null)
                {
                    CurrentInterior = new Interior(InteriorID, "");
                }
            }
        }
    }
}
