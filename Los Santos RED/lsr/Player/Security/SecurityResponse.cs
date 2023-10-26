using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
namespace LosSantosRED.lsr
{
    public class SecurityResponse
    {
        public List<CrimeEvent> CrimesObserved = new List<CrimeEvent>();
        private IPoliceRespondable Player;
        private ISettingsProvideable Settings;
        private ITimeReportable Time;
        private IEntityProvideable World;

        public SecurityResponse(IPoliceRespondable player, ISettingsProvideable settings, ITimeReportable time, IEntityProvideable world)
        {
            Player = player;
            Settings = settings;
            Time = time;
            World = world;
        }
        public void Dispose()
        {

        }
        public void Reset()
        {
            CrimesObserved.Clear();
            foreach (SecurityGuard sg in World.Pedestrians.SecurityGuardList)
            {
                sg.PedReactions.Reset();
            }
        }
    }
}