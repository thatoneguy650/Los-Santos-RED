using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Linq;

public class WaveHandsActivity : DynamicActivity
{
    private bool IsCancelled;
    private IActionable Player;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;


    public WaveHandsActivity(IActionable currentPlayer, IEntityProvideable world, ISettingsProvideable settings)
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
        Player.ActivityManager.IsWavingHands = false;
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
        if (player.ActivityManager.CanPerformActivitesBase)
        {
            return true;
        }
        Game.DisplayHelp($"Cannot Wave Hands");
        return false;
    }
    public void WaveHands()
    {
        Player.WeaponEquipment.SetUnarmed();
        string Animation;
        string DictionaryName;
        if (Player.IsMale)
        {
            DictionaryName = "anim@amb@waving@male";
        }
        else
        {
            DictionaryName = "anim@amb@waving@female";
        }
        Animation = "air_wave";
        AnimationDictionary.RequestAnimationDictionay(DictionaryName);

        Player.ActivityManager.IsWavingHands = true;


        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, DictionaryName, Animation, 2.0f, -2.0f, -1, (int)(eAnimationFlags.AF_UPPERBODY | eAnimationFlags.AF_SECONDARY), 0, false, false, false);
        uint GameTimeStartedWaving = Game.GameTime;

        Player.PlaySpeech("GENERIC_FRIGHTENED_HIGH", false);

        while (Player.ActivityManager.CanPerformActivitesBase && !IsCancelled && Game.GameTime - GameTimeStartedWaving <= 5000)
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
        Player.ActivityManager.IsWavingHands = false;
    }
}