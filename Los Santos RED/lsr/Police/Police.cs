using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Linq;

namespace LosSantosRED.lsr
{
    public class Police
    {
        private IPoliceRespondable Player;
        private IPerceptable Perceptable;
        private uint PoliceLastSeenVehicleHandle;
        private IEntityProvideable World;
        private ISettingsProvideable Settings;
        private uint GameTimeLastUpdatedPolice;
        private int TotalRan;
        private int TotalChecked;
        private IItemEquipable ItemEquipablePlayer;
        private uint GameTimeLastUpdatedSearchLocation;
        private bool PrevAnyPoliceKnowInteriorLocation;
        private ITimeReportable Time;
        private Vector3 prevPlacePoliceShouldSearchForPlayer;
        private Vector3 prevPlacePoliceLastSeenPlayer;


        public Police(IEntityProvideable world, IPoliceRespondable currentPlayer, IPerceptable perceptable, ISettingsProvideable settings, IItemEquipable itemEquipablePlayer, ITimeReportable time)
        {
            World = world;
            Player = currentPlayer;
            Settings = settings;
            Perceptable = perceptable;
            ItemEquipablePlayer = itemEquipablePlayer;
            Time = time;
        }
        public void Update()
        {
            UpdateCops();
            GameFiber.Yield();
            UpdateRecognition();
            if (Player.IsBustable && (Player.IsIncapacitated || Player.WantedLevel == 1 || (Player.WantedLevel > 1 && Player.IsDangerouslyArmed && Player.IsStill)) && Player.AnyPoliceCanSeePlayer && World.Pedestrians.PoliceList.Any(x => x.ShouldBustPlayer))
            {
                GameFiber.Yield();
                Player.Arrest();
            }
            if (Player.IsBustable && Player.IsAttemptingToSurrender && World.Pedestrians.PoliceList.Any(x => x.DistanceToPlayer <= 10f && x.HeightToPlayer <= 5f))
            {
                GameFiber.Yield();
                Player.Arrest();
            }
            if (Settings.SettingsManager.PerformanceSettings.PrintUpdateTimes)
            {
                EntryPoint.WriteToConsole($"Police.Update Ran Time Since {Game.GameTime - GameTimeLastUpdatedPolice} TotalRan: {TotalRan} TotalChecked: {TotalChecked}", 5);
            }
            GameTimeLastUpdatedPolice = Game.GameTime;
        }
        private void UpdateCops()
        {
            float closestDistanceToPlayer = 999f;
            TotalRan = 0;
            TotalChecked = 0;
            int localRan = 0;
            float closestCopDistance = 999f;
            Cop PrimaryPlayerCop = null;
            foreach (Cop Cop in World.Pedestrians.AllPoliceList)
            {

                try
                {
                    if (Cop.Pedestrian.Exists())
                    {
                        Cop.Update(Perceptable, Player, Player.PlacePoliceLastSeenPlayer, World);
                        if (Settings.SettingsManager.PoliceSettings.ManageLoadout)
                        {
                            Cop.WeaponInventory.UpdateLoadout(Player, Time.IsNight, Settings.SettingsManager.PoliceSettings.OverrideAccuracy);
                        }
                        if (Settings.SettingsManager.PoliceSpeechSettings.AllowAmbientSpeech)
                        {
                            Cop.UpdateSpeech(Player);
                        }
                        if (Settings.SettingsManager.PoliceTaskSettings.AllowChaseAssists)
                        {
                            if (Settings.SettingsManager.PoliceTaskSettings.AllowReducedCollisionPenaltyAssist)
                            {
                                Cop.AssistManager.UpdateCollision(Player.IsWanted);
                            }
                            if (Settings.SettingsManager.PoliceTaskSettings.AllowFrontVehicleClearAssist)
                            {
                                Cop.AssistManager.ClearFront(Player.IsWanted);
                            }
                            if (Settings.SettingsManager.PoliceTaskSettings.AllowPowerAssist)
                            {
                                Cop.AssistManager.PowerAssist(Player.IsWanted);
                            }
                        }
                        if (Cop.DistanceToPlayer <= closestDistanceToPlayer && Cop.Pedestrian.Exists() && Cop.Pedestrian.IsAlive)
                        {
                            closestDistanceToPlayer = Cop.DistanceToPlayer;
                        }
                        if (Cop.DistanceToPlayer < closestCopDistance)
                        {
                            PrimaryPlayerCop = Cop;
                            closestCopDistance = Cop.DistanceToPlayer;
                        }
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Updating Cop Data");
                }
                TotalRan++;
                TotalChecked++;
                GameFiber.Yield();
            }
            if (Player.ClosestCopToPlayer != null && PrimaryPlayerCop != null && Player.ClosestCopToPlayer.Handle != PrimaryPlayerCop.Handle)
            {
                if (Math.Abs(Player.ClosestCopToPlayer.DistanceToPlayer - PrimaryPlayerCop.DistanceToPlayer) >= 2f)
                {
                    Player.ClosestCopToPlayer = PrimaryPlayerCop;
                }
            }
            else
            {
                Player.ClosestCopToPlayer = PrimaryPlayerCop;
            }
            Player.ClosestPoliceDistanceToPlayer = closestDistanceToPlayer;
        }
        private void UpdateRecognition()
        {
            RecognitionLoop();
            UpdateGeneralItmes();
            UpdateWantedItems();
            NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, Player.PlacePoliceLastSeenPlayer.X, Player.PlacePoliceLastSeenPlayer.Y, Player.PlacePoliceLastSeenPlayer.Z);
        }
        private void RecognitionLoop()
        {
            bool anyPoliceCanSeePlayer = false;
            bool anyPoliceCanHearPlayer = false;
            bool anyPoliceCanRecognizePlayer = false;
            bool anyPoliceRecentlySeenPlayer = false;
            bool anyPoliceSawPlayerViolating = false;
            int tested = 0;
            foreach (Cop cop in World.Pedestrians.AllPoliceList)
            {
                if (cop.Pedestrian.Exists() && cop.Pedestrian.IsAlive && !cop.IsUnconscious)
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
                    if (cop.TimeContinuoslySeenPlayer >= Player.TimeToRecognize || (cop.CanSeePlayer && cop.DistanceToPlayer <= Settings.SettingsManager.PoliceSettings.AutoRecognizeDistance) || (cop.DistanceToPlayer <= Settings.SettingsManager.PoliceSettings.AlwaysRecognizeDistance && cop.DistanceToPlayer > 0.01f))
                    {
                        anyPoliceCanRecognizePlayer = true;
                    }
                    if (cop.SeenPlayerWithin(Settings.SettingsManager.PoliceSettings.RecentlySeenTime))
                    {
                        anyPoliceRecentlySeenPlayer = true;
                    }
                    if(cop.SawPlayerViolating)
                    {
                        anyPoliceSawPlayerViolating = true;
                    }
                }

                if (anyPoliceCanSeePlayer && anyPoliceCanRecognizePlayer)
                {
                    break;
                }
                tested++;
                if (tested >= 10)//20//10
                {
                    tested = 0;
                    GameFiber.Yield();
                }
                //GameFiber.Yield();
            }
            Player.AnyPoliceCanSeePlayer = anyPoliceCanSeePlayer;
            Player.AnyPoliceCanHearPlayer = anyPoliceCanHearPlayer;
            Player.AnyPoliceCanRecognizePlayer = anyPoliceCanRecognizePlayer;
            Player.AnyPoliceRecentlySeenPlayer = anyPoliceRecentlySeenPlayer;
            Player.AnyPoliceSawPlayerViolating = anyPoliceSawPlayerViolating;
        }
        private void UpdateGeneralItmes()
        {
            //if (Settings.SettingsManager.PoliceSettings.KnowsShootingSourceLocation && !Player.AnyPoliceCanSeePlayer)
            //{
            //    if (Player.RecentlyShot && Player.AnyPoliceCanHearPlayer && !Player.Character.IsCurrentWeaponSilenced)
            //    {
            //        Player.AnyPoliceCanSeePlayer = true;
            //        Player.AnyPoliceRecentlySeenPlayer = true;
            //    }
            //}
            HandleInteriorKnowledge();
            if (Player.AnyPoliceRecentlySeenPlayer || Player.AnyPoliceKnowInteriorLocation)
            {
                Player.PoliceLastSeenOnFoot = Player.IsOnFoot;
            }
        }
        private void HandleInteriorKnowledge()
        {
            if (Settings.SettingsManager.PoliceSettings.AllowBreachingLogic && Player.CurrentLocation.IsInside && (Player.AnyPoliceRecentlySeenPlayer || Player.SearchMode.IsInActiveMode))
            {
                Player.AnyPoliceKnowInteriorLocation = true;
            }
            if (Player.AnyPoliceKnowInteriorLocation)
            {
                if(Player.CurrentLocation.TimeOutside >= Settings.SettingsManager.PoliceSettings.BreachingExipireTimeOutsideWithMinDistance && Player.ClosestPoliceDistanceToPlayer >= Settings.SettingsManager.PoliceSettings.BreachingExipireDistanceOutsideWithMinDistance)// || Player.CurrentLocation.TimeOutside >= 25000 || Player.PlacePolicePhysicallyLastSeenPlayer.DistanceTo(Player.Position) >= 200f)
                {
                    Player.AnyPoliceKnowInteriorLocation = false;
                }
                else if(Player.CurrentLocation.TimeOutside >= Settings.SettingsManager.PoliceSettings.BreachingExpireTimeOutsideOnly)
                {
                    Player.AnyPoliceKnowInteriorLocation = false;
                }
                else if (Player.PlacePolicePhysicallyLastSeenPlayer.DistanceTo(Player.Position) >= Settings.SettingsManager.PoliceSettings.BreachingExpireTimeDistanceOnly)
                {
                    Player.AnyPoliceKnowInteriorLocation = false;
                }
            }
            if (PrevAnyPoliceKnowInteriorLocation != Player.AnyPoliceKnowInteriorLocation)
            {
                //EntryPoint.WriteToConsoleTestLong($"AnyPoliceKnowInteriorLocation changed to {Player.AnyPoliceKnowInteriorLocation}");
                PrevAnyPoliceKnowInteriorLocation = Player.AnyPoliceKnowInteriorLocation;
            }
        }
        private void UpdateWantedItems()
        {
            if (!Player.IsWanted)
            {
                return;
            }
            GameFiber.Yield();

            if (Player.AnyPoliceRecentlySeenPlayer || Player.AnyPoliceKnowInteriorLocation)
            {
                Player.PlacePoliceLastSeenPlayer = Player.Position;
            }
            else
            {
                if (Player.PoliceResponse.PlaceLastReportedCrime != Vector3.Zero && Player.PoliceResponse.PlaceLastReportedCrime != Player.PlacePoliceLastSeenPlayer && Player.Position.DistanceTo2D(Player.PoliceResponse.PlaceLastReportedCrime) <= Player.Position.DistanceTo2D(Player.PlacePoliceLastSeenPlayer))//They called in a place closer than your position, maybe go with time instead ot be more fair?
                {
                    Player.PlacePoliceLastSeenPlayer = Player.PoliceResponse.PlaceLastReportedCrime;
                    //EntryPoint.WriteToConsole($"POLICE EVENT: Updated Place Police Last Seen To A Citizen Reported Location", 3);
                }
            }

            if (Player.PlacePoliceLastSeenPlayer.DistanceTo(prevPlacePoliceLastSeenPlayer) >= 5.0f)
            {
                //EntryPoint.WriteToConsole("POLICE PlacePoliceLastSeenPlayer CHANGED");
                Player.StreetPlacePoliceLastSeenPlayer = NativeHelper.GetStreetPosition(Player.PlacePoliceLastSeenPlayer, true);
                prevPlacePoliceLastSeenPlayer = Player.PlacePoliceLastSeenPlayer;
            }

            if(Player.AnyPoliceRecentlySeenPlayer)
            {
                Player.PlacePolicePhysicallyLastSeenPlayer = Player.Position;
            }

            DeterimineInterestedLocation();

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
        private void DeterimineInterestedLocation()
        {
            if (Player.SearchMode.IsInStartOfSearchMode)
            {
                if (Player.PlacePoliceShouldSearchForPlayer.DistanceTo2D(Player.Position) >= 10f)
                {
                    Player.PlacePoliceShouldSearchForPlayer = Player.Position;
                }
                if (Game.GameTime - GameTimeLastUpdatedSearchLocation >= 1000)
                {
                    //EntryPoint.WriteToConsole("Ghost Position Update for Cop Tasking");
                    GameTimeLastUpdatedSearchLocation = Game.GameTime;
                }
            }
            else
            {
                Player.PlacePoliceShouldSearchForPlayer = Player.PlacePoliceLastSeenPlayer;
            }

            if (Player.PlacePoliceShouldSearchForPlayer.DistanceTo(prevPlacePoliceShouldSearchForPlayer) >= 5f)
            {
                //EntryPoint.WriteToConsole("POLICE PlacePoliceShouldSearchForPlayer CHANGED");
                Player.StreetPlacePoliceShouldSearchForPlayer = NativeHelper.GetStreetPosition(Player.PlacePoliceShouldSearchForPlayer, true);
                prevPlacePoliceShouldSearchForPlayer = Player.PlacePoliceShouldSearchForPlayer;
            }

            Player.IsNearbyPlacePoliceShouldSearchForPlayer = Player.PlacePoliceShouldSearchForPlayer.DistanceTo2D(Player.Position) <= 200f;
        }
    }

}