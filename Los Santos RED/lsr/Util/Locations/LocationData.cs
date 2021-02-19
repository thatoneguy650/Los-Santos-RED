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

        public LocationData(Ped characterToLocate, IStreets streets, IZones zones)
        {
            Streets = streets;
            Zones = zones;
            CharacterToLocate = characterToLocate;
        }

        public Ped CharacterToLocate { get; set; }
        public Street CurrentStreet { get; private set; }
        public Street CurrentCrossStreet { get; private set; }
        public Zone CurrentZone { get; private set; }
        public bool IsOffroad { get; private set; }
        public void Update()
        {
            GetZone();
            GetNode();
            GetStreets();
        }
        private void GetZone()
        {
            CurrentZone = Zones.GetZone(CharacterToLocate.Position);
        }
        private void GetNode()
        {
            ClosestNode = Rage.World.GetNextPositionOnStreet(CharacterToLocate.Position);
            if (ClosestNode.DistanceTo2D(CharacterToLocate) >= 15f)//was 15f
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
            }
            else
            {
                CurrentCrossStreetName = "";
            }

            CurrentStreet = Streets.GetStreet(CurrentStreetName);
            CurrentCrossStreet = Streets.GetStreet(CurrentCrossStreetName);

            if (CurrentStreet == null)
            {
                CurrentStreet = new Street("Calle Sin Nombre", 60f);
            }
        }
    }
}
