using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Linq;

public class HailCabActivity : DynamicActivity
{
    private bool IsCancelled;
    private IActionable Player;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;


    public HailCabActivity(IActionable currentPlayer, IEntityProvideable world, ISettingsProvideable settings)
    {
        Player = currentPlayer;
        World = world;
        Settings = settings;
    }

    public override ModItem ModItem { get; set; }
    public override string DebugString => "";
    public override bool CanPause { get; set; } = false;
    public override bool CanCancel { get; set; } = false;
    public override bool IsUpperBodyOnly { get; set; } = true;
    public override string PausePrompt { get; set; } = "Pause Activity";
    public override string CancelPrompt { get; set; } = "Stop Activity";
    public override string ContinuePrompt { get; set; } = "Continue Activity";
    public override void Cancel()
    {
        IsCancelled = true;
        Player.ActivityManager.IsPerformingActivity = false;
        Player.ActivityManager.IsHailingTaxi = false;
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
        GameFiber WaveWatcher = GameFiber.StartNew(delegate
        {
            try
            {
                WaveHands();
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "WaveHandsActivity");
    }
    public override bool CanPerform(IActionable player)
    {
        if (player.ActivityManager.CanPerformActivitesBase && Player.IsAliveAndFree)
        {
            return true;
        }
        Game.DisplayHelp($"Cannot Hail Taxi");
        return false;
    }
    public void WaveHands()
    {
        Player.WeaponEquipment.SetUnarmed();
        string Animation = "hail_taxi";
        string DictionaryName = "taxi_hail";
        AnimationDictionary.RequestAnimationDictionay(DictionaryName);

        Player.ActivityManager.IsHailingTaxi = true;


        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, DictionaryName, Animation, 2.0f, -2.0f, -1, (int)(eAnimationFlags.AF_UPPERBODY | eAnimationFlags.AF_SECONDARY), 0, false, false, false);
        uint GameTimeStartedWaving = Game.GameTime;

        Player.PlaySpeech("TAXI_HAIL", false);

        while (Player.ActivityManager.CanPerformActivitesBase && !IsCancelled && Game.GameTime - GameTimeStartedWaving <= 2000)
        {
            Player.WeaponEquipment.SetUnarmed();
            float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, DictionaryName, Animation);
            if (AnimationTime >= 1.0f)
            {
                break;
            }
            GameFiber.Yield();
        }
        NativeFunction.Natives.CLEAR_PED_SECONDARY_TASK(Player.Character);
        Player.ActivityManager.IsPerformingActivity = false;
        Player.ActivityManager.IsHailingTaxi = false;
    }
}