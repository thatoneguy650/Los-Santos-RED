using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionsMethods;


public static class Surrendering
{
    public static bool IsCommitingSuicide { get; set; }
    public static void RaiseHands()
    {
        if (Game.LocalPlayer.Character.IsWearingHelmet)
            Game.LocalPlayer.Character.RemoveHelmet(true);

        if (Game.LocalPlayer.WantedLevel > 0 && Police.CurrentCrimes.KillingPolice.InstancesObserved < 5)
            Police.CurrentPoliceState = Police.PoliceState.ArrestedWait;

        if (LosSantosRED.HandsAreUp)
            return;

        VehicleEngine.TurnOffEngine();

        LosSantosRED.HandsAreUp = true;
        bool inVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        var sDict = (inVehicle) ? "veh@busted_std" : "ped";
        LosSantosRED.RequestAnimationDictionay(sDict);
        if (inVehicle)
        {
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "stay_in_car_crim", 2.0f, -2.0f, -1, 50, 0, true, false, true);
        }
        else
        {
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, sDict, "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
        }

    }
    public static void SetArrestedAnimation(Ped PedToArrest, bool MarkAsNoLongerNeeded)
    {
        GameFiber SetArrestedAnimation = GameFiber.StartNew(delegate
        {
            LosSantosRED.RequestAnimationDictionay("veh@busted_std");
            LosSantosRED.RequestAnimationDictionay("busted");
            LosSantosRED.RequestAnimationDictionay("ped");

            if (!PedToArrest.Exists())
                return;

            while (PedToArrest.Exists() && (PedToArrest.IsRagdoll || PedToArrest.IsStunned))
                GameFiber.Yield();

            if (!PedToArrest.Exists())
                return;

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

            if (PedToArrest == Game.LocalPlayer.Character && !LosSantosRED.IsBusted)
                return;

            if (LosSantosRED.PlayerWantedLevel <= 2)
            {
                NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
            }
            else
            {
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_2_hands_up", 8.0f, -8.0f, -1, 2, 0, false, false, false);
                GameFiber.Wait(6000);

                if (!PedToArrest.Exists() || (PedToArrest == Game.LocalPlayer.Character && !LosSantosRED.IsBusted))
                    return;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "busted", "idle_a", 8.0f, -8.0f, -1, 1, 0, false, false, false);
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
                LosSantosRED.RequestAnimationDictionay("random@arrests");
                LosSantosRED.RequestAnimationDictionay("busted");
                LosSantosRED.RequestAnimationDictionay("ped");

                if (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_a", 1) || NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "busted", "idle_2_hands_up", 1))
                {
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToArrest, "random@arrests", "kneeling_arrest_escape", 8.0f, -8.0f, -1, 4096, 0, 0, 1, 0);//"random@arrests", "kneeling_arrest_escape", 8.0f, -8.0f, -1, 120, 0, 0, 1, 0);//"random@arrests", "kneeling_arrest_escape", 8.0f, -8.0f, -1, 4096, 0, 0, 1, 0);
                    GameFiber.Wait(1000);//1250
                    PedToArrest.Tasks.Clear();
                }
                else if (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToArrest, "ped", "handsup_enter", 1))
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

        if (Game.LocalPlayer.Character.IsInAnyVehicle(false) || Game.LocalPlayer.Character.IsRagdoll || Game.LocalPlayer.Character.IsSwimming || Game.LocalPlayer.Character.IsInCover)//tons more should probably be checked
        {
            return;
        }

        GameFiber Suicide = GameFiber.StartNew(delegate
        {

            if (!PedToSuicide.IsInAnyVehicle(false))
            {
                IsCommitingSuicide = true;
                LosSantosRED.RequestAnimationDictionay("mp_suicide");

                GTAWeapon CurrentGun = null;
                if (PedToSuicide.Inventory.EquippedWeapon != null)
                    CurrentGun = GTAWeapons.WeaponsList.Where(x => (WeaponHash)x.Hash == PedToSuicide.Inventory.EquippedWeapon.Hash && x.CanPistolSuicide).FirstOrDefault();

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
                        bool Cancel = false;
                        while (Game.GameTime - GameTimeStartedSuicide <= 20000)
                        {
                            float ScenePhase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", Scene1);
                            if (Extensions.IsMoveControlPressed())
                            {
                                Cancel = true;
                                break;
                            }
                            if (Game.LocalPlayer.Character.IsDead)
                            {
                                Cancel = true;
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
                    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToSuicide, "mp_suicide", "pill", 8.0f, -8.0f, -1, 1, 0, false, false, false);

                    if (PedToSuicide.Handle != Game.LocalPlayer.Character.Handle)
                    {
                        GameFiber.Wait(6000);
                        Game.LocalPlayer.Character.Kill();
                    }
                    else
                    {
                        bool Cancel = false;
                        uint GameTimeStartedPillSuicide = Game.GameTime;
                        while(Game.GameTime - GameTimeStartedPillSuicide <= 2000)
                        {
                            if(Extensions.IsMoveControlPressed())
                            {
                                Cancel = true;
                                Game.LocalPlayer.Character.Tasks.Clear();
                                break;
                            }
                            GameFiber.Yield();
                        }
                        if(!Cancel)
                        {
                            GameFiber.Wait(4000);
                            Game.LocalPlayer.Character.Kill();
                        }
                    }
                }
                IsCommitingSuicide = false;
            }
        }, "Suicide");
        Debugging.GameFibers.Add(Suicide);
    }
    //public static void CommitSuicideRequiringIntervention()
    //{
    //    if (IsCommitingSuicide)
    //        return;

    //    if(Game.LocalPlayer.Character.IsInAnyVehicle(false))
    //    {
    //        Game.LocalPlayer.Character.Kill();
    //        return;
    //    }

    //    GameFiber Suicide = GameFiber.StartNew(delegate
    //    {

    //        Ped PedToSuicide = Game.LocalPlayer.Character;
    //        if (!PedToSuicide.IsInAnyVehicle(false))
    //        {
    //            IsCommitingSuicide = true;
    //            LosSantosRED.RequestAnimationDictionay("mp_suicide");

    //            GTAWeapon CurrentGun = null;
    //            if (PedToSuicide.Inventory.EquippedWeapon != null)
    //                CurrentGun = GTAWeapons.WeaponsList.Where(x => (WeaponHash)x.Hash == PedToSuicide.Inventory.EquippedWeapon.Hash && x.CanPistolSuicide).FirstOrDefault();

    //            if (CurrentGun != null)
    //            {
    //                Vector3 SuicidePosition = PedToSuicide.Position;

    //                int Scene1 = NativeFunction.CallByName<int>("CREATE_SYNCHRONIZED_SCENE", SuicidePosition.X, SuicidePosition.Y, SuicidePosition.Z, 0.0f, 0.0f, PedToSuicide.Heading, 2);//270f //old
    //                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_LOOPED", Scene1, false);
    //                NativeFunction.CallByName<bool>("TASK_SYNCHRONIZED_SCENE", PedToSuicide, Scene1, "mp_suicide", "pistol", 1000.0f, -4.0f, 64, 0, 0x447a0000, 0);//std_perp_ds_a
    //                NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_PHASE", Scene1, 0.0f);

    //                uint GameTimeStartedSuicide = Game.GameTime;
    //                bool Cancel = false;
    //                while (Game.GameTime - GameTimeStartedSuicide <= 5000)
    //                {
    //                    float ScenePhase = NativeFunction.CallByName<float>("GET_SYNCHRONIZED_SCENE_PHASE", Scene1);
    //                    if (Extensions.IsMoveControlPressed())
    //                    {
    //                        Cancel = true;
    //                        break;
    //                    }
    //                    if (Game.LocalPlayer.Character.IsDead)
    //                    {
    //                        Cancel = true;
    //                        break;
    //                    }
    //                    if (ScenePhase >= 0.3f)
    //                    {
    //                        NativeFunction.CallByName<bool>("SET_SYNCHRONIZED_SCENE_RATE", Scene1, 0f);
    //                        if (Game.IsControlJustPressed(2, GameControl.Attack))
    //                        {
    //                            Vector3 HeadCoordinated = PedToSuicide.GetBonePosition(PedBoneId.Head);
    //                            NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", PedToSuicide, HeadCoordinated.X, HeadCoordinated.Y, HeadCoordinated.Z, true);
    //                            Game.LocalPlayer.Character.Kill();
    //                            break;
    //                        }
    //                    }
    //                    GameFiber.Yield();
    //                }
    //                PedToSuicide.Tasks.Clear();
    //            }
    //            else
    //            {
    //                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", PedToSuicide, (uint)2725352035, true);
    //                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToSuicide, "mp_suicide", "pill", 8.0f, -8.0f, -1, 1, 0, false, false, false);
    //                GameFiber.Wait(6000);
    //            }
    //            IsCommitingSuicide = false;
    //        }
    //    }, "Suicide");
    //    Debugging.GameFibers.Add(Suicide);
    //}

    //public static void CommitSuicide(Ped PedToSuicide)
    //{
    //    GameFiber Suicide = GameFiber.StartNew(delegate
    //    {
    //        if (!PedToSuicide.IsInAnyVehicle(false))
    //        {
    //            LosSantosRED.RequestAnimationDictionay("mp_suicide");

    //            GTAWeapon CurrentGun = null;
    //            if (PedToSuicide.Inventory.EquippedWeapon != null)
    //                CurrentGun = GTAWeapons.WeaponsList.Where(x => (WeaponHash)x.Hash == PedToSuicide.Inventory.EquippedWeapon.Hash && x.CanPistolSuicide).FirstOrDefault();

    //            if (CurrentGun != null)
    //            {
    //                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToSuicide, "mp_suicide", "pistol", 8.0f, -8.0f, -1, 1, 0, false, false, false);
    //                GameFiber.Wait(750);
    //                Vector3 HeadCoordinated = PedToSuicide.GetBonePosition(PedBoneId.Head);
    //                NativeFunction.CallByName<bool>("SET_PED_SHOOTS_AT_COORD", PedToSuicide, HeadCoordinated.X, HeadCoordinated.Y, HeadCoordinated.Z, true);
    //            }
    //            else
    //            {
    //                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", PedToSuicide, (uint)2725352035, true);
    //                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToSuicide, "mp_suicide", "pill", 8.0f, -8.0f, -1, 1, 0, false, false, false);
    //                GameFiber.Wait(6000);
    //            }
    //        }
    //        PedToSuicide.Kill();
    //    }, "Suicide");
    //    Debugging.GameFibers.Add(Suicide);
    //}
}

