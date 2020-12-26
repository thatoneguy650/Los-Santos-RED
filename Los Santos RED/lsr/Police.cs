using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Linq;

namespace LosSantosRED.lsr
{
    public class Police : IPolice
    {
        private uint GameTimePoliceNoticedVehicleChange;
        private IPlayer CurrentPlayer;
        private IWorld World;
        private uint PoliceLastSeenVehicleHandle;
        public Police(IWorld world, IPlayer currentPlayer)
        {
            World = world;
            CurrentPlayer = currentPlayer;
        }
        public float ActiveDistance
        {
            get
            {
                return 400f + (CurrentPlayer.WantedLevel * 200f);//500f
            }
        }
        public bool AnyCanHearPlayer { get; private set; }
        public bool AnyCanRecognizePlayer { get; private set; }
        public bool AnyCanSeePlayer { get; private set; }
        public bool AnyRecentlySeenPlayer { get; private set; }
        public bool AnySeenPlayerCurrentWanted { get; private set; }
        public Vector3 PlaceLastSeenPlayer { get; private set; }
        public bool RecentlyNoticedVehicleChange => GameTimePoliceNoticedVehicleChange != 0 && Game.GameTime - GameTimePoliceNoticedVehicleChange <= 15000;
        private float TimeToRecognizePlayer
        {
            get
            {
                float Time = 2000;
                if (World.IsNight)
                {
                    Time += 3500;
                }
                else if (CurrentPlayer.IsInVehicle)
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

            if (CurrentPlayer.IsBustable && World.PoliceList.Any(x => x.ShouldBustPlayer))
            {
                CurrentPlayer.StartManualArrest();
            }

        }
        private void UpdateCops()
        {
            foreach (Cop Cop in World.PoliceList)
            {
                Cop.Update(CurrentPlayer,PlaceLastSeenPlayer);
                Cop.Loadout.Update(CurrentPlayer.CurrentPoliceResponse.IsDeadlyChase,CurrentPlayer.WantedLevel);
                Cop.CheckSpeech(CurrentPlayer);





            }
        }
        private void UpdateRecognition()//most likely remove this and let these be proepties instead of cached values
        {
            AnyCanSeePlayer = World.PoliceList.Any(x => x.CanSeePlayer);
            AnyCanHearPlayer = World.PoliceList.Any(x => x.WithinWeaponsAudioRange);
            if (AnyCanSeePlayer)
            {
                AnyRecentlySeenPlayer = true;
            }
            else
            {
                AnyRecentlySeenPlayer = World.PoliceList.Any(x => x.SeenPlayerFor(DataMart.Instance.Settings.SettingsManager.Police.PoliceRecentlySeenTime));
            }
            AnyCanRecognizePlayer = World.PoliceList.Any(x => x.TimeContinuoslySeenPlayer >= TimeToRecognizePlayer || (x.CanSeePlayer && x.DistanceToPlayer <= 20f) || (x.DistanceToPlayer <= 7f && x.DistanceToPlayer > 0.01f));
            if (!AnySeenPlayerCurrentWanted && AnyRecentlySeenPlayer && CurrentPlayer.IsWanted)
            {
                AnySeenPlayerCurrentWanted = true;
            }
            if (AnyRecentlySeenPlayer)
            {
                if (!AnySeenPlayerCurrentWanted)
                {
                    PlaceLastSeenPlayer = CurrentPlayer.CurrentPoliceResponse.PlaceWantedStarted;
                }
                else if (!CurrentPlayer.IsInSearchMode)
                {
                    PlaceLastSeenPlayer = Game.LocalPlayer.Character.Position;
                }
            }
            else
            {
                if (CurrentPlayer.IsInSearchMode && CurrentPlayer.CurrentPoliceResponse.HasReportedCrimes && CurrentPlayer.CurrentPoliceResponse.CurrentCrimes.PlaceLastReportedCrime != Vector3.Zero)
                {
                    PlaceLastSeenPlayer = CurrentPlayer.CurrentPoliceResponse.CurrentCrimes.PlaceLastReportedCrime;
                }
            }
            NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, PlaceLastSeenPlayer.X, PlaceLastSeenPlayer.Y, PlaceLastSeenPlayer.Z);

            if (AnyCanSeePlayer && CurrentPlayer.IsWanted && !CurrentPlayer.IsInSearchMode)
            {
                if (CurrentPlayer.CurrentSeenVehicle != null && CurrentPlayer.CurrentSeenVehicle.Vehicle.Exists() && PoliceLastSeenVehicleHandle != 0 && PoliceLastSeenVehicleHandle != CurrentPlayer.CurrentSeenVehicle.Vehicle.Handle && !CurrentPlayer.CurrentSeenVehicle.HasBeenDescribedByDispatch)
                {
                    GameTimePoliceNoticedVehicleChange = Game.GameTime;
                    Debug.Instance.WriteToLog("PlayerState", string.Format("PoliceRecentlyNoticedVehicleChange {0}", GameTimePoliceNoticedVehicleChange));
                }
                PoliceLastSeenVehicleHandle = CurrentPlayer.CurrentSeenVehicle.Vehicle.Handle;
            }

            if (AnyCanRecognizePlayer && CurrentPlayer.IsWanted && !CurrentPlayer.IsInSearchMode && CurrentPlayer.CurrentVehicle != null)
            {
                CurrentPlayer.CurrentVehicle.UpdateDescription();
            }
            CurrentPlayer.AnyPoliceCanSeePlayer = AnyCanSeePlayer;
            CurrentPlayer.AnyPoliceRecentlySeenPlayer = AnyRecentlySeenPlayer;
            CurrentPlayer.AnyPoliceCanHearPlayer = AnyCanHearPlayer;
            CurrentPlayer.PlacePoliceLastSeenPlayer = PlaceLastSeenPlayer;
            CurrentPlayer.AnyPoliceCanRecognizePlayer = AnyCanRecognizePlayer;
            CurrentPlayer.AnyPoliceSeenPlayerCurrentWanted = AnySeenPlayerCurrentWanted;
        }
    }
}