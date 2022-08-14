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

        public Police(IEntityProvideable world, IPoliceRespondable currentPlayer, IPerceptable perceptable, ISettingsProvideable settings, IItemEquipable itemEquipablePlayer)
        {
            World = world;
            Player = currentPlayer;
            Settings = settings;
            Perceptable = perceptable;
            ItemEquipablePlayer = itemEquipablePlayer;
        }
        public void Update()
        {
            UpdateCops();
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
            if (Settings.SettingsManager.DebugSettings.PrintUpdateTimes)
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
            foreach (Cop Cop in World.Pedestrians.PoliceList)
            {
                try
                {
                    if (Cop.Pedestrian.Exists())
                    {
                        bool yield = false;
                        if (Cop.NeedsFullUpdate)
                        {
                            yield = true;
                            TotalRan++;
                            localRan++;
                        }
                        Cop.Update(Perceptable, Player, Player.PlacePoliceLastSeenPlayer, World);
                        if (Settings.SettingsManager.PoliceSettings.ManageLoadout)
                        {
                            Cop.WeaponInventory.UpdateLoadout(Player);
                        }
                        if (Settings.SettingsManager.PoliceSettings.AllowAmbientSpeech)
                        {
                            Cop.UpdateSpeech(Player);
                        }
                        if (Settings.SettingsManager.PoliceSettings.AllowChaseAssists)
                        {
                            if (Settings.SettingsManager.PoliceSettings.AllowReducedCollisionPenaltyAssist)
                            {
                                Cop.AssistManager.UpdateCollision(Player.IsWanted);
                            }
                            if (Settings.SettingsManager.PoliceSettings.AllowFrontVehicleClearAssist)
                            {
                                Cop.AssistManager.ClearFront(Player.IsWanted);
                            }
                            if (Settings.SettingsManager.PoliceSettings.AllowPowerAssist)
                            {
                                Cop.AssistManager.PowerAssist(Player.IsWanted);
                            }
                        }
                        if(Cop.DistanceToPlayer <= closestDistanceToPlayer && Cop.Pedestrian.Exists() && Cop.Pedestrian.IsAlive)
                        {
                            closestDistanceToPlayer = Cop.DistanceToPlayer;
                        }
                        if(Cop.DistanceToPlayer < closestCopDistance)
                        {
                            PrimaryPlayerCop = Cop;
                            closestCopDistance = Cop.DistanceToPlayer;
                        }
                        if (yield && localRan == Settings.SettingsManager.DebugSettings.PoliceUpdateBatch)//1
                        {
                            GameFiber.Yield();
                            localRan = 0;
                        }
                        TotalChecked++;
                    }
                }
                catch (Exception e)
                {
                    EntryPoint.WriteToConsole("Error" + e.Message + " : " + e.StackTrace, 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~o~Error", "Los Santos ~r~RED", "Los Santos ~r~RED ~s~ Error Updating Cop Data");
                }
                GameFiber.Yield();
            }
            if(Player.ClosestCopToPlayer != null && PrimaryPlayerCop != null && Player.ClosestCopToPlayer.Handle != PrimaryPlayerCop.Handle)
            {
                if(Math.Abs(Player.ClosestCopToPlayer.DistanceToPlayer - PrimaryPlayerCop.DistanceToPlayer) >= 2f)
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
            bool anyPoliceCanSeePlayer = false;
            bool anyPoliceCanHearPlayer = false;
            bool anyPoliceCanRecognizePlayer = false;
            bool anyPoliceRecentlySeenPlayer = false;
            foreach (Cop cop in World.Pedestrians.PoliceList)
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
                    if (cop.TimeContinuoslySeenPlayer >= Player.TimeToRecognize || (cop.CanSeePlayer && cop.DistanceToPlayer <= Settings.SettingsManager.PoliceSettings.AutoRecognizeDistance) || (cop.DistanceToPlayer <= Settings.SettingsManager.PoliceSettings.AlwaysRecognizeDistance && cop.DistanceToPlayer > 0.01f))
                    {
                        anyPoliceCanRecognizePlayer = true;
                    }
                    if (cop.SeenPlayerWithin(Settings.SettingsManager.PoliceSettings.RecentlySeenTime))
                    {
                        anyPoliceRecentlySeenPlayer = true;
                    }
                }

                if (anyPoliceCanSeePlayer && anyPoliceCanRecognizePlayer)
                {
                    break;
                }
                //GameFiber.Yield();
            }
            Player.AnyPoliceCanSeePlayer = anyPoliceCanSeePlayer;
            Player.AnyPoliceCanHearPlayer = anyPoliceCanHearPlayer;
            Player.AnyPoliceCanRecognizePlayer = anyPoliceCanRecognizePlayer;
            Player.AnyPoliceRecentlySeenPlayer = anyPoliceRecentlySeenPlayer;


            if(Settings.SettingsManager.PoliceSettings.KnowsShootingSourceLocation && !anyPoliceCanSeePlayer)
            {
                if(Player.RecentlyShot && anyPoliceCanHearPlayer)
                {
                    Player.AnyPoliceCanSeePlayer = true;
                    Player.AnyPoliceRecentlySeenPlayer = true;
                }
            }




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


                if (Player.SearchMode.IsInStartOfSearchMode)
                {
                    //if (Game.GameTime - GameTimeLastUpdatedSearchLocation >= 10000)
                    //{
                    if (Player.PlacePoliceShouldSearchForPlayer.DistanceTo2D(Player.Position) >= 10f)
                    {
                        Player.PlacePoliceShouldSearchForPlayer = Player.Position;
                    }
                        //GameTimeLastUpdatedSearchLocation = Game.GameTime;
                    //}

                    if(Game.GameTime - GameTimeLastUpdatedSearchLocation >= 1000)
                    {
                        EntryPoint.WriteToConsole("Ghost Position Update for Cop Tasking");
                        GameTimeLastUpdatedSearchLocation = Game.GameTime;
                    }

                }
                else
                {
                    Player.PlacePoliceShouldSearchForPlayer = Player.PlacePoliceLastSeenPlayer;
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
    }
}