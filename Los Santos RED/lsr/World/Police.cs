using Rage;
using Rage.Native;
using System.Linq;

namespace LosSantosRED.lsr
{
    public class Police
    {
        public float ActiveDistance
        {
            get
            {
                return 400f + (Mod.Player.Instance.WantedLevel * 200f);//500f
            }
        }
        public bool AnyCanHearPlayer { get; private set; }
        public bool AnyCanRecognizePlayer { get; private set; }
        public bool AnyCanSeePlayer { get; private set; }
        public bool AnyRecentlySeenPlayer { get; private set; }
        public bool AnySeenPlayerCurrentWanted { get; private set; }
        public Vector3 PlaceLastSeenPlayer { get; private set; }
        private float TimeToRecognizePlayer
        {
            get
            {
                float Time = 2000;
                if (Mod.World.Instance.IsNight)
                {
                    Time += 3500;
                }
                else if (Mod.Player.Instance.IsInVehicle)
                {
                    Time += 750;
                }
                return Time;
            }
        }
        public void Reset()
        {
            AnySeenPlayerCurrentWanted = false;
        }
        public void Update()
        {
            UpdateCops();
            UpdateRecognition();
        }
        private void UpdateCops()
        {
            foreach (Cop Cop in Mod.World.Instance.PoliceList)
            {
                Cop.Update();
                Cop.Loadout.Update();
                Cop.CheckSpeech();
            }
        }
        private void UpdateRecognition()//most likely remove this and let these be proepties instead of cached values
        {
            AnyCanSeePlayer = Mod.World.Instance.PoliceList.Any(x => x.CanSeePlayer);
            AnyCanHearPlayer = Mod.World.Instance.PoliceList.Any(x => x.WithinWeaponsAudioRange);
            if (AnyCanSeePlayer)
            {
                AnyRecentlySeenPlayer = true;
            }
            else
            {
                AnyRecentlySeenPlayer = Mod.World.Instance.PoliceList.Any(x => x.SeenPlayerFor(DataMart.Instance.Settings.SettingsManager.Police.PoliceRecentlySeenTime));
            }
            AnyCanRecognizePlayer = Mod.World.Instance.PoliceList.Any(x => x.TimeContinuoslySeenPlayer >= TimeToRecognizePlayer || (x.CanSeePlayer && x.DistanceToPlayer <= 20f) || (x.DistanceToPlayer <= 7f && x.DistanceToPlayer > 0.01f));
            if (!AnySeenPlayerCurrentWanted && AnyRecentlySeenPlayer && Mod.Player.Instance.IsWanted)
            {
                AnySeenPlayerCurrentWanted = true;
            }
            if (AnyRecentlySeenPlayer)
            {
                if (!AnySeenPlayerCurrentWanted)
                {
                    PlaceLastSeenPlayer = Mod.Player.Instance.CurrentPoliceResponse.PlaceWantedStarted;
                }
                else if (!Mod.Player.Instance.AreStarsGreyedOut)
                {
                    PlaceLastSeenPlayer = Game.LocalPlayer.Character.Position;
                }
            }
            else
            {
                if (Mod.Player.Instance.AreStarsGreyedOut && Mod.Player.Instance.CurrentPoliceResponse.HasReportedCrimes && Mod.Player.Instance.CurrentPoliceResponse.CurrentCrimes.PlaceLastReportedCrime != Vector3.Zero)
                {
                    PlaceLastSeenPlayer = Mod.Player.Instance.CurrentPoliceResponse.CurrentCrimes.PlaceLastReportedCrime;
                }
            }
            NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, PlaceLastSeenPlayer.X, PlaceLastSeenPlayer.Y, PlaceLastSeenPlayer.Z);
        }
    }
}