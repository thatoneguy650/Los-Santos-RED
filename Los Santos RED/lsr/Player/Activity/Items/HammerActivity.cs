using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Drawing;
using System.Linq;

namespace LosSantosRED.lsr.Player
{
    public class HammerActivity : DynamicActivity
    {
        private bool IsCancelled;
        private IActionable Player;
        private ISettingsProvideable Settings;
        private HammerItem HammerItem;
        private MeleeWeaponAlias meleeWeaponAlias;

        public HammerActivity(IActionable player, ISettingsProvideable settings, ICameraControllable cameraControllable, HammerItem shovelItem) : base()
        {
            Player = player;
            ModItem = shovelItem;
            Settings = settings;
            HammerItem = shovelItem;
        }

        public override ModItem ModItem { get; set; }
        public override string DebugString => "";
        public override bool CanPause { get; set; } = false;
        public override bool CanCancel { get; set; } = true;
        public override bool IsUpperBodyOnly { get; set; } = true;
        public override string PausePrompt { get; set; } = "Pause Hammer";
        public override string CancelPrompt { get; set; } = "Put Away Hammer";
        public override string ContinuePrompt { get; set; } = "Continue Hammer";
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
            //EntryPoint.WriteToConsole($"Hammer Start");
            GameFiber ShovelWatcher = GameFiber.StartNew(delegate
            {
                try
                {
                    Setup();
                    meleeWeaponAlias = new MeleeWeaponAlias(Player, Settings, HammerItem);
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
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "HammerActivity");
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
            //EntryPoint.WriteToConsoleTestLong("HAMMER ACTIVITY END");
            IsCancelled = true;
            Player.ActivityManager.IsPerformingActivity = false;
            meleeWeaponAlias?.Dispose();
        }
    }
}