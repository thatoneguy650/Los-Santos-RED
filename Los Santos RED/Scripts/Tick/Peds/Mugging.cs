using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class MuggingScript
{
    public static bool IsRunning { get; set; }
    public static bool IsMugging { get; set; }
    public static void Initialize()
    {
        IsRunning = true;
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            if (!IsMugging)
            {
                if (Game.LocalPlayer.Character.IsConsideredArmed() && !PlayerState.IsInVehicle)
                {
                    if (Game.LocalPlayer.Character.IsAiming && !IsHoldingMelee())
                    {
                        CheckArmedMugging();
                    }
                    else
                    {
                        CheckUnarmedMugging();
                    }
                }
            }
        }

    }
    private static void CheckArmedMugging()
    {
        Entity ArmedMuggingTargetPed = Game.LocalPlayer.GetFreeAimingTarget();
        if (ArmedMuggingTargetPed.Exists() && ArmedMuggingTargetPed is Ped)
        {
            PedExt GTAPedTarget = PedList.GetCivilian(ArmedMuggingTargetPed.Handle);
            if (GTAPedTarget != null)
            {
                if (!GTAPedTarget.HasBeenMugged && !GTAPedTarget.Pedestrian.IsInAnyVehicle(false) && GTAPedTarget.Pedestrian.IsAlive)
                {
                    if (GTAPedTarget.DistanceToPlayer <= 7f)
                        MugTarget(GTAPedTarget, false);
                    else if (GTAPedTarget.DistanceToPlayer <= 15f && GTAPedTarget.CanSeePlayer)
                        MugTarget(GTAPedTarget, false);
                }
            }
        }
    }
    private static void CheckUnarmedMugging()
    {
        PedExt GTAPedTarget = PedList.GetCivilian(GetTargetHandle());
        if (GTAPedTarget != null)
        {
            if (!GTAPedTarget.HasBeenMugged && GTAPedTarget.Pedestrian.IsAlive)
                MugTarget(GTAPedTarget, true);
        }
    }
    private static void MugTarget(PedExt MuggingTarget,bool IsMelee)
    {
        if (!MuggingTarget.CanBeTasked)
            return;

        GameFiber.StartNew(delegate
        {
            IsMugging = true;
            MuggingTarget.CanBeTasked = false;
            uint GameTimeStartedMugging = Game.GameTime;
            MuggingTarget.Pedestrian.BlockPermanentEvents = true;

            General.RequestAnimationDictionay("ped");

            if (!Game.LocalPlayer.Character.IsAnySpeechPlaying)
                Game.LocalPlayer.Character.PlayAmbientSpeech("CHALLENGE_THREATEN");
            
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", MuggingTarget.Pedestrian, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
            GameFiber.Sleep(750);
            MuggingTarget.Pedestrian.PlayAmbientSpeech("GUN_BEG");
            while (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", MuggingTarget.Pedestrian, "ped", "handsup_enter", 1))
            {
                GameFiber.Sleep(100);


                if(Game.GameTime - GameTimeStartedMugging >= 2500)
                {
                    IsMugging = false;
                    MuggingTarget.CanBeTasked = true;
                    break;
                }
            }

            if (!IsMugging)
                return;

            GameFiber.Sleep(500);

            GameTimeStartedMugging = Game.GameTime;
            bool Intimidated = false;
            while (Game.GameTime - GameTimeStartedMugging <= 1500)
            {      
                if (!IsMelee && !Game.LocalPlayer.IsFreeAiming)
                {
                    Intimidated = false;
                    break;
                }
                else if (IsMelee && !NativeFunction.CallByName<bool>("IS_PLAYER_TARGETTING_ANYTHING", Game.LocalPlayer))
                {
                    Intimidated = false;
                    break;
                }
                Intimidated = true;
                GameFiber.Yield();
            }

            MuggingTarget.Pedestrian.BlockPermanentEvents = false;
            if (Intimidated)
            {
                NativeFunction.CallByName<bool>("SET_PED_MONEY", MuggingTarget.Pedestrian, 0);
                Vector3 MoneyPos = MuggingTarget.Pedestrian.Position.Around2D(0.5f, 1.5f);
                NativeFunction.CallByName<bool>("CREATE_AMBIENT_PICKUP", Game.GetHashKey("PICKUP_MONEY_VARIABLE"), MoneyPos.X, MoneyPos.Y, MoneyPos.Z, 0, General.MyRand.Next(15, 100), 1, false, true);
                MuggingTarget.HasBeenMugged = true;
                MuggingTarget.AddCrime(Crimes.Mugging,MuggingTarget.Pedestrian.Position);
            }
            MuggingTarget.CanBeTasked = true;
            IsMugging = false;      
        });
    }
    private static uint GetTargetHandle()
    {
        uint TargetEntity;
        bool Found;
        unsafe
        {
            Found = NativeFunction.CallByName<bool>("GET_PLAYER_TARGET_ENTITY", Game.LocalPlayer, &TargetEntity);
        }
        if (!Found)
            return 0;

        uint Handle = TargetEntity;
        return Handle;
    }
    private static bool IsHoldingMelee()
    {
        GTAWeapon MyWeapon = General.GetCurrentWeapon(Game.LocalPlayer.Character);
        if (MyWeapon == null || MyWeapon.Category != GTAWeapon.WeaponCategory.Melee)
            return false;
        else
            return true;
    }
}
