using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Drawing;
using System.Linq;

namespace LosSantosRED.lsr.Player
{
    public class DrillActivity : DynamicActivity
    {
        private bool IsCancelled;
        private IActionable Player;
        private ISettingsProvideable Settings;
        private DrillItem DrillItem;
        private MeleeWeaponAlias meleeWeaponAlias;

        public DrillActivity(IActionable player, ISettingsProvideable settings, DrillItem drillItem) : base()
        {
            Player = player;
            ModItem = drillItem;
            Settings = settings;
            DrillItem = drillItem;
        }

        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override string PausePrompt { get; set; } = "Pause Drill";
        public override string CancelPrompt { get; set; } = "Put Away Drill";
        public override string ContinuePrompt { get; set; } = "Continue Drill";
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
            EntryPoint.WriteToConsole($"Drill ACTIVITY Start", 5);
            GameFiber ShovelWatcher = GameFiber.StartNew(delegate
            {
                Setup();
                meleeWeaponAlias = new MeleeWeaponAlias(Player, Settings, DrillItem, 1317494643);
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
            }, "DrillActivity");
        }
        private void Setup()
        {

        }
        private void Dispose()
        {
            EntryPoint.WriteToConsole("Drill ACTIVITY END");
            IsCancelled = true;
            Player.ActivityManager.IsPerformingActivity = false;
            meleeWeaponAlias?.Dispose();
        }
    }
}