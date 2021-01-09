using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Linq;

namespace LosSantosRED.lsr
{
    public class Police
    {
        private uint GameTimePoliceNoticedVehicleChange;
        private IPoliceRespondable Player;
        private uint PoliceLastSeenVehicleHandle;
        private IEntityProvideable World;
        public Police(IEntityProvideable world, IPoliceRespondable currentPlayer)
        {
            World = world;
            Player = currentPlayer;
        }
        private bool RecentlyNoticedVehicleChange => GameTimePoliceNoticedVehicleChange != 0 && Game.GameTime - GameTimePoliceNoticedVehicleChange <= 15000;
        public void Update()
        {
            UpdateCops();
            UpdateRecognition();
            if (Player.IsBustable && World.PoliceList.Any(x => x.ShouldBustPlayer))
            {
                Player.Arrest();
            }
        }
        private void UpdateCops()
        {
            foreach (Cop Cop in World.PoliceList)
            {
                Cop.Update(Player, Player.PlacePoliceLastSeenPlayer);
                Cop.UpdateDrivingFlags();
                Cop.UpdateLoadout(Player.PoliceResponse.IsDeadlyChase, Player.WantedLevel);
                Cop.UpdateSpeech(Player);
            }
        }
        private void UpdateRecognition()
        {
            Player.AnyPoliceCanSeePlayer = World.PoliceList.Any(x => x.CanSeePlayer);
            Player.AnyPoliceCanHearPlayer = World.PoliceList.Any(x => x.WithinWeaponsAudioRange);
            if (Player.AnyPoliceCanSeePlayer)
            {
                Player.AnyPoliceRecentlySeenPlayer = true;
            }
            else
            {
                Player.AnyPoliceRecentlySeenPlayer = World.PoliceList.Any(x => x.SeenPlayerFor(17000));
            }
            Player.AnyPoliceCanRecognizePlayer = World.PoliceList.Any(x => x.TimeContinuoslySeenPlayer >= Player.TimeToRecognize || (x.CanSeePlayer && x.DistanceToPlayer <= 20f) || (x.DistanceToPlayer <= 7f && x.DistanceToPlayer > 0.01f));
            if (!Player.AnyPoliceSeenPlayerCurrentWanted && Player.AnyPoliceRecentlySeenPlayer && Player.IsWanted)
            {
                Player.AnyPoliceSeenPlayerCurrentWanted = true;
            }
            if (Player.AnyPoliceRecentlySeenPlayer)
            {
                if (!Player.AnyPoliceSeenPlayerCurrentWanted)
                {
                    Player.PlacePoliceLastSeenPlayer = Player.PoliceResponse.PlaceWantedStarted;
                }
                else if (!Player.IsInSearchMode)
                {
                    Player.PlacePoliceLastSeenPlayer = Game.LocalPlayer.Character.Position;
                }
            }
            //else
            //{
            //    if (Player.IsInSearchMode && Player.PoliceResponse.HasReportedCrimes && Player.PoliceResponse.PlaceLastReportedCrime != Vector3.Zero)
            //    {
            //        Player.PlacePoliceLastSeenPlayer = Player.PoliceResponse.PlaceLastReportedCrime;
            //    }
            //}
            NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, Player.PlacePoliceLastSeenPlayer.X, Player.PlacePoliceLastSeenPlayer.Y, Player.PlacePoliceLastSeenPlayer.Z);
            if (Player.AnyPoliceCanSeePlayer && Player.IsWanted && !Player.IsInSearchMode && Player.CurrentSeenVehicle != null && Player.CurrentSeenVehicle.Vehicle.Exists())
            {
                if (PoliceLastSeenVehicleHandle != 0 && PoliceLastSeenVehicleHandle != Player.CurrentSeenVehicle.Vehicle.Handle && !Player.CurrentSeenVehicle.HasBeenDescribedByDispatch)
                {
                    GameTimePoliceNoticedVehicleChange = Game.GameTime;
                }
                PoliceLastSeenVehicleHandle = Player.CurrentSeenVehicle.Vehicle.Handle;
            }

            if (Player.AnyPoliceCanRecognizePlayer && Player.IsWanted && !Player.IsInSearchMode && Player.CurrentVehicle != null)
            {
                Player.CurrentVehicle.UpdateDescription();
            }
            Player.PoliceRecentlyNoticedVehicleChange = RecentlyNoticedVehicleChange;
        }
    }
}