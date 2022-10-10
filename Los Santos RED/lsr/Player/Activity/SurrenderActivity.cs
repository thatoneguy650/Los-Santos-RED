using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Linq;

public class SurrenderActivity : DynamicActivity
{
    private IInputable Player;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private uint GameTimeLastYelled;
    private uint GameTimeLastToggledSurrender;
    private bool IsToggled;

    public SurrenderActivity(IInputable currentPlayer, IEntityProvideable world, ISettingsProvideable settings)
    {
        Player = currentPlayer;
        World = world;
        Settings = settings;
    }
    public bool CanSurrender => !HandsAreUp && !Player.IsAiming && (!Player.IsInVehicle || !Player.IsMoving) && Player.IsWanted;
    public bool CanWaveHands => !HandsAreUp && !IsWavingHands && !Player.IsAiming && (!Player.IsInVehicle || !Player.IsMoving) && Player.IsNotWanted;
    public override ModItem ModItem { get; set; }
    public override string DebugString => "";
    public override bool CanPause { get; set; } = false;
    public override bool CanCancel { get; set; } = false;
    public override string PausePrompt { get; set; } = "Pause Activity";
    public override string CancelPrompt { get; set; } = "Stop Activity";
    public override string ContinuePrompt { get; set; } = "Continue Activity";
    public bool IsCommitingSuicide { get; set; }

    public bool IsWavingHands { get; private set; }
    public bool HandsAreUp { get; private set; }


    public override void Cancel()
    {
        LowerHands();
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
        RaiseHands();
    }
    public void LowerHands()
    {
        if (!IsToggled)
        {
            EntryPoint.WriteToConsole($"PLAYER EVENT: Lower Hands", 3);
            HandsAreUp = false; // You put your hands down
            IsWavingHands = false;
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            Player.IsPerformingActivity = false;
        }
    }
    public void RaiseHands()
    {
        if (!IsToggled)
        {
            EntryPoint.WriteToConsole($"PLAYER EVENT: Raise Hands", 3);
            if (Player.Character.IsWearingHelmet)
            {
                Player.Character.RemoveHelmet(true);
            }
            if (HandsAreUp)
            {
                return;
            }
            Player.WeaponEquipment.SetUnarmed();
            HandsAreUp = true;
            if (Player.IsInVehicle)
            {
                AnimationDictionary.RequestAnimationDictionay("veh@busted_std");
                NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, "veh@busted_std", "stay_in_car_crim", 2.0f, -2.0f, -1, 50, 0, false, false, false);
            }
            else
            {
                AnimationDictionary.RequestAnimationDictionay("ped");
                NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
            }
        }
    }
    public void WaveHands()
    {
        if (!IsToggled)
        {
            EntryPoint.WriteToConsole($"PLAYER EVENT: Wave Hands", 3);
            if (HandsAreUp || Player.IsInVehicle || IsWavingHands)
            {
                return;
            }
            Player.WeaponEquipment.SetUnarmed();
            IsWavingHands = true;

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
            //if (RandomItems.RandomPercent(50))
            //{
            //    Animation = "ground_wave";
            //}
            //else
            //{
            Animation = "air_wave";
            //}
            AnimationDictionary.RequestAnimationDictionay(DictionaryName);
            NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, DictionaryName, Animation, 2.0f, -2.0f, -1, 49, 0, false, false, false);
        }

    }
    public void SetArrestedAnimation(bool StayStanding)
    {
        //StayStanding = false;
        GameFiber SetArrestedAnimation = GameFiber.StartNew(delegate
        {
            AnimationDictionary.RequestAnimationDictionay("veh@busted_std");
            AnimationDictionary.RequestAnimationDictionay("busted");
            AnimationDictionary.RequestAnimationDictionay("ped");
            if (!Player.Character.Exists())
            {
                return;
            }
            while (Player.Character.Exists() && (Player.Character.IsRagdoll || Player.Character.IsStunned))
            {
                GameFiber.Yield();
            }
            if (!Player.Character.Exists() || !Player.IsBusted)
            {
                return;
            }


            if (!Settings.SettingsManager.PoliceSettings.DropWeaponWhenBusted)
            {

                Player.WeaponEquipment.SetUnarmed();
            }


            if (Player.Character.IsInAnyVehicle(false))
            {
                Vehicle oldVehicle = Player.Character.CurrentVehicle;
                if (Player.Character.Exists() && oldVehicle.Exists())
                {
                    NativeFunction.Natives.TASK_LEAVE_VEHICLE(Player.Character, oldVehicle, 256);
                    while(Player.Character.IsInAnyVehicle(false) && Player.IsBusted)
                    {
                        GameFiber.Yield();
                    }
                    if (!Player.Character.Exists() || !Player.IsBusted)
                    {
                        return;
                    }
                }
            }


            if (StayStanding)
            {
                //Player.Equipment.SetUnarmed();
                if (IsNotPlayingAnimation("ped", "handsup_enter"))
                {
                    NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);

                    GameFiber.Wait(500);
                    if(Player.IsBusted && Player.WeaponEquipment.CurrentWeapon != null && Settings.SettingsManager.PoliceSettings.DropWeaponWhenBusted)
                    {
                        DropWeapon(false);
                    }
                }
            }
            else
            {
                if (IsNotPlayingAnimation("busted", "idle_a") && IsNotPlayingAnimation("busted", "idle_2_hands_up") && IsNotPlayingAnimation("busted", "idle_2_hands_up_2h"))
                {
                    bool isOneHanded = false;
                    if(Player.WeaponEquipment.CurrentWeapon != null && !Player.WeaponEquipment.CurrentWeaponIsOneHanded)
                    {
                        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, "busted", "idle_2_hands_up_2h", 2.0f, -2.0f, -1, 2, 0, false, false, false);

                    }
                    else
                    {
                        isOneHanded = true;
                        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, "busted", "idle_2_hands_up", 2.0f, -2.0f, -1, 2, 0, false, false, false);
                    }
                    GameFiber.Wait(1000);
                    if (!Player.Character.Exists() || !Player.IsBusted)
                    {
                        if (Player.Character.Exists())
                        {
                            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                        }
                        return;
                    }
                    if (isOneHanded)
                    {
                        if (Player.IsBusted && Player.WeaponEquipment.CurrentWeapon != null && Settings.SettingsManager.PoliceSettings.DropWeaponWhenBusted)
                        {
                            DropWeapon(false);
                        }
                        GameFiber.Wait(5000);//was just 6000 here
                    }
                    else
                    {
                        GameFiber.Wait(2000);//was just 6000 here
                        if (Player.IsBusted && Player.WeaponEquipment.CurrentWeapon != null && Settings.SettingsManager.PoliceSettings.DropWeaponWhenBusted)
                        {
                            DropWeapon(true);
                        }
                        GameFiber.Wait(3000);//was just 6000 here
                    }
                    if (!Player.Character.Exists() || !Player.IsBusted)
                    {
                        if(Player.Character.Exists())
                        {
                            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                        }
                        
                        return;
                    }
                    NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, "busted", "idle_a", 8.0f, -8.0f, -1, 1, 0, false, false, false);
                }
            }
            NativeFunction.Natives.SET_PED_KEEP_TASK(Player.Character, true);
            //Player.Character.KeepTasks = true;
        }, "SetArrestedAnimation");
    }





    private void DropWeapon(bool isLow)
    {
        //NativeFunction.Natives.x616093EC6B139DD9(Game.LocalPlayer, NativeFunction.Natives.xD6429A016084F1A5<uint>((int)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash), false);

        //NativeFunction.Natives.x88EAEC617CD26926(NativeFunction.Natives.xD6429A016084F1A5<uint>((int)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash), false);
        //NativeFunction.Natives.SET_PED_CONFIG_FLAG<bool>(Game.LocalPlayer.Character, 186, false);
        Vector3 HandPosition = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 57005));
        Vector3 RootPosition = NativeFunction.CallByName<Vector3>("GET_WORLD_POSITION_OF_ENTITY_BONE", Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 0));
        Vector3 ResultPosition = HandPosition - RootPosition;


        ResultPosition = new Vector3(ResultPosition.X - 0.2f, ResultPosition.Y + 0.2f, ResultPosition.Z -0.2f);
        ResultPosition = new Vector3(0.6f, 0.6f, 0.6f);


        if(isLow)
        {
            ResultPosition = new Vector3(0.75f, 0.75f, -0.3f);
        }



        Player.WeaponEquipment.DisableWeaponPickup((uint)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash);


        NativeFunction.Natives.SET_PED_DROPS_INVENTORY_WEAPON(Game.LocalPlayer.Character, (int)Game.LocalPlayer.Character.Inventory.EquippedWeapon.Hash, ResultPosition.X, ResultPosition.Y, ResultPosition.Z, -1);




        if (!(Game.LocalPlayer.Character.Inventory.EquippedWeapon == null))
        {
            NativeFunction.Natives.SET_CURRENT_PED_WEAPON(Game.LocalPlayer.Character, (uint)2725352035, true);
        }
        //NativeFunction.Natives.SET_PED_DROPS_WEAPON(Player.Character);
    }


    public void UnSetArrestedAnimation()
    {
        GameFiber UnSetArrestedAnimationGF = GameFiber.StartNew(delegate
        {
            EntryPoint.WriteToConsole("UnsetArrestedRan");
            AnimationDictionary.RequestAnimationDictionay("random@arrests");
            AnimationDictionary.RequestAnimationDictionay("busted");
            AnimationDictionary.RequestAnimationDictionay("ped");
            if (IsPlayingAnimation("busted", "idle_a") || IsPlayingAnimation("busted", "idle_2_hands_up") || IsPlayingAnimation("busted", "idle_2_hands_up_2h"))
            {
                EntryPoint.WriteToConsole("UnsetArrestedRan Playing Kneeling");

                NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, "busted", "hands_up_2_idle", 2.0f, -2.0f, -1, 4096, 0, 0, 1, 0);
                GameFiber.Wait(1500);//1250
                if (!Player.Character.Exists() || !Player.IsBusted)
                {
                    if (Player.Character.Exists())
                    {
                        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                    }
                    return;
                }
                if (Player.Character.Exists())
                {
                    NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                }
            }
            else if (IsPlayingAnimation("ped", "handsup_enter"))// NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", Player.Character, "ped", "handsup_enter", 3) || NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Player.Character, "ped", "handsup_enter") > 0f)
            {
                EntryPoint.WriteToConsole("UnsetArrestedRan Playing HandsUp");
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            }
            else
            {
                EntryPoint.WriteToConsole("UnsetArrestedRan No Animation Detected, Clearing Tasks");
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            }
        }, "UnSetArrestedAnimation");
    }
    private bool IsPlayingAnimation(string dictionary, string animation)
    {
        return NativeFunction.Natives.IS_ENTITY_PLAYING_ANIM<bool>(Player.Character, dictionary, animation, 3) || NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Player.Character, dictionary, animation) > 0f;
    }
    private bool IsNotPlayingAnimation(string dictionary, string animation)
    {
        return !NativeFunction.Natives.IS_ENTITY_PLAYING_ANIM<bool>(Player.Character, dictionary, animation, 3) || NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Player.Character, dictionary, animation) == 0f;
    }

    public void OnPlayerBusted()
    {
        HandsAreUp = false;
        IsWavingHands = false;
        if (Player.WantedLevel > 1)
        {
            SetArrestedAnimation(Player.WantedLevel <= 2);//needs to move
        }
    }
    public void ToggleSurrender()
    {
        
        if (Game.GameTime - GameTimeLastToggledSurrender >= 500)
        {
            EntryPoint.WriteToConsole($"Surrender Toggle START {IsToggled}");
            if (HandsAreUp)
            {
                if (!Player.IsBusted)
                {
                    IsToggled = false;
                    LowerHands();
                    
                }
            }
            else if (IsWavingHands)
            {
                IsToggled = false;
                LowerHands();
                
            }
            else
            {
                if (CanSurrender)
                {
                    RaiseHands();
                    IsToggled = true;
                }
                else if (CanWaveHands)
                {
                    WaveHands();
                    IsToggled = true;
                }
            }
            EntryPoint.WriteToConsole($"Surrender Toggle END {IsToggled}");
            GameTimeLastToggledSurrender = Game.GameTime;
        }
    }
}