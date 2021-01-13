using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;

public class Robbery : Interaction
{
    private uint GameTimeStartedMugging;
    private IInteractionable Player;
    private PedExt Target;
    private bool IsTargetIntimidated;
    private uint GameTimeStartedIntimidating;
    private bool IsAttemptingToIntimidate => GameTimeStartedIntimidating != 0 && Game.GameTime - GameTimeStartedIntimidating <= 1500;
    public Robbery(IInteractionable player, PedExt target)
    {
        Player = player;
        Target = target;
    }
    public override string Prompt => IsAttemptingToIntimidate ? "Aim at the Target to Intimidate!" : "";
    public override void Dispose()
    {
        CleanUp();
    }
    public override void Start()
    {
        if (Target.CanBeTasked)
        {
            GameFiber.StartNew(delegate
            {
                Setup();
            });
        }
    }
    private void CheckIntimidation()
    {
        GameTimeStartedIntimidating = Game.GameTime;
        while (IsAttemptingToIntimidate)
        {
            if (!Game.LocalPlayer.IsFreeAiming && !NativeFunction.CallByName<bool>("IS_PLAYER_TARGETTING_ANYTHING", Game.LocalPlayer))
            {
                IsTargetIntimidated = false;
                break;
            }
            IsTargetIntimidated = true;
            GameFiber.Yield();
        }
        if (IsTargetIntimidated)
        {
            CreateMoneyDrop();
        }
        Target.HasBeenMugged = true;
        CleanUp();
    }
    private void CleanUp()
    {
        Target.Pedestrian.BlockPermanentEvents = false;
        Target.Pedestrian.Tasks.Flee(Player.Character, 100f, -1);
        Target.CanBeTasked = true;
        Player.IsMugging = false;
    }
    private void CreateMoneyDrop()
    {
        NativeFunction.CallByName<bool>("SET_PED_MONEY", Target.Pedestrian, 0);
        Vector3 MoneyPos = Target.Pedestrian.Position.Around2D(0.5f, 1.5f);
        NativeFunction.CallByName<bool>("CREATE_AMBIENT_PICKUP", Game.GetHashKey("PICKUP_MONEY_VARIABLE"), MoneyPos.X, MoneyPos.Y, MoneyPos.Z, 0, RandomItems.MyRand.Next(15, 100), 1, false, true);
    }
    private void EnterHandsUp()
    {
        if (!Game.LocalPlayer.Character.IsAnySpeechPlaying)
        {
            Game.LocalPlayer.Character.PlayAmbientSpeech("CHALLENGE_THREATEN");
        }
        NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Target.Pedestrian, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
        GameFiber.Sleep(750);
        Target.Pedestrian.PlayAmbientSpeech("GUN_BEG");
        while (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", Target.Pedestrian, "ped", "handsup_enter", 1) && Game.GameTime - GameTimeStartedMugging <= 2500)
        {
            GameFiber.Sleep(100);
        }
        if (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", Target.Pedestrian, "ped", "handsup_enter", 1))
        {      
            CleanUp();
        }
        else
        {      
            CheckIntimidation();
        }
    }
    private void Setup()
    {
        Player.IsMugging = true;
        Target.CanBeTasked = false;
        GameTimeStartedMugging = Game.GameTime;
        Target.Pedestrian.BlockPermanentEvents = true;
        AnimationDictionary.RequestAnimationDictionay("ped");
        EnterHandsUp();
    }
}