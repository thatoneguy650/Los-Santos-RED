﻿using Rage;
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
        private uint GameTimeGotOnFreeway;
        private uint GameTimeGotOffFreeway;
        private Vector3 ClosestNode;
        public LocationData(Ped character)
        {
            CharacterToLocate = character;
        }
        public Ped CharacterToLocate { get; set; }
        public Street CurrentStreet { get; private set; }
        public Street CurrentCrossStreet { get; private set; }
        public Zone CurrentZone { get; private set; }
        public bool IsOffroad { get; private set; }
        public bool IsOnFreeway { get; private set; }
        public bool RecentlyGotOnFreeway
        {
            get
            {
                if (IsOnFreeway && Game.GameTime - GameTimeGotOnFreeway <= 10000)
                    return true;
                else
                    return false;
            }
        }
        public bool RecentlyGotOffFreeway
        {
            get
            {
                if (!IsOnFreeway && Game.GameTime - GameTimeGotOffFreeway <= 10000)
                    return true;
                else
                    return false;
            }
        }
        public void Update()
        {
            GetZone();
            GetNode();
            GetStreets();
        }
        private void GetZone()
        {
            CurrentZone = Mod.DataMart.Zones.GetZone(CharacterToLocate.Position);
        }
        private void GetNode()
        {
            ClosestNode = Rage.World.GetNextPositionOnStreet(CharacterToLocate.Position);
            if (ClosestNode.DistanceTo2D(CharacterToLocate) >= 15f)//was 25f
            {
                IsOffroad = true;
            }
            else
            {
                IsOffroad = false;
            }
        }
        private void GetStreets()
        {
            if (IsOffroad)
            {
                CurrentStreet = null;
                CurrentCrossStreet = null;
                IsOnFreeway = false;
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
            }
            else
                CurrentStreetName = "";

            string CrossStreetName = string.Empty;
            if (CrossingHash != 0)
            {
                unsafe
                {
                    IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_STREET_NAME_FROM_HASH_KEY", CrossingHash);
                    CrossStreetName = Marshal.PtrToStringAnsi(ptr);
                }
                CurrentCrossStreetName = CrossStreetName;
            }
            else
                CurrentCrossStreetName = "";


            CurrentStreet = Mod.DataMart.Streets.GetStreet(CurrentStreetName);
            CurrentCrossStreet = Mod.DataMart.Streets.GetStreet(CurrentCrossStreetName);

            if (CurrentStreet == null)
            {
                CurrentStreet = new Street(Mod.DataMart.Streets.GetStreet(CharacterToLocate.Position) + "?", 60f);
                if (CurrentStreet.IsHighway)
                {
                    if (!IsOnFreeway)
                        GameTimeGotOnFreeway = Game.GameTime;

                    IsOnFreeway = true;
                }
                else
                {
                    if (IsOnFreeway)
                        GameTimeGotOffFreeway = Game.GameTime;

                    IsOnFreeway = false;
                }
            }
        }
    }
}