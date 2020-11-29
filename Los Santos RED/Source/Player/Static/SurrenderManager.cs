using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionsMethods;


public static class SurrenderManager
{
    public static bool IsCommitingSuicide { get; set; }
    public static bool CanSurrender
    {
        get
        {
            if(!Game.LocalPlayer.IsFreeAiming && (!Game.LocalPlayer.Character.IsInAnyVehicle(false) || Game.LocalPlayer.Character.CurrentVehicle.Speed < 2.5f))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public static void RaiseHands()
    {
        if (Game.LocalPlayer.Character.IsWearingHelmet)
            Game.LocalPlayer.Character.RemoveHelmet(true);

        if (PlayerStateManager.HandsAreUp)
            return;

        if (VehicleFuelManager.CanPumpFuel)//usees the same key, might need to change
            return;

        General.SetPedUnarmed(Game.LocalPlayer.Character, false);
        VehicleEngineManager.TurnOffEngine();
        PlayerStateManager.HandsAreUp = true;
        bool inVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        var sDict = (inVehicle) ? "veh@busted_std" : "ped";
        General.RequestAnimationDictionay(sDict);
        General.RequestAnimationDictionay("busted");
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
            //    Debugging.WriteToLog("RaiseHands", "Finish");
            //}, "SetArrestedAnimation");
            //Debugging.GameFibers.Add(RaiseHandsAnimation);
        }


        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) && Game.LocalPlayer.Character.CurrentVehicle.Speed <= 10f)
            Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = false;
    }
    public static void LowerHands()
    {
        PlayerStateManager.HandsAreUp = false; // You put your hands down
        Game.LocalPlayer.Character.Tasks.Clear();
        if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
            Game.LocalPlayer.Character.CurrentVehicle.IsDriveable = true;
    }
    public static void SetArrestedAnimation(Ped PedToArrest, bool MarkAsNoLongerNeeded, bool StayStanding)
    {
        GameFiber SetArrestedAnimation = GameFiber.StartNew(delegate
        {
            General.RequestAnimationDictionay("veh@busted_std");
            General.RequestAnimationDictionay("busted");
            General.RequestAnimationDictionay("ped");

            if (!PedToArrest.Exists())
                return;

            while (PedToArrest.Exists() && (PedToArrest.IsRagdoll || PedToArrest.IsStunned))
                GameFiber.Yield();

            if (!PedToArrest.Exists())
                return;

            General.SetPedUnarmed(PedToArrest, false);

            if (PedToArrest.IsInAnyVehicle(false))
            {
                Vehicle oldVehicle = PedToArrest.CurrentVehicle;
                if (PedToArrest.Exists() && oldVehicle.Exists())
                {
                    Debugging.WriteToLog("SetArrestedAnimation", "Tasked to leave the vehicle");
                    NativeFunction.CallByName<uint>("TASK_LEAVE_VEHICLE", PedToArrest, oldVehicle, 256);
                    GameFiber.Wait(2500);
                }
            }

            //if (PedToArrest == Game.LocalPlayer.Character && !PlayerState.IsBusted)
            //    return;

            if (StayStanding)
            {
                if (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "ped", "handsup_enter", 3))
                {
                    NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
                    Debugging.WriteToLog("SetArrestedAnimation", "Standing Animation");
                }
            }
            else
            {
                if (!NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_2_hands_up", 3) && !NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_a", 3))
                {
                    Debugging.WriteToLog("SetArrestedAnimation", "Kneel Animation");
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_2_hands_up", 8.0f, -8.0f, -1, 2, 0, false, false, false);
                    GameFiber.Wait(6000);

                    if (!PedToArrest.Exists() || (PedToArrest == Game.LocalPlayer.Character && !PlayerStateManager.IsBusted))
                        return;

                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_a", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                }
            }
            PedToArrest.KeepTasks = true;

            if (MarkAsNoLongerNeeded)
                PedToArrest.IsPersistent = false;
        }, "SetArrestedAnimation");
        Debugging.GameFibers.Add(SetArrestedAnimation);

    }
    public static void UnSetArrestedAnimation(Ped PedToArrest)
    {
            GameFiber UnSetArrestedAnimationGF = GameFiber.StartNew(delegate
            {
                General.RequestAnimationDictionay("random@arrests");
                General.RequestAnimationDictionay("busted");
                General.RequestAnimationDictionay("ped");

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
            Debugging.GameFibers.Add(UnSetArrestedAnimationGF);
    }
    public static void CommitSuicide(Ped PedToSuicide)
    {
        if (IsCommitingSuicide)
            return;

        if (PlayerStateManager.HandsAreUp)
            return;

        if (PedToSuicide.IsInAnyVehicle(false) || PedToSuicide.IsRagdoll || PedToSuicide.IsSwimming || PedToSuicide.IsInCover)
        {
            return;
        }

        GameFiber Suicide = GameFiber.StartNew(delegate
        {

            if (!PedToSuicide.IsInAnyVehicle(false))
            {
                IsCommitingSuicide = true;
                General.RequestAnimationDictionay("mp_suicide");

                WeaponInformation CurrentGun = null;
                if (PedToSuicide.Inventory.EquippedWeapon != null)
                    CurrentGun = WeaponManager.WeaponsList.Where(x => (WeaponHash)x.Hash == PedToSuicide.Inventory.EquippedWeapon.Hash && x.CanPistolSuicide).FirstOrDefault();

                if (CurrentGun != null)
                {
                    if(PedToSuicide.Handle != Game.LocalPlayer.Character.Handle)
                    {
                        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToSuicide, "mp_suicide", "pistol", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                        GameFiber.Wait(750);
                        Vector3 HeadCoordinated = PedToSuicide.GetBonePosition(PedBoneId.Head);
                        NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", PedToSuicide, HeadCoordinated.X, HeadCoordinated.Y, HeadCoordinated.Z, true);
                    }
                    else
                    {
                        Vector3 SuicidePosition = PedToSuicide.Position;

                        int Scene1 = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", SuicidePosition.X, SuicidePosition.Y, SuicidePosition.Z, 0.0f, 0.0f, PedToSuicide.Heading, 2);//270f //old
                        NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", Scene1, false);
                        NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", PedToSuicide, Scene1, "mp_suicide", "pistol", 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
                        NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", Scene1, 0.0f);

                        uint GameTimeStartedSuicide = Game.GameTime;
                        while (Game.GameTime - GameTimeStartedSuicide <= 20000)
                        {
                            float ScenePhase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", Scene1);
                            if (Extensions.IsMoveControlPressed())
                            {
                                break;
                            }
                            if (Game.LocalPlayer.Character.IsDead)
                            {
                                break;
                            }
                            if (ScenePhase >= 0.3f)
                            {
                                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_RATE", Scene1, 0f);
                                if (Game.IsControlJustPressed(2, GameControl.Attack))
                                {
                                    Vector3 HeadCoordinated = PedToSuicide.GetBonePosition(PedBoneId.Head);
                                    NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", PedToSuicide, HeadCoordinated.X, HeadCoordinated.Y, HeadCoordinated.Z, true);
                                    Game.LocalPlayer.Character.Kill();
                                    break;
                                }
                            }
                            GameFiber.Yield();
                        }
                        PedToSuicide.Tasks.Clear();
                    }         
                }
                else
                {
                    NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", PedToSuicide, (uint)2725352035, true);
                    
                    if (PedToSuicide.Handle != Game.LocalPlayer.Character.Handle)
                    {
                        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToSuicide, "mp_suicide", "pill", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                        GameFiber.Wait(6000);
                        PedToSuicide.Kill();
                    }
                    else
                    {
                        Vector3 SuicidePosition = PedToSuicide.Position;

                        int Scene1 = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", SuicidePosition.X, SuicidePosition.Y, SuicidePosition.Z, 0.0f, 0.0f, PedToSuicide.Heading, 2);//270f //old
                        NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", Scene1, false);
                        NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", PedToSuicide, Scene1, "mp_suicide", "pill", 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
                        NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", Scene1, 0.0f);

                        uint GameTimeStartedSuicide = Game.GameTime;
                        bool IsSuicide = false;
                        while (Game.GameTime - GameTimeStartedSuicide <= 20000 && NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", Scene1) < 1.0f)
                        {
                            float ScenePhase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", Scene1);
                            if (Extensions.IsMoveControlPressed())
                            {
                                break;
                            }
                            if (Game.LocalPlayer.Character.IsDead)
                            {
                                break;
                            }
                            if (ScenePhase >= 0.2f)
                            {
                                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_RATE", Scene1, 0.8f);
                            }
                            if (ScenePhase >= 0.25f && !IsSuicide)
                            {
                                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_RATE", Scene1, 0f);
                                if (Game.IsControlJustPressed(2, GameControl.Attack))
                                {
                                    IsSuicide = true;
                                    NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_RATE", Scene1, 1f);
                                }
                            }
                            GameFiber.Yield();
                        }
                        if(IsSuicide)
                            PedToSuicide.Kill();
                        else
                            PedToSuicide.Tasks.Clear();
                    }
                }
                IsCommitingSuicide = false;
            }
        }, "Suicide");
        Debugging.GameFibers.Add(Suicide);
    }
}

