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
            EntryPoint.WriteToConsole($"Screwdriver ACTIVITY Start", 5);
            GameFiber ShovelWatcher = GameFiber.StartNew(delegate
            {
                Setup();
                meleeWeaponAlias = new MeleeWeaponAlias(Player, Settings, ScrewdriverItem);
                meleeWeaponAlias.Start();
                while (!IsCancelled)
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
            }, "ScrewdriverActivity");
        }
        private void Setup()
        {

        }
        private void Dispose()
        {
            EntryPoint.WriteToConsole("Screwdriver ACTIVITY END");
            IsCancelled = true;
            Player.ActivityManager.IsPerformingActivity = false;
            meleeWeaponAlias?.Dispose();
        }
    }
}