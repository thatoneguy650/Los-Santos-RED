//using ExtensionsMethods;
//using LosSantosRED.lsr.Interface;
//using LosSantosRED.lsr.Player;
//using Rage;
//using Rage.Native;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Windows.Forms;

//public class TreatmentActivity_Old : DynamicActivity
//{
//    private uint GameTimeStartedConversing;
//    private bool IsActivelyConversing;
//    private bool IsTasked;
//    private bool IsBlockedEvents;
//    private PedExt Ped;
//    private IInteractionable Player;
//    private bool CancelledConversation;
//    private ISettingsProvideable Settings;
//    private ICrimes Crimes;
//    private dynamic pedHeadshotHandle;
//    private IModItems ModItems;
//    private bool IsCancelled;
//    private WeaponInformation LastWeapon;

//    //private WeaponInformation previousWeapon;

//    public TreatmentActivity_Old(IInteractionable player, PedExt ped, ISettingsProvideable settings, ICrimes crimes, IModItems modItems)
//    {
//        Player = player;
//        Ped = ped;
//        Settings = settings;
//        Crimes = crimes;
//        ModItems = modItems;
//    }
//    public override string DebugString => $"TimesInsultedByPlayer {Ped.TimesInsultedByPlayer} FedUp {Ped.IsFedUpWithPlayer}";
//    public override ModItem ModItem { get; set; }
//    public override bool CanPause { get; set; } = false;
//    public override bool CanCancel { get; set; } = true;
//    public override bool IsUpperBodyOnly { get; set; } = false;
//    public override string PausePrompt { get; set; } = "Pause Treating";
//    public override string CancelPrompt { get; set; } = "Stop Treating";
//    public override string ContinuePrompt { get; set; } = "Continue Treating";
//    public override void Start()
//    {
//        if (Ped.Pedestrian.Exists())
//        {
//            //EntryPoint.WriteToConsole($"Revive Started");
//            Player.ActivityManager.IsTreatingPed = true;
//            GameFiber.StartNew(delegate
//            {
//                try
//                {
//                    RevivePed();
//                    Cancel();
//                }
//                catch (Exception ex)
//                {
//                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
//                    EntryPoint.ModController.CrashUnload();
//                }
//            }, "Conversation");
//        }
//    }
//    public override bool CanPerform(IActionable player)
//    {
//        if (!player.ActivityManager.CanReviveLookedAtPed)
//        {
//            Game.DisplayHelp($"Cannot treat ped");
//            return false;
//        }
//        if (player.IsOnFoot && player.ActivityManager.CanPerformActivitesBase)
//        {
//            return true;
//        }
//        Game.DisplayHelp($"Cannot treat ped");
//        return false;
//    }
//    private void RevivePed()
//    {
//        if (MoveToBody())
//        {
//            bool hasCompletedTasks = DoReviveAnimation();
//            if (hasCompletedTasks)
//            {
//                FinishRevive();
//                PlayAnimation("amb@medic@standing@tendtodead@exit", "exit");
//            }
//            if (LastWeapon != null)
//            {
//                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Game.LocalPlayer.Character, (uint)LastWeapon.Hash, true);
//            }
//        }
//    }
//    private void FinishRevive()
//    {
//        if (Ped.OnTreatedByEMT(Settings.SettingsManager.EMSSettings.RevivePercentage))//true if died, w/.e
//        {
//            Player.PlaySpeech("GENERIC_SHOCKED_HIGH", false);
//        }
//    }
//    private bool MoveToBody()
//    {
//        pedHeadshotHandle = NativeFunction.Natives.RegisterPedheadshot<uint>(Ped.Pedestrian);
//        Vector3 DesiredPosition = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Ped.Pedestrian, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Ped.Pedestrian, 0));
//        float DesiredHeading = Game.LocalPlayer.Character.Heading;
//        NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", Player.Character, Ped.Pedestrian, -1, 1.75f, 0.75f, 1073741824, 1); //Original and works ok
//        uint GameTimeStartedMovingToBody = Game.GameTime;
//        float heading = Game.LocalPlayer.Character.Heading;
//        bool IsFacingDirection = true;
//        bool IsCloseEnough = false;
//        while (Game.GameTime - GameTimeStartedMovingToBody <= 5000 && !IsCloseEnough && !IsCancelled)
//        {
//            if (Player.IsMoveControlPressed)
//            {
//                IsCancelled = true;
//            }
//            if (!Ped.Pedestrian.Exists() || !Player.IsAliveAndFree)
//            {
//                IsCancelled = true;
//                break;
//            }
//            IsCloseEnough = Game.LocalPlayer.Character.DistanceTo2D(Ped.Pedestrian) <= 1.85f;
//            // Rage.Debug.DrawArrowDebug(DesiredPosition, Vector3.Zero, Rotator.Zero, 1f, System.Drawing.Color.Yellow);
//            GameFiber.Yield();
//        }
//        Vector3 PedRoot = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Ped.Pedestrian, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Ped.Pedestrian, 0));
//        float calcHeading = (float)GetHeading(Player.Character.Position, PedRoot);
//        float calcHeading2 = (float)CalculeAngle(PedRoot, Player.Character.Position);
//        DesiredHeading = calcHeading2;
//        NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", Player.Character, Ped.Pedestrian, 1000);
//        GameFiber.Sleep(1000);
//        if (IsCloseEnough && IsFacingDirection && !IsCancelled)
//        {
//            // EntryPoint.WriteToConsole($"MoveToBody IN POSITION {Game.LocalPlayer.Character.DistanceTo(DesiredPosition)} {Extensions.GetHeadingDifference(heading, DesiredHeading)} {heading} {DesiredHeading}");
//            return true;
//        }
//        else
//        {
//            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
//            //EntryPoint.WriteToConsole($"MoveToBody NOT IN POSITION EXIT {Game.LocalPlayer.Character.DistanceTo(DesiredPosition)} {Extensions.GetHeadingDifference(heading, DesiredHeading)} {heading} {DesiredHeading}");
//            return false;
//        }
//    }
//    private double GetHeading(Vector3 a, Vector3 b)
//    {
//        double x = b.X - a.X;
//        double y = b.Y - a.Y;
//        return 270 - Math.Atan2(y, x) * (180 / Math.PI);
//    }
//    private double CalculeAngle(Vector3 start, Vector3 arrival)
//    {
//        var deltaX = Math.Pow((arrival.X - start.X), 2);
//        var deltaY = Math.Pow((arrival.Y - start.Y), 2);
//        var radian = Math.Atan2((arrival.Y - start.Y), (arrival.X - start.X));
//        var angle = (radian * (180 / Math.PI) + 360) % 360;

//        return angle;
//    }
//    private bool DoReviveAnimation()
//    {
//        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@enter");
//        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@base");
//        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@exit");
//        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@idle_a");
//        if (Player.WeaponEquipment.CurrentWeapon != null)
//        {
//            LastWeapon = Player.WeaponEquipment.CurrentWeapon;
//        }
//        else
//        {
//            LastWeapon = null;
//        }

//        Player.WeaponEquipment.SetUnarmed();
//        List<string> IdleToPlay = new List<string>() { "idle_a", "idle_b", "idle_c" };
//        if (PlayAnimation("amb@medic@standing@tendtodead@enter", "enter") && PlayAnimation("amb@medic@standing@tendtodead@idle_a", IdleToPlay.PickRandom()))
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }
//    private bool PlayAnimation(string dictionary, string animation)
//    {
//        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, dictionary, animation, 8.0f, -8.0f, -1, 2, 0, false, false, false);
//        uint GameTimeStartedReviveAnimation = Game.GameTime;
//        float AnimationTime = 0.0f;
//        while (AnimationTime < 1.0f && !IsCancelled && Game.GameTime - GameTimeStartedReviveAnimation <= 10000)
//        {
//            AnimationTime = NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Player.Character, dictionary, animation);
//            if (Player.IsMoveControlPressed)
//            {
//                IsCancelled = true;
//            }
//            if (!Ped.Pedestrian.Exists() || !Player.IsAliveAndFree)
//            {
//                IsCancelled = true;
//                break;
//            }
//            GameFiber.Yield();
//        }
//        if (!IsCancelled && AnimationTime >= 1.0f)
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }
//    public override void Continue()
//    {

//    }
//    public override void Cancel()
//    {
//        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
//        Player.ActivityManager.IsTreatingPed = false;
//        Player.ActivityManager.IsPerformingActivity = false;
//    }
//    public override void Pause()
//    {
//        Cancel();
//    }
//    public override bool IsPaused() => false;
//}