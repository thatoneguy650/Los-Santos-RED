using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using Rage.Native;

//need to redo as an actiual activity
public class SurrenderActivity : DynamicActivity
{
    private IInputable Player;
    public SurrenderActivity(IInputable currentPlayer)
    {
        Player = currentPlayer;
    }
    public bool CanSurrender => !Player.HandsAreUp && !Player.IsAiming && (!Player.IsInVehicle || !Player.IsMoving);
    public override string DebugString => "";
    public bool IsCommitingSuicide { get; set; }
    public override void Cancel()
    {
        LowerHands();
    }
    public override void Continue()
    {
    }
    public override void Start()
    {
        RaiseHands();
    }
    public void LowerHands()
    {
        EntryPoint.WriteToConsole($"PLAYER EVENT: Lower Hands", 3);
        Player.HandsAreUp = false; // You put your hands down
        Game.LocalPlayer.Character.Tasks.Clear();
        if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
            Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = true;
    }
    public void RaiseHands()
    {
        EntryPoint.WriteToConsole($"PLAYER EVENT: Raise Hands", 3);
        if (Game.LocalPlayer.Character.IsWearingHelmet)
            Game.LocalPlayer.Character.RemoveHelmet(true);

        if (Player.HandsAreUp)
            return;

        //if (Mod.Player.Instance.CurrentVehicle == null || Mod.Player.Instance.CurrentVehicle.FuelTank.CanPump)//usees the same key, might need to change
        //    return;

        Player.SetUnarmed();
        //if (Mod.Player.Instance.CurrentVehicle != null)
        //{
        //    Mod.Player.Instance.CurrentVehicle.ToggleEngine(false);
        //}
        Player.HandsAreUp = true;
        bool inVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        var sDict = (inVehicle) ? "veh@busted_std" : "ped";
        AnimationDictionary.RequestAnimationDictionay(sDict);
        AnimationDictionary.RequestAnimationDictionay("busted");
        if (inVehicle)
        {
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "stay_in_car_crim", 2.0f, -2.0f, -1, 50, 0, true, false, true);
        }
        else
        {
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);

            //works but need to change the unset arrested animation to work with it
            //GameFiber RaiseHandsAnimation = GameFiber.StartNew(delegate
            //{
            //    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "busted", "idle_2_hands_up", 8.0f, -8.0f, -1, 2, 0, false, false, false);
            //    uint GameTimeStartedRaisingHands = Game.GameTime;
            //    bool Cancel = false;
            //    while(Game.GameTime - GameTimeStartedRaisingHands <= 5500)
            //    {
            //        if(!Game.IsKeyDownRightNow(LosSantosRED.MySettings.SurrenderKey))
            //        {
            //            Cancel = true;
            //            break;
            //        }
            //        GameFiber.Sleep(100);
            //    }
            //    if(Cancel)
            //    {
            //        AreHandsRaised = false;
            //        Game.LocalPlayer.Character.Tasks.Clear();
            //    }
            //    else
            //    {
            //        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "busted", "idle_a", 8.0f, -8.0f, -1, 1, 0, false, false, false);

            //        while(Game.IsKeyDownRightNow(LosSantosRED.MySettings.SurrenderKey))
            //        {
            //            GameFiber.Sleep(100);
            //        }
            //        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "busted", "hands_up_2_idle", 8.0f, -2.0f, -1, 1, 0, false, false, false);
            //        GameFiber.Sleep(2000);
            //        AreHandsRaised = false;
            //        Game.LocalPlayer.Character.Tasks.Clear();

            //    }
            //    AreHandsRaised = false;
            //    Mod.Debugging.WriteToLog("RaiseHands", "Finish");
            //}, "SetArrestedAnimation");
            //Debugging.GameFibers.Add(RaiseHandsAnimation);
        }

        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Speed <= 10f)
            Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = false;
    }
    public void SetArrestedAnimation(Ped PedToArrest, bool MarkAsNoLongerNeeded, bool StayStanding)
    {
        GameFiber SetArrestedAnimation = GameFiber.StartNew(delegate
        {
            AnimationDictionary.RequestAnimationDictionay("veh@busted_std");
            AnimationDictionary.RequestAnimationDictionay("busted");
            AnimationDictionary.RequestAnimationDictionay("ped");

            if (!PedToArrest.Exists())
            {
                return;
            }

            while (PedToArrest.Exists() && (PedToArrest.IsRagdoll || PedToArrest.IsStunned))
            {
                GameFiber.Yield();
            }

            if (!PedToArrest.Exists())
            {
                return;
            }

            Player.SetUnarmed();

            if (PedToArrest.IsInAnyVehicle(false))
            {
                Vehicle oldVehicle = PedToArrest.CurrentVehicle;
                if (PedToArrest.Exists() && oldVehicle.Exists())
                {
                    //EntryPoint.WriteToConsole("SetArrestedAnimation! Tasked to leave the vehicle");
                    NativeFunction.CallByName<uint>("TASK_LEAVE_VEHICLE", PedToArrest, oldVehicle, 256);
                    GameFiber.Wait(2500);
                }
            }
            if (StayStanding)
            {
                if (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "ped", "handsup_enter", 3))
                {
                    NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
                    //EntryPoint.WriteToConsole("SetArrestedAnimation! Standing Animation");
                }
            }
            else
            {
                if (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_2_hands_up", 3) && !NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_a", 3))
                {
                    //EntryPoint.WriteToConsole("SetArrestedAnimation! Kneel Animation");
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_2_hands_up", 8.0f, -8.0f, -1, 2, 0, false, false, false);
                    GameFiber.Wait(6000);

                    if (!PedToArrest.Exists() || (PedToArrest == Game.LocalPlayer.Character && !Player.IsBusted))
                    {
                        return;
                    }

                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_a", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                }
            }
            PedToArrest.KeepTasks = true;

            if (MarkAsNoLongerNeeded)
            {
                PedToArrest.IsPersistent = false;
            }
        }, "SetArrestedAnimation");
    }
    public void UnSetArrestedAnimation(Ped PedToArrest)
    {
        GameFiber UnSetArrestedAnimationGF = GameFiber.StartNew(delegate
        {
            AnimationDictionary.RequestAnimationDictionay("random@arrests");
            AnimationDictionary.RequestAnimationDictionay("busted");
            AnimationDictionary.RequestAnimationDictionay("ped");

            if (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_a", 3) || NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_2_hands_up", 3))
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "random@arrests", "kneeling_arrest_escape", 8.0f, -8.0f, -1, 4096, 0, 0, 1, 0);//"random@arrests", "kneeling_arrest_escape", 8.0f, -8.0f, -1, 120, 0, 0, 1, 0);//"random@arrests", "kneeling_arrest_escape", 8.0f, -8.0f, -1, 4096, 0, 0, 1, 0);
                GameFiber.Wait(1000);//1250
                PedToArrest.Tasks.Clear();
            }
            else if (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "ped", "handsup_enter", 3))
            {
                PedToArrest.Tasks.Clear();
            }
        }, "UnSetArrestedAnimation");
    }
}