using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Linq;

namespace LosSantosRED.lsr
{
    public class Police
    {
        private IPoliceRespondable Player;
        private uint PoliceLastSeenVehicleHandle;
        private IEntityProvideable World;
        public Police(IEntityProvideable world, IPoliceRespondable currentPlayer)
        {
            World = world;
            Player = currentPlayer;
        }
        public void Update()
        {
            UpdateCops();
            UpdateRecognition();
            if (Player.IsBustable && World.PoliceList.Any(x => x.ShouldBustPlayer))
            {
                GameFiber.Yield();
                Player.Arrest();
            }
        }
        private void UpdateCops()
        {
            foreach (Cop Cop in World.PoliceList.OrderBy(x=> x.GameTimeLastUpdated).Take(5))//THIS TAKE 5 and order by is new, maybe dont need to updated to cops so frequently? maybe i do well see
            {
                Cop.Update(Player, Player.PlacePoliceLastSeenPlayer);
                Cop.UpdateLoadout(Player.PoliceResponse.IsDeadlyChase, Player.WantedLevel);
                Cop.UpdateSpeech(Player);
                Cop.UpdateAssists(Player.IsWanted);
                GameFiber.Yield();
            }
        }
        private void UpdateRecognition()
        {
            bool anyPoliceCanSeePlayer = false;
            bool anyPoliceCanHearPlayer = false;
            bool anyPoliceCanRecognizePlayer = false;
            bool anyPoliceRecentlySeenPlayer = false;
            foreach (Cop cop in World.PoliceList)
            {
                if(cop.Pedestrian.Exists() && cop.Pedestrian.IsAlive)
                {
                    if (cop.CanSeePlayer)
                    {
                        anyPoliceCanSeePlayer = true;
                        anyPoliceCanHearPlayer = true;
                        anyPoliceRecentlySeenPlayer = true;
                    }
                    else if (cop.WithinWeaponsAudioRange)
                    {
                        anyPoliceCanHearPlayer = true;
                    }
                    if (cop.TimeContinuoslySeenPlayer >= Player.TimeToRecognize || (cop.CanSeePlayer && cop.DistanceToPlayer <= 20f) || (cop.DistanceToPlayer <= 7f && cop.DistanceToPlayer > 0.01f))
                    {
                        anyPoliceCanRecognizePlayer = true;
                    }
                    if (cop.SeenPlayerWithin(17000))
                    {
                        anyPoliceRecentlySeenPlayer = true;
                    }
                }

                if(anyPoliceCanSeePlayer && anyPoliceCanRecognizePlayer)
                {
                    break;
                }
            }
            Player.AnyPoliceCanSeePlayer = anyPoliceCanSeePlayer;
            Player.AnyPoliceCanHearPlayer = anyPoliceCanHearPlayer;
            Player.AnyPoliceCanRecognizePlayer = anyPoliceCanRecognizePlayer;
            Player.AnyPoliceRecentlySeenPlayer = anyPoliceRecentlySeenPlayer;
            if(Player.IsWanted)
            {
                if (Player.AnyPoliceRecentlySeenPlayer)
                {
                    Player.PlacePoliceLastSeenPlayer = Player.Position;
                }     
                else
                {
                    if (Player.PoliceResponse.PlaceLastReportedCrime != Vector3.Zero && Player.PoliceResponse.PlaceLastReportedCrime != Player.PlacePoliceLastSeenPlayer && Player.Position.DistanceTo2D(Player.PoliceResponse.PlaceLastReportedCrime) <= Player.Position.DistanceTo2D(Player.PlacePoliceLastSeenPlayer))//They called in a place closer than your position, maybe go with time instead ot be more fair?
                    {
                        Player.PlacePoliceLastSeenPlayer = Player.PoliceResponse.PlaceLastReportedCrime;
                        EntryPoint.WriteToConsole($"POLICE EVENT: Updated Place Police Last Seen To A Citizen Reported Location", 3);
                    }
                }
                if (Player.AnyPoliceCanSeePlayer && Player.CurrentSeenVehicle != null && Player.CurrentSeenVehicle.Vehicle.Exists())
                {
                    if (PoliceLastSeenVehicleHandle != 0 && PoliceLastSeenVehicleHandle != Player.CurrentSeenVehicle.Vehicle.Handle && !Player.CurrentSeenVehicle.HasBeenDescribedByDispatch)
                    {
                        Player.OnPoliceNoticeVehicleChange();
                    }
                    PoliceLastSeenVehicleHandle = Player.CurrentSeenVehicle.Vehicle.Handle;
                }
                if (Player.AnyPoliceCanSeePlayer && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
                {
                    Player.CurrentVehicle.UpdateDescription();
                }
                
            }
            NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, Player.PlacePoliceLastSeenPlayer.X, Player.PlacePoliceLastSeenPlayer.Y, Player.PlacePoliceLastSeenPlayer.Z);
        }
        //private void UpdateRecognition_Old()
        //{
        //    bool anyPoliceCanSeePlayer = false;
        //    bool anyPoliceCanHearPlayer = false;
        //    bool anyPoliceCanRecognizePlayer = false;
        //    bool anyPoliceRecentlySeenPlayer = false;
        //    foreach (Cop cop in World.PoliceList)
        //    {
        //        if (cop.Pedestrian.Exists() && cop.Pedestrian.IsAlive)
        //        {
        //            if (cop.CanSeePlayer)
        //            {
        //                anyPoliceCanSeePlayer = true;
        //                anyPoliceCanHearPlayer = true;

        //            }
        //            else if (cop.WithinWeaponsAudioRange)
        //            {
        //                anyPoliceCanHearPlayer = true;
        //            }
        //            if (cop.TimeContinuoslySeenPlayer >= Player.TimeToRecognize || (cop.CanSeePlayer && cop.DistanceToPlayer <= 20f) || (cop.DistanceToPlayer <= 7f && cop.DistanceToPlayer > 0.01f))
        //            {
        //                anyPoliceCanRecognizePlayer = true;
        //            }
        //        }

        //        if (anyPoliceCanSeePlayer && anyPoliceCanHearPlayer && anyPoliceCanRecognizePlayer)
        //        {
        //            return;
        //        }
        //    }

        //    Player.AnyPoliceCanSeePlayer = anyPoliceCanSeePlayer;
        //    Player.AnyPoliceCanHearPlayer = anyPoliceCanHearPlayer;
        //    Player.AnyPoliceCanRecognizePlayer = anyPoliceCanRecognizePlayer;
        //    Player.AnyPoliceRecentlySeenPlayer = anyPoliceRecentlySeenPlayer;


        //    /// OLD \/

        //    Player.AnyPoliceCanSeePlayer = World.PoliceList.Any(x => x.CanSeePlayer);
        //    Player.AnyPoliceCanHearPlayer = World.PoliceList.Any(x => x.WithinWeaponsAudioRange);
        //    if (Player.AnyPoliceCanSeePlayer)
        //    {
        //        Player.AnyPoliceRecentlySeenPlayer = true;
        //    }
        //    else
        //    {
        //        Player.AnyPoliceRecentlySeenPlayer = World.PoliceList.Any(x => x.SeenPlayerWithin(17000));
        //    }
        //    Player.AnyPoliceCanRecognizePlayer = World.PoliceList.Any(x => x.TimeContinuoslySeenPlayer >= Player.TimeToRecognize || (x.CanSeePlayer && x.DistanceToPlayer <= 20f) || (x.DistanceToPlayer <= 7f && x.DistanceToPlayer > 0.01f));

        //    if (!Player.AnyPoliceSeenPlayerCurrentWanted && Player.AnyPoliceRecentlySeenPlayer && Player.IsWanted)
        //    {
        //        Player.AnyPoliceSeenPlayerCurrentWanted = true;
        //    }
        //    else
        //    {
        //        Player.AnyPoliceSeenPlayerCurrentWanted = false;
        //    }




        //    if (Player.IsWanted)
        //    {
        //        if (Player.IsInSearchMode)
        //        {
        //            // Player.PlacePoliceLastSeenPlayer = Player.PoliceResponse.PlaceLastReportedCrime;
        //        }
        //        else
        //        {
        //            Player.PlacePoliceLastSeenPlayer = Game.LocalPlayer.Character.Position;
        //        }
        //    }


        //    //if (Player.AnyPoliceRecentlySeenPlayer)
        //    //{
        //    //    if (!Player.IsInSearchMode)
        //    //    {
        //    //        Player.PlacePoliceLastSeenPlayer = Game.LocalPlayer.Character.Position;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    if (!Player.AnyPoliceSeenPlayerCurrentWanted)
        //    //    {
        //    //        Player.PlacePoliceLastSeenPlayer = Player.PoliceResponse.PlaceWantedStarted;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    if (Player.IsInSearchMode && Player.PoliceResponse.HasReportedCrimes && Player.PoliceResponse.PlaceLastReportedCrime != Vector3.Zero)
        //    //    {
        //    //        Player.PlacePoliceLastSeenPlayer = Player.PoliceResponse.PlaceLastReportedCrime;
        //    //    }
        //    //}
        //    NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, Player.PlacePoliceLastSeenPlayer.X, Player.PlacePoliceLastSeenPlayer.Y, Player.PlacePoliceLastSeenPlayer.Z);
        //    if (Player.AnyPoliceCanSeePlayer && Player.IsWanted && !Player.IsInSearchMode && Player.CurrentSeenVehicle != null && Player.CurrentSeenVehicle.Vehicle.Exists())
        //    {
        //        if (PoliceLastSeenVehicleHandle != 0 && PoliceLastSeenVehicleHandle != Player.CurrentSeenVehicle.Vehicle.Handle && !Player.CurrentSeenVehicle.HasBeenDescribedByDispatch)
        //        {
        //            Player.OnPoliceNoticeVehicleChange();
        //        }
        //        PoliceLastSeenVehicleHandle = Player.CurrentSeenVehicle.Vehicle.Handle;
        //    }

        //    if (Player.AnyPoliceCanRecognizePlayer && Player.IsWanted && !Player.IsInSearchMode && Player.CurrentVehicle != null)
        //    {
        //        Player.CurrentVehicle.UpdateDescription();
        //    }
        //}
    }
}