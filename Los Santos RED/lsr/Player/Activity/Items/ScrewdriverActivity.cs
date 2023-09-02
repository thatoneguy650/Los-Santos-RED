using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Drawing;
using System.Linq;

namespace LosSantosRED.lsr.Player
{
    public class ScrewdriverActivity : DynamicActivity
    {
        private bool IsCancelled;
        private IActionable Player;
        private ISettingsProvideable Settings;
        private ScrewdriverItem ScrewdriverItem;
        private MeleeWeaponAlias meleeWeaponAlias;

        public ScrewdriverActivity(IActionable player, ISettingsProvideable settings, ScrewdriverItem screwdriverItem) : base()
        {
            Player = player;
            ModItem = screwdriverItem;
            Settings = settings;
            ScrewdriverItem = screwdriverItem;
        }

        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override bool IsUpperBodyOnly { get; set; } = true;
        public override string PausePrompt { get; set; } = "Pause Screwdriver";
        public override string CancelPrompt { get; set; } = "Put Away Screwdriver";
        public override string ContinuePrompt { get; set; } = "Continue Screwdriver";
        public override void Cancel()
        {
            Dispose();
        }
        public override void Pause()
        {

        }
        public override bool IsPaused() => false;
        public override void Continue()
        {

        }
        public override void Start()
        {
            //EntryPoint.WriteToConsole($"Screwdriver ACTIVITY Start");
            GameFiber ShovelWatcher = GameFiber.StartNew(delegate
            {
                try
                {
                    Setup();
                    meleeWeaponAlias = new MeleeWeaponAlias(Player, Settings, ScrewdriverItem);
                    meleeWeaponAlias.Start();
                    Player.ActivityManager.HasScrewdriverInHand = true;
                    Player.ActivityManager.CurrentScrewdriver = ScrewdriverItem;
                    while (!IsCancelled)
                    {
                        meleeWeaponAlias.Update();
                        AddVehiclePrompts();
                        if (meleeWeaponAlias.IsCancelled)
                        {
                            meleeWeaponAlias.Dispose();
                            break;
                        }
                        GameFiber.Yield();
                    }
                    Dispose();
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "ScrewdriverActivity");
        }

        private void AddVehiclePrompts()
        {
            if(Player.IsInVehicle || Player.CurrentLookedAtVehicle == null || !Player.CurrentLookedAtVehicle.Vehicle.Exists())
            {
                Player.ButtonPrompts.RemovePrompts("Screwdriver");
                return;
            }
            Player.ButtonPrompts.AddPrompt("Screwdriver", "Pick Lock", "PickLock", Settings.SettingsManager.KeySettings.InteractPositiveOrYes, 12);
            Player.ButtonPrompts.AddPrompt("Screwdriver", "Remove Plate", "RemovePlate", Settings.SettingsManager.KeySettings.InteractNegativeOrNo, 13);
            if (Player.ButtonPrompts.IsPressed("PickLock"))
            {
                Dispose();
                Player.ActivityManager.EnterVehicleGeneric();
            }
            if (Player.ButtonPrompts.IsPressed("RemovePlate"))
            {
                Dispose();
                Player.ActivityManager.RemovePlate();
            }
        }

        public override bool CanPerform(IActionable player)
        {
            if (player.IsOnFoot && player.ActivityManager.CanPerformActivitesBase)
            {
                return true;
            }
            Game.DisplayHelp($"Cannot Start Activity: {ModItem?.Name}");
            return false;
        }
        private void Setup()
        {

        }
        private void Dispose()
        {
            //EntryPoint.WriteToConsoleTestLong("Screwdriver ACTIVITY END");
            IsCancelled = true;
            Player.ActivityManager.IsPerformingActivity = false;
            meleeWeaponAlias?.Dispose();
            Player.ActivityManager.HasScrewdriverInHand = false;
            Player.ActivityManager.CurrentScrewdriver = null;
            Player.ButtonPrompts.RemovePrompts("Screwdriver");
        }
    }
}