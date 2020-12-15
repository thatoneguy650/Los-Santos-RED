using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr
{
    public class PoliceForce
    {
        public bool AnyCanSeePlayer { get; private set; }
        public bool AnyCanHearPlayer { get; private set; }
        public bool AnyCanRecognizePlayer { get; private set; }
        public bool AnyRecentlySeenPlayer { get; private set; }
        public bool AnySeenPlayerCurrentWanted { get; private set; }
        public Vector3 PlaceLastSeenPlayer { get; private set; }
        public Vector3 PlayerLastSeenForwardVector { get; set; }
        public bool WasPlayerLastSeenInVehicle { get; set; }
        public float PlayerLastSeenHeading { get; set; }
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
                if (Mod.World.IsNightTime)
                    return 3500;
                else if (Mod.Player.IsInVehicle)
                    return 750;
                else
                    return 2000;
            }
        }
        public void Tick()
        {
            UpdateCops();
            UpdateRecognition();
        }
        public void SpeechTick()
        {
            foreach (Cop Cop in Mod.World.Pedestrians.Cops.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive))
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
            foreach (Cop Cop in Mod.World.Pedestrians.Cops.Where(x=> x.Pedestrian.Exists() && x.Pedestrian.IsAlive))
            {
                Cop.Update();
                if (Cop.ShouldBustPlayer)
                {
                    Mod.Player.StartManualArrest();
                }
                Cop.Loadout.Update();//Here for now
            }
            Mod.World.Vehicles.PoliceVehicles.RemoveAll(x => !x.Exists());
        }

        private void UpdateRecognition()
        {
            AnyCanSeePlayer = Mod.World.Pedestrians.Cops.Any(x => x.CanSeePlayer);
            AnyCanHearPlayer = Mod.World.Pedestrians.Cops.Any(x => x.WithinWeaponsAudioRange);

            if (AnyCanSeePlayer)
                AnyRecentlySeenPlayer = true;
            else
                AnyRecentlySeenPlayer = Mod.World.Pedestrians.Cops.Any(x => x.SeenPlayerSince(Mod.DataMart.Settings.SettingsManager.Police.PoliceRecentlySeenTime));

            AnyCanRecognizePlayer = Mod.World.Pedestrians.Cops.Any(x => x.TimeContinuoslySeenPlayer >= TimeToRecognizePlayer || (x.CanSeePlayer && x.DistanceToPlayer <= 20f) || (x.DistanceToPlayer <= 7f && x.DistanceToPlayer > 0.01f));

            if (!AnySeenPlayerCurrentWanted && AnyRecentlySeenPlayer && Mod.Player.IsWanted)
                AnySeenPlayerCurrentWanted = true;

            if (AnyRecentlySeenPlayer)
            {
                if (!AnySeenPlayerCurrentWanted)
                    PlaceLastSeenPlayer = Mod.Player.CurrentPoliceResponse.PlaceWantedStarted;
                else if (!Mod.Player.AreStarsGreyedOut)
                    PlaceLastSeenPlayer = Game.LocalPlayer.Character.Position;
            }

            NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, PlaceLastSeenPlayer.X, PlaceLastSeenPlayer.Y, PlaceLastSeenPlayer.Z);
        }

    }


}
