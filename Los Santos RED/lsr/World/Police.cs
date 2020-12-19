using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr
{
    public class Police
    {
        public bool AnyCanSeePlayer { get; private set; }
        public bool AnyCanHearPlayer { get; private set; }
        public bool AnyCanRecognizePlayer { get; private set; }
        public bool AnyRecentlySeenPlayer { get; private set; }
        public bool AnySeenPlayerCurrentWanted { get; private set; }
        public Vector3 PlaceLastSeenPlayer { get; private set; }
        public float ActiveDistance
        {
            get
            {
                return 400f + (Mod.Player.WantedLevel * 200f);//500f
            }
        }
        private float TimeToRecognizePlayer
        {
            get
            {
                float Time = 2000;
                if (Mod.World.IsNightTime)
                {
                    Time+= 3500;
                }
                else if (Mod.Player.IsInVehicle)
                {
                    Time += 750;
                }
                return Time;
            }
        }
        public void Tick()
        {
            UpdateCops();
            UpdateRecognition();
        }
        public void SpeechTick()
        {
            foreach (Cop Cop in Mod.World.Pedestrians.Police.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive))
            {
                if(Cop.CanRadioIn)
                {
                    Cop.RadioIn();
                }
                if (Cop.CanSpeak)
                {
                    Cop.Speak();
                }
            }
        }
        public void Reset()
        {
            AnySeenPlayerCurrentWanted = false;
        }
        private void UpdateCops()
        {
            foreach (Cop Cop in Mod.World.Pedestrians.Police.Where(x=> x.Pedestrian.Exists() && x.Pedestrian.IsAlive))
            {
                Cop.Update();
                Cop.Loadout.Update();
            }
        }
        private void UpdateRecognition()//most likely remove this and let these be proepties instead of cached values
        {
            AnyCanSeePlayer = Mod.World.Pedestrians.Police.Any(x => x.CanSeePlayer);
            AnyCanHearPlayer = Mod.World.Pedestrians.Police.Any(x => x.WithinWeaponsAudioRange);
            if (AnyCanSeePlayer)
            {
                AnyRecentlySeenPlayer = true;
            }
            else
            {
                AnyRecentlySeenPlayer = Mod.World.Pedestrians.Police.Any(x => x.SeenPlayerSince(Mod.DataMart.Settings.SettingsManager.Police.PoliceRecentlySeenTime));
            }
            AnyCanRecognizePlayer = Mod.World.Pedestrians.Police.Any(x => x.TimeContinuoslySeenPlayer >= TimeToRecognizePlayer || (x.CanSeePlayer && x.DistanceToPlayer <= 20f) || (x.DistanceToPlayer <= 7f && x.DistanceToPlayer > 0.01f));
            if (!AnySeenPlayerCurrentWanted && AnyRecentlySeenPlayer && Mod.Player.IsWanted)
            {
                AnySeenPlayerCurrentWanted = true;
            }
            if (AnyRecentlySeenPlayer)
            {
                if (!AnySeenPlayerCurrentWanted)
                {
                    PlaceLastSeenPlayer = Mod.Player.CurrentPoliceResponse.PlaceWantedStarted;
                }
                else if (!Mod.Player.AreStarsGreyedOut)
                {
                    PlaceLastSeenPlayer = Game.LocalPlayer.Character.Position;
                }
            }
            else
            {
                if (Mod.Player.AreStarsGreyedOut && Mod.Player.CurrentPoliceResponse.HasReportedCrimes && Mod.Player.CurrentPoliceResponse.CurrentCrimes.PlaceLastReportedCrime != Vector3.Zero)
                {
                    PlaceLastSeenPlayer = Mod.Player.CurrentPoliceResponse.CurrentCrimes.PlaceLastReportedCrime;
                }
            }
            NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, PlaceLastSeenPlayer.X, PlaceLastSeenPlayer.Y, PlaceLastSeenPlayer.Z);
        }
    }
}
