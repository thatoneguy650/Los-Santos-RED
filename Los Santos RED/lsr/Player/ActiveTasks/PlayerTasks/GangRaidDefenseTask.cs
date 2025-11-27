using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
namespace LosSantosRED.lsr.Player.ActiveTasks
{
    public class GangRaidDefenseTask : IPlayerTask
    {
        private Gang Gang;
        private int TimeToDefend { get; set; }
        private bool IsActive;
        private bool IsProtecting;
        private TimeSpan Remaining;
        private ISettingsProvideable Settings;
        private GameLocation RaidedLocation;
        private GangDispatcher GangDispatcher;
        private ILocationInteractable LocationInteractable;
        private IDispatchable Player;
        private const float MAX_DEFENSE_DISTANCE = 100f;
        private bool HasDefendedSuccessfully;
        private List<PedExt> SpawnedPeds;
        private bool HasSpawnedPeds;
        private int RaidedLocationCellX, RaidedLocationCellY;
        private const float MAXRAIDERDISTANCE = 100f;
        private const uint EXISTENCECHECK = 5000;
        public GangRaidDefenseTask(Gang gang, ISettingsProvideable settings, GameLocation raidedLocation, GangDispatcher gangDispatcher, ILocationInteractable locationInteractable, IDispatchable player)
        {
            Gang = gang;
            Remaining = TimeSpan.FromSeconds(TimeToDefend);
            Settings = settings;
            RaidedLocation = raidedLocation;
            GangDispatcher = gangDispatcher;
            LocationInteractable = locationInteractable;
            Player = player;
            TimeToDefend = Settings.SettingsManager.GangSettings.AllowedTimeForRaidDefense;
        }
        public void Dispose()
        {
            SpawnedPeds.ForEach(x => { x.SetNonPersistent(); x.DeleteBlip(); });
            GangDispatcher.IsBeingRaided = false;
        }

        public void Setup()
        {
            GangDispatcher.IsBeingRaided = true;
            StartProcess();
            StartDraw();
        }
        public void Loop()
        {
            while ((IsActive && TimeToDefend > 0) || IsProtecting)
            {
                if (Player.IsDead)
                {
                    break;
                }
                if (!HasSpawnedPeds)
                {
                    AttemptSpawn();
                    GameFiber.Yield();
                }
                IsProtecting = LocationInteractable.Position.DistanceTo(RaidedLocation.EntrancePosition) < MAX_DEFENSE_DISTANCE && !Player.IsDead ? true : false;
                if (!IsProtecting && TimeToDefend <= 0)
                {
                    break;
                }
                if ((IsProtecting && HasSpawnedPeds && SpawnedPeds.Count > 0 && SpawnedPeds.All(x => x.IsDead)) 
                    || (IsProtecting && HasSpawnedPeds && SpawnedPeds.Count > 0 && SpawnedPeds.All(x => RaidedLocation.EntrancePosition.DistanceTo(x.Position) > MAXRAIDERDISTANCE && x.HasExistedFor > EXISTENCECHECK)))
                {
                    HasDefendedSuccessfully = true;
                    break;
                }
                if (!Game.IsPaused && TimeToDefend > 0)
                {
                    TimeToDefend--;
                }
                if (!EntryPoint.ModController.IsRunning)
                {
                    IsActive = false;
                }
                GameFiber.Sleep(1000);
            }
            IsActive = false;
            HandleDefenseEnd();
        }
        private string FormatTime(int seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return string.Format("{0}:{1:00}", (int)time.TotalMinutes, time.Seconds);
        }
        public void Draw()
        {
            while (IsActive)
            {
                if (!IsProtecting)
                {
                    NativeHelper.DisplayTextOnScreen($"Defend Property: {FormatTime(TimeToDefend)}",
                                            Settings.SettingsManager.LSRHUDSettings.RacingPositionPositionTimeY,
                                            Settings.SettingsManager.LSRHUDSettings.RacingPositionPositionX,
                                            Settings.SettingsManager.LSRHUDSettings.RacingPositionSmallerScale,
                                            Color.FromName(Settings.SettingsManager.LSRHUDSettings.RacingPositionColor),
                                            (GTAFont)Settings.SettingsManager.LSRHUDSettings.RacingPositionFont,
                                            (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.RacingPositionJustificationID,
                                            false);
                }
                GameFiber.Yield();
            }
        }
        private void HandleDefenseEnd()
        {
            if (HasDefendedSuccessfully)
            {
                Game.DisplayNotification("I think we're safe for now.");
            }
            else
            {
                Game.DisplayNotification("You have failed to defend your property");
                RaidedLocation.HandleRaid();
            }
            Dispose();
        }
        private void StartProcess()
        {
            GameFiber RaidDefenseTaskFiber = GameFiber.StartNew(delegate
            {
                try
                {
                    IsActive = true;
                    IsProtecting = false;
                    HasDefendedSuccessfully = false;
                    HasSpawnedPeds = false;
                    RaidedLocationCellX = (int)(RaidedLocation.EntrancePosition.X / EntryPoint.CellSize);
                    RaidedLocationCellY = (int)(RaidedLocation.EntrancePosition.Y / EntryPoint.CellSize);
                    Loop();
                }
                catch (Exception ex)
                {
                    IsActive = false;
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "RaidDefenseTaskFiber");
        }
        private void StartDraw()
        {
            GameFiber RaidDefenseUIFiber = GameFiber.StartNew(delegate
            {
                try
                {
                    Draw();
                }
                catch (Exception ex)
                {
                    IsActive = false;
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "RaidDefenseTaskFiber");
        }
        private void AttemptSpawn()
        {
            if (NativeHelper.IsNearby(EntryPoint.FocusCellX, EntryPoint.FocusCellY, RaidedLocationCellX, RaidedLocationCellY, 6))
            {
                const int MAXNUMTRIES = 50;
                int tryNumber = 1;
                do
                {
                    SpawnedPeds = GangDispatcher.DispatchRaidSquad(Gang, RaidedLocation);
                    tryNumber++;
                } while (SpawnedPeds.Count == 0 && tryNumber <= MAXNUMTRIES);
                if (SpawnedPeds.Count > 0)
                {
                    HasSpawnedPeds = true;
                }
            }
        }
    }
}
