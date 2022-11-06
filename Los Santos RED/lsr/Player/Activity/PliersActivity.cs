using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Drawing;
using System.Linq;

namespace LosSantosRED.lsr.Player
{
    public class PliersActivity : DynamicActivity
    {
        private bool IsCancelled;
        private IActionable Player;
        private ISettingsProvideable Settings;
        private PliersItem PliersItem;
        private MeleeWeaponAlias meleeWeaponAlias;

        public PliersActivity(IActionable player, ISettingsProvideable settings, PliersItem plersItem) : base()
        {
            Player = player;
            ModItem = plersItem;
            Settings = settings;
            PliersItem = plersItem;
        }

        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override string PausePrompt { get; set; } = "Pause Pliers";
        public override string CancelPrompt { get; set; } = "Put Away Pliers";
        public override string ContinuePrompt { get; set; } = "Continue Pliers";
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
            EntryPoint.WriteToConsole($"Pliers ACTIVITY Start", 5);
            GameFiber ShovelWatcher = GameFiber.StartNew(delegate
            {
                Setup();
                meleeWeaponAlias = new MeleeWeaponAlias(Player, Settings, PliersItem, 1317494643);
                meleeWeaponAlias.Start();
                while (Player.ActivityManager.CanPerformMobileActivities && !IsCancelled)
                {
                    meleeWeaponAlias.Update();
                    if (meleeWeaponAlias.IsCancelled)
                    {
                        meleeWeaponAlias.Dispose();
                        break;
                    }
                    GameFiber.Yield();
                }
                Dispose();
            }, "PliersActivity");
        }
        private void Setup()
        {

        }
        private void Dispose()
        {
            EntryPoint.WriteToConsole("Pliers ACTIVITY END");
            IsCancelled = true;
            Player.ActivityManager.IsPerformingActivity = false;
            meleeWeaponAlias?.Dispose();
        }
    }
}